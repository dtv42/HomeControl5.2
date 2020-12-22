// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadOptions.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>19-12-2020 22:46</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace FroniusApp.Options
{
    #region Using Directives

    using System.CommandLine;

    using UtilityLib.Console;

    using FroniusLib.Models;

    #endregion Using Directives

    /// <summary>
    /// The options for the read sub command.
    /// </summary>
    public class ReadOptions
    {
        public string Name { get; set; } = string.Empty;
        public bool Data { get; set; }
        public bool Common { get; set; }
        public bool Inverter { get; set; }
        public bool Logger { get; set; }
        public bool MinMax { get; set; }
        public bool Phase { get; set; }
        public bool Status { get; set; }

        /// <summary>
        /// Helper method to check options.
        /// </summary>
        /// <param name="console">The console used for messages.</param>
        /// <returns>True if options OK.</returns>
        public bool CheckOptions(IConsole console)
        {
            int options = 0;

            if (Data) ++options;
            if (Common) ++options;
            if (Inverter) ++options;
            if (Logger) ++options;
            if (MinMax) ++options;
            if (Phase) ++options;

            if (options > 1)
            {
                console.RedWriteLine("Please specifiy a single property type option.");
                return false;
            }
            else if (options == 0)
            {
                console.RedWriteLine("Please select a single property type (-d|-c|-i|-l|-m|-p)");
                return false;
            }

            if (!string.IsNullOrEmpty(Name))
            {
                if (Data)
                {
                    if (typeof(FroniusData).GetProperty(Name) is null)
                    {
                        console.RedWriteLine($"The property '{Name}' has not been found.");
                        return false;
                    }
                }

                if (Common)
                {
                    if (typeof(CommonData).GetProperty(Name) is null)
                    {
                        console.RedWriteLine($"The property '{Name}' has not been found.");
                        return false;
                    }
                }

                if (Inverter)
                {
                    if (typeof(InverterInfo).GetProperty(Name) is null)
                    {
                        console.RedWriteLine($"The property '{Name}' has not been found.");
                        return false;
                    }
                }

                if (Logger)
                {
                    if (typeof(LoggerInfo).GetProperty(Name) is null)
                    {
                        console.RedWriteLine($"The property '{Name}' has not been found.");
                        return false;
                    }
                }

                if (MinMax)
                {
                    if (typeof(MinMaxData).GetProperty(Name) is null)
                    {
                        console.RedWriteLine($"The property '{Name}' has not been found.");
                        return false;
                    }
                }

                if (Phase)
                {
                    if (typeof(PhaseData).GetProperty(Name) is null)
                    {
                        console.RedWriteLine($"The property '{Name}' has not been found.");
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
