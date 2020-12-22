// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Phase3Data.cs" company="DTV-Online">
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
    /// Class holding selected data from the b-Control EM300LR energy manager.
    /// Note that this class uses the property names for JSON serialization.
    /// </summary>
    public class Phase3Data
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
        /// Updates the Properties used in EM300LR data.
        /// </summary>
        /// <param name="data">The EM300LR data.</param>
        public void Refresh(EM300LRTcpData data)
        {
            ActivePowerPlus = data.ActivePowerPlusL3;
            ActiveEnergyPlus = data.ActiveEnergyPlusL3;
            ActivePowerMinus = data.ActivePowerMinusL3;
            ActiveEnergyMinus = data.ActiveEnergyMinusL3;
            ReactivePowerPlus = data.ReactivePowerPlusL3;
            ReactiveEnergyPlus = data.ReactiveEnergyPlusL3;
            ReactivePowerMinus = data.ReactivePowerMinusL3;
            ReactiveEnergyMinus = data.ReactiveEnergyMinusL3;
            ApparentPowerPlus = data.ApparentPowerPlusL3;
            ApparentEnergyPlus = data.ApparentEnergyPlusL3;
            ApparentPowerMinus = data.ApparentPowerMinusL3;
            ApparentEnergyMinus = data.ApparentEnergyMinusL3;
            PowerFactor = data.PowerFactorL3;
            Current = data.CurrentL3;
            Voltage = data.VoltageL3;
        }

        #endregion Public Methods
    }
}
