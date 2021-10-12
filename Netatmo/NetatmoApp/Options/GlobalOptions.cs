// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GlobalOptions.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>19-12-2020 17:54</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace NetatmoApp.Options
{
    #region Using Directives

    using System;
    using System.ComponentModel.DataAnnotations;

    using UtilityLib;
    using UtilityLib.Console;

    using NetatmoLib.Models;

    #endregion Using Directives

    /// <summary>
    /// The application global options. The default global options are inherited from <see cref="BaseOptions"/>.
    /// Note that secret options like the Password option is typically set using the ASP.NET Core Secret Manager. 
    /// </summary>
    public class GlobalOptions : BaseOptions, INetatmoSettings
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
        /// Login user name for the Netatmo web service.
        /// </summary>
        public string User { get; set; } = string.Empty;

        /// <summary>
        /// Login password for the Netatmo web service.
        /// </summary>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Client ID of the Netatmo web service used in login.
        /// </summary>
        public string ClientID { get; set; } = string.Empty;

        /// <summary>
        /// Client secret of the Netatmo web service used in login.
        /// </summary>
        public string ClientSecret { get; set; } = string.Empty;
    }
}
