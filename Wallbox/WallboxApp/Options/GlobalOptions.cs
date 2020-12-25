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
namespace WallboxApp.Options
{
    #region Using Directives

    using System;
    using System.ComponentModel.DataAnnotations;

    using UtilityLib;
    using UtilityLib.Console;

    using WallboxLib.Models;

    #endregion Using Directives

    /// <summary>
    /// The application global options. The default global options are inherited from <see cref="BaseOptions"/>.
    /// Note that secret options like the Password option is typically set using the ASP.NET Core Secret Manager. 
    /// </summary>
    public class GlobalOptions : BaseOptions, IWallboxSettings
    {
        /// <summary>
        /// The Udp endpoint.
        /// </summary>
        [IPEndPoint]
        public string EndPoint { get; set; } = "localhost";

        /// <summary>
        /// The Udp port number.
        /// </summary>
        [Range(0, 65535)]
        public int Port { get; set; }

        /// <summary>
        /// The Udp client timeout (msec).
        /// </summary>
        [Range(0, double.MaxValue)]
        public double Timeout { get; set; }
    }
}
