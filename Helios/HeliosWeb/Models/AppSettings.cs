// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace HeliosWeb.Models
{
    #region Using Directives

    using UtilityLib.Webapp;

    using HeliosLib.Models;

    #endregion Using Directives

    /// <summary>
    /// Only the Helios settings from <see cref="HeliosLib.Models.HeliosSettings"/> are used here. 
    /// </summary>
    public class AppSettings
    {
        public HeliosSettings GatewaySettings { get; set; } = new HeliosSettings();
        public PingHealthCheckOptions PingOptions { get; set; } = new PingHealthCheckOptions();
    }
}