// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadCommand.cs" company="DTV-Online">
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

    using System.Text.Json;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    using McMaster.Extensions.CommandLineUtils;

    using UtilityLib;
    using HeliosLib;
    using HeliosLib.Models;
    using HeliosApp.Models;

    #endregion

    /// <summary>
    /// Application command "read".
    /// </summary>
    [Command(Name = "read",
             FullName = "Helios Read Command",
             Description = "Reading data values from Helios KWL EC 200 ventilation system.",
             ExtendedHelpText = "\nCopyright (c) 2020 Dr. Peter Trimmel - All rights reserved.")]
    public class ReadCommand : BaseCommand<ReadCommand, AppSettings>
    {
        #region Private Data Members

        private readonly JsonSerializerOptions _options = JsonExtensions.DefaultSerializerOptions;
        private readonly HeliosGateway _gateway;

        #endregion

        #region Private Properties

        /// <summary>
        /// This is a reference to the parent command <see cref="RootCommand"/>.
        /// </summary>
        private RootCommand? Parent { get; }

        #endregion

        #region Public Properties

        [Option("-a|--alldata", Description = "Reads all data.")]
        public bool Data { get; set; }

        [Option("-b|--booster", Description = "Get the booster data.")]
        public bool Booster { get; set; }

        [Option("-d|--device", Description = "Get the device data.")]
        public bool Device { get; set; }

        [Option("-e|--error", Description = "Get the current error data.")]
        public bool Error { get; set; }

        [Option("-f|--fan", Description = "Get the fan data.")]
        public bool Fan { get; set; }

        [Option("-h|--heater", Description = "Get the heater data.")]
        public bool Heater { get; set; }

        [Option("-i|--info", Description = "Get the info data.")]
        public bool Info { get; set; }

        [Option("-l|--label", Description = "Get data by label.")]
        public bool Label { get; set; }

        [Option("-n|--network", Description = "Get the network data.")]
        public bool Network { get; set; }

        [Option("-o|--operation", Description = "Get the initial operation data.")]
        public bool Operation { get; set; }

        [Option("-p|--display", Description = "Get the current system status data.")]
        public bool Display { get; set; }

        [Option("-s|--sensor", Description = "Get the sensor data.")]
        public bool Sensor { get; set; }

        [Option("-t|--technical", Description = "Get the technical data.")]
        public bool Technical { get; set; }

        [Option("-v|--vacation", Description = "Get the vacation data.")]
        public bool Vacation { get; set; }

        [Option("-y|--system", Description = "Get the system data.")]
        public bool System { get; set; }

        [Argument(0, Description = "Reads the named property.")]
        public string Property { get; } = string.Empty;

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
        public ReadCommand(HeliosGateway gateway,
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
                if (!(Parent is null))
                {
                    // Overriding Helios options.
                    _settings.BaseAddress = Parent.BaseAddress;
                    _settings.Timeout = Parent.Timeout;
                    _settings.Password = Parent.Password;
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
                        _console.WriteLine($"Reading all data from Helios ventilation system.");
                        DataStatus status = _gateway.ReadAll();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Data:");
                            _console.WriteLine(JsonSerializer.Serialize<HeliosData>(_gateway.Data, _options));
                        }
                        else
                        {
                            _console.WriteLine($"Error reading data from Helios ventilation system.");
                        }
                    }

                    if (Booster)
                    {
                        _console.WriteLine($"Reading booster data from Helios ventilation system.");
                        DataStatus status = _gateway.ReadBoosterData();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Booster:");
                            _console.WriteLine(JsonSerializer.Serialize<BoosterData>(_gateway.BoosterData, _options));
                        }
                        else
                        {
                            _console.WriteLine($"Error reading booster data from Helios ventilation system.");
                        }
                    }

                    if (Device)
                    {
                        _console.WriteLine($"Reading device data from Helios ventilation system.");
                        DataStatus status = _gateway.ReadDeviceData();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Device:");
                            _console.WriteLine(JsonSerializer.Serialize<DeviceData>(_gateway.DeviceData, _options));
                        }
                        else
                        {
                            _console.WriteLine($"Error reading device data from Helios ventilation system.");
                        }
                    }

                    if (Display)
                    {
                        _console.WriteLine($"Reading display data from Helios ventilation system.");
                        DataStatus status = _gateway.ReadDisplayData();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Status:");
                            _console.WriteLine(JsonSerializer.Serialize<DisplayData>(_gateway.DisplayData, _options));
                        }
                        else
                        {
                            _console.WriteLine($"Error reading status data from Helios ventilation system.");
                        }
                    }

                    if (Error)
                    {
                        _console.WriteLine($"Reading error data from Helios ventilation system.");
                        DataStatus status = _gateway.ReadErrorData();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Error:");
                            _console.WriteLine(JsonSerializer.Serialize<ErrorData>(_gateway.ErrorData, _options));
                        }
                        else
                        {
                            _console.WriteLine($"Error reading error data from Helios ventilation system.");
                        }
                    }

                    if (Fan)
                    {
                        _console.WriteLine($"Reading fan data from Helios ventilation system.");
                        DataStatus status = _gateway.ReadFanData();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Fan:");
                            _console.WriteLine(JsonSerializer.Serialize<FanData>(_gateway.FanData, _options));
                        }
                        else
                        {
                            _console.WriteLine($"Error reading fan data from Helios ventilation system.");
                        }
                    }

                    if (Heater)
                    {
                        _console.WriteLine($"Reading heater data from Helios ventilation system.");
                        DataStatus status = _gateway.ReadHeaterData();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Heater:");
                            _console.WriteLine(JsonSerializer.Serialize<HeaterData>(_gateway.HeaterData, _options));
                        }
                        else
                        {
                            _console.WriteLine($"Error reading heater data from Helios ventilation system.");
                        }
                    }

                    if (Info)
                    {
                        _console.WriteLine($"Reading info data from Helios ventilation system.");
                        DataStatus status = _gateway.ReadInfoData();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Info:");
                            _console.WriteLine(JsonSerializer.Serialize<InfoData>(_gateway.InfoData, _options));
                        }
                        else
                        {
                            _console.WriteLine($"Error reading info data from Helios ventilation system.");
                        }
                    }

                    if (Network)
                    {
                        _console.WriteLine($"Reading network data from Helios ventilation system.");
                        DataStatus status = _gateway.ReadNetworkData();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Network:");
                            _console.WriteLine(JsonSerializer.Serialize<NetworkData>(_gateway.NetworkData, _options));
                        }
                        else
                        {
                            _console.WriteLine($"Error reading network data from Helios ventilation system.");
                        }
                    }

                    if (Operation)
                    {
                        _console.WriteLine($"Reading operation data from Helios ventilation system.");
                        DataStatus status = _gateway.ReadOperationData();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Operation:");
                            _console.WriteLine(JsonSerializer.Serialize<OperationData>(_gateway.OperationData, _options));
                        }
                        else
                        {
                            _console.WriteLine($"Error reading operation data from Helios ventilation system.");
                        }
                    }

                    if (Sensor)
                    {
                        _console.WriteLine($"Reading sensor data from Helios ventilation system.");
                        DataStatus status = _gateway.ReadSensorData();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Sensor:");
                            _console.WriteLine(JsonSerializer.Serialize<SensorData>(_gateway.SensorData, _options));
                        }
                        else
                        {
                            _console.WriteLine($"Error reading sensor data from Helios ventilation system.");
                        }
                    }

                    if (System)
                    {
                        _console.WriteLine($"Reading system data from Helios ventilation system.");
                        DataStatus status = _gateway.ReadSystemData();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"System:");
                            _console.WriteLine(JsonSerializer.Serialize<SystemData>(_gateway.SystemData, _options));
                        }
                        else
                        {
                            _console.WriteLine($"Error reading system data from Helios ventilation system.");
                        }
                    }

                    if (Technical)
                    {
                        _console.WriteLine($"Reading technical data from Helios ventilation system.");
                        DataStatus status = _gateway.ReadTechnicalData();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Technical:");
                            _console.WriteLine(JsonSerializer.Serialize<TechnicalData>(_gateway.TechnicalData, _options));
                        }
                        else
                        {
                            _console.WriteLine($"Error reading technical data from Helios ventilation system.");
                        }
                    }

                    if (Vacation)
                    {
                        _console.WriteLine($"Reading vacation data from Helios ventilation system.");
                        DataStatus status = _gateway.ReadVacationData();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Vacation:");
                            _console.WriteLine(JsonSerializer.Serialize<VacationData>(_gateway.VacationData, _options));
                        }
                        else
                        {
                            _console.WriteLine($"Error reading vacation data from Helios ventilation system.");
                        }
                    }
                }
                else
                {
                    if (Data)
                    {
                        DataStatus status = _gateway.ReadAll();

                        if (status.IsGood)
                        {
                            if (Label)
                            {
                                _console.WriteLine($"Value of property '{HeliosData.GetHeliosProperty(Property)}' with label '{Property}' = {_gateway.Data.GetHeliosValue(Property)}");
                            }
                            else
                            {
                                _console.WriteLine($"Value of property '{Property}' = {_gateway.Data.GetPropertyValue(Property)}");
                            }
                        }
                        else
                        {
                            _console.WriteLine($"Error reading data from Helios ventilation system.");
                        }
                    }

                    if (Booster)
                    {
                        DataStatus status = _gateway.ReadBoosterData();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Value of property '{Property}' = {_gateway.BoosterData.GetPropertyValue(Property)}");
                        }
                        else
                        {
                            _console.WriteLine($"Error reading booster data from Helios ventilation system.");
                        }
                    }
                    
                    if (Device)
                    {
                        DataStatus status = _gateway.ReadDeviceData();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Value of property '{Property}' = {_gateway.DeviceData.GetPropertyValue(Property)}");
                        }
                        else
                        {
                            _console.WriteLine($"Error reading device data from Helios ventilation system.");
                        }
                    }
                    
                    if (Display)
                    {
                        DataStatus status = _gateway.ReadDisplayData();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Value of property '{Property}' = {_gateway.DisplayData.GetPropertyValue(Property)}");
                        }
                        else
                        {
                            _console.WriteLine($"Error reading display data from Helios ventilation system.");
                        }
                    }
                    
                    if (Error)
                    {
                        DataStatus status = _gateway.ReadErrorData();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Value of property '{Property}' = {_gateway.ErrorData.GetPropertyValue(Property)}");
                        }
                        else
                        {
                            _console.WriteLine($"Error reading error data from Helios ventilation system.");
                        }
                    }
                    
                    if (Fan)
                    {
                        DataStatus status = _gateway.ReadFanData();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Value of property '{Property}' = {_gateway.FanData.GetPropertyValue(Property)}");
                        }
                        else
                        {
                            _console.WriteLine($"Error reading fan data from Helios ventilation system.");
                        }
                    }
                    
                    if (Heater)
                    {
                        DataStatus status = _gateway.ReadHeaterData();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Value of property '{Property}' = {_gateway.HeaterData.GetPropertyValue(Property)}");
                        }
                        else
                        {
                            _console.WriteLine($"Error reading heater data from Helios ventilation system.");
                        }
                    }
                    
                    if (Info)
                    {
                        DataStatus status = _gateway.ReadInfoData();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Value of property '{Property}' = {_gateway.InfoData.GetPropertyValue(Property)}");
                        }
                        else
                        {
                            _console.WriteLine($"Error reading info data from Helios ventilation system.");
                        }
                    }
                    
                    if (Network)
                    {
                        DataStatus status = _gateway.ReadNetworkData();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Value of property '{Property}' = {_gateway.NetworkData.GetPropertyValue(Property)}");
                        }
                        else
                        {
                            _console.WriteLine($"Error reading network data from Helios ventilation system.");
                        }
                    }
                    
                    if (Operation)
                    {
                        DataStatus status = _gateway.ReadOperationData();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Value of property '{Property}' = {_gateway.OperationData.GetPropertyValue(Property)}");
                        }
                        else
                        {
                            _console.WriteLine($"Error reading initial operation data from Helios ventilation system.");
                        }
                    }
                    
                    if (Sensor)
                    {
                        DataStatus status = _gateway.ReadSensorData();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Value of property '{Property}' = {_gateway.SensorData.GetPropertyValue(Property)}");
                        }
                        else
                        {
                            _console.WriteLine($"Error reading sensor data from Helios ventilation system.");
                        }
                    }
                    
                    if (System)
                    {
                        DataStatus status = _gateway.ReadSystemData();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Value of property '{Property}' = {_gateway.SystemData.GetPropertyValue(Property)}");
                        }
                        else
                        {
                            _console.WriteLine($"Error reading system data from Helios ventilation system.");
                        }
                    }
                    
                    if (Technical)
                    {
                        DataStatus status = _gateway.ReadTechnicalData();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Value of property '{Property}' = {_gateway.TechnicalData.GetPropertyValue(Property)}");
                        }
                        else
                        {
                            _console.WriteLine($"Error reading technical data from Helios ventilation system.");
                        }
                    }
                    
                    if (Vacation)
                    {
                        DataStatus status = _gateway.ReadVacationData();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Value of property '{Property}' = {_gateway.VacationData.GetPropertyValue(Property)}");
                        }
                        else
                        {
                            _console.WriteLine($"Error reading ^vacation data from Helios ventilation system.");
                        }
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

                if (Booster) ++options;
                if (Device) ++options;
                if (Display) ++options;
                if (Error) ++options;
                if (Fan) ++options;
                if (Heater) ++options;
                if (Info) ++options;
                if (Network) ++options;
                if (Operation) ++options;
                if (Sensor) ++options;
                if (System) ++options;
                if (Technical) ++options;
                if (Vacation) ++options;

                if (Label)
                {
                    if (string.IsNullOrEmpty(Property))
                    {
                        if (!Data || (options > 0))
                        {
                            _console.WriteLine("Helios label option (-l|--label) can only be used with the (-a|--alldata) option or when specifying a label");
                            return false;
                        }
                    }
                    else
                    {
                        if (!Data || (options > 0))
                        {
                            _console.WriteLine("Helios label option (-l|--label) can only be used with the (-a|--alldata) option or when specifying a label");
                            return false;
                        }

                        if (!HeliosData.IsHelios(Property.ToLower()))
                        {
                            _logger?.LogError($"The property with Helios label '{Property}' has not been found.");
                            return false;
                        }
                    }
                }
                else 
                {
                    if (!string.IsNullOrEmpty(Property))
                    {
                        if ((Data && (options > 0)) || (!Data && (options != 1)))
                        {
                            _console.WriteLine("Please specifiy a single data option when specifying a property");
                            return false;
                        }

                        if (Data)
                        {
                            if (!typeof(HeliosData).IsProperty(Property))
                            {
                                _logger?.LogError($"The property '{Property}' has not been found in Helios data.");
                                return false;
                            }
                        }

                        if (Booster)
                        {
                            if (!typeof(BoosterData).IsProperty(Property))
                            {
                                _logger?.LogError($"The property '{Property}' has not been found in booster data.");
                                return false;
                            }
                        }

                        if (Device)
                        {
                            if (!typeof(DeviceData).IsProperty(Property))
                            {
                                _logger?.LogError($"The property '{Property}' has not been found in device data.");
                                return false;
                            }
                        }

                        if (Display)
                        {
                            if (!typeof(DisplayData).IsProperty(Property))
                            {
                                _logger?.LogError($"The property '{Property}' has not been found in display data.");
                                return false;
                            }
                        }

                        if (Error)
                        {
                            if (!typeof(ErrorData).IsProperty(Property))
                            {
                                _logger?.LogError($"The property '{Property}' has not been found in error data.");
                                return false;
                            }
                        }

                        if (Fan)
                        {
                            if (!typeof(FanData).IsProperty(Property))
                            {
                                _logger?.LogError($"The property '{Property}' has not been found in fan data.");
                                return false;
                            }
                        }

                        if (Heater)
                        {
                            if (!typeof(HeaterData).IsProperty(Property))
                            {
                                _logger?.LogError($"The property '{Property}' has not been found in heater data.");
                                return false;
                            }
                        }

                        if (Info)
                        {
                            if (!typeof(InfoData).IsProperty(Property))
                            {
                                _logger?.LogError($"The property '{Property}' has not been found in info data.");
                                return false;
                            }
                        }

                        if (Network)
                        {
                            if (!typeof(NetworkData).IsProperty(Property))
                            {
                                _logger?.LogError($"The property '{Property}' has not been found in network data.");
                                return false;
                            }
                        }

                        if (Operation)
                        {
                            if (!typeof(OperationData).IsProperty(Property))
                            {
                                _logger?.LogError($"The property '{Property}' has not been found in operation data.");
                                return false;
                            }
                        }

                        if (Sensor)
                        {
                            if (!typeof(SensorData).IsProperty(Property))
                            {
                                _logger?.LogError($"The property '{Property}' has not been found in sensor data.");
                                return false;
                            }
                        }

                        if (System)
                        {
                            if (!typeof(SystemData).IsProperty(Property))
                            {
                                _logger?.LogError($"The property '{Property}' has not been found in system data.");
                                return false;
                            }
                        }

                        if (Technical)
                        {
                            if (!typeof(TechnicalData).IsProperty(Property))
                            {
                                _logger?.LogError($"The property '{Property}' has not been found in technical data.");
                                return false;
                            }
                        }

                        if (Vacation)
                        {
                            if (!typeof(VacationData).IsProperty(Property))
                            {
                                _logger?.LogError($"The property '{Property}' has not been found in vacation data.");
                                return false;
                            }
                        }
                    }
                    else
                    {
                        if (!Data && (options == 0))
                        {
                            _console.WriteLine("Please specifiy a data option.");
                            return false;
                        }

                        if (Data && (options > 0))
                        {
                            _console.WriteLine("The data option overrides other data options.");

                            Booster = false;
                            Device = false;
                            Display = false;
                            Error = false;
                            Fan = false;
                            Heater = false;
                            Info = false;
                            Network = false;
                            Operation = false;
                            Sensor = false;
                            System = false;
                            Technical = false;
                            Vacation = false;
                        }
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
