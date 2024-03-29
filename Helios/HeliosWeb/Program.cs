// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>20-4-2020 13:22</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace HeliosWeb
{
    #region Using Directives

    using System.Net;

    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Hosting;

    using Serilog;

    #endregion Using Directives

    /// <summary>
    ///  Application class providing the main entry point.
    /// </summary>
    public static class Program
    {
        /// <summary>
        ///  Main application entrypoint creating a Host instance and run the web application.
        /// </summary>
        /// <param name="args">The command line arguments</param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        ///  Helper function to prepare testdata and services.
        /// </summary>
        /// <param name="args"></param>
        /// <returns>The host builder instance</returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .CaptureStartupErrors(true)
                        .UseSerilog((context, logger) =>
                        {
                            logger.ReadFrom.Configuration(context.Configuration);
                        })
                        .UseStartup<Startup>()
                        .UseKestrel(opts =>
                        {
                            opts.Listen(IPAddress.Loopback, port: 8003);
                            opts.ListenAnyIP(8003);
                        });
                });
    }
}