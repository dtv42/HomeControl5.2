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

    using UtilityLib;

    #endregion Using Directives

    /// <summary>
    /// Class holding all Fronius settings (device ID).
    /// </summary>
    public class FroniusSettings : HttpClientSettings
    {
        /// <summary>
        /// The Fronius settings;
        /// </summary>
        public string DeviceID { get; set; } = string.Empty;
    }
}