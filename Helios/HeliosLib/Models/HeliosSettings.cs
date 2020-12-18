// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeliosSettings.cs" company="DTV-Online">
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
    #region Using Directives

    using UtilityLib;

    #endregion Using Directives

    /// <summary>
    /// Class holding all Helios settings (Password).
    /// </summary>
    public class HeliosSettings : HttpClientSettings
    {
        /// <summary>
        /// The Helios settings;
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}