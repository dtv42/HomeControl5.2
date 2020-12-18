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

    using System.Net;
    using System.Text.Json;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using McMaster.Extensions.CommandLineUtils;

    using UtilityLib;
    using WallboxLib;
    using WallboxApp.Models;
    using System.ComponentModel.DataAnnotations;
    using System;

    #endregion

    /// <summary>
    /// This is the root command of the application.
    /// </summary>
    [Command(Name = "WallboxApp",
             FullName = "Wallbox Application",
             Description = "Allows to access a BMW Wallbox charging station.",
             ExtendedHelpText = "\nCopyright (c) 2020 Dr. Peter Trimmel - All rights reserved.")]
    [Subcommand(
        typeof(InfoCommand),
        typeof(ReadCommand),
        typeof(ControlCommand),
        typeof(MonitorCommand))]
    public class RootCommand : BaseCommand<RootCommand, AppSettings>
    {
        #region Private Data Members

        private readonly JsonSerializerOptions _options = JsonExtensions.DefaultSerializerOptions;
        private readonly WallboxGateway _gateway;

        #endregion

        #region Public Properties

        [Option("--endpoint <ENDPOINT>", Description = "Sets the Wallbox IP endpoint.", Inherited = true)]
        [IPAddress]
        public string EndPoint { get; set; } = string.Empty;

        [Option("--port <NUMBER>", Description = "Sets the Wallbox port number.", Inherited = true)]
        [Range(0, 65535)]
        public int Port { get; set; }

        [Option("--timeout <NUMBER>", Description = "Sets the Wallbox receive timeout.", Inherited = true)]
        [Range(0, Int32.MaxValue)]
        public double Timeout { get; set; }

        [Option("--verbose", Inherited = true, Description = "Verbose output...")]
        public bool Verbose { get; }

        [Option("--config", Inherited = true, Description = "Show configuration...")]
        public bool ShowConfig { get; }

        [Option("--settings", Description = "Show settings.", Inherited = true)]
        public bool ShowSettings { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RootCommand"/> class.
        /// The RootCommand sets default values for some properties using the application settings.
        /// </summary>
        /// <param name="gateway"></param>
        /// <param name="console"></param>
        /// <param name="settings"></param>
        /// <param name="config"></param>
        /// <param name="environment"></param>
        /// <param name="lifetime"></param>
        /// <param name="logger"></param>
        /// <param name="application"></param>
        public RootCommand(WallboxGateway gateway,
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
            EndPoint = _settings.EndPoint;
            Port = _settings.Port;
            Timeout = _settings.Timeout;

            // Setting the Wallbox instance.
            _gateway = gateway;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Runs when the commandline application command is executed.
        /// </summary>
        /// <returns>The exit code</returns>
        public int OnExecute()
        {
            try
            {
                // Overriding Wallbox data options.
                _settings.EndPoint = EndPoint;
                _settings.Port = Port;
                _settings.Timeout = Timeout;

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
                    _console.WriteLine(JsonSerializer.Serialize<AppSettings>(_settings, _options));
                }

                if (_gateway.CheckAccess())
                {
                    _console.WriteLine($"Wallbox UDP service with firmware '{_gateway.Info.Firmware}' found at {EndPoint}.");
                }
                else
                {
                    _console.WriteLine($"Wallbox UDP service not found at {EndPoint}.");
                }
            }
            catch
            {
                _logger.LogError("RootCommand exception");
                throw;
            }

            return ExitCodes.SuccessfullyCompleted;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Helper method to check options.
        /// </summary>
        /// <returns>True if options are OK.</returns>
        public override bool CheckOptions()
        {
            if (string.IsNullOrEmpty(EndPoint))
            {
                throw new CommandParsingException(_application, "Missing IP endpoint");
            }

            return true;
        }

        #endregion
    }
}
