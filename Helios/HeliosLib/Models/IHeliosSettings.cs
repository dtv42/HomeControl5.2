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

    using UtilityLib.Webapp;

    #endregion Using Directives

    /// <summary>
    /// Interface for Helios settings (Password).
    /// </summary>
    public interface IHeliosSettings : IHttpClientSettings
    {
        /// <summary>
        /// The Helios settings;
        /// </summary>
        string Password { get; set; }
    }
}