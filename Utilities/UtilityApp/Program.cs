// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>22-11-2020 11:13</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace UtilityApp
{
    #region Using Directives

    using System.Threading.Tasks;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;

    using Serilog;
    using Serilog.Sinks.SystemConsole.Themes;

    using UtilityLib;
    using UtilityApp.Commands;
    using UtilityApp.Options;

    #endregion

    internal static class Program
    {
        /// <summary>
        /// The entry point for the program. The RunCommandLineAsync() is configuring the commandline parser.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>When complete, an integer representing success (0) or failure (non-0).</returns>
        public static async Task<int> Main(string[] args)
        {
            // Create host using serilog, adding commands and options services.
            return await Host.CreateDefaultBuilder()
                .ConfigureHostConfiguration(config =>
                {

                })
                .ConfigureAppConfiguration(config =>
                {
                    config.AddJsonFile("testdata.json", optional: false, reloadOnChange: false);
                })
                .ConfigureServices((context, services) =>
                {
                    services
                        .AddCommand<ErrorCommand>()
                        .AddCommand<AsyncCommand>()
                        .AddCommand<LoggingCommand>()
                        .AddCommand<SettingsCommand>()
                        .AddCommandOptions<GreetCommand, GreetOptions>()
                        .AddCommandOptions<PropertyCommand, PropertyOptions>()
                        .AddCommandOptions<TestdataCommand, TestdataOptions>()
                        .AddCommandOptions<ValidateCommand, ValidateOptions>()
                        .AddRootCommandOptions<AppCommand, GlobalOptions>();
                })
                .ConfigureLogging((context, logger) =>
                {

                })
                .UseSerilog((context, logger) =>
                {
                    logger.ReadFrom.Configuration(context.Configuration)
                          .Enrich.FromLogContext()
                          .WriteTo.File(
                              "Logs/log-.log",
                              rollingInterval: RollingInterval.Day,
                              outputTemplate: "{Timestamp: HH:mm:ss.fff zzz} {SourceContext} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                          .WriteTo.Console(
                              theme: AnsiConsoleTheme.Code,
                              outputTemplate: "{Timestamp: HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}");
                })
                .Build()
                .RunCommandLineAsync(args);
        }
    }
}
