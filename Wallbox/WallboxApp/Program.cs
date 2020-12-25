// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>26-4-2020 10:01</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace WallboxApp
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

    using WallboxLib;
    using WallboxLib.Models;

    using WallboxApp.Commands;
    using WallboxApp.Models;
    using WallboxApp.Options;


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

                        // Configure the singleton Wallbox client instance.
                        services
                            .AddSingleton<IWallboxSettings>(settings.GlobalOptions)
                            .AddSingleton<WallboxClient>()
                            .AddSingleton<WallboxGateway>()

                        // Add command options.
                            .AddSingletonFromSection<GlobalOptions>()
                            .AddSingleton<InfoOptions>()
                            .AddSingleton<ReadOptions>()
                            .AddSingleton<MonitorOptions>()

                            // Add commands.
                            .AddSingleton<InfoCommand>()
                            .AddSingleton<ReadCommand>()
                            .AddSingleton<MonitorCommand>()
                            .AddSingleton<ControlCommand>()

                            // Add sub commands.
                            .AddSingleton<CurrentCommand>()
                            .AddSingleton<EnergyCommand>()
                            .AddSingleton<OutputCommand>()
                            .AddSingleton<StartCommand>()
                            .AddSingleton<StopCommand>()
                            .AddSingleton<DisableCommand>()
                            .AddSingleton<UnlockCommand>()

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
