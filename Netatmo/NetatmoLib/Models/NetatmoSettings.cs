// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NetatmoSettings.cs" company="DTV-Online">
//   Copyright (c) 2021 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>15-1-2021 10:46</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace NetatmoLib.Models
{
    #region Using Directives

    using System.ComponentModel.DataAnnotations;

    using UtilityLib;

    #endregion Using Directives

    /// <summary>
    /// Class holding all Helios settings (Password).
    /// </summary>
    public class NetatmoSettings : INetatmoSettings
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
        /// The Netatmo settings;
        /// </summary>
        public string User { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string ClientID { get; set; } = string.Empty;
        public string ClientSecret { get; set; } = string.Empty;
    }
}