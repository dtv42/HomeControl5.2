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
    public class RawCommonData
    {
        public DoubleValueData FAC { get; set; } = new DoubleValueData();
        public DoubleValueData IDC { get; set; } = new DoubleValueData();
        public DoubleValueData IAC { get; set; } = new DoubleValueData();
        public DoubleValueData UDC { get; set; } = new DoubleValueData();
        public DoubleValueData UAC { get; set; } = new DoubleValueData();
        public DoubleValueData PAC { get; set; } = new DoubleValueData();
        public DoubleValueData DAY_ENERGY { get; set; } = new DoubleValueData();
        public DoubleValueData YEAR_ENERGY { get; set; } = new DoubleValueData();
        public DoubleValueData TOTAL_ENERGY { get; set; } = new DoubleValueData();
        public DeviceStatusData DeviceStatus { get; set; } = new DeviceStatusData();
    }
}
