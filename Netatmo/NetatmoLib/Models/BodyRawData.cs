// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BodyData.cs" company="DTV-Online">
//   Copyright (c) 2021 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>15-1-2021 15:57</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace NetatmoLib.Models
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    #endregion

    public class BodyRawData
    {
        [JsonPropertyName("devices")]
        public List<DeviceRawData> Devices { get; set; } = new List<DeviceRawData>();

        [JsonPropertyName("user")]
        public UserRawData User { get; set; } = new UserRawData();
    }
}
