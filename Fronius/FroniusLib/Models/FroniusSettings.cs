// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingsData.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
// Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace FroniusLib.Models
{
    #region Using Directives

    using System.ComponentModel.DataAnnotations;

    using UtilityLib;

    #endregion Using Directives

    /// <summary>
    /// Class holding all Fronius settings (device ID).
    /// </summary>
    public class FroniusSettings : IFroniusSettings
    {
        /// <summary>
        /// The Http client base address.
        /// </summary>
        [Uri]
        public string Address { get; set; } = "http://localhost";

        /// <summary>
        /// The Http client timeout (msec).
        /// </summary>
        [Range(0, int.MaxValue)]
        public int Timeout { get; set; } = 5000;

        /// <summary>
        /// The Fronius settings;
        /// </summary>
        public string DeviceID { get; set; } = string.Empty;
    }
}