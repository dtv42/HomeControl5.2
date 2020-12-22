﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>17-12-2020 12:52</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace ETAPU11App.Models
{
    #region Using Directives

    using ETAPU11App.Options;

    #endregion

    /// <summary>
    /// Only the ETAPU11 settings from <see cref="ETAPU11.Models.SettingsData"/> are used here. 
    /// </summary>
    public class AppSettings
    {
        public GlobalOptions GlobalOptions { get; set; } = new GlobalOptions();
    }
}
