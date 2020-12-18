// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TotalData.cs" company="DTV-Online">
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
    public class TotalData
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
        public double SupplyFrequency     { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Updates the Properties used in EM300LR total data.
        /// </summary>
        /// <param name="data">The EM300LR data.</param>
        public void Refresh(EM300LRTcpData data)
        {
            ActivePowerPlus = data.ActivePowerPlus;
            ActiveEnergyPlus = data.ActiveEnergyPlus;
            ActivePowerMinus = data.ActivePowerMinus;
            ActiveEnergyMinus = data.ActiveEnergyMinus;
            ReactivePowerPlus = data.ReactivePowerPlus;
            ReactiveEnergyPlus = data.ReactiveEnergyPlus;
            ReactivePowerMinus = data.ReactivePowerMinus;
            ReactiveEnergyMinus = data.ReactiveEnergyMinus;
            ApparentPowerPlus = data.ApparentPowerPlus;
            ApparentEnergyPlus = data.ApparentEnergyPlus;
            ApparentPowerMinus = data.ApparentPowerMinus;
            ApparentEnergyMinus = data.ApparentEnergyMinus;
            PowerFactor = data.PowerFactor;
            SupplyFrequency = data.SupplyFrequency;
        }

        #endregion Public Methods
    }
}