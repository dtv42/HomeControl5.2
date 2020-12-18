// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RootCommand.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace ETAPU11App.Commands
{
    #region Using Directives

    using System.ComponentModel.DataAnnotations;
    using System.Net;
    using System.Text.Json;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    using McMaster.Extensions.CommandLineUtils;

    using UtilityLib;
    using ETAPU11Lib;
    using ETAPU11App.Models;

    #endregion

    /// <summary>
    /// Root command for the application providing inherited option.
    /// Note that the default value is set from the application settings.
    /// </summary>
    [Command(Name = "ETAPU11App",
             FullName = "ETAPU11 Application",
             Description = "Allows to read and write ETAPU11 data using Modbus TCP.",
             ExtendedHelpText = "\nCopyright (c) 2020 Dr. Peter Trimmel - All rights reserved.")]
    [Subcommand( typeof(InfoCommand))]
    [Subcommand(typeof(ReadCommand))]
    [Subcommand(typeof(WriteCommand))]
    [Subcommand(typeof(MonitorCommand))]
    public class RootCommand : BaseCommand<RootCommand, AppSettings>
    {
        #region Private Data Members

        private readonly JsonSerializerOptions _options = JsonExtensions.DefaultSerializerOptions;
        private readonly ETAPU11Gateway _gateway;

        #endregion

        #region Public Properties

        [Option("--address <IP>", Description = "Sets the Modbus slave IP address.", Inherited = true)]
        [IPAddress]
        public string Address { get; } = string.Empty;

        [Option("--port <NUMBER>", Description = "Sets the Modbus slave port number.", Inherited = true)]
        [Range(0, 65535)]
        public int Port { get; }

        [Option("--slaveid <NUMBER>", Description = "Sets the Modbus slave ID.", Inherited = true)]
        public byte SlaveID { get; }

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
        public RootCommand(ETAPU11Gateway gateway,
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
            Address = _settings.TcpSlave.Address;
            Port = _settings.TcpSlave.Port;
            SlaveID = _settings.TcpSlave.ID;

            // Setting the ETAPU11 instance.
            _gateway = gateway;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Runs when the commandline application command is executed.
        /// </summary>
        /// <returns>The exit code</returns>
        private int OnExecute()
        {
            try
            {
                // Overriding ETAPU11 options.
                _settings.TcpSlave.Address = Address;
                _settings.TcpSlave.Port = Port;
                _settings.TcpSlave.ID = SlaveID;

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
                    _console.WriteLine($"Modbus TCP client found at {Address}:{Port}.");
                }
                else
                {
                    _console.WriteLine($"Modbus TCP client not found at {Address}:{Port}.");
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
            if (string.IsNullOrEmpty(Address))
            {
                throw new CommandParsingException(_application, "Missing IP address");
            }

            return true;
        }

        #endregion
    }
}
