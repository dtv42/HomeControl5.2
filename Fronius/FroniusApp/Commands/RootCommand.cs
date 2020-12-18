// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RootCommand.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>25-4-2020 18:06</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace FroniusApp.Commands
{
    #region Using Directives

    using System;
    using System.Text.Json;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    using McMaster.Extensions.CommandLineUtils;

    using UtilityLib;
    using FroniusLib;
    using FroniusApp.Models;

    #endregion

    /// <summary>
    /// This is the root command of the application.
    /// </summary>
    [Command(Name = "FroniusApp",
             FullName = "Fronius Application",
             Description = "Allows to read data from a Fronius Symo 8.2-3-M solar inverter.",
             ExtendedHelpText = "\nCopyright (c) 2020 Dr. Peter Trimmel - All rights reserved.")]
    [Subcommand(
        typeof(InfoCommand),
        typeof(ReadCommand),
        typeof(MonitorCommand))]
    public class RootCommand : BaseCommand<RootCommand, AppSettings>
    {
        #region Private Data Members

        private readonly JsonSerializerOptions _options = JsonExtensions.DefaultSerializerOptions;
        private readonly FroniusGateway _gateway;

        #endregion

        #region Public Properties

        [Option("--address <URL>", Description = "Sets the Fronius base address.", Inherited = true)]
        public string BaseAddress { get; } = string.Empty;

        [Option("--timeout <NUMBER>", Description = "Sets the test web service request time out in seconds.", Inherited = true)]
        public int Timeout { get; }

        [Option("--device <ID>", Description = "Sets the Fronius device id.", Inherited = true)]
        public string DeviceID { get; } = string.Empty;

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
        public RootCommand(FroniusGateway gateway,
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
            DeviceID = _settings.DeviceID;

            // Setting the Fronius instance.
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
                // Overriding Fronius data options.
                _settings.BaseAddress = BaseAddress;
                _settings.Timeout = Timeout;
                _settings.DeviceID = DeviceID;
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
                    _console.WriteLine(JsonSerializer.Serialize<AppSettings>(_settings, _options));
                }

                if (_gateway.CheckAccess())
                {
                    Console.WriteLine($"Fronius web service with device ID '{_settings.DeviceID}' found at {BaseAddress}.");
                }
                else
                {
                    Console.WriteLine($"Fronius web service with device ID '{_settings.DeviceID}' not found at {BaseAddress}.");
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
            if (string.IsNullOrEmpty(BaseAddress))
            {
                throw new CommandParsingException(_application, "Missing base address");
            }

            if (!Uri.TryCreate(BaseAddress, UriKind.Absolute, out Uri _))
            {
                throw new CommandParsingException(_application, $"Invalid base address specified.");
            }

            if (Timeout <= 0)
            {
                throw new CommandParsingException(_application, $"Invalid timeout specified: {Timeout}");
            }

            if (string.IsNullOrEmpty(DeviceID))
            {
                throw new CommandParsingException(_application, "Missing Fronius device ID");
            }

            return true;
        }

        #endregion Private Methods
    }
}
