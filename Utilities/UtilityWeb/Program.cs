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
    using Microsoft.Extensions.Hosting;

    using Serilog;
    using Serilog.Sinks.SystemConsole.Themes;

    #endregion Using Directives

    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .ConfigureAppConfiguration((context, config) =>
                        {
                            config.AddJsonFile("testdata.json", optional: false, reloadOnChange: false);
                        })
                        .UseSerilog((context, logger) =>
                        {
                            logger.ReadFrom.Configuration(context.Configuration)
                                  .WriteTo.File(
                                      "Logs/log-.log",
                                      rollingInterval: RollingInterval.Day,
                                      outputTemplate: "{Timestamp: HH:mm:ss.fff zzz} {SourceContext} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                                  .WriteTo.Console(
                                      theme: AnsiConsoleTheme.Code,
                                      outputTemplate: "{Timestamp: HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}");
                        })
                        .UseStartup<Startup>();
                });
    }
}
