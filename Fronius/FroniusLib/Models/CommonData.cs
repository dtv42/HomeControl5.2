// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommonData.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
// Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace FroniusLib.Models
{
    #region Using Directives

    using Extensions;

    #endregion

    /// <summary>
    /// Class holding selected data from the Fronius Symo 8.2-3-M inverter.
    /// </summary>
    public class CommonData
    {
        #region Public Properties

        public double Frequency { get; set; }
        public double CurrentDC { get; set; }
        public double CurrentAC { get; set; }
        public double VoltageDC { get; set; }
        public double VoltageAC { get; set; }
        public double PowerAC { get; set; }
        public double DailyEnergy { get; set; }
        public double YearlyEnergy { get; set; }
        public double TotalEnergy { get; set; }
        public StatusCodes StatusCode { get; set; } = new StatusCodes();

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the Properties used in CommonData.
        /// </summary>
        /// <param name="data">The common device data.</param>
        public void Refresh(CommonDeviceData data)
        {
            Frequency = data.Inverter.Frequency.Value;
            CurrentDC = data.Inverter.CurrentDC.Value;
            CurrentAC = data.Inverter.CurrentAC.Value;
            VoltageDC = data.Inverter.VoltageDC.Value;
            VoltageAC = data.Inverter.VoltageAC.Value;
            PowerAC = data.Inverter.PowerAC.Value;
            DailyEnergy = data.Inverter.DailyEnergy.Value;
            YearlyEnergy = data.Inverter.YearlyEnergy.Value;
            TotalEnergy = data.Inverter.TotalEnergy.Value;
            StatusCode = data.Inverter.DeviceStatus.StatusCode.ToStatusCode();
        }

        #endregion
    }
}
