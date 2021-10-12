// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NetatmoData.cs" company="DTV-Online">
//   Copyright (c) 2021 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>15-1-2021 10:46</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace NetatmoLib.Models
{
    #region Using Directives

    using System.Text.Json.Serialization;

    #endregion

    /// <summary>
    /// Class holding JSON data from the Netatmo weather station.
    /// </summary>
    public class NetatmoData
    {
        [JsonPropertyName("body")]
        public BodyRawData Body { get; set; } = new BodyRawData();

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("time_exec")]
        public double TimeExec { get; set; }

        [JsonPropertyName("time_server")]
        public long TimeServer { get; set; }
    }
}
