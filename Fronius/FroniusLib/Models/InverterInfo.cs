﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InverterInfo.cs" company="DTV-Online">
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
    public class InverterInfo
    {
        #region Public Properties

        public string Index { get; set; } = string.Empty;
        public int DeviceType { get; set; }
        public int PVPower { get; set; }
        public string CustomName { get; set; } = string.Empty;
        public bool Show { get; set; }
        public string UniqueID { get; set; } = string.Empty;
        public int ErrorCode { get; set; }
        public StatusCodes StatusCode { get; set; } = new StatusCodes();

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the Properties used in InverterInfo.
        /// </summary>
        /// <param name="data">The inverter device data.</param>
        public void Refresh(InverterDeviceData data)
        {
            Index = data.Inverter.Index;
            DeviceType = data.Inverter.DeviceType;
            PVPower = data.Inverter.PVPower;
            CustomName = data.Inverter.CustomName;
            Show = data.Inverter.Show;
            UniqueID = data.Inverter.UniqueID;
            ErrorCode = data.Inverter.ErrorCode;
            StatusCode = data.Inverter.StatusCode;
        }

        #endregion
    }
}
