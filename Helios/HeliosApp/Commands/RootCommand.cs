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
    using System.Text.Json;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    using McMaster.Extensions.CommandLineUtils;

    using UtilityLib;
    using HeliosLib;
    using HeliosApp.Models;

    #endregion

    /// <summary>
    /// This is the root command of the application.
    /// </summary>
    [Command(Name = "HeliosApp",
             FullName = "Helios Application",
             Description = "Allows to read data from a Helios KWL EC 200 ventilation system.",
             ExtendedHelpText = "\nCopyright (c) 2020 Dr. Peter Trimmel - All rights reserved.")]
    [Subcommand(typeof(InfoCommand))]
    [Subcommand(typeof(ReadCommand))]
    [Subcommand(typeof(MonitorCommand))]
    [Subcommand(typeof(ControlCommand))]
    public class RootCommand : BaseCommand<RootCommand, AppSettings>
    {
        #region Private Data Members

        private readonly JsonSerializerOptions _options = JsonExtensions.DefaultSerializerOptions;
        private readonly HeliosGateway _gateway;

        #endregion

        #region Public Properties

        [Option("--address <URL>", Description = "Sets the Helios base address.", Inherited = true)]
        public string BaseAddress { get; } = string.Empty;

        [Option("--timeout <NUMBER>", Description = "Sets the test web service request time out in seconds.", Inherited = true)]
        public int Timeout { get; }

        [Option("--password <STRING>", Description = "Sets the Helios password.", Inherited = true)]
        public string Password { get; } = string.Empty;

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
        public RootCommand(HeliosGateway gateway,
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

            // Setting the Helios instance.
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
                // Overriding Helios data options.
                _settings.BaseAddress = BaseAddress;
                _settings.Timeout = Timeout;
                _settings.Password = Password;
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
                    Console.WriteLine($"Helios web service found at {BaseAddress}.");
                }
                else
                {
                    Console.WriteLine($"Helios web service not found at {BaseAddress}.");
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

            if (string.IsNullOrEmpty(Password))
            {
                throw new CommandParsingException(_application, "Missing Helios password");
            }

            return true;
        }

        #endregion Private Methods
    }
}
