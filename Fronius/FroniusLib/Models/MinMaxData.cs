// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MinMaxData.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
// Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace FroniusLib.Models
{
    /// <summary>
    /// Class holding selected data from the Fronius Symo 8.2-3-M inverter.
    /// </summary>
    public class MinMaxData
    {
        #region Public Properties

        public double DailyMaxVoltageDC { get; set; }
        public double DailyMaxVoltageAC { get; set; }
        public double DailyMinVoltageAC { get; set; }
        public double YearlyMaxVoltageDC { get; set; }
        public double YearlyMaxVoltageAC { get; set; }
        public double YearlyMinVoltageAC { get; set; }
        public double TotalMaxVoltageDC { get; set; }
        public double TotalMaxVoltageAC { get; set; }
        public double TotalMinVoltageAC { get; set; }
        public double DailyMaxPower { get ; set; }
        public double YearlyMaxPower { get; set; }
        public double TotalMaxPower { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the Properties used in MinMaxData.
        /// </summary>
        /// <param name="data">The minmax device data.</param>
        public void Refresh(MinMaxDeviceData data)
        {
            DailyMaxVoltageDC = data.Inverter.DailyMaxVoltageDC.Value;
            DailyMaxVoltageAC = data.Inverter.DailyMaxVoltageAC.Value;
            DailyMinVoltageAC = data.Inverter.DailyMinVoltageAC.Value;
            YearlyMaxVoltageDC = data.Inverter.YearlyMaxVoltageDC.Value;
            YearlyMaxVoltageAC = data.Inverter.YearlyMaxVoltageAC.Value;
            YearlyMinVoltageAC = data.Inverter.YearlyMinVoltageAC.Value;
            TotalMaxVoltageDC = data.Inverter.TotalMaxVoltageDC.Value;
            TotalMaxVoltageAC = data.Inverter.TotalMaxVoltageAC.Value;
            TotalMinVoltageAC = data.Inverter.TotalMinVoltageAC.Value;
            DailyMaxPower = data.Inverter.DailyMaxPower.Value;
            YearlyMaxPower = data.Inverter.YearlyMaxPower.Value;
            TotalMaxPower = data.Inverter.TotalMaxPower.Value;
        }

        #endregion
    }
}