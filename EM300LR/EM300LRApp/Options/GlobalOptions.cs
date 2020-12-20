// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GlobalOptions.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>10-12-2020 16:19</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace EM300LRApp.Options
{
    #region Using Directives

    using System;
    using System.ComponentModel.DataAnnotations;

    using UtilityLib;
    using UtilityLib.Console;

    using EM300LRLib.Models;

    #endregion Using Directives

    /// <summary>
    /// The application global options. The default global options are inherited from <see cref="BaseOptions"/>.
    /// Note that secret options like the Password option is typically set using the ASP.NET Core Secret Manager. 
    /// </summary>
    public class GlobalOptions : BaseOptions, IEM300LRSettings
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
        public int Timeout { get; set; } = 10000;

        /// <summary>
        /// Login password for the EM300LR web service.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Serial number of the EM300LR device used in login.
        /// </summary>
        public string SerialNumber { get; set; } = string.Empty;
    }
}
