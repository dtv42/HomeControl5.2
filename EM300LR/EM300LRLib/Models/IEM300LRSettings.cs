// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEM300LRSettings.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>17-12-2020 12:51</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace EM300LRLib.Models
{
    #region Using Directives

    using UtilityLib.Webapp;

    #endregion Using Directives

    /// <summary>
    /// Interface supporting all EM300LR settings (password, and serial number).
    /// </summary>
    public interface IEM300LRSettings : IHttpClientSettings
    {
        /// <summary>
        /// Login password for the EM300LR web service.
        /// </summary>
        string Password { get; set; }

        /// <summary>
        /// Serial number of the EM300LR device used in login.
        /// </summary>
        string SerialNumber { get; set; }
    }
}