// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Phase1Data.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>17-12-2020 12:51</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace EM300LRLib.Models
{
    /// <summary>
    /// Class holding selected data from the b-Control data energy manager.
    /// Note that this class uses the property names for JSON serialization.
    /// </summary>
    public class Phase1Data
    {
        #region Public Properties

        public double ActivePowerPlus     { get; set; }
        public double ActiveEnergyPlus    { get; set; }
        public double ActivePowerMinus    { get; set; }
        public double ActiveEnergyMinus   { get; set; }
        public double ReactivePowerPlus   { get; set; }
        public double ReactiveEnergyPlus  { get; set; }
        public double ReactivePowerMinus  { get; set; }
        public double ReactiveEnergyMinus { get; set; }
        public double ApparentPowerPlus   { get; set; }
        public double ApparentEnergyPlus  { get; set; }
        public double ApparentPowerMinus  { get; set; }
        public double ApparentEnergyMinus { get; set; }
        public double PowerFactor         { get; set; }
        public double Current             { get; set; }
        public double Voltage             { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Updates the Properties used in Phase 1 data.
        /// </summary>
        /// <param name="data">The data data.</param>
        public void Refresh(EM300LRTcpData data)
        {
            ActivePowerPlus = data.ActivePowerPlusL1;
            ActiveEnergyPlus = data.ActiveEnergyPlusL1;
            ActivePowerMinus = data.ActivePowerMinusL1;
            ActiveEnergyMinus = data.ActiveEnergyMinusL1;
            ReactivePowerPlus = data.ReactivePowerPlusL1;
            ReactiveEnergyPlus = data.ReactiveEnergyPlusL1;
            ReactivePowerMinus = data.ReactivePowerMinusL1;
            ReactiveEnergyMinus = data.ReactiveEnergyMinusL1;
            ApparentPowerPlus = data.ApparentPowerPlusL1;
            ApparentEnergyPlus = data.ApparentEnergyPlusL1;
            ApparentPowerMinus = data.ApparentPowerMinusL1;
            ApparentEnergyMinus = data.ApparentEnergyMinusL1;
            PowerFactor = data.PowerFactorL1;
            Current = data.CurrentL1;
            Voltage = data.VoltageL1;
        }

        #endregion Public Methods
    }
}
