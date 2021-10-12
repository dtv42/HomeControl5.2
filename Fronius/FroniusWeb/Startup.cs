// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Startup.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>20-4-2020 13:22</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace FroniusWeb
{
    #region Using Directives

    using System;
    using System.Collections.Generic;

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

    using FroniusLib;
    using FroniusLib.Models;
    using FroniusWeb.Models;

    #endregion Using Directives

    /// <summary>
    ///  Standard Startup implementation for a rest based web using controllers and Swagger.
    /// </summary>
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
        ///  This method gets called by the runtime. This method adds services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Get application settings.
            var settings = _configuration.GetSection("AppSettings").Get<AppSettings>();

            services
            // Add the gateway and ping settings.
                .AddSingleton<IPingHealthCheckOptions>(settings.PingOptions)
                .AddSingleton<IFroniusSettings>(settings.GatewaySettings)

            // Add the named gateway Http client (supporting request error policies).
                .AddPollyHttpClient<FroniusClient>("FroniusClient",
                    new List<TimeSpan>
                    {
                        TimeSpan.FromSeconds(10),
                        TimeSpan.FromSeconds(20),
                        TimeSpan.FromSeconds(30)
                    },
                    client =>
                    {
                        client.BaseAddress = new Uri(settings.GatewaySettings.Address);
                        client.Timeout = TimeSpan.FromMilliseconds(settings.GatewaySettings.Timeout);
                    });

            // Add the gateway service.
            services
                .AddSingleton<FroniusGateway>()

                // Configure health checks.
                .AddHealthChecks()
                    .AddProcessAllocatedMemoryHealthCheck(maximumMegabytesAllocated: 100, tags: new[] { "process", "memory" })
                    .AddCheck<GatewayHealthCheck<FroniusGateway>>("gateway1", tags: new[] { "gateway" })
                    .AddCheck<PingHealthCheck>("gateway2", tags: new[] { "gateway" })
                ;

            // Adding healthchecks UI configuring endpoints.
            services
                .AddHealthChecksUI(settings =>
                {
                    settings.SetHeaderText("EM300LR Gatway - Health Checks Status");
                    settings.AddHealthCheckEndpoint("Process", "http://localhost/health-process");
                    settings.AddHealthCheckEndpoint("Gateway", "http://localhost/health-gateway");
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
                    c.SwaggerDoc("v1", new OpenApiInfo
                    {
                        Title = "Fronius Gateway Web API",
                        Description = "This is a web gateway service for a Fronius Symo 8.2-3-M solar inverter.",
                        Version = "v1"
                    });
                });
        }

        /// <summary>
        ///  This method gets called by the runtime. This method configures the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.ApplicationServices.GetRequiredService<FroniusGateway>().Startup();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseHealthChecks("/healthchecks");

            app.UseSerilogRequestLogging();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fronius Gateway API V1");
            });

            app.UseEndpoints(endpoints =>
            {
                // adding endpoint of health check for the health check ui in UI format
                endpoints.MapHealthChecks("/health-gateway", new HealthCheckOptions
                {
                    Predicate = r => r.Tags.Contains("gateway"),
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