// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppCommand.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>17-12-2020 12:52</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace ETAPU11App.Commands
{
    #region Using Directives

    using System;
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using System.CommandLine.IO;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using UtilityLib;
    using UtilityLib.Console;

    using ETAPU11Lib;
    using ETAPU11App.Models;
    using ETAPU11App.Options;

    #endregion

    public sealed class AppCommand : BaseRootCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AppCommand"/> class.
        /// </summary>
        /// <param name="configuration">The configuration instance.</param>
        /// <param name="gateway">The gateway instance.</param>
        /// <param name="options">The root command options.</param>
        /// <param name="infoCommand">The info command instance.</param>
        /// <param name="readCommand">The read command instance.</param>
        /// <param name="monitorCommand">The monitor command instance.</param>
        /// <param name="writeCommand">The write command instance.</param>
        /// <param name="logger">The logger instance.</param>
        public AppCommand(IConfiguration configuration,
                          ETAPU11Gateway gateway,
                          GlobalOptions options,
                          InfoCommand infoCommand,
                          ReadCommand readCommand,
                          MonitorCommand monitorCommand,
                          WriteCommand writeCommand,
                          ILogger<AppCommand> logger)
            : base(options, logger, "Allows to read and write ETAPU11 data using Modbus TCP.")
        {
            logger.LogDebug("AppCommand()");

            // Adding global options to the default global options.
            AddGlobalOption(new Option<string>(
                alias: "--address",
                description: "Sets the Modbus slave IP address.")
                .Default(options.TcpSlave.Address)
                .Name("string")
                .IPAddress()
            );

            AddGlobalOption(new Option<ushort>(
                alias: "--port",
                description: "Sets the Modbus slave port number.")
                .Default(options.TcpSlave.Port)
                .Name("number")
            );

            AddGlobalOption(new Option<byte>(
                alias: "--slaveid",
                description: "Sets the Modbus slave ID.")
                .Default(options.TcpSlave.ID)
                .Name("number")
            );

            // Adding sub commands to root command.
            AddCommand(infoCommand);
            AddCommand(readCommand);
            AddCommand(monitorCommand);
            AddCommand(writeCommand);

            // Get settings from configuration and gateway instance.
            var settings = configuration.GetSection("AppSettings").Get<AppSettings>();

            // Setup execution handler.
            Handler = CommandHandler.Create<IConsole, GlobalOptions>((console, options) =>
            {
                logger.LogDebug("Handler()");

                if (options.Verbose)
                {
                    console.Out.WriteLine($"Commandline Application: {ExecutableName}");
                    console.Out.WriteLine();
                    console.Out.WriteLine($"Configuration: {options.Configuration}");
                    console.Out.WriteLine($"Settings:      {options.Settings}");
                    console.Out.WriteLine($"Verbose:       {options.Verbose}");
                    console.Out.WriteLine($"Slave Address: {options.TcpSlave.Address}");
                    console.Out.WriteLine($"Slave Port:    {options.TcpSlave.Port}");
                    console.Out.WriteLine($"Slave ID:      {options.TcpSlave.ID}");
                    console.Out.WriteLine();
                }

                ShowSettings(console, options, settings);
                ShowConfiguration(console, options, configuration);

                if (gateway.CheckAccess())
                {
                    Console.WriteLine($"Modbus TCP client found at {options.TcpSlave.Address}:{options.TcpSlave.Port}.");
                }
                else
                {
                    Console.WriteLine($"Modbus TCP client not found at {options.TcpSlave.Address}:{options.TcpSlave.Port}.");
                }

                return (int)ExitCodes.SuccessfullyCompleted;
            });
        }

        #endregion Constructors
    }
}
