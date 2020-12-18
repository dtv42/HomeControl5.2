// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HotWaterData.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>24-4-2020 16:40</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace ETAPU11Lib.Models
{
    #region Using Directives

    using System.Text.Json.Serialization;

    using static ETAPU11Lib.Models.ETAPU11Data;

    #endregion

    public class HotwaterData
    {
        #region Public Properties

        /// <summary>
        /// The ETAPU11 property subset.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public HWTankStates HotwaterTankState { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OnOffStates ChargingTimesState { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OnOffStates ChargingTimesSwitchStatus { get; set; }
        public double ChargingTimesTemperature { get; set; }
        public double HotwaterSwitchonDiff { get; set; }
        public double HotwaterTarget { get; set; }
        public double HotwaterTemperature { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the Properties used in HotwaterData.
        /// </summary>
        /// <param name="data">The ETAPU11 data.</param>
        public void Refresh(ETAPU11Data data)
        {
            HotwaterTankState = data.HotwaterTankState;
            ChargingTimesState = data.ChargingTimesState;
            ChargingTimesSwitchStatus = data.ChargingTimesSwitchStatus;
            ChargingTimesTemperature = data.ChargingTimesTemperature;
            HotwaterSwitchonDiff = data.HotwaterSwitchonDiff;
            HotwaterTarget = data.HotwaterTarget;
            HotwaterTemperature = data.HotwaterTemperature;
        }

        #endregion
    }
}