// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Startup.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>14-12-2020 14:55</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace UtilityWeb
{
    #region Using Directives

    using System;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.AspNetCore.Hosting;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;

    using HealthChecks.UI.Client;
    using Serilog;

    using UtilityLib;
    using UtilityLib.Webapp;

    using UtilityWeb.Models;
    using UtilityWeb.Services;

    #endregion Using Directives

    public class Startup
    {
        /// <summary>
        /// The application configuration.
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        ///  Initializes a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The application configuration instance.</param>
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services">The service collection.</param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Get application settings.
            var settings = _configuration.GetSection("AppSettings").Get<AppSettings>();
            var gateway = settings.GatewaySettings;

            services
            // Add the named gateway Http client (supporting request error policies).
               .AddPollyHttpClient("Gateway", gateway.Retries, gateway.Wait,
                   client =>
                   {
                       client.BaseAddress = new Uri(gateway.Address);
                       client.Timeout = TimeSpan.FromMilliseconds(gateway.Timeout);
                   });

            // Add the gateway service.
            services
                .AddSingleton<WebGateway>()

            // Add the ping settings.
                .AddSingleton<IPingHealthCheckOptions>(settings.PingOptions)

            // Configure health checks.
                .AddHealthChecks()
                    .AddProcessAllocatedMemoryHealthCheck(maximumMegabytesAllocated: 100, tags: new[] { "process", "memory" })
                    .AddCheck<GatewayHealthCheck<WebGateway>>("gateway1", tags: new[] { "gateway" })
                    .AddCheck<PingHealthCheck>("gateway2", tags: new[] { "gateway" })
                    .AddCheck<RandomHealthCheck>("random1", tags: new[] { "random" })
                    .AddCheck<RandomHealthCheck>("random2", tags: new[] { "random" })
                ;

            // Adding healthchecks UI configuring endpoints.
            services
                .AddHealthChecksUI(settings =>
                {
                    settings.SetHeaderText("UtilityWeb - Health Checks Status");
                    settings.AddHealthCheckEndpoint("Random", "/health-random");
                    settings.AddHealthCheckEndpoint("Process", "/health-process");
                    settings.AddHealthCheckEndpoint("Gateway", "/health-gateway");
                })
                .AddInMemoryStorage()
                ;

            // Setup the default Json serialization options.
            services
                .AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.AddDefaultOptions())
                ;

            // Add Swagger support.
            services
                .AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "UtilityWeb", Version = "v1" });
                });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">The application builder instance.</param>
        /// <param name="env">The web hosting environment.</param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.ApplicationServices.GetService<WebGateway>().Startup();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseHealthChecks("/healthchecks");

            app.UseSerilogRequestLogging();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UtilityWeb v1"));

            app.UseRouting();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // adding endpoint of health check for the health check ui in UI format
                endpoints.MapHealthChecks("/health-gateway", new HealthCheckOptions
                {
                    Predicate = r => r.Tags.Contains("gateway"),
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

                endpoints.MapHealthChecks("/health-random", new HealthCheckOptions
                {
                    Predicate = r => r.Tags.Contains("random"),
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

                endpoints.MapHealthChecks("/health-process", new HealthCheckOptions
                {
                    Predicate = r => r.Tags.Contains("process"),
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
                });

                // map healthcheck ui endpoint (/healthchecks-ui) and use custom style sheet.
                endpoints.MapHealthChecksUI(setup =>
                {
                    setup.AddCustomStylesheet("wwwroot/css/HealthCheck.css");
                });

                endpoints.MapControllers();
            });
        }
    }
}
