// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Phase2Data.cs" company="DTV-Online">
//   Copyright(c) 2019 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace EM300LRLib.Models
{
    /// <summary>
    /// Class holding selected data from the b-Control EM300LR energy manager.
    /// Note that this class uses the property names for JSON serialization.
    /// </summary>
    public class Phase2Data
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
            ActivePowerPlus = data.ActivePowerPlusL2;
            ActiveEnergyPlus = data.ActiveEnergyPlusL2;
            ActivePowerMinus = data.ActivePowerMinusL2;
            ActiveEnergyMinus = data.ActiveEnergyMinusL2;
            ReactivePowerPlus = data.ReactivePowerPlusL2;
            ReactiveEnergyPlus = data.ReactiveEnergyPlusL2;
            ReactivePowerMinus = data.ReactivePowerMinusL2;
            ReactiveEnergyMinus = data.ReactiveEnergyMinusL2;
            ApparentPowerPlus = data.ApparentPowerPlusL2;
            ApparentEnergyPlus = data.ApparentEnergyPlusL2;
            ApparentPowerMinus = data.ApparentPowerMinusL2;
            ApparentEnergyMinus = data.ApparentEnergyMinusL2;
            PowerFactor = data.PowerFactorL2;
            Current = data.CurrentL2;
            Voltage = data.VoltageL2;
        }

        #endregion Public Methods
    }
}
