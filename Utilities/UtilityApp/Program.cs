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
                    logger.ReadFrom.Configuration(context.Configuration);
                })
                .Build()
                .RunCommandLineAsync(args);
        }
    }
}
