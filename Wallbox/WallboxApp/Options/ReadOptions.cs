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
namespace WallboxApp.Options
{
    #region Using Directives

    using System.CommandLine;
    using UtilityLib;
    using UtilityLib.Console;

    using WallboxLib.Models;

    #endregion Using Directives

    /// <summary>
    /// The options for the read sub command.
    /// </summary>
    public class ReadOptions
    {
        public string Name { get; set; } = string.Empty;
        public bool Data { get; set; }
        public bool Report1 { get; set; }
        public bool Report2 { get; set; }
        public bool Report3 { get; set; }
        public bool Reports { get; set; }
        public bool Last { get; set; }
        public ushort Number { get; set; }
        public bool Info { get; set; }
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
            if (Info) ++options;
            if (Last) ++options;
            if (Report1) ++options;
            if (Report2) ++options;
            if (Report3) ++options;
            if (Reports) ++options;
            if (Number > 0) ++options;

            if (options > 1)
            {
                console.RedWriteLine("Please specifiy a single report option.");
                return false;
            }
            else if (options == 0)
            {
                console.RedWriteLine("Please select a report option (-d|-1|-2|-3|-i|-n|-r)");
                return false;
            }

            if (!string.IsNullOrEmpty(Name))
            {
                if (Data)
                {
                    console.RedWriteLine("The data option (-d|--data) can not be used with a property argument.");
                    return false;
                }

                if (Reports)
                {
                    console.RedWriteLine("The reports option (-r|--reports) can not be used with a property argument.");
                    return false;
                }

                if (Report1)
                {
                    if (!typeof(Report1Data).IsProperty(Name))
                    {
                        console.RedWriteLine($"The property '{Name}' has not been found.");
                        return false;
                    }
                }

                if (Report2)
                {
                    if (!typeof(Report2Data).IsProperty(Name))
                    {
                        console.RedWriteLine($"The property '{Name}' has not been found.");
                        return false;
                    }
                }

                if (Report3)
                {
                    if (!typeof(Report3Data).IsProperty(Name))
                    {
                        console.RedWriteLine($"The property '{Name}' has not been found.");
                        return false;
                    }
                }

                if (Last || (Number > 0))
                {
                    if (!typeof(ReportsData).IsProperty(Name))
                    {
                        console.RedWriteLine($"The property '{Name}' has not been found.");
                        return false;
                    }
                }

                if (Info)
                {
                    if (!typeof(InfoData).IsProperty(Name))
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
