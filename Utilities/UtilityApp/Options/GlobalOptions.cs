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
namespace UtilityApp.Options
{
    #region Using Directives

    using System;

    using UtilityLib;

    #endregion Using Directives

    /// <summary>
    /// The application global options. The default global options are inherited from <see cref="BaseOptions"/>.
    /// Note that secret options like the Password option is typically set using the ASP.NET Core Secret Manager. 
    /// </summary>
    public class GlobalOptions : BaseOptions
    {
        public string Password { get; set; } = string.Empty;
        public string Host { get; set; } = "http://localhost";
    }
}
