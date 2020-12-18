// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LoggerInfo.cs" company="DTV-Online">
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
    public class LoggerInfo
    {
        #region Public Properties

        public string UniqueID { get; set; } = string.Empty;
        public string ProductID { get; set; } = string.Empty;
        public string PlatformID { get; set; } = string.Empty;
        public string HWVersion { get; set; } = string.Empty;
        public string SWVersion { get; set; } = string.Empty;
        public string TimezoneLocation { get; set; } = string.Empty;
        public string TimezoneName { get; set; } = string.Empty;
        public int UTCOffset { get; set; }
        public string DefaultLanguage { get; set; } = string.Empty;
        public double CashFactor { get; set; }
        public string CashCurrency { get; set; } = string.Empty;
        public double CO2Factor { get; set; }
        public string CO2Unit { get; set; } = string.Empty;

        #endregion

        #region Public Methods

        /// <summary>
        /// Updates the Properties used in LoggerInfo.
        /// </summary>
        /// <param name="data">The logger device data.</param>
        public void Refresh(LoggerDeviceData data)
        {
            UniqueID = data.Logger.UniqueID;
            ProductID = data.Logger.ProductID;
            PlatformID = data.Logger.PlatformID;
            HWVersion = data.Logger.HWVersion;
            SWVersion = data.Logger.SWVersion;
            TimezoneLocation = data.Logger.TimezoneLocation;
            TimezoneName = data.Logger.TimezoneName;
            UTCOffset = data.Logger.UTCOffset;
            DefaultLanguage = data.Logger.DefaultLanguage;
            CashFactor = data.Logger.CashFactor;
            CashCurrency = data.Logger.CashCurrency;
            CO2Factor = data.Logger.CO2Factor;
            CO2Unit = data.Logger.CO2Unit;
        }

        #endregion
    }
}
