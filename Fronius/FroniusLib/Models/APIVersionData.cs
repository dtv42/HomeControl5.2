// --------------------------------------------------------------------------------------------------------------------
// <copyright file="APIVersionData.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
// Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace FroniusLib.Models
{
    #region Using Directives

    using System.Text.Json.Serialization;

    using UtilityLib;

    #endregion Using Directives

    public class APIVersionData
    {
        [JsonConverter(typeof(NumberConverter))]
        public string APIVersion { get; set; } = string.Empty;
        public string BaseURL { get; set; } = string.Empty;
        public string CompatibilityRange { get; set; } = string.Empty;
    }
}
