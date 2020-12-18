// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AuthentificationData.cs" company="DTV-Online">
//   Copyright(c) 2019 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace EM300LRLib.Models
{
    #region Using Directives

    using System.Text.Json.Serialization;

    using UtilityLib;

    #endregion Using Directives

    public class AuthentificationData
    {
        [JsonPropertyName("http_statuscode")]
        public int HttpStatusCode { get; set; }

        [JsonPropertyName("ieq_serial")]
        [JsonConverter(typeof(NumberConverter))]
        public string IEQSerial { get; set; } = string.Empty;

        [JsonPropertyName("serial")]
        [JsonConverter(typeof(NumberConverter))]
        public string SerialNumber { get; set; } = string.Empty;

        [JsonPropertyName("app_version")]
        [JsonConverter(typeof(NumberConverter))]
        public string AppVersion { get; set; } = string.Empty;

        [JsonPropertyName("ieqbox_label")]
        [JsonConverter(typeof(NumberConverter))]
        public string IEQBoxLabel { get; set; } = string.Empty;

        [JsonPropertyName("auth_mode")]
        public string AuthenticationMode { get; set; } = string.Empty;

        [JsonPropertyName("authentication")]
        public bool Authentication { get; set; }
    }
}