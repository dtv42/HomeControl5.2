// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PhaseData.cs" company="DTV-Online">
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
    public class PhaseData
    {
        #region Public Properties

        public double CurrentL1 { get; set; }
        public double CurrentL2 { get; set; }
        public double CurrentL3 { get; set; }
        public double VoltageL1N { get; set; }
        public double VoltageL2N { get; set; }
        public double VoltageL3N { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the Properties used in PhaseData.
        /// </summary>
        /// <param name="data">The phase device data.</param>
        public void Refresh(PhaseDeviceData data)
        {
            CurrentL1 = data.Inverter.CurrentL1.Value;
            CurrentL2 = data.Inverter.CurrentL2.Value;
            CurrentL3 = data.Inverter.CurrentL3.Value;
            VoltageL1N = data.Inverter.VoltageL1N.Value;
            VoltageL2N = data.Inverter.VoltageL2N.Value;
            VoltageL3N = data.Inverter.VoltageL3N.Value;
        }

        #endregion
    }
}
