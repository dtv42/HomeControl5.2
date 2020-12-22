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
namespace EM300LRApp.Options
{
    #region Using Directives

    using System.CommandLine;

    using UtilityLib.Console;

    using EM300LRLib.Models;

    #endregion Using Directives

    /// <summary>
    /// The options for the read sub command.
    /// </summary>
    public class ReadOptions
    {
        public string Name { get; set; } = string.Empty;
        public bool Data { get; set; }
        public bool Total { get; set; }
        public bool Phase1 { get; set; }
        public bool Phase2 { get; set; }
        public bool Phase3 { get; set; }
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
            if (Total) ++options;
            if (Phase1) ++options;
            if (Phase2) ++options;
            if (Phase3) ++options;

            if (options > 1)
            {
                console.RedWriteLine("Please specifiy a single property type option.");
                return false;
            }
            else if (options == 0)
            {
                console.RedWriteLine("Please select a single property type (-d|-t|-1|-2|-3)");
                return false;
            }

            if (!string.IsNullOrEmpty(Name))
            {
                if (Data)
                {
                    if (typeof(EM300LRData).GetProperty(Name) is null)
                    {
                        console.RedWriteLine($"The property '{Name}' has not been found.");
                        return false;
                    }
                }

                if (Total)
                {
                    if (typeof(TotalData).GetProperty(Name) is null)
                    {
                        console.RedWriteLine($"The property '{Name}' has not been found.");
                        return false;
                    }
                }

                if (Phase1)
                {
                    if (typeof(Phase1Data).GetProperty(Name) is null)
                    {
                        console.RedWriteLine($"The property '{Name}' has not been found.");
                        return false;
                    }
                }

                if (Phase2)
                {
                    if (typeof(Phase2Data).GetProperty(Name) is null)
                    {
                        console.RedWriteLine($"The property '{Name}' has not been found.");
                        return false;
                    }
                }

                if (Phase3)
                {
                    if (typeof(Phase3Data).GetProperty(Name) is null)
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
