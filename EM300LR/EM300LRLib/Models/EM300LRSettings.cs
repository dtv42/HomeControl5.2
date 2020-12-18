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

        [AbsoluteUri]
        public string BaseAddress { get; set; } = "http://localhost";

        [Range(0, Int32.MaxValue)]
        public int Timeout { get; set; } = 100;

        public string Password { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;

        #endregion
    }
}