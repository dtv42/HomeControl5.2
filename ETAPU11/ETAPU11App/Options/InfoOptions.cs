// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InfoOptions.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>20-12-2020 20:34</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace ETAPU11App.Options
{
    #region Using Directives

    using System.CommandLine;

    using UtilityLib.Console;

    using ETAPU11Lib.Models;

    #endregion Using Directives

    /// <summary>
    /// The options for the info sub command.
    /// </summary>
    public class InfoOptions
    {
        public string Name  { get; set; } = string.Empty;
        public bool Data    { get; set; }
        public bool Boiler  { get; set; }
        public bool Water   { get; set; }
        public bool Circuit { get; set; }
        public bool Storage { get; set; }
        public bool System  { get; set; }

        /// <summary>
        /// Helper method to check options.
        /// </summary>
        /// <param name="console">The console used for messages.</param>
        /// <returns>True if options OK.</returns>
        public bool CheckOptions(IConsole console)
        {
            int options = 0;

            if (Data) ++options;
            if (Boiler) ++options;
            if (Water) ++options;
            if (Circuit) ++options;
            if (Storage) ++options;
            if (System) ++options;

            if (options > 1)
            {
                console.RedWriteLine("Please specifiy a single property type option.");
                return false;
            }
            else if (options == 0)
            {
                console.RedWriteLine("Please select a single property type (-d|-b|-w|-c|-s|-y)");
                return false;
            }

            if (!string.IsNullOrEmpty(Name))
            {
                if (Data)
                {
                    if (typeof(ETAPU11Data).GetProperty(Name) is null)
                    {
                        console.RedWriteLine($"The property '{Name}' has not been found.");
                        return false;
                    }
                }

                if (Boiler)
                {
                    if (typeof(BoilerData).GetProperty(Name) is null)
                    {
                        console.RedWriteLine($"The property '{Name}' has not been found.");
                        return false;
                    }
                }

                if (Water)
                {
                    if (typeof(HotwaterData).GetProperty(Name) is null)
                    {
                        console.RedWriteLine($"The property '{Name}' has not been found.");
                        return false;
                    }
                }

                if (Circuit)
                {
                    if (typeof(HeatingData).GetProperty(Name) is null)
                    {
                        console.RedWriteLine($"The property '{Name}' has not been found.");
                        return false;
                    }
                }

                if (Storage)
                {
                    if (typeof(StorageData).GetProperty(Name) is null)
                    {
                        console.RedWriteLine($"The property '{Name}' has not been found.");
                        return false;
                    }
                }

                if (Storage)
                {
                    if (typeof(StorageData).GetProperty(Name) is null)
                    {
                        console.RedWriteLine($"The property '{Name}' has not been found.");
                        return false;
                    }
                }

                if (System)
                {
                    if (typeof(SystemData).GetProperty(Name) is null)
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
