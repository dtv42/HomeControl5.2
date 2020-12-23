// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ControlOptions.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>19-12-2020 22:46</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace HeliosApp.Options
{
    #region Using Directives

    using System.CommandLine;

    using UtilityLib.Console;

    using HeliosLib.Models;

    #endregion Using Directives

    /// <summary>
    /// The options for the control sub command.
    /// </summary>
    public class ControlOptions
    {
        public string Operation { get; set; } = string.Empty;
        public bool Booster { get; set; }
        public bool Standby { get; set; }
        public int Fan { get; set; } = -1;
        public string Mode { get; set; } = string.Empty;
        public int Level { get; set; } = -1;
        public int Duration { get; set; }
        public bool Status { get; set; }

        /// <summary>
        /// Helper method to check options.
        /// </summary>
        /// <param name="console">The console used for messages.</param>
        /// <returns>True if options OK.</returns>
        public bool CheckOptions(IConsole console)
        {
            int options = 0;
            var fanOption = Fan != 1;
            var modeOption = !string.IsNullOrEmpty(Mode);
            var levelOption = Level != -1;
            var durationOption = Duration > 0;
            var operationOption = !string.IsNullOrEmpty(Operation);

            if (operationOption) ++options;
            if (Booster) ++options;
            if (Standby) ++options;
            if (fanOption) ++options;

            if (options > 1)
            {
                console.RedWriteLine("Please specifiy a single control option.");
                return false;
            }
            else if (options == 0)
            {
                console.RedWriteLine("Please select a control option (-o|-b|-s|-f)");
                return false;
            }

            if (Booster && (!modeOption && !levelOption && !durationOption))
            {
                console.RedWriteLine("Please specifiy booster operation data (-m|-l|-d)");
                return false;
            }

            if (Standby && (!modeOption && !levelOption && !durationOption))
            {
                console.RedWriteLine("Please specifiy standby operation data (-m|-l|-d)");
                return false;
            }

            if (modeOption && (operationOption || fanOption))
            {
                console.YellowWriteLine("Mode data are ignored");
            }

            if (levelOption && (operationOption || fanOption))
            {
                console.YellowWriteLine("Level data are ignored");
            }

            if (durationOption && (operationOption || fanOption))
            {
                console.RedWriteLine("Duration data are ignored");
            }

            return true;
        }
    }
}
