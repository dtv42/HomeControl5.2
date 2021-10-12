// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlaceData.cs" company="DTV-Online">
//   Copyright (c) 2021 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>15-1-2021 15:54</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace NetatmoLib.Models
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    #endregion

    public class PlaceRawData
    {
        [JsonPropertyName("altitude")]
        public int Altitude { get; set; }

        [JsonPropertyName("city")]
        public string City { get; set; } = string.Empty;

        [JsonPropertyName("country")]
        public string Country { get; set; } = string.Empty;

        [JsonPropertyName("timezone")]
        public string Timezone { get; set; } = string.Empty;

        [JsonPropertyName("location")]
        public List<double> Location { get; set; } = new List<double> { };
    }
}
