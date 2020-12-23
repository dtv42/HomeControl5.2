// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InfoOptions.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>19-12-2020 22:29</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace HeliosApp.Options
{
    #region Using Directives

    using System.CommandLine;

    using UtilityLib.Console;

    using HeliosLib.Models;
    using System.CommandLine.IO;
    using UtilityLib;

    #endregion Using Directives

    /// <summary>
    /// The options for the info sub command.
    /// </summary>
    public class InfoOptions
    {
        public string Name { get; set; } = string.Empty;
        public bool AllData { get; set; }
        public bool Booster { get; set; }
        public bool Device { get; set; }
        public bool Error { get; set; }
        public bool Fan { get; set; }
        public bool Heater { get; set; }
        public bool Info { get; set; }
        public bool Label { get; set; }
        public bool Network { get; set; }
        public bool Operation { get; set; }
        public bool Display { get; set; }
        public bool Sensor { get; set; }
        public bool Technical { get; set; }
        public bool Vacation { get; set; }
        public bool System { get; set; }

        /// <summary>
        /// Helper method to check options.
        /// </summary>
        /// <param name="console">The console used for messages.</param>
        /// <returns>True if options OK.</returns>
        public bool CheckOptions(IConsole console)
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
                if (!AllData || (options > 0))
                {
                    console.RedWriteLine("Helios label option (-l|--label) can only be used with the (-a|--alldata) option or when specifying a label");
                    return false;
                }
                if (!string.IsNullOrEmpty(Name))
                {
                    if (!AllData || (options > 0))
                    {
                        console.RedWriteLine("Helios label option (-l|--label) can only be used with the (-a|--alldata) option or when specifying a label");
                        return false;
                    }

                    if (!HeliosData.IsHelios(Name.ToLower()))
                    {
                        console.RedWriteLine($"The property with Helios label '{Name}' has not been found.");
                        return false;
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(Name))
                {
                    if ((AllData && (options > 0)) || (!AllData && (options != 1)))
                    {
                        console.RedWriteLine("Please specifiy a single data option when specifying a property");
                        return false;
                    }

                    if (AllData)
                    {
                        if (!typeof(HeliosData).IsProperty(Name))
                        {
                            console.RedWriteLine($"The property '{Name}' has not been found in Helios data.");
                            return false;
                        }
                    }

                    if (Booster)
                    {
                        if (!typeof(BoosterData).IsProperty(Name))
                        {
                            console.RedWriteLine($"The property '{Name}' has not been found in booster data.");
                            return false;
                        }
                    }

                    if (Device)
                    {
                        if (!typeof(DeviceData).IsProperty(Name))
                        {
                            console.RedWriteLine($"The property '{Name}' has not been found in device data.");
                            return false;
                        }
                    }

                    if (Display)
                    {
                        if (!typeof(DisplayData).IsProperty(Name))
                        {
                            console.RedWriteLine($"The property '{Name}' has not been found in display data.");
                            return false;
                        }
                    }

                    if (Error)
                    {
                        if (!typeof(ErrorData).IsProperty(Name))
                        {
                            console.RedWriteLine($"The property '{Name}' has not been found in error data.");
                            return false;
                        }
                    }

                    if (Fan)
                    {
                        if (!typeof(FanData).IsProperty(Name))
                        {
                            console.RedWriteLine($"The property '{Name}' has not been found in fan data.");
                            return false;
                        }
                    }

                    if (Heater)
                    {
                        if (!typeof(HeaterData).IsProperty(Name))
                        {
                            console.RedWriteLine($"The property '{Name}' has not been found in heater data.");
                            return false;
                        }
                    }

                    if (Info)
                    {
                        if (!typeof(InfoData).IsProperty(Name))
                        {
                            console.RedWriteLine($"The property '{Name}' has not been found in info data.");
                            return false;
                        }
                    }

                    if (Network)
                    {
                        if (!typeof(NetworkData).IsProperty(Name))
                        {
                            console.RedWriteLine($"The property '{Name}' has not been found in network data.");
                            return false;
                        }
                    }

                    if (Operation)
                    {
                        if (!typeof(OperationData).IsProperty(Name))
                        {
                            console.RedWriteLine($"The property '{Name}' has not been found in operation data.");
                            return false;
                        }
                    }

                    if (Sensor)
                    {
                        if (!typeof(SensorData).IsProperty(Name))
                        {
                            console.RedWriteLine($"The property '{Name}' has not been found in sensor data.");
                            return false;
                        }
                    }

                    if (System)
                    {
                        if (!typeof(SystemData).IsProperty(Name))
                        {
                            console.RedWriteLine($"The property '{Name}' has not been found in system data.");
                            return false;
                        }
                    }

                    if (Technical)
                    {
                        if (!typeof(TechnicalData).IsProperty(Name))
                        {
                            console.RedWriteLine($"The property '{Name}' has not been found in technical data.");
                            return false;
                        }
                    }

                    if (Vacation)
                    {
                        if (!typeof(VacationData).IsProperty(Name))
                        {
                            console.RedWriteLine($"The property '{Name}' has not been found in vacation data.");
                            return false;
                        }
                    }
                }
                else
                {
                    if (!AllData && (options == 0))
                    {
                        console.RedWriteLine("Please specifiy a data option.");
                        return false;
                    }

                    if (AllData && (options > 0))
                    {
                        console.YellowWriteLine("The data option overrides other data options.");

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
            
            return true;
        }
    }
}
