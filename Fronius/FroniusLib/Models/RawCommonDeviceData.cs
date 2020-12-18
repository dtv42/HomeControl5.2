﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommonDeviceData.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
// Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace FroniusLib.Models
{
    public class RawCommonDeviceData
    {
        public DoubleValueData Frequency { get; set; } = new DoubleValueData();
        public DoubleValueData CurrentDC { get; set; } = new DoubleValueData();
        public DoubleValueData CurrentAC { get; set; } = new DoubleValueData();
        public DoubleValueData VoltageDC { get; set; } = new DoubleValueData();
        public DoubleValueData VoltageAC { get; set; } = new DoubleValueData();
        public DoubleValueData PowerAC { get; set; } = new DoubleValueData();
        public DoubleValueData DailyEnergy { get; set; } = new DoubleValueData();
        public DoubleValueData YearlyEnergy { get; set; } = new DoubleValueData();
        public DoubleValueData TotalEnergy { get; set; } = new DoubleValueData();
        public DeviceStatusData DeviceStatus { get; set; } = new DeviceStatusData();
    }
}
