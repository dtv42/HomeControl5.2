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
    using System.Text.Json;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using UtilityLib;
    using UtilityApp.Options;

    #endregion Using Directives

    /// <summary>
    /// The application root command.
    /// </summary>
    public sealed class AppCommand : BaseRootCommand
    {
        #region Private Data Members

        /// <summary>
        /// 
        /// </summary>
        private readonly JsonSerializerOptions _jsonoptions = JsonExtensions.DefaultSerializerOptions;

        /// <summary>
        /// The application configuration instance.
        /// </summary>
        private readonly IConfiguration _configuration;

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
            AddGlobalOption(new Option<Uri>(
                alias: "--uri",
                description: "Global URI option")
                .Default(options.Uri)
                .Name("URI")
                .Uri()
            );

            AddGlobalOption(new Option<string>(
                alias: "--password",
                description: "Global password option")
                .Default(options.Password)
                .Name("STRING")
            );

            Handler = CommandHandler.Create<IConsole, GlobalOptions>((console, options) =>
            {
                logger.LogInformation("Handler()");

                if (options.Settings)
                {
                    console.Out.WriteLine($"AppSettings: {JsonSerializer.Serialize(Program.Settings, _jsonoptions)}");
                }

                if (options.Configuration)
                {
                    console.Out.WriteLine($"Configuration: {JsonSerializer.Serialize(_configuration.AsEnumerable(), _jsonoptions)}");
                }

                if (options.Verbose)
                {
                    console.Out.WriteLine($"Commandline Application: {ExecutableName}");
                    console.Out.WriteLine();
                    console.Out.WriteLine($"Password: {options.Password}");
                    console.Out.WriteLine($"Verbose:  {options.Verbose}");
                    console.Out.WriteLine($"Uri:      {options.Uri}");
                }

                Console.Out.WriteLine("Hello Console!");
            });

            _configuration = configuration;
        }

        #endregion Constructors
    }
}
