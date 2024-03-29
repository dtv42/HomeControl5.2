﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WallboxSettings.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>26-4-2020 20:19</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace WallboxLib.Models
{
    #region Using Directives

    using System.ComponentModel.DataAnnotations;

    using UtilityLib;
    using UtilityLib.Webapp;

    #endregion Using Directives

    /// <summary>
    /// Class holding all Fronius settings.
    /// </summary>
    public class WallboxSettings : IWallboxSettings
    {
        [IPEndPoint]
        public string EndPoint { get; set; } = "localhost";

        [Range(0, 65535)] 
        public int Port { get; set; }

        [Range(0, double.MaxValue)]
        public double Timeout { get; set; }
    }
}