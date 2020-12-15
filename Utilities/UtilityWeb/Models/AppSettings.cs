﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>14-12-2020 15:00</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace UtilityWeb.Models
{
    /// <summary>
    /// Application specific settings using data settings.
    /// </summary>
    public sealed class AppSettings
    {
        public SettingsData Data { get; set; } = new SettingsData();
    }
}