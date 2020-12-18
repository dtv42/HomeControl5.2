// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OperationData.cs" company="DTV-Online">
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

    public class OperationData
    {
        #region Public Properties

        public string ItemDescription { get; set; } = string.Empty;
        public string OrderNumber { get; set; } = string.Empty;
        public string MacAddress { get; set; } = string.Empty;
        public string Language { get; set; } = string.Empty;
        public DateTime Date { get; set; } = new DateTime();
        public TimeSpan Time { get; set; } = new TimeSpan();
        public DaylightSaving DayLightSaving { get; set; } = new DaylightSaving();
        public AutoSoftwareUpdates AutoUpdateEnabled { get; set; } = new AutoSoftwareUpdates();
        public HeliosPortalAccess PortalAccessEnabled { get; set; } = new HeliosPortalAccess();
        public double ExhaustVentilatorVoltageLevel1 { get; set; }
        public double SupplyVentilatorVoltageLevel1 { get; set; }
        public double ExhaustVentilatorVoltageLevel2 { get; set; }
        public double SupplyVentilatorVoltageLevel2 { get; set; }
        public double ExhaustVentilatorVoltageLevel3 { get; set; }
        public double SupplyVentilatorVoltageLevel3 { get; set; }
        public double ExhaustVentilatorVoltageLevel4 { get; set; }
        public double SupplyVentilatorVoltageLevel4 { get; set; }
        public MinimumFanLevels MinimumVentilationLevel { get; set; } = new MinimumFanLevels();
        public StatusTypes KwlBeEnabled { get; set; } = new StatusTypes();
        public StatusTypes KwlBecEnabled { get; set; } = new StatusTypes();
        public ConfigOptions DeviceConfiguration { get; set; } = new ConfigOptions();
        public StatusTypes PreheaterStatus { get; set; } = new StatusTypes();
        public KwlFTFConfig KwlFTFConfig0 { get; set; } = new KwlFTFConfig();
        public KwlFTFConfig KwlFTFConfig1 { get; set; } = new KwlFTFConfig();
        public KwlFTFConfig KwlFTFConfig2 { get; set; } = new KwlFTFConfig();
        public KwlFTFConfig KwlFTFConfig3 { get; set; } = new KwlFTFConfig();
        public KwlFTFConfig KwlFTFConfig4 { get; set; } = new KwlFTFConfig();
        public KwlFTFConfig KwlFTFConfig5 { get; set; } = new KwlFTFConfig();
        public KwlFTFConfig KwlFTFConfig6 { get; set; } = new KwlFTFConfig();
        public KwlFTFConfig KwlFTFConfig7 { get; set; } = new KwlFTFConfig();
        public SensorStatus HumidityControlStatus { get; set; } = new SensorStatus();
        public int HumidityControlTarget { get; set; }
        public int HumidityControlStep { get; set; }
        public int HumidityControlStop { get; set; }
        public SensorStatus CO2ControlStatus { get; set; } = new SensorStatus();
        public int CO2ControlTarget { get; set; }
        public int CO2ControlStep { get; set; }
        public SensorStatus VOCControlStatus { get; set; } = new SensorStatus();
        public int VOCControlTarget { get; set; }
        public int VOCControlStep { get; set; }
        public double ThermalComfortTemperature { get; set; }
        public int TimeZoneOffset { get; set; }
        public DateFormats DateFormat { get; set; } = new DateFormats();
        public HeatExchangerTypes HeatExchangerType { get; set; } = new HeatExchangerTypes();
        public FunctionTypes KwlFunctionType { get; set; } = new FunctionTypes();
        public string SensorName1 { get; set; } = string.Empty;
        public string SensorName2 { get; set; } = string.Empty;
        public string SensorName3 { get; set; } = string.Empty;
        public string SensorName4 { get; set; } = string.Empty;
        public string SensorName5 { get; set; } = string.Empty;
        public string SensorName6 { get; set; } = string.Empty;
        public string SensorName7 { get; set; } = string.Empty;
        public string SensorName8 { get; set; } = string.Empty;
        public string StatusFlags { get; set; } = string.Empty;
        public KwlSensorConfig SensorConfig1 { get; set; } = new KwlSensorConfig();
        public KwlSensorConfig SensorConfig2 { get; set; } = new KwlSensorConfig();
        public KwlSensorConfig SensorConfig3 { get; set; } = new KwlSensorConfig();
        public KwlSensorConfig SensorConfig4 { get; set; } = new KwlSensorConfig();
        public KwlSensorConfig SensorConfig5 { get; set; } = new KwlSensorConfig();
        public KwlSensorConfig SensorConfig6 { get; set; } = new KwlSensorConfig();
        public KwlSensorConfig SensorConfig7 { get; set; } = new KwlSensorConfig();
        public KwlSensorConfig SensorConfig8 { get; set; } = new KwlSensorConfig();

        #endregion

        #region Public Methods

        public void Refresh(HeliosData data)
        {
            ItemDescription = data.ItemDescription;
            OrderNumber = data.OrderNumber;
            MacAddress = data.MacAddress;
            Language = data.Language;
            Date = data.Date;
            Time = data.Time;
            DayLightSaving = data.DayLightSaving;
            AutoUpdateEnabled = data.AutoUpdateEnabled;
            PortalAccessEnabled = data.PortalAccessEnabled;
            ExhaustVentilatorVoltageLevel1 = data.ExhaustVentilatorVoltageLevel1;
            SupplyVentilatorVoltageLevel1 = data.SupplyVentilatorVoltageLevel1;
            ExhaustVentilatorVoltageLevel2 = data.ExhaustVentilatorVoltageLevel2;
            SupplyVentilatorVoltageLevel2 = data.SupplyVentilatorVoltageLevel2;
            ExhaustVentilatorVoltageLevel3 = data.ExhaustVentilatorVoltageLevel3;
            SupplyVentilatorVoltageLevel3 = data.SupplyVentilatorVoltageLevel3;
            ExhaustVentilatorVoltageLevel4 = data.ExhaustVentilatorVoltageLevel4;
            SupplyVentilatorVoltageLevel4 = data.SupplyVentilatorVoltageLevel4;
            MinimumVentilationLevel = data.MinimumVentilationLevel;
            KwlBeEnabled = data.KwlBeEnabled;
            KwlBecEnabled = data.KwlBecEnabled;
            DeviceConfiguration = data.DeviceConfiguration;
            PreheaterStatus = data.PreheaterStatus;
            KwlFTFConfig0 = data.KwlFTFConfig0;
            KwlFTFConfig1 = data.KwlFTFConfig1;
            KwlFTFConfig2 = data.KwlFTFConfig2;
            KwlFTFConfig3 = data.KwlFTFConfig3;
            KwlFTFConfig4 = data.KwlFTFConfig4;
            KwlFTFConfig5 = data.KwlFTFConfig5;
            KwlFTFConfig6 = data.KwlFTFConfig6;
            KwlFTFConfig7 = data.KwlFTFConfig7;
            HumidityControlStatus = data.HumidityControlStatus;
            HumidityControlTarget = data.HumidityControlTarget;
            HumidityControlStep = data.HumidityControlStep;
            HumidityControlStop = data.HumidityControlStop;
            CO2ControlStatus = data.CO2ControlStatus;
            CO2ControlTarget = data.CO2ControlTarget;
            CO2ControlStep = data.CO2ControlStep;
            VOCControlStatus = data.VOCControlStatus;
            VOCControlTarget = data.VOCControlTarget;
            VOCControlStep = data.VOCControlStep;
            ThermalComfortTemperature = data.ThermalComfortTemperature;
            TimeZoneOffset = data.TimeZoneOffset;
            DateFormat = data.DateFormat;
            HeatExchangerType = data.HeatExchangerType;
            KwlFunctionType = data.KwlFunctionType;
            SensorName1 = data.SensorName1;
            SensorName2 = data.SensorName2;
            SensorName3 = data.SensorName3;
            SensorName4 = data.SensorName4;
            SensorName5 = data.SensorName5;
            SensorName6 = data.SensorName6;
            SensorName7 = data.SensorName7;
            SensorName8 = data.SensorName8;
            StatusFlags = data.StatusFlags;
            SensorConfig1 = data.SensorConfig1;
            SensorConfig2 = data.SensorConfig2;
            SensorConfig3 = data.SensorConfig3;
            SensorConfig4 = data.SensorConfig4;
            SensorConfig5 = data.SensorConfig5;
            SensorConfig6 = data.SensorConfig6;
            SensorConfig7 = data.SensorConfig7;
            SensorConfig8 = data.SensorConfig8;
        }

        #endregion
    }
}
