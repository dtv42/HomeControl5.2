// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Report1Data.cs" company="DTV-Online">
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

    #endregion

    public class Report1Data
    {
        #region Public Properties

        /// <summary>
        /// ID of the report
        /// </summary>
        public ushort ID { get; set; }

        /// <summary>
        /// Product name as defined by the manufacturer
        /// </summary>
        public string Product { get; set; } = string.Empty;

        /// <summary>
        /// Serial number of the device
        /// </summary>
        public string Serial { get; set; } = string.Empty;

        /// <summary>
        /// Firmware version of the device
        /// </summary>
        public string Firmware { get; set; } = string.Empty;

        /// <summary>
        /// Communication module.
        /// </summary>
        public ComModulePresent ComModule { get; set; }

        /// <summary>
        /// Backend communication status.
        /// </summary>
        public BackendPresent Backend { get; set; }

        /// <summary>
        /// Synced time.
        /// </summary>
        public ushort TimeQ { get; set; }

        /// <summary>
        /// "DIP-Sw1": "0x26" (undocumented).
        /// </summary>
        public DipSwitches DIPSwitch1 { get; set; }

        /// <summary>
        /// "DIP-Sw2": "0x00" (undocumented).
        /// </summary>
        public DipSwitches DIPSwitch2 { get; set; }

        /// <summary>
        /// Current state of the system clock in seconds from the last startup of the device.
        /// </summary>
        public uint Seconds { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the Properties used in Report1Data.
        /// </summary>
        /// <param name="data">The Wallbox UDP data.</param>
        public void Refresh(Report1Udp data)
        {
            ID = ushort.Parse(data.ID);
            Product = data.Product;
            Serial = data.Serial;
            Firmware = data.Firmware;
            ComModule = (ComModulePresent)data.ComModule;
            Backend = (BackendPresent)data.Backend;
            TimeQ = data.TimeQ;
            DIPSwitch1 = (DipSwitches)Convert.ToInt64(data.DipSW1, 16);
            DIPSwitch2 = (DipSwitches)Convert.ToInt64(data.DipSW2, 16);
            Seconds = data.Sec;
        }

        #endregion
    }
}
