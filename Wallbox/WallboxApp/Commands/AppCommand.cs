// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RootCommand.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>26-4-2020 20:18</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace WallboxApp.Commands
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

    using WallboxLib;
    using WallboxApp.Models;
    using WallboxApp.Options;

    #endregion

    public class AppCommand : BaseRootCommand
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
        /// <param name="controlCommand">The control command instance.</param>
        /// <param name="logger">The logger instance.</param>
        public AppCommand(IConfiguration configuration,
                          WallboxGateway gateway,
                          GlobalOptions options,
                          InfoCommand infoCommand,
                          ReadCommand readCommand,
                          MonitorCommand monitorCommand,
                          ControlCommand controlCommand,
                          ILogger<AppCommand> logger)
            : base(options, logger, "Allows to access a BMW Wallbox charging station.")
        {
            logger.LogDebug("AppCommand()");

            // Adding global options to the default global options.
            AddGlobalOption(new Option<string>(
                alias: "--endpoint",
                description: "Global endpoint option")
                .Default(options.EndPoint)
                .Name("string")
                .IPEndpoint()
            );

            AddGlobalOption(new Option<int>(
                alias: "--port",
                description: "Global port option")
                .Default(options.Port)
                .Name("number")
            );

            AddGlobalOption(new Option<string>(
                alias: "--timeout",
                description: "Global timeout option")
                .Default(options.Timeout)
                .Name("number")
            );


            // Adding sub commands to root command.
            AddCommand(infoCommand);
            AddCommand(readCommand);
            AddCommand(monitorCommand);
            AddCommand(controlCommand);

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
                    console.Out.WriteLine($"Endpoint:      {options.EndPoint}");
                    console.Out.WriteLine($"Port:          {options.Port}");
                    console.Out.WriteLine($"Timeout:       {options.Timeout}");
                    console.Out.WriteLine();
                }

                ShowSettings(console, options, settings);
                ShowConfiguration(console, options, configuration);

                if (gateway.CheckAccess())
                {
                    Console.WriteLine($"Wallbox UDP service with firmware '{gateway.Info.Firmware}' found at {options.EndPoint}.");
                }
                else
                {
                    Console.WriteLine($"Wallbox UDP service not found at {options.EndPoint}.");
                }

                return (int)ExitCodes.SuccessfullyCompleted;
            });
        }

        #endregion Constructors
    }
}
