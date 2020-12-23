// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RootCommand.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>26-4-2020 10:05</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace HeliosApp.Commands
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

    using HeliosLib;
    using HeliosApp.Models;
    using HeliosApp.Options;

    #endregion

    /// <summary>
    /// This is the root command of the application.
    /// </summary>
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
                          HeliosGateway gateway,
                          GlobalOptions options,
                          InfoCommand infoCommand,
                          ReadCommand readCommand,
                          MonitorCommand monitorCommand,
                          ControlCommand controlCommand,
                          ILogger<AppCommand> logger)
            : base(options, logger, "Allows to read data from a Helios KWL EC 200 ventilation system.")
        {
            logger.LogDebug("AppCommand()");

            // Adding global options to the default global options.
            AddGlobalOption(new Option<string>(
                alias: "--address",
                description: "Global address option")
                .Default(options.Address)
                .Name("uri")
                .Uri()
            );

            AddGlobalOption(new Option<string>(
                alias: "--timeout",
                description: "Global timeout option")
                .Default(options.Timeout)
                .Name("number")
            );

            AddGlobalOption(new Option<string>(
                alias: "--password",
                description: "Global password option")
                .Default(options.Password)
                .Name("string")
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
                    console.Out.WriteLine($"Password:      {options.Password}");
                    console.Out.WriteLine($"Address:       {options.Address}");
                    console.Out.WriteLine($"Timeout:       {options.Timeout}");
                    console.Out.WriteLine();
                }

                ShowSettings(console, options, settings);
                ShowConfiguration(console, options, configuration);

                if (gateway.CheckAccess())
                {
                    Console.WriteLine($"Helios web service found at {options.Address}.");
                }
                else
                {
                    Console.WriteLine($"Helios web service not found at {options.Address}.");
                }

                return (int)ExitCodes.SuccessfullyCompleted;
            });
        }

        #endregion Constructors
    }
}
