// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ModuleData.cs" company="DTV-Online">
//   Copyright (c) 2021 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>15-1-2021 15:55</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace NetatmoLib.Models
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    #endregion

    public class ModuleRawData
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("module_name")]
        public string ModuleName { get; set; } = string.Empty;

        [JsonPropertyName("last_setup")]
        public long LastSetup { get; set; }

        [JsonPropertyName("data_type")]
        public List<string> DataType { get; set; } = new List<string> { };

        [JsonPropertyName("battery_percent")]
        public int BatteryPercent { get; set; }

        [JsonPropertyName("reachable")]
        public bool Reachable { get; set; }

        [JsonPropertyName("firmware")]
        public int Firmware { get; set; }

        [JsonPropertyName("last_message")]
        public long LastMessage { get; set; }

        [JsonPropertyName("last_seen")]
        public long LastSeen { get; set; }

        [JsonPropertyName("rf_status")]
        public int RfStatus { get; set; }

        [JsonPropertyName("battery_vp")]
        public int BatteryVp { get; set; }

        [JsonPropertyName("dashboard_data")]
        public DashboardRawData DashboardData { get; set; } = new DashboardRawData();
    }
}
