// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Startup.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>13-5-2020 13:52</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace ModbusRTU
{
    #region Using Directives

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

    using ModbusLib;
    using ModbusLib.Models;
    using ModbusRTU.Models;

    using ModbusRTU.Services;

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

            // Configure health checks.
            services
                .AddHealthChecks()
                    .AddProcessAllocatedMemoryHealthCheck(maximumMegabytesAllocated: 100, tags: new[] { "process", "memory" })
                    .AddCheck<RtuHealthCheck>("gateway", tags: new[] { "gateway" })
                ;

            // Adding healthchecks UI configuring endpoints.
            services
                .AddHealthChecksUI(settings =>
                {
                    settings.SetHeaderText("ModbusRTU - Health Checks Status");
                    settings.AddHealthCheckEndpoint("Process", "/health-process");
                    settings.AddHealthCheckEndpoint("Gateway", "/health-gateway");
                })
                .AddInMemoryStorage()
                ;


            // Setup the default Json serialization options.
            services.AddControllers().AddJsonOptions(options => options.JsonSerializerOptions.AddDefaultOptions());

            // Add a singleton service using the application settings implementing ITcpClientSettings.
            services.AddSingleton((IRtuClientSettings)settings);
            services.AddSingleton<IRtuModbusClient, RtuModbusClient>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "ModbusRTU", Version = "v1" });
            });
        }

        /// <summary>
        ///  This method gets called by the runtime. This method configures the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseHealthChecks("/healthchecks");

            app.UseSerilogRequestLogging();

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ModbusRTU v1"));

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