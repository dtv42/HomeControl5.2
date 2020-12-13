// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestdataOptions.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>10-12-2020 17:09</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace UtilityApp.Options
{
    #region Using Directives

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Net;
    using System.Text.Json.Serialization;
    using UtilityApp.Models;
    using UtilityLib;

    #endregion

    /// <summary>
    ///  A collection of options for the validate command. The testdata fields are used as options.
    /// </summary>
    internal class TestdataOptions : TestData
    {
        public bool Data { get; set; }
    }
}
