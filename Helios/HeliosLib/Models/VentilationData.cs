// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VentilationData.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>26-4-2020 10:05</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace HeliosLib.Models
{
    /// <summary>
    /// Helper class to provide parameters for setting the ventilation mode (booster, standby).
    /// </summary>
    public class VentilationData
    {
        #region Public Properties

        public bool Mode { get; set; }
        public FanLevels Level { get; set; }
        public int Duration { get; set; } = 120;

        #endregion
    }
}
