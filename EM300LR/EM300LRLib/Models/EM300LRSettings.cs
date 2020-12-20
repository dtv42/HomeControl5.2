// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EM300LRSettings.cs" company="DTV-Online">
//   Copyright(c) 2019 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace EM300LRLib.Models
{
    #region Using Directives

    using System;
    using System.ComponentModel.DataAnnotations;

    using UtilityLib;

    #endregion Using Directives

    /// <summary>
    /// Class holding all EM300LR settings (password, and serial number).
    /// </summary>
    public class EM300LRSettings : IEM300LRSettings
    {
        #region Public Properties

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
        /// Login password for the EM300LR web service.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Serial number of the EM300LR device used in login.
        /// </summary>
        public string SerialNumber { get; set; } = string.Empty;

        #endregion
    }
}