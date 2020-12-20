// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace EM300LRWeb.Models
{
    #region Using Directives

    using EM300LRLib.Models;
    using UtilityLib.Webapp;

    #endregion Using Directives

    /// <summary>
    ///  Only the EM300LR settings from <see cref="EM300LRLib.Models.EM300LRSettings"/> are used here. 
    ///  This allows the use of UserSecrets for the gateway settings.
    /// </summary>
    public class AppSettings
    {
        public EM300LRSettings GatewaySettings { get; set; } = new EM300LRSettings();
        public PingHealthCheckOptions PingOptions { get; set; } = new PingHealthCheckOptions();
    }
}