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
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using Serilog;
    using Serilog.Core;
    using Serilog.Events;
    using Serilog.Sinks.SystemConsole.Themes;

    using UtilityLib;
    using UtilityApp.Commands;
    using UtilityApp.Models;
    using UtilityApp.Options;
    using System.CommandLine;

    #endregion

    internal static class Program
    {
        /// <summary>
        /// The application settings instance.
        /// </summary>
        public static AppSettings Settings { get; set; } = new AppSettings();

        /// <summary>
        /// The (Serilog) logging level switch instances.
        /// </summary>
        public static LoggingLevelSwitch ConsoleSwitch { get; set; } = new LoggingLevelSwitch();
        public static LoggingLevelSwitch LogFileSwitch { get; set; } = new LoggingLevelSwitch();

        /// <summary>
        /// The entry point for the program.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>When complete, an integer representing success (0) or failure (non-0).</returns>
        public static async Task<int> Main(string[] args)
        {
            // Create host using serilog, adding commands and options services.
            return await Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration(config =>
                {
                    config.AddJsonFile("testdata.json", optional: false, reloadOnChange: false)
                        .Build()
                        .GetSection("AppSettings").Bind(Settings);
                })
                .ConfigureServices((context, services) =>
                {
                    services
                        // Add application specific settings.
                        .AddSingleton(context.Configuration.GetSection("AppSettings").Get<AppSettings>().ValidateAndThrow())
                        .AddSingleton(context.Configuration.GetSection("TestData").Get<Testdata>().ValidateAndThrow())
                        // Add commands and options.
                        .AddCommand<LogCommand>()
                        .AddCommand<AsyncCommand>()
                        .AddCommand<SettingsCommand>()
                        .AddCommandOptions<GreetCommand, GreetOptions>()
                        .AddCommandOptions<PropertyCommand, PropertyOptions>()
                        .AddCommandOptions<TestdataCommand, TestdataOptions>()
                        .AddCommandOptions<ValidateCommand, ValidateOptions>()
                        .AddRootCommandOptions<AppCommand, GlobalOptions>();
                })
                .UseSerilog((context, logger) =>
                {
                    ConsoleSwitch.MinimumLevel = context.Configuration.GetValue<LogEventLevel>("Serilog:LevelSwitches:$ConsoleSwitch");
                    LogFileSwitch.MinimumLevel = context.Configuration.GetValue<LogEventLevel>("Serilog:LevelSwitches:$FileSwitch");

                    logger.ReadFrom.Configuration(context.Configuration)
                          .Enrich.FromLogContext()
                          .WriteTo.File(
                              "Logs/log-.log",
                              levelSwitch: LogFileSwitch,
                              rollingInterval: RollingInterval.Day,
                              outputTemplate: "{Timestamp: HH:mm:ss.fff zzz} {SourceContext} [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                          .WriteTo.Console(
                              levelSwitch: ConsoleSwitch,
                              theme: AnsiConsoleTheme.Code,
                              outputTemplate: "{Timestamp: HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}");
                })
                .Build()
                .RunCommandLineAsync(args);
        }
    }
}
