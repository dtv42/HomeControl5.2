// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>2-12-2020 11:06</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace UtilityApp.Models
{
    #region Using Directives

    using UtilityApp.Options;

    #endregion Using Directives

    /// <summary>
    /// Application specific settings using global options, selected command options and data settings.
    /// </summary>
    public sealed class AppSettings
    {
        public GlobalOptions GlobalOptions { get; set; } = new GlobalOptions();
        public GreetOptions GreetOptions { get; set; } = new GreetOptions();
        public SettingsData Data { get; set; } = new SettingsData();
    }
}
