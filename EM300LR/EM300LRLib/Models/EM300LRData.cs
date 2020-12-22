// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EM300LRData.cs" company="DTV-Online">
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
    /// Class holding all data from the b-Control EM300LR energy manager.
    /// </summary>
    public class EM300LRData
    {
        #region Public Properties

        public string Serial { get; set; } = string.Empty;

        public double ActivePowerPlus       { get; set; }
        public double ActiveEnergyPlus      { get; set; }
        public double ActivePowerMinus      { get; set; }
        public double ActiveEnergyMinus     { get; set; }
        public double ReactivePowerPlus     { get; set; }
        public double ReactiveEnergyPlus    { get; set; }
        public double ReactivePowerMinus    { get; set; }
        public double ReactiveEnergyMinus   { get; set; }
        public double ApparentPowerPlus     { get; set; }
        public double ApparentEnergyPlus    { get; set; }
        public double ApparentPowerMinus    { get; set; }
        public double ApparentEnergyMinus   { get; set; }
        public double PowerFactor           { get; set; }
        public double SupplyFrequency       { get; set; }
        public double ActivePowerPlusL1     { get; set; }
        public double ActiveEnergyPlusL1    { get; set; }
        public double ActivePowerMinusL1    { get; set; }
        public double ActiveEnergyMinusL1   { get; set; }
        public double ReactivePowerPlusL1   { get; set; }
        public double ReactiveEnergyPlusL1  { get; set; }
        public double ReactivePowerMinusL1  { get; set; }
        public double ReactiveEnergyMinusL1 { get; set; }
        public double ApparentPowerPlusL1   { get; set; }
        public double ApparentEnergyPlusL1  { get; set; }
        public double ApparentPowerMinusL1  { get; set; }
        public double ApparentEnergyMinusL1 { get; set; }
        public double CurrentL1             { get; set; }
        public double VoltageL1             { get; set; }
        public double PowerFactorL1         { get; set; }
        public double ActivePowerPlusL2     { get; set; }
        public double ActiveEnergyPlusL2    { get; set; }
        public double ActivePowerMinusL2    { get; set; }
        public double ActiveEnergyMinusL2   { get; set; }
        public double ReactivePowerPlusL2   { get; set; }
        public double ReactiveEnergyPlusL2  { get; set; }
        public double ReactivePowerMinusL2  { get; set; }
        public double ReactiveEnergyMinusL2 { get; set; }
        public double ApparentPowerPlusL2   { get; set; }
        public double ApparentEnergyPlusL2  { get; set; }
        public double ApparentPowerMinusL2  { get; set; }
        public double ApparentEnergyMinusL2 { get; set; }
        public double CurrentL2             { get; set; }
        public double VoltageL2             { get; set; }
        public double PowerFactorL2         { get; set; }
        public double ActivePowerPlusL3     { get; set; }
        public double ActiveEnergyPlusL3    { get; set; }
        public double ActivePowerMinusL3    { get; set; }
        public double ActiveEnergyMinusL3   { get; set; }
        public double ReactivePowerPlusL3   { get; set; }
        public double ReactiveEnergyPlusL3  { get; set; }
        public double ReactivePowerMinusL3  { get; set; }
        public double ReactiveEnergyMinusL3 { get; set; }
        public double ApparentPowerPlusL3   { get; set; }
        public double ApparentEnergyPlusL3  { get; set; }
        public double ApparentPowerMinusL3  { get; set; }
        public double ApparentEnergyMinusL3 { get; set; }
        public double CurrentL3             { get; set; }
        public double VoltageL3             { get; set; }
        public double PowerFactorL3         { get; set; }

        public int    StatusCode            { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Updates the Properties used in EM300LR data.
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
            ActivePowerPlusL1 = data.ActivePowerPlusL1;
            ActiveEnergyPlusL1 = data.ActiveEnergyPlusL1;
            ActivePowerMinusL1 = data.ActivePowerMinusL1;
            ActiveEnergyMinusL1 = data.ActiveEnergyMinusL1;
            ReactivePowerPlusL1 = data.ReactivePowerPlusL1;
            ReactiveEnergyPlusL1 = data.ReactiveEnergyPlusL1;
            ReactivePowerMinusL1 = data.ReactivePowerMinusL1;
            ReactiveEnergyMinusL1 = data.ReactiveEnergyMinusL1;
            ApparentPowerPlusL1 = data.ApparentPowerPlusL1;
            ApparentEnergyPlusL1 = data.ApparentEnergyPlusL1;
            ApparentPowerMinusL1 = data.ApparentPowerMinusL1;
            ApparentEnergyMinusL1 = data.ApparentEnergyMinusL1;
            CurrentL1 = data.CurrentL1;
            VoltageL1 = data.VoltageL1;
            PowerFactorL1 = data.PowerFactorL1;
            ActivePowerPlusL2 = data.ActivePowerPlusL2;
            ActiveEnergyPlusL2 = data.ActiveEnergyPlusL2;
            ActivePowerMinusL2 = data.ActivePowerMinusL2;
            ActiveEnergyMinusL2 = data.ActiveEnergyMinusL2;
            ReactivePowerPlusL2 = data.ReactivePowerPlusL2;
            ReactiveEnergyPlusL2 = data.ReactiveEnergyPlusL2;
            ReactivePowerMinusL2 = data.ReactivePowerMinusL2;
            ReactiveEnergyMinusL2 = data.ReactiveEnergyMinusL2;
            ApparentPowerPlusL2 = data.ApparentPowerPlusL2;
            ApparentEnergyPlusL2 = data.ApparentEnergyPlusL2;
            ApparentPowerMinusL2 = data.ApparentPowerMinusL2;
            ApparentEnergyMinusL2 = data.ApparentEnergyMinusL2;
            CurrentL2 = data.CurrentL2;
            VoltageL2 = data.VoltageL2;
            PowerFactorL2 = data.PowerFactorL2;
            ActivePowerPlusL3 = data.ActivePowerPlusL3;
            ActiveEnergyPlusL3 = data.ActiveEnergyPlusL3;
            ActivePowerMinusL3 = data.ActivePowerMinusL3;
            ActiveEnergyMinusL3 = data.ActiveEnergyMinusL3;
            ReactivePowerPlusL3 = data.ReactivePowerPlusL3;
            ReactiveEnergyPlusL3 = data.ReactiveEnergyPlusL3;
            ReactivePowerMinusL3 = data.ReactivePowerMinusL3;
            ReactiveEnergyMinusL3 = data.ReactiveEnergyMinusL3;
            ApparentPowerPlusL3 = data.ApparentPowerPlusL3;
            ApparentEnergyPlusL3 = data.ApparentEnergyPlusL3;
            ApparentPowerMinusL3 = data.ApparentPowerMinusL3;
            ApparentEnergyMinusL3 = data.ApparentEnergyMinusL3;
            CurrentL3 = data.CurrentL3;
            VoltageL3 = data.VoltageL3;
            PowerFactorL3 = data.PowerFactorL3;
            Serial = data.Serial;
            StatusCode = data.StatusCode;
        }

        #endregion
    }
}
