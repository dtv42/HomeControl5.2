// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StorageData.cs" company="DTV-Online">
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

    public class StorageData
    {
        #region Public Properties

        /// <summary>
        /// The ETAPU11 property subset.
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public DemandValuesEx DischargeScrewDemand { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ScrewStates DischargeScrewState { get; set; }
        public double DischargeScrewMotorCurr { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public ConveyingSystemStates ConveyingSystem { get; set; }
        public double Stock { get; set; }
        public double StockWarningLimit { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the Properties used in StorageData.
        /// </summary>
        /// <param name="data">The ETAPU11 data.</param>
        public void Refresh(ETAPU11Data data)
        {
            DischargeScrewDemand = data.DischargeScrewDemand;
            DischargeScrewState = data.DischargeScrewState;
            DischargeScrewMotorCurr = data.DischargeScrewMotorCurr;
            ConveyingSystem = data.ConveyingSystem;
            Stock = data.Stock;
            StockWarningLimit = data.StockWarningLimit;
        }

        #endregion
    }
}