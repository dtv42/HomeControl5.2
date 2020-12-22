// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>17-12-2020 12:52</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace ETAPU11Web.Models
{
    #region Using Directives

    using UtilityLib.Webapp;

    using ETAPU11Lib.Models;

    #endregion

    /// <summary>
    /// Only the ETAPU11 settings from <see cref="ETAPU11Lib.Models.ETAPU11Settings"/> are used here. 
    /// </summary>
    public class AppSettings : ETAPU11Settings
    {
        public ETAPU11Settings GatewaySettings { get; set; } = new ETAPU11Settings();
        public PingHealthCheckOptions PingOptions { get; set; } = new PingHealthCheckOptions();
    }
}
