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
    using System.Threading.Tasks;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using Serilog;

    using UtilityLib;
    using UtilityLib.Console;
    using UtilityApp.Commands;
    using UtilityApp.Options;

    #endregion

    /// <summary>
    ///  Application class providing the main entry point.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The entry point for the program reading testdata.json configuration, configuring the logger
        /// using Serilog, and configuring all application commands and options.
        /// The RunCommandLineAsync() is configuring the commandline parser.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>When complete, an integer representing success (0) or failure (non-0).</returns>
        public static async Task<int> Main(string[] args)
        {
            try
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
                            // Add command options.
                            .AddSingletonFromSection<GlobalOptions>()
                            .AddSingletonFromSection<GreetOptions>()
                            .AddSingleton<TestOptions>()
                            .AddSingleton<PropertyOptions>()
                            .AddSingleton<TestdataOptions>()
                            .AddSingleton<ValidateOptions>()
                            // Add commands.
                            .AddSingleton<TestCommand>()
                            .AddSingleton<ErrorCommand>()
                            .AddSingleton<AsyncCommand>()
                            .AddSingleton<LoggingCommand>()
                            .AddSingleton<SettingsCommand>()
                            .AddSingleton<GreetCommand>()
                            .AddSingleton<PropertyCommand>()
                            .AddSingleton<TestdataCommand>()
                            .AddSingleton<ValidateCommand>()
                            // Add root command.
                            .AddSingleton<RootCommand, AppCommand>();
                    })
                    .ConfigureLogging((context, logger) =>
                    {

                    })
                    .UseSerilog((context, logger) =>
                    {
                        logger.ReadFrom.Configuration(context.Configuration);
                    })
                    .Build()
                    .RunCommandLineAsync(args);
            }
            catch (Exception exception)
            {
                {
                    Console.ForegroundColor = ConsoleColor.Red;

                    Console.Error.WriteLine($"Unhandled exception: {exception.Message}");

                    if (exception.InnerException is not null)
                    {
                        Console.Error.WriteLine($"    Inner Exception: {exception.InnerException.Message}");
                    }

                    Console.ResetColor();
                    return (int)ExitCodes.UnhandledException;
                }
            }
        }
    }
}
