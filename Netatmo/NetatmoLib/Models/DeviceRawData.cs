// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeviceData.cs" company="DTV-Online">
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

    using System.Collections.Generic;
    using System.Text.Json.Serialization;

    #endregion

    public class DeviceRawData
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("station_name")]
        public string StationName { get; set; } = string.Empty;

        [JsonPropertyName("date_setup")]
        public long DateSetup { get; set; }

        [JsonPropertyName("last_setup")]
        public long LastSetup { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("last_status_store")]
        public long LastStatusStore { get; set; }

        [JsonPropertyName("module_name")]
        public string ModuleName { get; set; } = string.Empty;

        [JsonPropertyName("firmware")]
        public int Firmware { get; set; }

        [JsonPropertyName("last_upgrade")]
        public long LastUpgrade { get; set; }

        [JsonPropertyName("wifi_status")]
        public int WifiStatus { get; set; }

        [JsonPropertyName("reachable")]
        public bool Reachable { get; set; }

        [JsonPropertyName("co2_calibrating")]
        public bool Co2Calibrating { get; set; }

        [JsonPropertyName("data_type")]
        public List<string> DataType { get; set; } = new List<string> { };

        [JsonPropertyName("place")]
        public PlaceRawData Place { get; set; } = new PlaceRawData();

        [JsonPropertyName("home_id")]
        public string HomeId { get; set; } = string.Empty;

        [JsonPropertyName("home_name")]
        public string HomeName { get; set; } = string.Empty;

        [JsonPropertyName("dashboard_data")]
        public DashboardRawData DashboardData { get; set; } = new DashboardRawData();

        [JsonPropertyName("modules")]
        public List<ModuleRawData> Modules { get; set; } = new List<ModuleRawData> { };
    }
}
