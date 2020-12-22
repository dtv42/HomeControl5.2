// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace FroniusWeb.Models
{
    #region Using Directives

    using UtilityLib.Webapp;

    using FroniusLib.Models;

    #endregion Using Directives

    /// <summary>
    /// Only the Fronius settings from <see cref="FroniusLib.Models.FroniusSettings"/> are used here. 
    /// </summary>
    public class AppSettings
    {
        public FroniusSettings GatewaySettings { get; set; } = new FroniusSettings();
        public PingHealthCheckOptions PingOptions { get; set; } = new PingHealthCheckOptions();
    }
}