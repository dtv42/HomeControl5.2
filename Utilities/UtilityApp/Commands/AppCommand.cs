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
    using System.Text.Json;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using Serilog.Core;
    using Serilog.Events;

    using UtilityLib;
    using UtilityApp.Models;
    using System.Collections.Generic;

    #endregion

    public class AppCommand : RootCommand
    {
        private readonly IConfiguration _config;
        private readonly ILogger<AppCommand> _logger;
        private readonly LoggingLevelSwitch _levelSwitch;

        public AppCommand(IConfiguration config, ILogger<AppCommand> logger, LoggingLevelSwitch levelSwitch, GlobalOptions options) : base("Console app root command.")
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

            AddGlobalOption(new Option<bool>(
                alias: "--verbose",
                description: "Global verbose option")
                .Default(options.Verbose)
            );

            AddGlobalOption(new Option<LogEventLevel>(
                alias: "--loglevel",
                description: "Global log level option")
                .Default(options.LogLevel)
            );

            AddOption(new Option<bool>(
                alias: "--logging",
                description: "Command logging option")
            );

            AddOption(new Option<bool>(
                alias: "--config",
                description: "Command config option")
            );

            Handler = CommandHandler.Create((bool logging, bool config, GlobalOptions options) => HandleCommand(logging, config, options));

            _config = config;
            _logger = logger;
            _levelSwitch = levelSwitch;
        }

        private int HandleCommand(bool logging, bool config, GlobalOptions options)
        {
            try
            {
                _levelSwitch.MinimumLevel = options.LogLevel;

                if (options.Verbose)
                {
                    Console.WriteLine($"Password: {options.Password}");
                    Console.WriteLine($"Verbose:  {options.Verbose}");
                    Console.WriteLine($"LogLevel: {options.LogLevel}");
                    Console.WriteLine($"Uri:      {options.Uri}");
                    Console.WriteLine($"Logging:  {logging}");
                }

                if (config)
                {
                    var serializerOptions = new JsonSerializerOptions { WriteIndented = true };
                    Console.WriteLine($"Configuration: {JsonSerializer.Serialize(_config.AsEnumerable(), serializerOptions)}");
                }

                if (logging)
                {
                    _logger.LogInformation("Logging information.");
                    _logger.LogCritical("Logging critical information.");
                    _logger.LogDebug("Logging debug information.");
                    _logger.LogError("Logging error information.");
                    _logger.LogTrace("Logging trace");
                    _logger.LogWarning("Logging warning.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }

            return 0;
        }
    }
}
