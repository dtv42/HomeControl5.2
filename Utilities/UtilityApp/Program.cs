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
    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using Serilog;
    using Serilog.Core;
    using Serilog.Formatting.Json;
    using UtilityApp.Commands;
    using UtilityApp.Models;
    using UtilityLib;

    #endregion

    internal class Program
    {
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
                    services.AddSingleton<LoggingLevelSwitch>();
                    services.AddCommandOptions<GreetCommand, GreetOptions>();
                    services.AddRootCommandOptions<AppCommand, GlobalOptions>();
                });

            hostBuilder.UseSerilog();

            var serviceProvider = hostBuilder.Build().Services;
            var globalOptions = serviceProvider.GetRequiredService<GlobalOptions>();
            var levelSwitch = serviceProvider.GetRequiredService<LoggingLevelSwitch>();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.ControlledBy(levelSwitch)
                .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
                .WriteTo.Console()
                .CreateLogger();

            var rootCommand = serviceProvider.GetRequiredService<RootCommand>();
            var commandLineBuilder = new CommandLineBuilder(rootCommand);

            foreach (Command command in serviceProvider.GetServices<Command>())
            {
                commandLineBuilder.AddCommand(command);
            }

            var parser = commandLineBuilder
                .UseParseErrorReporting()
                .UseVersionOption()
                .UseDefaults()
                .Build();

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
                Log.CloseAndFlush();
            }
        }
    }
}
