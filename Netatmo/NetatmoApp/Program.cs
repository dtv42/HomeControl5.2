namespace NetatmoApp
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.CommandLine;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using Serilog;

    using UtilityLib;
    using UtilityLib.Console;
    using UtilityLib.Webapp;

    using NetatmoLib;
    using NetatmoLib.Models;

    using NetatmoApp.Commands;
    using NetatmoApp.Models;
    using NetatmoApp.Options;

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

                        // Configure the singleton Netatmo client instance.
                        services
                            .AddPollyHttpClient<NetatmoClient>("NetatmoClient",
                                new List<TimeSpan>
                                {
                                    TimeSpan.FromSeconds(10),
                                    TimeSpan.FromSeconds(20),
                                    TimeSpan.FromSeconds(30)
                                },
                                client =>
                                {
                                    client.BaseAddress = new Uri(settings.GlobalOptions.Address);
                                    client.Timeout = TimeSpan.FromMilliseconds(settings.GlobalOptions.Timeout);
                                });

                        // Add single gateway.
                        services
                            .AddSingleton<INetatmoSettings>(settings.GlobalOptions)
                            .AddSingleton<NetatmoGateway>()

                        // Add command options.
                            .AddSingletonFromSection<GlobalOptions>()
                            .AddSingleton<InfoOptions>()
                            .AddSingleton<ReadOptions>()
                            .AddSingleton<MonitorOptions>()

                            // Add commands.
                            .AddSingleton<InfoCommand>()
                            .AddSingleton<MonitorCommand>()
                            .AddSingleton<ReadCommand>()

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
