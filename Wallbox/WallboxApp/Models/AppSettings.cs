// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>26-4-2020 20:19</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace WallboxApp.Models
{
    #region Using Directives

    using WallboxApp.Options;

    #endregion

    /// <summary>
    /// The application settings. The class contains all application settings as properties and are configured
    /// using application configuration files (e.g. appsettings.json), or environment variables.
    /// </summary>
    public class AppSettings
    {
        public GlobalOptions GlobalOptions { get; set; } = new GlobalOptions();
    }
}
