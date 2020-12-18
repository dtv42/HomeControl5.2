// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RootCommand.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>22-4-2020 17:19</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace EM300LRApp.Commands
{
    #region Using Directives

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    using McMaster.Extensions.CommandLineUtils;

    using UtilityLib;
    using EM300LRLib;
    using EM300LRApp.Models;

    #endregion Using Directives

    /// <summary>
    /// This is the root command of the application.
    /// </summary>
    [Command(Name = "EM300LRApp",
             FullName = "EM300LR Application",
             Description = "Allows to read data from a b-Control EM300LR energy manager..",
             ExtendedHelpText = "\nCopyright (c) 2020 Dr. Peter Trimmel - All rights reserved.")]
    [Subcommand(typeof(InfoCommand))]
    [Subcommand(typeof(ReadCommand))]
    [Subcommand(typeof(MonitorCommand))]
    public class RootCommand : BaseCommand<RootCommand, AppSettings>
    {
        #region Private Data Members

        private readonly JsonSerializerOptions _options = JsonExtensions.DefaultSerializerOptions;
        private readonly EM300LRGateway _gateway;

        #endregion Private Data Members

        #region Public Properties

        [Option("--address <URL>", Description = "Sets the EM300LR base address.", Inherited = true)]
        [AbsoluteUri]
        public string BaseAddress { get; } = string.Empty;

        [Option("--timeout <NUMBER>", Description = "Sets the test web service request time out in seconds.", Inherited = true)]
        [Range(0, Int32.MaxValue)]
        public int Timeout { get; }

        [Option("--password <STRING>", Description = "Sets the EM300LR password.", Inherited = true)]
        public string Password { get; } = string.Empty;

        [Option("--serialnumber <STRING>", Description = "Sets the EM300LR serial number.", Inherited = true)]
        public string SerialNumber { get; } = string.Empty;

        [Option("--verbose", Inherited = true, Description = "Verbose output...")]
        public bool Verbose { get; }

        [Option("--config", Inherited = true, Description = "Show configuration...")]
        public bool ShowConfig { get; }

        [Option("--settings", Description = "Show settings.", Inherited = true)]
        public bool ShowSettings { get; }

        #endregion Public Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RootCommand"/> class.
        /// Selected properties are initialized with data from the AppSettings instance.
        /// </summary>
        /// <param name="gateway"></param>
        /// <param name="console"></param>
        /// <param name="settings"></param>
        /// <param name="config"></param>
        /// <param name="environment"></param>
        /// <param name="lifetime"></param>
        /// <param name="logger"></param>
        /// <param name="application"></param>
        public RootCommand(EM300LRGateway gateway, 
                           IConsole console,
                           AppSettings settings,
                           IConfiguration config,
                           IHostEnvironment environment,
                           IHostApplicationLifetime lifetime,
                           ILogger<RootCommand> logger,
                           CommandLineApplication application)
            : base(console, settings, config, environment, lifetime, logger, application)
        {
            _logger?.LogDebug("RootCommand()");

            // Setting default options from appsettings.json file.
            BaseAddress = _settings.BaseAddress;
            Timeout = _settings.Timeout;
            Password = _settings.Password;
            SerialNumber = _settings.SerialNumber;

            // Setting the EM300LR instance.
            _gateway = gateway;
        }

        #endregion Constructors

        #region Private Methods

        /// <summary>
        /// Runs when the commandline application command is executed.
        /// </summary>
        /// <returns>The exit code</returns>
        private int OnExecute()
        {
            try
            {
                // Overriding EM300LR options.
                _settings.BaseAddress = BaseAddress;
                _settings.Timeout = Timeout;
                _settings.Password = Password;
                _settings.SerialNumber = SerialNumber;
                _gateway.UpdateClient();

                if (Verbose)
                {
                    _console.WriteLine($"Commandline application: {_application.Name}");
                    _console.WriteLine($"Console Log level: {CommandLineHost.ConsoleSwitch.MinimumLevel}");
                    _console.WriteLine($"File Log level: {CommandLineHost.FileSwitch.MinimumLevel}");
                }

                if (ShowConfig)
                {
                    _console.WriteLine("Configuration:");

                    foreach (var item in _config.AsEnumerable())
                    {
                        _console.WriteLine($"    {item.Key}: {item.Value}");
                    }

                    _console.WriteLine();
                }

                if (ShowSettings)
                {
                    _console.WriteLine($"AppSettings: {JsonSerializer.Serialize(_settings, _options)}");
                    _console.WriteLine();
                }

                if (_gateway.CheckAccess())
                {
                    Console.WriteLine($"EM300LR web service with serial number '{_settings.SerialNumber}' found at {BaseAddress}.");
                }
                else
                {
                    Console.WriteLine($"EM300LR web service with serial number '{_settings.SerialNumber}' not found at {BaseAddress}.");
                }
            }
            catch
            {
                _logger.LogError("RootCommand exception");
                throw;
            }

            return ExitCodes.SuccessfullyCompleted;
        }

        /// <summary>
        /// Helper method to check options.
        /// </summary>
        /// <returns>True if options are OK.</returns>
        public override bool CheckOptions()
        {
            if (string.IsNullOrEmpty(BaseAddress))
            {
                throw new CommandParsingException(_application, "Missing base address");
            }

            if (string.IsNullOrEmpty(Password))
            {
                throw new CommandParsingException(_application, "Missing EM300LR password");
            }

            if (string.IsNullOrEmpty(SerialNumber))
            {
                throw new CommandParsingException(_application, "Missing EM300LR serial number");
            }

            return true;
        }

        #endregion Private Methods
    }
}