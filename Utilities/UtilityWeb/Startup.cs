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
    using HealthChecks.UI.Client;
    #region Using Directives

    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics.HealthChecks;
    using Microsoft.AspNetCore.Hosting;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using Microsoft.Extensions.Hosting;
    using Microsoft.OpenApi.Models;

    using Serilog;

    using UtilityLib;
    using UtilityLib.Webapp;

    using UtilityWeb.Models;
    using UtilityWeb.Services;

    #endregion Using Directives

    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Get application settings.
            var settings = _configuration.GetSection("AppSettings").Get<AppSettings>();

            services
            // Add the gateway client.
               .AddPollyHttpClient<WebGateway>("Gateway", settings.GatewaySettings)

            // Add the gateway service.
                .AddSingleton<IGateway, WebGateway>()

            // Add the ping settings.
                .AddSingleton<IPingHealthCheckOptions>(settings.PingOptions)

            // Configure health checks.
                .AddHealthChecks()
                    .AddProcessAllocatedMemoryHealthCheck(maximumMegabytesAllocated: 100, tags: new[] { "process", "memory" })
                    .AddCheck<RandomHealthCheck> ("random1",  tags: new[] { "random"  })
                    .AddCheck<RandomHealthCheck> ("random2",  tags: new[] { "random"  })
                    .AddCheck<GatewayHealthCheck>("gateway1", tags: new[] { "gateway" })
                    .AddCheck<PingHealthCheck>   ("gateway2", tags: new[] { "gateway" })
                ;

            //adding healthchecks UI
            services
                .AddHealthChecksUI(settings =>
                {
                    settings.SetHeaderText("UtilityWeb - Health Checks Status");
                    settings.AddHealthCheckEndpoint("endpoint1", "/health-random");
                    settings.AddHealthCheckEndpoint("endpoint2", "/health-process");
                    settings.AddHealthCheckEndpoint("endpoint3", "/health-gateway");
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

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.ApplicationServices.GetService<IGateway>().Startup();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "UtilityWeb v1"));
            }

            //adding health check endpoint
            app.UseHealthChecks("/healthcheck");

            app.UseSerilogRequestLogging();

            app.UseRouting();
            
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                //adding endpoint of health check for the health check ui in UI format
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

                //map healthcheck ui endpoing - default is /healthchecks-ui/
                endpoints.MapHealthChecksUI(setup =>
                {
                    setup.AddCustomStylesheet("HealthCheck.css");
                });
                endpoints.MapControllers();
            });
        }
    }
}
