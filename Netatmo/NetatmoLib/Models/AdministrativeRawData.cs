// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AdministrativeData.cs" company="DTV-Online">
//   Copyright (c) 2021 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>15-1-2021 15:56</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace NetatmoLib.Models
{
    #region Using Directives

    using System.Text.Json.Serialization;

    #endregion

    public class AdministrativeRawData
    {
        [JsonPropertyName("country")]
        public string Country { get; set; } = string.Empty;

        [JsonPropertyName("feel_like_algo")]
        public int FeelLikeAlgo { get; set; }

        [JsonPropertyName("lang")]
        public string Lang { get; set; } = string.Empty;

        [JsonPropertyName("pressureunit")]
        public int Pressureunit { get; set; }

        [JsonPropertyName("reg_locale")]
        public string RegLocale { get; set; } = string.Empty;

        [JsonPropertyName("unit")]
        public int Unit { get; set; }

        [JsonPropertyName("windunit")]
        public int Windunit { get; set; }
    }
}
