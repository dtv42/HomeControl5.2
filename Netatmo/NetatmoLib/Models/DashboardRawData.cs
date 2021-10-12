// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DashboardData.cs" company="DTV-Online">
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

    using System.Text.Json.Serialization;

    #endregion

    public class DashboardRawData
    {
        [JsonPropertyName("time_utc")]
        public long TimeUtc { get; set; }

        [JsonPropertyName("Temperature")]
        public double Temperature { get; set; }

        [JsonPropertyName("CO2")]
        public double CO2 { get; set; }

        [JsonPropertyName("Humidity")]
        public double Humidity { get; set; }

        [JsonPropertyName("Noise")]
        public double Noise { get; set; }

        [JsonPropertyName("Pressure")]
        public double Pressure { get; set; }

        [JsonPropertyName("AbsolutePressure")]
        public double AbsolutePressure { get; set; }

        [JsonPropertyName("min_temp")]
        public double MinTemp { get; set; }

        [JsonPropertyName("max_temp")]
        public double MaxTemp { get; set; }

        [JsonPropertyName("date_max_temp")]
        public long DateMaxTemp { get; set; }

        [JsonPropertyName("date_min_temp")]
        public long DateMinTemp { get; set; }

        [JsonPropertyName("pressure_trend")]
        public string PressureTrend { get; set; } = string.Empty;

        [JsonPropertyName("Rain")]
        public double Rain { get; set; }

        [JsonPropertyName("sum_rain_1")]
        public double SumRain1 { get; set; }

        [JsonPropertyName("sum_rain_24")]
        public double SumRain24 { get; set; }

        [JsonPropertyName("WindStrength")]
        public double WindStrength { get; set; }

        [JsonPropertyName("WindAngle")]
        public double WindAngle { get; set; }

        [JsonPropertyName("GustStrength")]
        public double GustStrength { get; set; }

        [JsonPropertyName("GustAngle")]
        public double GustAngle { get; set; }

        [JsonPropertyName("max_wind_str")]
        public double MaxWindStr { get; set; }

        [JsonPropertyName("max_wind_angle")]
        public double MaxWindAngle { get; set; }

        [JsonPropertyName("date_max_wind_str")]
        public long DateMaxWindStr { get; set; }
    }
}
