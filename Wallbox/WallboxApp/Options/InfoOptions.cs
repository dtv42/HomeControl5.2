﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InfoOptions.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>19-12-2020 22:29</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace WallboxApp.Options
{
    #region Using Directives

    using System.CommandLine;

    using UtilityLib.Console;

    using WallboxLib.Models;

    #endregion Using Directives

    /// <summary>
    /// The options for the info sub command.
    /// </summary>
    public class InfoOptions
    {
        public string Name { get; set; } = string.Empty;
        public bool Report1 { get; set; }
        public bool Report2 { get; set; }
        public bool Report3 { get; set; }
        public bool Reports { get; set; }
        public bool Info { get; set; }

        /// <summary>
        /// Helper method to check options.
        /// </summary>
        /// <param name="console">The console used for messages.</param>
        /// <returns>True if options OK.</returns>
        public bool CheckOptions(IConsole console)
        {
            int options = 0;

            if (Report1) ++options;
            if (Report2) ++options;
            if (Report3) ++options;
            if (Reports) ++options;
            if (Info) ++options;

            if (options > 1)
            {
                console.RedWriteLine("Please specifiy a single report option.");
                return false;
            }
            else if (options == 0)
            {
                console.RedWriteLine("Please select a single report option (-1|-2|-3|-r|-i)");
                return false;
            }

            if (!string.IsNullOrEmpty(Name))
            {
                if (Report1)
                {
                    if (typeof(Report1Data).GetProperty(Name) is null)
                    {
                        console.RedWriteLine($"The property '{Name}' has not been found.");
                        return false;
                    }
                }

                if (Report2)
                {
                    if (typeof(Report2Data).GetProperty(Name) is null)
                    {
                        console.RedWriteLine($"The property '{Name}' has not been found.");
                        return false;
                    }
                }

                if (Report3)
                {
                    if (typeof(Report3Data).GetProperty(Name) is null)
                    {
                        console.RedWriteLine($"The property '{Name}' has not been found.");
                        return false;
                    }
                }

                if (Reports)
                {
                    if (typeof(ReportsData).GetProperty(Name) is null)
                    {
                        console.RedWriteLine($"The property '{Name}' has not been found.");
                        return false;
                    }
                }

                if (Info)
                {
                    if (typeof(InfoData).GetProperty(Name) is null)
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
