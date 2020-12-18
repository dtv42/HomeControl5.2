// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="DTV-Online">
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

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Serilog;
    using Serilog.Extensions.Logging;

    #endregion Using Directives

    /// <summary>
    ///  Application class providing the main entry point.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        ///  Main application entrypoint creating a Host instance, adding the testdata.json configuration,
        ///  configuring the logger using Serilog and run the web application.
        /// </summary>
        /// <param name="args">The command line arguments</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        ///  Helper function to prepare testdata and logging.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <returns>The host builder instance.</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .CaptureStartupErrors(true)
                        .ConfigureAppConfiguration((context, config) =>
                        {
                            config.AddJsonFile("testdata.json", optional: false, reloadOnChange: false);
                        })
                        .UseSerilog((context, logger) =>
                        {
                            logger.ReadFrom.Configuration(context.Configuration);
                        })
                        .UseStartup<Startup>();
                });
    }
}
