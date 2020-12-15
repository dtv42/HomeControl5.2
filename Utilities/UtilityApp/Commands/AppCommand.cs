// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppCommand.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>2-12-2020 11:03</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace UtilityApp.Commands
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
    using UtilityApp.Options;
    using UtilityApp.Models;

    #endregion Using Directives

    /// <summary>
    /// The application root command. Supports additional global options.
    /// </summary>
    public sealed class AppCommand : BaseRootCommand
    {
        #region Private Data Members

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AppCommand"/> class.
        /// </summary>
        /// <param name="configuration">The configuration instance.</param>
        /// <param name="options">The root command options.</param>
        /// <param name="logger">The logger instance.</param>
        public AppCommand(IConfiguration configuration, GlobalOptions options, ILogger<AppCommand> logger)
            : base(options, logger, "Console app root command.")
        {
            // Adding global options to the default global options.
            AddGlobalOption(new Option<string>(
                alias: "--host",
                description: "Global host option")
                .Default(options.Host)
                .Name("uri")
                .Uri()
            );

            AddGlobalOption(new Option<string>(
                alias: "--password",
                description: "Global password option")
                .Default(options.Password)
                .Name("string")
            );

            Handler = CommandHandler.Create<IConsole, GlobalOptions>((console, options) =>
            {
                logger.LogDebug("Handler()");

                // Get settings from configuration.
                AppSettings settings = new AppSettings();
                configuration.GetSection("AppSettings").Bind(settings);

                if (options.Verbose)
                {
                    console.Out.WriteLine($"Commandline Application: {ExecutableName}");
                    console.Out.WriteLine();
                    console.Out.WriteLine($"Configuration: {options.Configuration}");
                    console.Out.WriteLine($"Settings:      {options.Settings}");
                    console.Out.WriteLine($"Password:      {options.Password}");
                    console.Out.WriteLine($"Verbose:       {options.Verbose}");
                    console.Out.WriteLine($"Host:          {options.Host}");
                    console.Out.WriteLine();
                }

                ShowSettings(console, options, settings);
                ShowConfiguration(console, options, configuration);

                Console.Out.WriteLine("Hello Console!");

                return (int)ExitCodes.SuccessfullyCompleted;
            });
        }

        #endregion Constructors
    }
}
