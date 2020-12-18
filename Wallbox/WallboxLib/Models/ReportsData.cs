﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportsData.cs" company="DTV-Online">
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
    #region Using Directives

    using System;
    using System.Globalization;

    #endregion

    public class ReportsData
    {
        #region Public Properties

        /// <summary>
        /// ID of the report
        /// </summary>
        public ushort ID { get; set; }

        /// <summary>
        /// ID of the current charging session.
        /// </summary>
        public uint SessionID { get; set; }

        /// <summary>
        /// Maximum current value in mA that can be supported by the hardware of the device.
        /// </summary>
        public double CurrentHW { get; set; }

        /// <summary>
        /// Total energy consumption (persistent, device related) without the current charging session.
        public double EnergyConsumption { get; set; }

        /// <summary>
        /// Energy transferred in the current charging session.
        /// </summary>
        public double EnergyTransferred { get; set; }

        /// <summary>
        /// System clock in seconds from the last startup of the device at the start of the charging session.
        /// </summary>
        public uint StartedSeconds { get; set; }

        /// <summary>
        /// System clock in seconds from the last startup of the device at the end of the charging session.
        /// </summary>
        public uint EndedSeconds { get; set; }

        /// <summary>
        /// Date stamp representing the current time in UTC at the start of the charging session.
        /// </summary>
        public DateTime Started { get; set; } = new DateTime();

        /// <summary>
        /// Date stamp representing the current time in UTC at the end of the charging session.
        /// </summary>
        public DateTime Ended { get; set; } = new DateTime();

        /// <summary>
        /// Enum indicating the reason for ending the charging session.
        /// </summary>
        public Reasons Reason { get; set; }

        /// <summary>
        /// Synced time.
        /// </summary>
        public ushort TimeQ { get; set; }

        /// <summary>
        /// RFID Token ID if session started with RFID.
        /// </summary>
        public string RFID { get; set; } = string.Empty;

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
        /// Updates the Properties used in ReportsData.
        /// </summary>
        /// <param name="data">The Wallbox UDP data.</param>
        public void Refresh(ReportsUdp data)
        {
            CultureInfo provider = CultureInfo.InvariantCulture;

            ID = ushort.Parse(data.ID);
            SessionID = data.SessionID;
            CurrentHW = data.CurrHW / 1000.0;
            EnergyConsumption = data.Estart / 10000.0;
            EnergyTransferred = data.Epres / 10000.0;
            StartedSeconds = data.StartedSec;
            EndedSeconds = data.EndedSec;
            Started = DateTime.TryParse(data.Started, out DateTime started) ? started : new DateTime();
            Ended = DateTime.TryParse(data.Ended, out DateTime ended) ? ended : new DateTime();
            Reason = data.Reason;
            TimeQ = data.TimeQ;
            RFID = data.RFIDclass;
            Serial = data.Serial;
            Seconds = data.Sec;
        }

        #endregion
    }
}
