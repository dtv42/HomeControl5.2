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

    using System;
    using System.CommandLine;
    using System.CommandLine.Builder;
    using System.CommandLine.Parsing;
    using System.Diagnostics;
    using System.Globalization;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using Serilog;
    using Serilog.Core;

    using UtilityLib;
    using UtilityApp.Commands;
    using UtilityApp.Models;

    #endregion

    internal static class Program
    {
        /// <summary>
        /// The application settings instance.
        /// </summary>
        public static AppSettings Settings { get; set; } = new AppSettings();

        /// <summary>
        /// The (Serilog) logging level switch instance.
        /// </summary>
        public static LoggingLevelSwitch LevelSwitch { get; set; } = new LoggingLevelSwitch();

        /// <summary>
        /// The entry point for the program.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>When complete, an integer representing success (0) or failure (non-0).</returns>
        public static async Task<int> Main(string[] args)
        {
            var hostBuilder = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddCommandOptions<GreetCommand, GreetOptions>();
                    services.AddRootCommandOptions<AppCommand, GlobalOptions>();
                });

            hostBuilder.UseSerilog();

            var host = hostBuilder.Build();
            var serviceProvider = host.Services;
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();

            configuration.GetSection("AppSettings").Bind(Settings);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(LevelSwitch)
                .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.Console()
                .CreateLogger();

            Log.Debug("Setup root command");
            var rootCommand = serviceProvider.GetRequiredService<RootCommand>();
            var commandLineBuilder = new CommandLineBuilder(rootCommand);

            foreach (Command command in serviceProvider.GetServices<Command>())
            {
                Log.Debug($"Adding command {command.Name}");
                commandLineBuilder.AddCommand(command);
            }

            var parser = commandLineBuilder
                .UseParseErrorReporting()
                .UseVersionOption()
                .UseDefaults()
                .Build();

            CultureInfo.CurrentCulture = new CultureInfo("en-US");
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            try
            {
                return await parser.InvokeAsync(args).ConfigureAwait(false);
            }
            catch(Exception ex)
            {
                Log.Fatal(ex, "Application terminated unexpectedly");
                return -1;
            }
            finally
            {
                stopWatch.Stop();
                Console.WriteLine($"Time elapsed {stopWatch.Elapsed}");
                Log.CloseAndFlush();
            }
        }
    }
}
