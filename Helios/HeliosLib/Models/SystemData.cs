// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SystemData.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>26-4-2020 10:05</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace HeliosLib.Models
{
    #region Using Directives

    using System;

    #endregion

    public class SystemData
    {
        #region Public Properties

        public string Language { get; set; } = string.Empty;
        public DateTime Date { get; set; } = new DateTime();
        public TimeSpan Time { get; set; } = new TimeSpan();
        public DaylightSaving DayLightSaving { get; set; } = new DaylightSaving();
        public AutoSoftwareUpdates AutoUpdateEnabled { get; set; } = new AutoSoftwareUpdates();
        public HeliosPortalAccess PortalAccessEnabled { get; set; } = new HeliosPortalAccess();
        public int TimeZoneOffset { get; set; }
        public DateFormats DateFormat { get; set; } = new DateFormats();
        public int SupplyFanSpeed { get; set; }
        public int ExhaustFanSpeed { get; set; }
        public string SoftwareVersion { get; set; } = string.Empty;
        public int OperationMinutesSupply { get; set; }
        public int OperationMinutesExhaust { get; set; }
        public int OperationMinutesPreheater { get; set; }
        public int OperationMinutesAfterheater { get; set; }
        public double PowerPreheater { get; set; }
        public double PowerAfterheater { get; set; }
        public string StatusFlags { get; set; } = string.Empty;
        public bool ActivateAutoMode { get; set; }
        public string CountryCode { get; set; } = string.Empty;
        public int V02103 { get; set; }

        #endregion

        #region Public Methods

        public void Refresh(HeliosData data)
        {
            Language = data.Language;
            Date = data.Date;
            Time = data.Time;
            DayLightSaving = data.DayLightSaving;
            AutoUpdateEnabled = data.AutoUpdateEnabled;
            PortalAccessEnabled = data.PortalAccessEnabled;
            TimeZoneOffset = data.TimeZoneOffset;
            DateFormat = data.DateFormat;
            SupplyFanSpeed = data.SupplyFanSpeed;
            ExhaustFanSpeed = data.ExhaustFanSpeed;
            SoftwareVersion = data.SoftwareVersion;
            OperationMinutesSupply = data.OperationMinutesSupply;
            OperationMinutesExhaust = data.OperationMinutesExhaust;
            OperationMinutesPreheater = data.OperationMinutesPreheater;
            OperationMinutesAfterheater = data.OperationMinutesAfterheater;
            PowerPreheater = data.PowerPreheater;
            PowerAfterheater = data.PowerAfterheater;
            StatusFlags = data.StatusFlags;
            ActivateAutoMode = data.ActivateAutoMode;
            CountryCode = data.CountryCode;
            V02103 = data.V02103;
        }

        #endregion
    }
}
