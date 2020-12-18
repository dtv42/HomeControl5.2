// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BoilerData.cs" company="DTV-Online">
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

    using System;
    using System.Text.Json.Serialization;

    using static ETAPU11Lib.Models.ETAPU11Data;

    #endregion

    public class BoilerData
    {
        #region Public Properties

        /// <summary>
        /// The ETAPU11 property subset.
        /// </summary>
        public TimeSpan FullLoadHours { get; set; }
        public double TotalConsumed { get; set; }
        public double ConsumptionSinceDeAsh { get; set; }
        public double ConsumptionSinceAshBoxEmptied { get; set; }
        public double ConsumptionSinceMaintainence { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public StartValues HopperFillUpPelletBin { get; set; }
        public double HopperPelletBinContents { get; set; }
        public TimeSpan HopperFillUpTime { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public BoilerStates BoilerState { get; set; }
        public double BoilerPressure { get; set; }
        public double BoilerTemperature { get; set; }
        public double BoilerTarget { get; set; }
        public double BoilerBottom { get; set; }
        public double FlueGasTemperature { get; set; }
        public double DraughtFanSpeed { get; set; }
        public double ResidualO2 { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the Properties used in BoilerData.
        /// </summary>
        /// <param name="data">The ETAPU11 data.</param>
        public void Refresh(ETAPU11Data data)
        {
            FullLoadHours = data.FullLoadHours;
            TotalConsumed = data.TotalConsumed;
            ConsumptionSinceDeAsh = data.ConsumptionSinceDeAsh;
            ConsumptionSinceAshBoxEmptied = data.ConsumptionSinceAshBoxEmptied;
            ConsumptionSinceMaintainence = data.ConsumptionSinceMaintainence;
            HopperFillUpPelletBin = data.HopperFillUpPelletBin;
            HopperPelletBinContents = data.HopperPelletBinContents;
            HopperFillUpTime = data.HopperFillUpTime;
            BoilerState = data.BoilerState;
            BoilerPressure = data.BoilerPressure;
            BoilerTemperature = data.BoilerTemperature;
            BoilerTarget = data.BoilerTarget;
            BoilerBottom = data.BoilerBottom;
            FlueGasTemperature = data.FlueGasTemperature;
            DraughtFanSpeed = data.DraughtFanSpeed;
            ResidualO2 = data.ResidualO2;
        }

        #endregion
    }
}