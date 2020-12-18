// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadCommand.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace ETAPU11App.Commands
{
    #region Using Directives

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
    /// Application command "read".
    /// </summary>
    [Command(Name = "read",
             FullName = "ETAPU11 Read Command",
             Description = "Reading data values from ETA PU 11 pellet boiler.",
             ExtendedHelpText = "\nCopyright (c) 2020 Dr. Peter Trimmel - All rights reserved.")]
    public class ReadCommand : BaseCommand<ReadCommand, AppSettings>
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

        [Option("-d|--data", Description = "Reads all data.")]
        public bool Data { get; }

        [Option("-b|--boiler", Description = "Reads the boiler data.")]
        public bool Boiler { get; }

        [Option("-w|--hotwater", Description = "Reads the hot water data.")]
        public bool Hotwater { get; }

        [Option("-h|--heating", Description = "Reads the heating circuit data.")]
        public bool Heating { get; }

        [Option("-s|--storage", Description = "Reads the storage data.")]
        public bool Storage { get; }

        [Option("-y|--system", Description = "Reads the system data.")]
        public bool System { get; }

        [Argument(0, Description = "Reads the named property.")]
        public string Property { get; } = string.Empty;

        [Option("--block", Description = "Using block mode read (only when reading all data).")]
        public bool Block { get; }

        [Option("--status", Description = "Shows the data status.")]
        public bool Status { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadCommand"/> class.
        /// </summary>
        /// <param name="gateway"></param>
        /// <param name="console"></param>
        /// <param name="settings"></param>
        /// <param name="config"></param>
        /// <param name="environment"></param>
        /// <param name="lifetime"></param>
        /// <param name="logger"></param>
        /// <param name="application"></param>
        public ReadCommand(ETAPU11Gateway gateway,
                           IConsole console,
                           AppSettings settings,
                           IConfiguration config,
                           IHostEnvironment environment,
                           IHostApplicationLifetime lifetime,
                           ILogger<ReadCommand> logger,
                           CommandLineApplication application)
            : base(console, settings, config, environment, lifetime, logger, application)
        {
            _logger?.LogDebug("ReadCommand()");

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

                    if (Parent.ShowSettings)
                    {
                        _console.WriteLine(JsonSerializer.Serialize<AppSettings>(_settings, _options));
                    }
                }

                if (string.IsNullOrEmpty(Property))
                {
                    if (Data)
                    {
                        _console.WriteLine($"Reading all data from ETAPU11 pellet boiler.");
                        DataStatus status;

                        if (Block)
                        {
                            status = _gateway.ReadBlockAll();
                        }
                        else
                        {
                            status = _gateway.ReadAll();
                        }

                        if (status.IsGood)
                        {
                            // Fix: Json Serializer cannot serialize NaN
                            if (double.IsNaN(_gateway.Data.Flow)) _gateway.Data.Flow = 0;
                            if (double.IsNaN(_gateway.Data.ResidualO2)) _gateway.Data.ResidualO2 = 0;

                            _console.WriteLine(JsonSerializer.Serialize<ETAPU11Data>(_gateway.Data, _options));
                        }
                        else
                        {
                            _console.WriteLine($"Error reading data from ETAPU11 pellet boiler.");
                        }
                    }
                    
                    if (Boiler)
                    {
                        _console.WriteLine($"Reading boiler data from ETAPU11 pellet boiler.");
                        DataStatus status = _gateway.ReadBoilerData();

                        if (status.IsGood)
                        {
                            // Fix: Json Serializer cannot serialize NaN
                            if (double.IsNaN(_gateway.Data.ResidualO2)) _gateway.Data.ResidualO2 = 0;

                            _console.WriteLine(JsonSerializer.Serialize<BoilerData>(_gateway.BoilerData, _options));
                        }
                        else
                        {
                            _console.WriteLine($"Error reading boiler data from ETAPU11 pellet boiler.");
                        }
                    }
                    
                    if (Hotwater)
                    {
                        _console.WriteLine($"Reading hotwater data from ETAPU11 pellet boiler.");
                        DataStatus status = _gateway.ReadHotwaterData();

                        if (status.IsGood)
                        {
                            _console.WriteLine(JsonSerializer.Serialize<HotwaterData>(_gateway.HotwaterData, _options));
                        }
                        else
                        {
                            _console.WriteLine($"Error reading hotwater data from ETAPU11 pellet boiler.");
                        }
                    }
                    
                    if (Heating)
                    {
                        _console.WriteLine($"Reading heating data from ETAPU11 pellet boiler.");
                        DataStatus status = _gateway.ReadHeatingData();

                        if (status.IsGood)
                        {
                            // Fix: Json Serializer cannot serialize NaN
                            if (double.IsNaN(_gateway.Data.Flow)) _gateway.Data.Flow = 0;

                            _console.WriteLine(JsonSerializer.Serialize<HeatingData>(_gateway.HeatingData, _options));
                        }
                        else
                        {
                            _console.WriteLine($"Error reading heating data from ETAPU11 pellet boiler.");
                        }
                    }
                    
                    if (Storage)
                    {
                        _console.WriteLine($"Reading storage data from ETAPU11 pellet boiler.");
                        DataStatus status = _gateway.ReadBoilerData();

                        if (status.IsGood)
                        {
                            _console.WriteLine(JsonSerializer.Serialize<StorageData>(_gateway.StorageData, _options));
                        }
                        else
                        {
                            _console.WriteLine($"Error reading storage data from ETAPU11 pellet boiler.");
                        }
                    }
                    
                    if (System)
                    {
                        _console.WriteLine($"Reading system data from ETAPU11 pellet boiler.");
                        DataStatus status = _gateway.ReadBoilerData();

                        if (status.IsGood)
                        {
                            _console.WriteLine(JsonSerializer.Serialize<SystemData>(_gateway.SystemData, _options));
                        }
                        else
                        {
                            _console.WriteLine($"Error reading system data from ETAPU11 pellet boiler.");
                        }
                    }
                }
                else
                {
                    _console.WriteLine($"Reading property '{Property}' from ETAPU11 pellet boiler");
                    var status = _gateway.ReadProperty(Property);

                    if (status.IsGood)
                    {
                        if (Data)
                        {
                            _console.WriteLine($"Value of property '{Property}' = {_gateway.Data.GetPropertyValue(Property)}");
                        }
                        else if (Boiler)
                        {
                            _console.WriteLine($"Value of property '{Property}' = {_gateway.BoilerData.GetPropertyValue(Property)}");
                        }
                        else if (Hotwater)
                        {
                            _console.WriteLine($"Value of property '{Property}' = {_gateway.HotwaterData.GetPropertyValue(Property)}");
                        }
                        else if (Heating)
                        {
                            _console.WriteLine($"Value of property '{Property}' = {_gateway.HeatingData.GetPropertyValue(Property)}");
                        }
                        else if (Storage)
                        {
                            _console.WriteLine($"Value of property '{Property}' = {_gateway.StorageData.GetPropertyValue(Property)}");
                        }
                        else if (System)
                        {
                            _console.WriteLine($"Value of property '{Property}' = {_gateway.SystemData.GetPropertyValue(Property)}");
                        }
                    }
                    else
                    {
                        _console.WriteLine($"Error reading property '{Property}' from ETAPU11 pellet boiler.");
                    }
                }

                if (Status)
                {
                    _console.WriteLine($"Status:");
                    _console.WriteLine(JsonSerializer.Serialize<DataStatus>(_gateway.Status, _options));
                }
            }
            catch
            {
                _logger.LogError("ReadCommand exception");
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

                if (Block && !Data)
                {
                    _console.WriteLine("Block read option is ignored.");
                }

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

                    if (!ETAPU11Data.IsReadable(Property))
                    {
                        _logger?.LogError($"The property '{Property}' is not readable.");
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
