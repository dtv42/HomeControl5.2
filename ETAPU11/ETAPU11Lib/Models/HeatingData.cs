// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeatingData.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>17-12-2020 12:52</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace ETAPU11Lib.Models
{
    #region Using Directives

    using System.Text.Json.Serialization;

    using static ETAPU11Lib.Models.ETAPU11Data;

    #endregion

    public class HeatingData
    {
        #region Public Properties

        /// <summary>
        /// The ETAPU11 property subset.
        /// </summary>
        public double RoomSensor { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public HeatingCircuitStates HeatingCircuitState { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public HWRunningStates RunningState { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OnOffStates HeatingTimes { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OnOffStates HeatingSwitchStatus { get; set; }
        public double HeatingTemperature { get; set; }
        public double RoomTemperature { get; set; }
        public double RoomTarget { get; set; }
        public double Flow { get; set; }
        public double DayHeatingThreshold { get; set; }
        public double NightHeatingThreshold { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the Properties used in HeatingData.
        /// </summary>
        /// <param name="data">The ETAPU11 data.</param>
        public void Refresh(ETAPU11Data data)
        {
            RoomSensor = data.RoomSensor;
            HeatingCircuitState = data.HeatingCircuitState;
            RunningState = data.RunningState;
            HeatingTimes = data.HeatingTimes;
            HeatingSwitchStatus = data.HeatingSwitchStatus;
            HeatingTemperature = data.HeatingTemperature;
            RoomTemperature = data.RoomTemperature;
            RoomTarget = data.RoomTarget;
            Flow = data.Flow;
            DayHeatingThreshold = data.DayHeatingThreshold;
            NightHeatingThreshold = data.NightHeatingThreshold;
        }

        #endregion
    }
}