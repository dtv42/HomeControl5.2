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

    using UtilityLib;
    using UtilityApp.Models;

    #endregion Using Directives

    public class AppCommand : BaseRootCommand
    {
        private readonly IConfiguration _configuration;

        public AppCommand(IConfiguration configuration, GlobalOptions options, ILogger<AppCommand> logger) : base(options, logger, "Console app root command.")
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

            AddOption(new Option<bool>(
                alias: "--logging",
                description: "Command logging option")
            );

            AddOption(new Option<bool>(
                alias: "--configuration",
                description: "Command show configuration")
            );

            Handler = CommandHandler.Create((bool logging, bool configuration, GlobalOptions options) => HandleCommand(logging, configuration, options));

            _configuration = configuration;
        }

        private int HandleCommand(bool logging, bool configuration, GlobalOptions options)
        {
            try
            {
                Program.LevelSwitch.MinimumLevel = options.LogLevel;

                if (options.Settings)
                {
                    var serializerOptions = new JsonSerializerOptions { WriteIndented = true };
                    Console.WriteLine($"AppSettings: {JsonSerializer.Serialize(Program.Settings, serializerOptions)}");
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

                if (configuration)
                {
                    var serializerOptions = new JsonSerializerOptions { WriteIndented = true };
                    Console.WriteLine($"Configuration: {JsonSerializer.Serialize(_configuration.AsEnumerable(), serializerOptions)}");
                }

                if (options.Verbose)
                {
                    Console.WriteLine($"Password: {options.Password}");
                    Console.WriteLine($"Verbose:  {options.Verbose}");
                    Console.WriteLine($"LogLevel: {options.LogLevel}");
                    Console.WriteLine($"Uri:      {options.Uri}");
                    Console.WriteLine($"Logging:  {logging}");
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
