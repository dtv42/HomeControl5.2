// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InfoCommand.cs" company="DTV-Online">
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
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    using McMaster.Extensions.CommandLineUtils;

    using UtilityLib;
    using HeliosLib.Models;

    using HeliosApp.Models;

    #endregion

    /// <summary>
    /// Application command "info".
    /// </summary>
    [Command(Name = "info",
             FullName = "Helios Info Command",
             Description = "Reading data values from Helios KWL EC 200 ventilation system.",
             ExtendedHelpText = "\nCopyright (c) 2020 Dr. Peter Trimmel - All rights reserved.")]
    public class InfoCommand : BaseCommand<InfoCommand, AppSettings>
    {
        #region Private Properties

        /// <summary>
        /// This is a reference to the parent command <see cref="RootCommand"/>.
        /// </summary>
        private RootCommand? Parent { get; }

        #endregion

        #region Public Properties

        [Option("-a|--alldata", Description = "Gets all data.")]
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

        [Argument(0, Description = "Get the named property.")]
        public string Property { get; } = string.Empty;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InfoCommand"/> class.
        /// </summary>
        /// <param name="console"></param>
        /// <param name="settings"></param>
        /// <param name="config"></param>
        /// <param name="environment"></param>
        /// <param name="lifetime"></param>
        /// <param name="logger"></param>
        /// <param name="application"></param>
        public InfoCommand(IConsole console,
                           AppSettings settings,
                           IConfiguration config,
                           IHostEnvironment environment,
                           IHostApplicationLifetime lifetime,
                           ILogger<InfoCommand> logger,
                           CommandLineApplication application)
            : base(console, settings, config, environment, lifetime, logger, application)
        {
            _logger?.LogDebug("InfoCommand()");
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
                if (string.IsNullOrEmpty(Property))
                {
                    if (Data)
                    {
                        _console.WriteLine($"Data:");

                        if (Label)
                        {
                            ShowLabels();
                        }
                        else
                        {
                            ShowProperties();
                        }
                    }

                    if (Booster)
                    {
                        _console.WriteLine($"Booster:");
                        ShowProperties(typeof(BoosterData));
                    }
                    if (Device)
                    {
                        _console.WriteLine($"Device:");
                        ShowProperties(typeof(DeviceData));
                    }
                    if (Display)
                    {
                        _console.WriteLine($"Display:");
                        ShowProperties(typeof(DisplayData));
                    }
                    if (Error)
                    {
                        _console.WriteLine($"Error:");
                        ShowProperties(typeof(ErrorData));
                    }
                    if (Fan)
                    {
                        _console.WriteLine($"Fan:");
                        ShowProperties(typeof(FanData));
                    }
                    if (Heater)
                    {
                        _console.WriteLine($"Heater:");
                        ShowProperties(typeof(HeaterData));
                    }
                    if (Info)
                    {
                        _console.WriteLine($"Info:");
                        ShowProperties(typeof(InfoData));
                    }
                    if (Network)
                    {
                        _console.WriteLine($"Network:");
                        ShowProperties(typeof(NetworkData));
                    }
                    if (Operation)
                    {
                        _console.WriteLine($"Operation:");
                        ShowProperties(typeof(OperationData));
                    }
                    if (Sensor)
                    {
                        _console.WriteLine($"Sensor:");
                        ShowProperties(typeof(SensorData));
                    }
                    if (System)
                    {
                        _console.WriteLine($"System:");
                        ShowProperties(typeof(SystemData));
                    }
                    if (Technical)
                    {
                        _console.WriteLine($"Technical:");
                        ShowProperties(typeof(TechnicalData));
                    }
                    if (Vacation)
                    {
                        _console.WriteLine($"Vacation:");
                        ShowProperties(typeof(VacationData));
                    }
                }
                else
                {
                    if (Data)
                    {
                        if (Label)
                        {
                            ShowProperty(typeof(HeliosData), HeliosData.GetHeliosProperty(Property));
                        }
                        else
                        {
                            ShowProperty(typeof(HeliosData), Property);
                        }
                    }

                    if (Booster)
                    {
                        ShowProperty(typeof(BoosterData), Property);
                    }
                    if (Device)
                    {
                        ShowProperty(typeof(DeviceData), Property);
                    }
                    if (Display)
                    {
                        ShowProperty(typeof(DisplayData), Property);
                    }
                    if (Error)
                    {
                        ShowProperty(typeof(ErrorData), Property);
                    }
                    if (Fan)
                    {
                        ShowProperty(typeof(FanData), Property);
                    }
                    if (Heater)
                    {
                        ShowProperty(typeof(HeaterData), Property);
                    }
                    if (Info)
                    {
                        ShowProperty(typeof(InfoData), Property);
                    }
                    if (Network)
                    {
                        ShowProperty(typeof(NetworkData), Property);
                    }
                    if (Operation)
                    {
                        ShowProperty(typeof(OperationData), Property);
                    }
                    if (Sensor)
                    {
                        ShowProperty(typeof(SensorData), Property);
                    }
                    if (System)
                    {
                        ShowProperty(typeof(SystemData), Property);
                    }
                    if (Technical)
                    {
                        ShowProperty(typeof(TechnicalData), Property);
                    }
                    if (Vacation)
                    {
                        ShowProperty(typeof(VacationData), Property);
                    }
                }
            }
            catch
            {
                _logger.LogError("InfoCommand exception");
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

        #region Private Methods

        /// <summary>
        /// Displays a list of property names.
        /// </summary>
        private void ShowLabels()
        {
            _console.WriteLine($"List of Helios Labels:");
            var names = HeliosData.GetLabels();

            foreach (var name in names)
            {
                _console.WriteLine($"    {name}");
            }

            _console.WriteLine();
        }

        /// <summary>
        /// Displays a list of property names.
        /// </summary>
        private void ShowProperties()
        {
            _console.WriteLine($"List of Properties:");
            var names = HeliosData.GetProperties();

            foreach (var name in names)
            {
                _console.WriteLine($"    {name}");
            }

            _console.WriteLine();
        }

        /// <summary>
        /// Displays a list of property names.
        /// </summary>
        /// <param name="type"></param>
        private void ShowProperties(Type type)
        {
            _console.WriteLine($"List of Properties:");
            var names = type.GetProperties().Select(p => p.Name);

            foreach (var name in names)
            {
                _console.WriteLine($"    {name}");
            }

            _console.WriteLine();
        }

        /// <summary>
        /// Displays selected property info data for a named property.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        private void ShowProperty(Type type, string name)
        {
            _console.WriteLine($"Property {name}:");
            var info = type.GetProperty(name);
            var pType = info?.PropertyType;

            _console.WriteLine($"   IsProperty:    {!(info is null)}");
            _console.WriteLine($"   CanRead:       {info?.CanRead}");
            _console.WriteLine($"   CanWrite:      {info?.CanWrite}");

            if (info?.PropertyType.IsArray ?? false)
            {
                _console.WriteLine($"   IsArray:       {pType?.IsArray}");
                _console.WriteLine($"   ElementType:   {pType?.GetElementType()}");
            }
            else if ((pType?.IsGenericType ?? false) && (pType?.GetGenericTypeDefinition() == typeof(List<>)))
            {
                _console.WriteLine($"   IsList:        List<ItempType>");
                _console.WriteLine($"   ItemType:      {pType?.GetGenericArguments().Single()}");
            }
            else
            {
                _console.WriteLine($"   PropertyType:  {pType?.Name}");
            }

            _console.WriteLine($"   Helios Label:  {HeliosData.GetHeliosAttribute(name).Name}");

            _console.WriteLine();
        }

        #endregion
    }
}
