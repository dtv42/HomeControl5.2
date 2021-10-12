// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppCommand.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>19-12-2020 17:50</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace NetatmoApp.Commands
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

    using NetatmoLib;
    using NetatmoLib.Models;

    using NetatmoApp.Options;
    using NetatmoApp.Models;

    #endregion Using Directives

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
        /// <param name="logger">The logger instance.</param>
        public AppCommand(IConfiguration configuration,
                          NetatmoGateway gateway,
                          GlobalOptions options,
                          InfoCommand infoCommand,
                          ReadCommand readCommand,
                          MonitorCommand monitorCommand,
                          ILogger<AppCommand> logger)
            : base(options, logger, "Allows to read data from the Netatmo web service.")
        {
            logger.LogDebug("AppCommand()");

            // Get settings from configuration.
            var settings = configuration.GetSection("AppSettings").Get<AppSettings>();

            // Adding global options to the default global options.
            AddGlobalOption(new Option<string>(
                alias: "--address",
                description: "Global address option")
                .Default(settings.GlobalOptions.Address)
                .Name("uri")
                .Uri()
            );

            AddGlobalOption(new Option<string>(
                alias: "--timeout",
                description: "Global timeout option")
                .Default(settings.GlobalOptions.Timeout)
                .Name("number")
            );

            AddGlobalOption(new Option<string>(
                alias: "--user",
                description: "Global user option")
                .Default(settings.GlobalOptions.User)
                .Name("string")
            );

            AddGlobalOption(new Option<string>(
                alias: "--password",
                description: "Global password option")
                .Default(settings.GlobalOptions.Password)
                .Name("string")
            );

            AddGlobalOption(new Option<string>(
                alias: "--clientid",
                description: "Global client ID option")
                .Default(settings.GlobalOptions.ClientID)
                .Name("string")
            );

            AddGlobalOption(new Option<string>(
                alias: "--clientsecret",
                description: "Global client secret option")
                .Default(settings.GlobalOptions.ClientSecret)
                .Name("string")
            );

            // Adding sub commands to root command.
            AddCommand(infoCommand);
            AddCommand(readCommand);
            AddCommand(monitorCommand);

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
                    console.Out.WriteLine($"User:          {options.User}");
                    console.Out.WriteLine($"Password:      {options.Password}");
                    console.Out.WriteLine($"ClientID:      {options.ClientID}");
                    console.Out.WriteLine($"ClientSecret:  {options.ClientSecret}");
                    console.Out.WriteLine($"Address:       {options.Address}");
                    console.Out.WriteLine($"Timeout:       {options.Timeout}");
                    console.Out.WriteLine();
                }

                ShowSettings(console, options, configuration.GetSection("AppSettings").Get<AppSettings>());
                ShowConfiguration(console, options, configuration);

                // Update settings with options.
                gateway.Settings.Address      = options.Address;
                gateway.Settings.Timeout      = options.Timeout;
                gateway.Settings.User         = options.User;
                gateway.Settings.Password     = options.Password;
                gateway.Settings.ClientID     = options.ClientID;
                gateway.Settings.ClientSecret = options.ClientSecret;

                if (gateway.CheckAccess())
                {
                    Console.WriteLine($"Netatmo web service found at {options.Address}.");
                }
                else
                {
                    Console.WriteLine($"Netatmo web service not found at {options.Address}.");
                }

                return (int)ExitCodes.SuccessfullyCompleted;
            });
        }

        #endregion Constructors
    }
}
