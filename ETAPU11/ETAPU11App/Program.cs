// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>17-12-2020 12:52</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace ETAPU11App
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

    using ModbusLib.Models;
    using ModbusLib;

    using ETAPU11Lib;
    using ETAPU11Lib.Models;

    using ETAPU11App.Commands;
    using ETAPU11App.Models;
    using ETAPU11App.Options;

    #endregion Using Directives

    /// <summary>
    /// Class providing the main application entry point.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The entry point for the program configuring the logger using Serilog, and configuring all 
        /// application commands and options. The RunCommandLineAsync() is configuring the commandline parser.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>When complete, an integer representing success (0) or failure (non-0).</returns>
        public static async Task<int> Main(string[] args)
        {
            try
            {
                // Create host using serilog, adding commands and options services.
                return await Host.CreateDefaultBuilder()
                    .ConfigureServices((context, services) =>
                    {
                        var settings = context.Configuration.GetSection("AppSettings").Get<AppSettings>();

                        // Configure the singleton EM300LR client instance.
                        services
                            .AddSingleton((ITcpClientSettings)settings.GlobalOptions)
                            .AddSingleton<TcpModbusClient>()
                            .AddSingleton<IETAPU11Settings>(settings.GlobalOptions)
                            .AddSingleton<ETAPU11Client>()

                            // Add single gateway.
                            .AddSingleton<ETAPU11Gateway>()

                            // Add command options.
                            .AddSingletonFromSection<GlobalOptions>()
                            .AddSingleton<InfoOptions>()
                            .AddSingleton<ReadOptions>()
                            .AddSingleton<MonitorOptions>()

                            // Add commands.
                            .AddSingleton<InfoCommand>()
                            .AddSingleton<MonitorCommand>()
                            .AddSingleton<ReadCommand>()
                            .AddSingleton<WriteCommand>()

                            // Add root command.
                            .AddSingleton<RootCommand, AppCommand>();
                    })
                    .UseSerilog((context, logger) =>
                    {
                        logger.ReadFrom.Configuration(context.Configuration);
                    })
                    .Build()
                    .RunCommandLineAsync(args);
            }
            catch (ArgumentException ax)
            {
                {
                    Console.ForegroundColor = ConsoleColor.Red;

                    Console.Error.WriteLine(ax.Message);

                    Console.ResetColor();
                    return (int)ExitCodes.IncorrectFunction;
                }
            }
            catch (Exception ex)
            {
                {
                    Console.ForegroundColor = ConsoleColor.Red;

                    Console.Error.WriteLine($"Unhandled exception: {ex.Message}");

                    if (ex.InnerException is not null)
                    {
                        Console.Error.WriteLine($"    Inner Exception: {ex.InnerException.Message}");
                    }

                    Console.ResetColor();
                    return (int)ExitCodes.UnhandledException;
                }
            }
        }
    }
}
