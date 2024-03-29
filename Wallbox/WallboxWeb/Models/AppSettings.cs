﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>26-4-2020 20:23</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace WallboxWeb.Models
{
    #region Using Directives

    using UtilityLib.Webapp;

    using WallboxLib.Models;

    #endregion Using Directives

    /// <summary>
    ///  Only the Wallbox settings from <see cref="WallboxLib.Models.WallboxSettings"/> are used here. 
    ///  This allows the use of UserSecrets for the gateway settings.
    /// </summary>
    public class AppSettings
    {
        public WallboxSettings GatewaySettings { get; set; } = new WallboxSettings();
        public PingHealthCheckOptions PingOptions { get; set; } = new PingHealthCheckOptions();
    }
}