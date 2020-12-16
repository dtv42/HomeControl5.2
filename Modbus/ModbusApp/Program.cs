namespace ModbusApp
{
    #region Using Directives

    using System;
    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;

    using Serilog;

    using UtilityLib;
    using UtilityLib.Console;

    using ModbusLib;

    using ModbusApp.Commands;
    using ModbusApp.Options;
    using System.CommandLine;
    using ModbusLib.Models;

    #endregion Using Directives

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

                })
                .ConfigureServices((context, services) =>
                {
                    services
                    // Configure the singleton Modbus client instances.
                        .AddSingleton<ITcpModbusClient, TcpModbusClient>()
                        .AddSingleton<IRtuModbusClient, RtuModbusClient>()

                    // Add command options.
                        .AddSingletonFromSection<IRtuClientSettings, RtuCommandOptions>("AppSettings:RtuOptions")
                        .AddSingleton<RtuMonitorCommandOptions>()
                        .AddSingleton<RtuReadCommandOptions>()
                        .AddSingleton<RtuWriteCommandOptions>()
                        .AddSingletonFromSection<ITcpClientSettings, TcpCommandOptions>("AppSettings:TcpOptions")
                        .AddSingleton<TcpMonitorCommandOptions>()
                        .AddSingleton<TcpReadCommandOptions>()
                        .AddSingleton<TcpWriteCommandOptions>()
                        .AddSingleton<GlobalOptions>()

                    // Add commands.
                        .AddSingleton<RtuMonitorCommand>()
                        .AddSingleton<RtuReadCommand>()
                        .AddSingleton<RtuWriteCommand>()
                        .AddSingleton<RtuCommand>()
                        .AddSingleton<TcpMonitorCommand>()
                        .AddSingleton<TcpReadCommand>()
                        .AddSingleton<TcpWriteCommand>()
                        .AddSingleton<TcpCommand>()

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
