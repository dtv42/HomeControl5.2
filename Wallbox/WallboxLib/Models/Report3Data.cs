// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Report3Data.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>26-4-2020 20:19</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace WallboxLib.Models
{
    public class Report3Data
    {
        #region Public Properties

        /// <summary>
        /// ID of the report
        /// </summary>
        public ushort ID { get; set; }

        /// <summary>
        /// Measured voltage value on phase 1 in V
        /// </summary>
        public double VoltageL1N { get; set; }

        /// <summary>
        /// Measured voltage value on phase 2 in V
        /// </summary>
        public double VoltageL2N { get; set; }

        /// <summary>
        /// Measured voltage value on phase 3 in V
        /// </summary>
        public double VoltageL3N { get; set; }

        /// <summary>
        /// Measured current value on phase 1 in A
        /// </summary>
        public double CurrentL1 { get; set; }

        /// <summary>
        /// Measured current value on phase 2 in A
        /// </summary>
        public double CurrentL2 { get; set; }

        /// <summary>
        /// Measured current value on phase 3 in A
        /// </summary>
        public double CurrentL3 { get; set; }

        /// <summary>
        /// Power in W (effective power).
        /// </summary>
        public double Power { get; set; }

        /// <summary>
        /// Current power factor(cosphi).
        /// </summary>
        public double PowerFactor { get; set; }

        /// <summary>
        /// Energy transferred in the current charging session in Wh.
        /// </summary>
        public double EnergyCharging { get; set; }

        /// <summary>
        /// Total energy consumption(persistent, device related) in Wh.
        /// </summary>
        public double EnergyTotal { get; set; }

        /// <summary>
        /// Serial number of the device.
        /// </summary>
        public string Serial { get; set; } = string.Empty;

        /// <summary>
        /// Current state of the system clock in seconds from the last startup of the device.
        /// </summary>
        public uint Seconds { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the Properties used in Report3Data.
        /// </summary>
        /// <param name="data">The Wallbox UDP data.</param>
        public void Refresh(Report3Udp data)
        {
            ID = ushort.Parse(data.ID);
            VoltageL1N = data.U1;
            VoltageL2N = data.U2;
            VoltageL3N = data.U3;
            CurrentL1 = data.I1 / 1000.0;
            CurrentL2 = data.I2 / 1000.0;
            CurrentL3 = data.I3 / 1000.0;
            Power = data.P / 1000000.0;
            PowerFactor = data.PF / 10.0;
            EnergyCharging = data.Epres / 10000.0;
            EnergyTotal = data.Etotal / 10000.0;
            Serial = data.Serial;
            Seconds = data.Sec;
        }

        #endregion
    }
}
