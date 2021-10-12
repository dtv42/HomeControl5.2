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
namespace NetatmoApp.Options
{
    #region Using Directives

    using System.CommandLine;

    using UtilityLib.Console;

    using NetatmoLib.Models;

    #endregion Using Directives

    /// <summary>
    /// The options for the info sub command.
    /// </summary>
    public class InfoOptions
    {
        public string Name { get; set; } = string.Empty;
        public bool Data { get; set; }
        public bool Main { get; set; }
        public bool Outdoor { get; set; }
        public bool Indoor { get; set; }
        public bool Rain { get; set; }
        public bool Wind { get; set; }

        /// <summary>
        /// Helper method to check options.
        /// </summary>
        /// <param name="console">The console used for messages.</param>
        /// <returns>True if options OK.</returns>
        public bool CheckOptions(IConsole console)
        {
            int options = 0;

            if (Data) ++options;
            if (Main) ++options;
            if (Outdoor) ++options;
            if (Indoor) ++options;
            if (Rain) ++options;
            if (Wind) ++options;

            if (options > 1)
            {
                console.RedWriteLine("Please specifiy a single property type option.");
                return false;
            }
            else if (options == 0)
            {
                console.RedWriteLine("Please select a single property type (-d|-m|-o|-i|-r|-w)");
                return false;
            }

            if (!string.IsNullOrEmpty(Name))
            {
                if (Data)
                {
                    if (typeof(NetatmoData).GetProperty(Name) is null)
                    {
                        console.RedWriteLine($"The property '{Name}' has not been found.");
                        return false;
                    }
                }

                if (Main)
                {
                    if (typeof(MainData).GetProperty(Name) is null)
                    {
                        console.RedWriteLine($"The property '{Name}' has not been found.");
                        return false;
                    }
                }

                if (Outdoor)
                {
                    if (typeof(OutdoorData).GetProperty(Name) is null)
                    {
                        console.RedWriteLine($"The property '{Name}' has not been found.");
                        return false;
                    }
                }

                if (Indoor)
                {
                    if (typeof(IndoorData).GetProperty(Name) is null)
                    {
                        console.RedWriteLine($"The property '{Name}' has not been found.");
                        return false;
                    }
                }

                if (Rain)
                {
                    if (typeof(RainData).GetProperty(Name) is null)
                    {
                        console.RedWriteLine($"The property '{Name}' has not been found.");
                        return false;
                    }
                }

                if (Wind)
                {
                    if (typeof(WindData).GetProperty(Name) is null)
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
