// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadCommand.cs" company="DTV-Online">
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

    using System.Text.Json;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    using McMaster.Extensions.CommandLineUtils;

    using UtilityLib;
    using FroniusLib;
    using FroniusLib.Models;
    using FroniusApp.Models;

    #endregion

    /// <summary>
    /// Application command "read".
    /// </summary>
    [Command(Name = "read",
             FullName = "Fronius Read Command",
             Description = "Reading data values from Fronius Symo 8.2-3-M solar inverter.",
             ExtendedHelpText = "\nCopyright (c) 2020 Dr. Peter Trimmel - All rights reserved.")]
    public class ReadCommand : BaseCommand<ReadCommand, AppSettings>
    {
        #region Private Data Members

        private readonly JsonSerializerOptions _options = JsonExtensions.DefaultSerializerOptions;
        private readonly FroniusGateway _gateway;

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

        [Option("-c|--common", Description = "Reads the inverter common data.")]
        public bool Common { get; }

        [Option("-i|--inverter", Description = "Reads the inverter info.")]
        public bool Inverter { get; }

        [Option("-l|--logger", Description = "Reads the data logger info.")]
        public bool Logger { get; }

        [Option("-m|--minmax", Description = "Reads the inverter minmax data.")]
        public bool MinMax { get; }

        [Option("-p|--phase", Description = "Reads the inverter phase data.")]
        public bool Phase { get; }

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
        public ReadCommand(FroniusGateway gateway,
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
                if (!(Parent is null))
                {
                    // Overriding Fronius options.
                    _settings.BaseAddress = Parent.BaseAddress;
                    _settings.Timeout = Parent.Timeout;
                    _settings.DeviceID = Parent.DeviceID;
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
                        _console.WriteLine($"Reading all data from Fronius solar inverter.");
                        DataStatus status = _gateway.ReadAll();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Data:");
                            _console.WriteLine(JsonSerializer.Serialize<FroniusData>(_gateway.Data, _options));
                        }
                        else
                        {
                            _console.WriteLine($"Error reading data from Fronius solar inverter.");
                        }
                    }

                    if (Common)
                    {
                        _console.WriteLine($"Reading common inverter data from Fronius solar inverter.");
                        DataStatus status = _gateway.ReadCommonData();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Common:");
                            _console.WriteLine(JsonSerializer.Serialize<CommonData>(_gateway.CommonData, _options));
                        }
                        else
                        {
                            _console.WriteLine($"Error reading common inverter data from Fronius solar inverter.");
                        }
                    }

                    if (Inverter)
                    {
                        _console.WriteLine($"Reading inverter info from Fronius solar inverter.");
                        DataStatus status = _gateway.ReadInverterInfo();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Inverter:");
                            _console.WriteLine(JsonSerializer.Serialize<InverterInfo>(_gateway.InverterInfo, _options));
                        }
                        else
                        {
                            _console.WriteLine($"Error reading hotwater data from Fronius solar inverter.");
                        }
                    }

                    if (Logger)
                    {
                        _console.WriteLine($"Reading logger info from Fronius solar inverter.");
                        DataStatus status = _gateway.ReadLoggerInfo();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Logger:");
                            _console.WriteLine(JsonSerializer.Serialize<LoggerInfo>(_gateway.LoggerInfo, _options));
                        }
                        else
                        {
                            _console.WriteLine($"Error reading logger info from Fronius solar inverter.");
                        }
                    }

                    if (MinMax)
                    {
                        _console.WriteLine($"Reading minmax data from Fronius solar inverter.");
                        DataStatus status = _gateway.ReadMinMaxData();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"MinMax:");
                            _console.WriteLine(JsonSerializer.Serialize<MinMaxData>(_gateway.MinMaxData, _options));
                        }
                        else
                        {
                            _console.WriteLine($"Error reading minmax data from Fronius solar inverter.");
                        }
                    }

                    if (Phase)
                    {
                        _console.WriteLine($"Reading phase data from Fronius solar inverter.");
                        DataStatus status = _gateway.ReadPhaseData();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Phase:");
                            _console.WriteLine(JsonSerializer.Serialize<PhaseData>(_gateway.PhaseData, _options));
                        }
                        else
                        {
                            _console.WriteLine($"Error reading phase data from Fronius solar inverter.");
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
                            _console.WriteLine($"Value of property '{Property}' = {_gateway.Data.GetPropertyValue(Property)}");
                        }
                        else
                        {
                            _console.WriteLine($"Error reading data from Fronius solar inverter.");
                        }
                    }
                    
                    if (Common)
                    {
                        DataStatus status = _gateway.ReadCommonData();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Value of property '{Property}' = {_gateway.CommonData.GetPropertyValue(Property)}");
                        }
                        else
                        {
                            _console.WriteLine($"Error reading common inverter data from Fronius solar inverter.");
                        }
                    }

                    if (Inverter)
                    {
                        DataStatus status = _gateway.ReadInverterInfo();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Value of property '{Property}' = {_gateway.InverterInfo.GetPropertyValue(Property)}");
                        }
                        else
                        {
                            _console.WriteLine($"Error reading inverter info from Fronius solar inverter.");
                        }
                    }

                    if (Logger)
                    {
                        DataStatus status = _gateway.ReadLoggerInfo();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Value of property '{Property}' = {_gateway.LoggerInfo.GetPropertyValue(Property)}");
                        }
                        else
                        {
                            _console.WriteLine($"Error reading logger info from Fronius solar inverter.");
                        }
                    }

                    if (MinMax)
                    {
                        DataStatus status = _gateway.ReadMinMaxData();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Value of property '{Property}' = {_gateway.MinMaxData.GetPropertyValue(Property)}");
                        }
                        else
                        {
                            _console.WriteLine($"Error reading minmax data from Fronius solar inverter.");
                        }
                    }

                    if (Phase)
                    {
                        DataStatus status = _gateway.ReadPhaseData();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Value of property '{Property}' = {_gateway.PhaseData.GetPropertyValue(Property)}");
                        }
                        else
                        {
                            _console.WriteLine($"Error reading phase data from Fronius solar inverter.");
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

                if (Data) ++options;
                if (Common) ++options;
                if (Inverter) ++options;
                if (Logger) ++options;
                if (MinMax) ++options;
                if (Phase) ++options;

                if (options != 1)
                {
                    _console.WriteLine("Please specifiy a single data option");
                    return false;
                }

                if (!string.IsNullOrEmpty(Property))
                {
                    if (Data)
                    {
                        if (!typeof(FroniusData).IsProperty(Property))
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }

                    if (Common)
                    {
                        if (!typeof(CommonData).IsProperty(Property))
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }

                    if (Inverter)
                    {
                        if (!typeof(InverterInfo).IsProperty(Property))
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }

                    if (Logger)
                    {
                        if (!typeof(LoggerInfo).IsProperty(Property))
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }

                    if (MinMax)
                    {
                        if (!typeof(MinMaxData).IsProperty(Property))
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }

                    if (Phase)
                    {
                        if (!typeof(PhaseData).IsProperty(Property))
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
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
