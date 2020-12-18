// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WriteCommand.cs" company="DTV-Online">
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
    using System.Text.Json;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    using McMaster.Extensions.CommandLineUtils;

    using UtilityLib;
    using ETAPU11Lib;
    using ETAPU11Lib.Models;
    using ETAPU11App.Models;

    #endregion

    /// <summary>
    /// Application command "write".
    /// </summary>
    [Command(Name = "write",
             FullName = "ETAPU11 Write Command",
             Description = "Writing data values to ETA PU 11 pellet boiler.",
             ExtendedHelpText = "\nCopyright (c) 2020 Dr. Peter Trimmel - All rights reserved.")]
    public class WriteCommand : BaseCommand<WriteCommand, AppSettings>
    {
        #region Private Data Members

        private readonly JsonSerializerOptions _options = JsonExtensions.DefaultSerializerOptions;
        private readonly ETAPU11Gateway _gateway;

        #endregion

        #region Private Properties

        /// <summary>
        /// This is a reference to the parent command <see cref="RootCommand"/>.
        /// </summary>
        private RootCommand? Parent { get; }

        #endregion

        #region Public Properties

        [Option("-d|--data", Description = "Writes ETAPU11 data.")]
        public bool Data { get; }

        [Option("-b|--boiler", Description = "Writes boiler data.")]
        public bool Boiler { get; }

        [Option("-w|--hotwater", Description = "Writes hot water data.")]
        public bool Hotwater { get; }

        [Option("-h|--heating", Description = "Writes heating circuit data.")]
        public bool Heating { get; }

        [Option("-s|--storage", Description = "Writes storage data.")]
        public bool Storage { get; }

        [Option("-y|--system", Description = "Writes system data.")]
        public bool System { get; }

        [Required]
        [Argument(0, Description = "Property name (required)")]
        public string Property { get; } = string.Empty;

        [Required]
        [Argument(1, Description = "Property value (required)")]
        public string Value { get; } = string.Empty;

        [Option("--status", Description = "Shows the data status.")]
        public bool Status { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WriteCommand"/> class.
        /// </summary>
        /// <param name="gateway"></param>
        /// <param name="console"></param>
        /// <param name="settings"></param>
        /// <param name="config"></param>
        /// <param name="environment"></param>
        /// <param name="lifetime"></param>
        /// <param name="logger"></param>
        /// <param name="application"></param>
        public WriteCommand(ETAPU11Gateway gateway,
                            IConsole console,
                            AppSettings settings,
                            IConfiguration config,
                            IHostEnvironment environment,
                            IHostApplicationLifetime lifetime,
                            ILogger<WriteCommand> logger,
                            CommandLineApplication application)
            : base(console, settings, config, environment, lifetime, logger, application)
        {
            _logger?.LogDebug("WriteCommand()");

            // Setting the ETAPU11 instance.
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
                if (!(Parent is null))
                {
                    // Overriding ETAPU11 options.
                    _gateway.Settings.TcpSlave.Address = Parent.Address;
                    _gateway.Settings.TcpSlave.Port = Parent.Port;
                    _gateway.Settings.TcpSlave.ID = Parent.SlaveID;
                    _gateway.UpdateClient();
                }

                if (Parent?.ShowSettings ?? false)
                {
                    _console.WriteLine(JsonSerializer.Serialize<AppSettings>(_settings, _options));
                }

                _console.WriteLine($"Writing value '{Value}' to property '{Property}' at ETAPU11 pellet boiler");
                var status = _gateway.WriteProperty(Property, Value);

                if (!status.IsGood)
                {
                    _console.WriteLine($"Error writing property '{Property}' to ETAPU11 pellet boiler.");
                }

                if (Status)
                {
                    _console.WriteLine($"Status:");
                    _console.WriteLine(JsonSerializer.Serialize<DataStatus>(_gateway.Status, _options));
                }
            }
            catch
            {
                _logger.LogError("WriteCommand exception");
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
            if (Parent?.CheckOptions() ?? false)
            {
                int options = 0;

                if (Data) ++options;
                if (Boiler) ++options;
                if (Hotwater) ++options;
                if (Heating) ++options;
                if (Storage) ++options;
                if (System) ++options;

                if (options != 1)
                {
                    _console.WriteLine("Please specifiy a single data option");
                    return false;
                }

                if (!string.IsNullOrEmpty(Property))
                {
                    if (!string.IsNullOrEmpty(Property))
                    {
                        if (Data)
                        {
                            if (!typeof(ETAPU11Data).IsProperty(Property))
                            {
                                _logger?.LogError($"The property '{Property}' has not been found.");
                                return false;
                            }
                        }

                        if (Boiler)
                        {
                            if (!typeof(BoilerData).IsProperty(Property))
                            {
                                _logger?.LogError($"The property '{Property}' has not been found.");
                                return false;
                            }
                        }

                        if (Hotwater)
                        {
                            if (!typeof(HotwaterData).IsProperty(Property))
                            {
                                _logger?.LogError($"The property '{Property}' has not been found.");
                                return false;
                            }
                        }

                        if (Heating)
                        {
                            if (!typeof(HeatingData).IsProperty(Property))
                            {
                                _logger?.LogError($"The property '{Property}' has not been found.");
                                return false;
                            }
                        }

                        if (Storage)
                        {
                            if (!typeof(StorageData).IsProperty(Property))
                            {
                                _logger?.LogError($"The property '{Property}' has not been found.");
                                return false;
                            }
                        }

                        if (System)
                        {
                            if (!typeof(SystemData).IsProperty(Property))
                            {
                                _logger?.LogError($"The property '{Property}' has not been found.");
                                return false;
                            }
                        }
                    }

                    if (!ETAPU11Data.IsWritable(Property))
                    {
                        _logger?.LogError($"The property '{Property}' is not writable.");
                        return false;
                    }
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
