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

    using UtilityLib.Webapp;

    #endregion Using Directives

    /// <summary>
    /// Interface for Fronius settings (device ID).
    /// </summary>
    public interface IFroniusSettings : IHttpClientSettings
    {
        /// <summary>
        /// The Fronius settings;
        /// </summary>
        string DeviceID { get; set; }
    }
}