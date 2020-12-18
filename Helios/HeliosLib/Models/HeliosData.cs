// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeliosData.cs" company="DTV-Online">
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using UtilityLib;

    #endregion

    /// <summary>
    /// Helios KWLEC200 properties (data fields).
    /// </summary>
    public class HeliosData
    {
        #region Public Properties

        [Helios("v00000")] public string ItemDescription { get; set; } = string.Empty;
        [Helios("v00001")] public string OrderNumber { get; set; } = string.Empty;
        [Helios("v00002")] public string MacAddress { get; set; } = string.Empty;
        [Helios("v00003")] public string Language { get; set; } = string.Empty;
        [Helios("v00004")] public DateTime Date { get; set; } = new DateTime();
        [Helios("v00005")] public TimeSpan Time { get; set; } = new TimeSpan();
        [Helios("v00006")] public DaylightSaving DayLightSaving { get; set; } = new DaylightSaving();
        [Helios("v00007")] public AutoSoftwareUpdates AutoUpdateEnabled { get; set; } = new AutoSoftwareUpdates();
        [Helios("v00008")] public HeliosPortalAccess PortalAccessEnabled { get; set; } = new HeliosPortalAccess();
        [Helios("v00012")] public double ExhaustVentilatorVoltageLevel1 { get; set; }
        [Helios("v00013")] public double SupplyVentilatorVoltageLevel1 { get; set; }
        [Helios("v00014")] public double ExhaustVentilatorVoltageLevel2 { get; set; }
        [Helios("v00015")] public double SupplyVentilatorVoltageLevel2 { get; set; }
        [Helios("v00016")] public double ExhaustVentilatorVoltageLevel3 { get; set; }
        [Helios("v00017")] public double SupplyVentilatorVoltageLevel3 { get; set; }
        [Helios("v00018")] public double ExhaustVentilatorVoltageLevel4 { get; set; }
        [Helios("v00019")] public double SupplyVentilatorVoltageLevel4 { get; set; }
        [Helios("v00020")] public MinimumFanLevels MinimumVentilationLevel { get; set; } = new MinimumFanLevels();
        [Helios("v00021")] public StatusTypes KwlBeEnabled { get; set; } = new StatusTypes();
        [Helios("v00022")] public StatusTypes KwlBecEnabled { get; set; } = new StatusTypes();
        [Helios("v00023")] public ConfigOptions DeviceConfiguration { get; set; } = new ConfigOptions();
        [Helios("v00024")] public StatusTypes PreheaterStatus { get; set; } = new StatusTypes();
        [Helios("v00025")] public KwlFTFConfig KwlFTFConfig0 { get; set; } = new KwlFTFConfig();
        [Helios("v00026")] public KwlFTFConfig KwlFTFConfig1 { get; set; } = new KwlFTFConfig();
        [Helios("v00027")] public KwlFTFConfig KwlFTFConfig2 { get; set; } = new KwlFTFConfig();
        [Helios("v00028")] public KwlFTFConfig KwlFTFConfig3 { get; set; } = new KwlFTFConfig();
        [Helios("v00029")] public KwlFTFConfig KwlFTFConfig4 { get; set; } = new KwlFTFConfig();
        [Helios("v00030")] public KwlFTFConfig KwlFTFConfig5 { get; set; } = new KwlFTFConfig();
        [Helios("v00031")] public KwlFTFConfig KwlFTFConfig6 { get; set; } = new KwlFTFConfig();
        [Helios("v00032")] public KwlFTFConfig KwlFTFConfig7 { get; set; } = new KwlFTFConfig();
        [Helios("v00033")] public SensorStatus HumidityControlStatus { get; set; } = new SensorStatus();
        [Helios("v00034")] public int HumidityControlTarget { get; set; }
        [Helios("v00035")] public int HumidityControlStep { get; set; }
        [Helios("v00036")] public int HumidityControlStop { get; set; }
        [Helios("v00037")] public SensorStatus CO2ControlStatus { get; set; } = new SensorStatus();
        [Helios("v00038")] public int CO2ControlTarget { get; set; }
        [Helios("v00039")] public int CO2ControlStep { get; set; }
        [Helios("v00040")] public SensorStatus VOCControlStatus { get; set; } = new SensorStatus();
        [Helios("v00041")] public int VOCControlTarget { get; set; }
        [Helios("v00042")] public int VOCControlStep { get; set; }
        [Helios("v00043")] public double ThermalComfortTemperature { get; set; }
        [Helios("v00051")] public int TimeZoneOffset { get; set; }
        [Helios("v00052")] public DateFormats DateFormat { get; set; } = new DateFormats();
        [Helios("v00053")] public HeatExchangerTypes HeatExchangerType { get; set; } = new HeatExchangerTypes();
        [Helios("v00091")] public int PartyOperationDuration { get; set; }
        [Helios("v00092")] public FanLevels PartyVentilationLevel { get; set; } = new FanLevels();
        [Helios("v00093")] public int PartyOperationRemaining { get; set; }
        [Helios("v00094")] public StatusTypes PartyOperationActivate { get; set; } = new StatusTypes();
        [Helios("v00096")] public int StandbyOperationDuration { get; set; }
        [Helios("v00097")] public FanLevels StandbyVentilationLevel { get; set; } = new FanLevels();
        [Helios("v00098")] public int StandbyOperationRemaining { get; set; }
        [Helios("v00099")] public StatusTypes StandbyOperationActivate { get; set; } = new StatusTypes();
        [Helios("v00101")] public OperationModes OperationMode { get; set; } = new OperationModes();
        [Helios("v00102")] public FanLevels VentilationLevel { get; set; } = new FanLevels();
        [Helios("v00103")] public int VentilationPercentage { get; set; }
        [Helios("v00104")] public double TemperatureOutdoor { get; set; }
        [Helios("v00105")] public double TemperatureSupply { get; set; }
        [Helios("v00106")] public double TemperatureExhaust { get; set; }
        [Helios("v00107")] public double TemperatureExtract { get; set; }
        [Helios("v00108")] public double TemperaturePreHeater { get; set; }
        [Helios("v00109")] public double V00109 { get; set; }
        [Helios("v00110")] public double TemperaturePostHeater { get; set; }
        [Helios("v00111")] public double ExternalHumiditySensor1 { get; set; }
        [Helios("v00112")] public double ExternalHumiditySensor2 { get; set; }
        [Helios("v00113")] public double ExternalHumiditySensor3 { get; set; }
        [Helios("v00114")] public double ExternalHumiditySensor4 { get; set; }
        [Helios("v00115")] public double ExternalHumiditySensor5 { get; set; }
        [Helios("v00116")] public double ExternalHumiditySensor6 { get; set; }
        [Helios("v00117")] public double ExternalHumiditySensor7 { get; set; }
        [Helios("v00118")] public double ExternalHumiditySensor8 { get; set; }
        [Helios("v00119")] public double ExternalHumidityTemperature1 { get; set; }
        [Helios("v00120")] public double ExternalHumidityTemperature2 { get; set; }
        [Helios("v00121")] public double ExternalHumidityTemperature3 { get; set; }
        [Helios("v00122")] public double ExternalHumidityTemperature4 { get; set; }
        [Helios("v00123")] public double ExternalHumidityTemperature5 { get; set; }
        [Helios("v00124")] public double ExternalHumidityTemperature6 { get; set; }
        [Helios("v00125")] public double ExternalHumidityTemperature7 { get; set; }
        [Helios("v00126")] public double ExternalHumidityTemperature8 { get; set; }
        [Helios("v00127")] public double V00127 { get; set; }
        [Helios("v00128")] public double ExternalCO2Sensor1 { get; set; }
        [Helios("v00129")] public double ExternalCO2Sensor2 { get; set; }
        [Helios("v00130")] public double ExternalCO2Sensor3 { get; set; }
        [Helios("v00131")] public double ExternalCO2Sensor4 { get; set; }
        [Helios("v00132")] public double ExternalCO2Sensor5 { get; set; }
        [Helios("v00133")] public double ExternalCO2Sensor6 { get; set; }
        [Helios("v00134")] public double ExternalCO2Sensor7 { get; set; }
        [Helios("v00135")] public double ExternalCO2Sensor8 { get; set; }
        [Helios("v00136")] public double ExternalVOCSensor1 { get; set; }
        [Helios("v00137")] public double ExternalVOCSensor2 { get; set; }
        [Helios("v00138")] public double ExternalVOCSensor3 { get; set; }
        [Helios("v00139")] public double ExternalVOCSensor4 { get; set; }
        [Helios("v00140")] public double ExternalVOCSensor5 { get; set; }
        [Helios("v00141")] public double ExternalVOCSensor6 { get; set; }
        [Helios("v00142")] public double ExternalVOCSensor7 { get; set; }
        [Helios("v00143")] public double ExternalVOCSensor8 { get; set; }
        [Helios("v00144")] public double V00144 { get; set; }
        [Helios("v00146")] public double TemperatureChannel { get; set; }
        [Helios("v00201")] public WeeklyProfiles WeeklyProfile { get; set; } = new WeeklyProfiles();
        [Helios("v00220")] public int V00220 { get; set; }
        [Helios("v00221")] public int V00221 { get; set; }
        [Helios("v00222")] public int V00222 { get; set; }
        [Helios("v00223")] public int V00223 { get; set; }
        [Helios("v00224")] public int V00224 { get; set; }
        [Helios("v00225")] public int V00225 { get; set; }
        [Helios("v00226")] public int V00226 { get; set; }
        [Helios("v00227")] public int V00227 { get; set; }
        [Helios("v00228")] public int V00228 { get; set; }
        [Helios("v00229")] public int V00229 { get; set; }
        [Helios("v00303")] public string SerialNumber { get; set; } = string.Empty;
        [Helios("v00304")] public string ProductionCode { get; set; } = string.Empty;
        [Helios("v00343")] public string SecurityCode { get; set; } = string.Empty;
        [Helios("v00348")] public int SupplyFanSpeed { get; set; }
        [Helios("v00349")] public int ExhaustFanSpeed { get; set; }
        [Helios("v00402")] public string Password { get; set; } = string.Empty;
        [Helios("v00403")] public bool Logout { get; set; }
        [Helios("v00601")] public VacationOperations VacationOperation { get; set; } = new VacationOperations();
        [Helios("v00602")] public FanLevels VacationVentilationLevel { get; set; } = new FanLevels();
        [Helios("v00603")] public DateTime VacationStartDate { get; set; } = new DateTime();
        [Helios("v00604")] public DateTime VacationEndDate { get; set; } = new DateTime();
        [Helios("v00605")] public int VacationInterval { get; set; }
        [Helios("v00606")] public int VacationDuration { get; set; }
        [Helios("v00901")] public int V00901 { get; set; }
        [Helios("v00920")] public int V00920 { get; set; }
        [Helios("v00921")] public int V00921 { get; set; }
        [Helios("v00922")] public int V00922 { get; set; }
        [Helios("v00923")] public int V00923 { get; set; }
        [Helios("v00924")] public int V00924 { get; set; }
        [Helios("v00925")] public int V00925 { get; set; }
        [Helios("v00926")] public int V00926 { get; set; }
        [Helios("v00927")] public int V00927 { get; set; }
        [Helios("v00928")] public int V00928 { get; set; }
        [Helios("v00929")] public int V00929 { get; set; }
        [Helios("v01001")] public bool UseDHCP { get; set; }
        [Helios("v01002")] public string IPAddress { get; set; } = string.Empty;
        [Helios("v01003")] public string SubnetMask { get; set; } = string.Empty;
        [Helios("v01004")] public string Gateway { get; set; } = string.Empty;
        [Helios("v01005")] public string StandardDNS { get; set; } = string.Empty;
        [Helios("v01006")] public string FallbackDNS { get; set; } = string.Empty;
        [Helios("v01007")] public string HostName { get; set; } = string.Empty;
        [Helios("v01010")] public PreheaterTypes PreheaterType { get; set; } = new PreheaterTypes();
        [Helios("v01017")] public FunctionTypes KwlFunctionType { get; set; } = new FunctionTypes();
        [Helios("v01019")] public int HeaterAfterRunTime { get; set; }
        [Helios("v01020")] public ContactTypes ExternalContact { get; set; } = new ContactTypes();
        [Helios("v01021")] public FaultTypes FaultTypeOutput { get; set; } = new FaultTypes();
        [Helios("v01031")] public StatusTypes FilterChange { get; set; } = new StatusTypes();
        [Helios("v01032")] public int FilterChangeInterval { get; set; }
        [Helios("v01033")] public int FilterChangeRemaining { get; set; }
        [Helios("v01035")] public int BypassRoomTemperature { get; set; }
        [Helios("v01036")] public int BypassOutdoorTemperature { get; set; }
        [Helios("v01037")] public int BypassOutdoorTemperature2 { get; set; }
        [Helios("v01038")] public bool RestartChangeInterval { get; set; }
        [Helios("v01041")] public bool StartReset { get; set; }
        [Helios("v01042")] public bool FactoryReset { get; set; }
        [Helios("v01050")] public FanLevels SupplyLevel { get; set; } = new FanLevels();
        [Helios("v01051")] public FanLevels ExhaustLevel { get; set; } = new FanLevels();
        [Helios("v01061")] public FanLevels FanLevelRegion02 { get; set; } = new FanLevels();
        [Helios("v01062")] public FanLevels FanLevelRegion24 { get; set; } = new FanLevels();
        [Helios("v01063")] public FanLevels FanLevelRegion46 { get; set; } = new FanLevels();
        [Helios("v01064")] public FanLevels FanLevelRegion68 { get; set; } = new FanLevels();
        [Helios("v01065")] public FanLevels FanLevelRegion80 { get; set; } = new FanLevels();
        [Helios("v01066")] public double OffsetExhaust { get; set; }
        [Helios("v01068")] public FanLevelConfig FanLevelConfiguration { get; set; } = new FanLevelConfig();
        [Helios("v01071")] public string SensorName1 { get; set; } = string.Empty;
        [Helios("v01072")] public string SensorName2 { get; set; } = string.Empty;
        [Helios("v01073")] public string SensorName3 { get; set; } = string.Empty;
        [Helios("v01074")] public string SensorName4 { get; set; } = string.Empty;
        [Helios("v01075")] public string SensorName5 { get; set; } = string.Empty;
        [Helios("v01076")] public string SensorName6 { get; set; } = string.Empty;
        [Helios("v01077")] public string SensorName7 { get; set; } = string.Empty;
        [Helios("v01078")] public string SensorName8 { get; set; } = string.Empty;
        [Helios("v01081")] public string CO2SensorName1 { get; set; } = string.Empty;
        [Helios("v01082")] public string CO2SensorName2 { get; set; } = string.Empty;
        [Helios("v01083")] public string CO2SensorName3 { get; set; } = string.Empty;
        [Helios("v01084")] public string CO2SensorName4 { get; set; } = string.Empty;
        [Helios("v01085")] public string CO2SensorName5 { get; set; } = string.Empty;
        [Helios("v01086")] public string CO2SensorName6 { get; set; } = string.Empty;
        [Helios("v01087")] public string CO2SensorName7 { get; set; } = string.Empty;
        [Helios("v01088")] public string CO2SensorName8 { get; set; } = string.Empty;
        [Helios("v01091")] public string VOCSensorName1 { get; set; } = string.Empty;
        [Helios("v01092")] public string VOCSensorName2 { get; set; } = string.Empty;
        [Helios("v01093")] public string VOCSensorName3 { get; set; } = string.Empty;
        [Helios("v01094")] public string VOCSensorName4 { get; set; } = string.Empty;
        [Helios("v01095")] public string VOCSensorName5 { get; set; } = string.Empty;
        [Helios("v01096")] public string VOCSensorName6 { get; set; } = string.Empty;
        [Helios("v01097")] public string VOCSensorName7 { get; set; } = string.Empty;
        [Helios("v01098")] public string VOCSensorName8 { get; set; } = string.Empty;
        [Helios("v01101")] public string SoftwareVersion { get; set; } = string.Empty;
        [Helios("v01102")] public int OperatingHours { get; set; }
        [Helios("v01103")] public int OperationMinutesSupply { get; set; }
        [Helios("v01104")] public int OperationMinutesExhaust { get; set; }
        [Helios("v01105")] public int OperationMinutesPreheater { get; set; }
        [Helios("v01106")] public int OperationMinutesAfterheater { get; set; }
        [Helios("v01108")] public double PowerPreheater { get; set; }
        [Helios("v01109")] public double PowerAfterheater { get; set; }
        [Helios("v01120")] public bool ResetFlag { get; set; }
        [Helios("v01123")] public int ErrorCode { get; set; }
        [Helios("v01124")] public int WarningCode { get; set; }
        [Helios("v01125")] public int InfoCode { get; set; }
        [Helios("v01200")] public bool ModbusActivated { get; set; }
        [Helios("v01300")] public int NumberOfErrors { get; set; }
        [Helios("v01301")] public int NumberOfWarnings { get; set; }
        [Helios("v01302")] public int NumberOfInfos { get; set; }
        [Helios("v01303")] public string Errors { get; set; } = string.Empty;
        [Helios("v01304")] public int Warnings { get; set; }
        [Helios("v01305")] public string Infos { get; set; } = string.Empty;
        [Helios("v01306")] public string StatusFlags { get; set; } = string.Empty;
        [Helios("v02013")] public GlobalUpdates GlobalUpdate { get; set; } = new GlobalUpdates();
        [Helios("v02014")] public int LastError { get; set; }
        [Helios("v02015")] public bool ClearError { get; set; }
        [Helios("v02020")] public KwlSensorConfig SensorConfig1 { get; set; } = new KwlSensorConfig();
        [Helios("v02021")] public KwlSensorConfig SensorConfig2 { get; set; } = new KwlSensorConfig();
        [Helios("v02022")] public KwlSensorConfig SensorConfig3 { get; set; } = new KwlSensorConfig();
        [Helios("v02023")] public KwlSensorConfig SensorConfig4 { get; set; } = new KwlSensorConfig();
        [Helios("v02024")] public KwlSensorConfig SensorConfig5 { get; set; } = new KwlSensorConfig();
        [Helios("v02025")] public KwlSensorConfig SensorConfig6 { get; set; } = new KwlSensorConfig();
        [Helios("v02026")] public KwlSensorConfig SensorConfig7 { get; set; } = new KwlSensorConfig();
        [Helios("v02027")] public KwlSensorConfig SensorConfig8 { get; set; } = new KwlSensorConfig();
        [Helios("v02103")] public int V02103 { get; set; }
        [Helios("v02104")] public int DataExchange { get; set; }
        [Helios("v02115")] public int V02115 { get; set; }
        [Helios("v02116")] public bool ActivateAutoMode { get; set; }
        [Helios("v02117")] public int V02117 { get; set; }
        [Helios("v02118")] public int V02118 { get; set; }
        [Helios("v02119")] public int V02119 { get; set; }
        [Helios("v02120")] public int V02120 { get; set; }
        [Helios("v02121")] public int V02121 { get; set; }
        [Helios("v02122")] public int V02122 { get; set; }
        [Helios("v02123")] public int V02123 { get; set; }
        [Helios("v02128")] public int V02128 { get; set; }
        [Helios("v02129")] public int V02129 { get; set; }
        [Helios("v02130")] public int V02130 { get; set; }
        [Helios("v02131")] public int V02131 { get; set; }
        [Helios("v02134")] public string CountryCode { get; set; } = string.Empty;
        [Helios("v02136")] public int V02136 { get; set; }
        [Helios("v02137")] public string V02137 { get; set; } = string.Empty;
        [Helios("v02142")] public int V02142 { get; set; }
        [Helios("v02143")] public int V02143 { get; set; }
        [Helios("v02144")] public int V02144 { get; set; }
        [Helios("v02145")] public int V02145 { get; set; }
        [Helios("v02146")] public int V02146 { get; set; }
        [Helios("v02147")] public int V02147 { get; set; }
        [Helios("v02148")] public int V02148 { get; set; }
        [Helios("v02149")] public TimeSpan V02149 { get; set; }
        [Helios("v02150")] public TimeSpan V02150 { get; set; }
        [Helios("v02151")] public int V02151 { get; set; }
        [Helios("v02152")] public int V02152 { get; set; }

        #endregion Public Properties

        #region Public Methods

        public void Update(RawData data)
        {
            foreach (var property in GetProperties())
            {
                var info = typeof(HeliosData).GetProperty(property);
                var attribute = GetHeliosAttribute(property);

                if (!(attribute is null))
                {
                    if (data.Parameters.ContainsKey(attribute.Name))
                    {
                        var value = data.Parameters.GetValueOrDefault(attribute.Name);

                        if (!string.IsNullOrEmpty(value) && (info != null))
                        {
                            switch (info.PropertyType)
                            {
                                case Type intType when intType == typeof(int):
                                    {
                                        if (int.TryParse(value, out int intValue))
                                            this.SetPropertyValue(property, intValue);
                                        break;
                                    }
                                case Type dblType when dblType == typeof(double):
                                    {
                                        if (value == "-")
                                        {
                                            this.SetPropertyValue(property, 0);
                                        }
                                        else if (double.TryParse(value, out double dblValue))
                                            this.SetPropertyValue(property, dblValue);
                                    }
                                    break;
                                case Type strType when strType == typeof(string):
                                    {
                                        this.SetPropertyValue(property, value);
                                        break;
                                    }
                                case Type dateType when dateType == typeof(DateTime):
                                    {
                                        if (DateTime.TryParse(value, out DateTime dateValue))
                                            this.SetPropertyValue(property, dateValue);
                                        break;
                                    }
                                case Type timeType when timeType == typeof(TimeSpan):
                                    {
                                        if (TimeSpan.TryParse(value, out TimeSpan timeValue))
                                            this.SetPropertyValue(property, timeValue);
                                        break;
                                    }
                                case Type enumType when enumType == typeof(AutoSoftwareUpdates):
                                    {
                                        if (int.TryParse(value, out int enumValue))
                                            this.SetPropertyValue(property, (AutoSoftwareUpdates)enumValue);
                                        break;
                                    }
                                case Type enumType when enumType == typeof(ConfigOptions):
                                    {
                                        if (int.TryParse(value, out int enumValue))
                                            this.SetPropertyValue(property, (ConfigOptions)enumValue);
                                        break;
                                    }
                                case Type enumType when enumType == typeof(ContactTypes):
                                    {
                                        if (int.TryParse(value, out int enumValue))
                                            this.SetPropertyValue(property, (ContactTypes)enumValue);
                                        break;
                                    }
                                case Type enumType when enumType == typeof(DateFormats):
                                    {
                                        if (int.TryParse(value, out int enumValue))
                                            this.SetPropertyValue(property, (DateFormats)enumValue);
                                        break;
                                    }
                                case Type enumType when enumType == typeof(DaylightSaving):
                                    {
                                        if (int.TryParse(value, out int enumValue))
                                            this.SetPropertyValue(property, (DaylightSaving)enumValue);
                                        break;
                                    }
                                case Type enumType when enumType == typeof(FanLevelConfig):
                                    {
                                        if (int.TryParse(value, out int enumValue))
                                            this.SetPropertyValue(property, (FanLevelConfig)enumValue);
                                        break;
                                    }
                                case Type enumType when enumType == typeof(FanLevels):
                                    {
                                        if (int.TryParse(value, out int enumValue))
                                            this.SetPropertyValue(property, (FanLevels)enumValue);
                                        break;
                                    }
                                case Type enumType when enumType == typeof(FaultTypes):
                                    {
                                        if (int.TryParse(value, out int enumValue))
                                            this.SetPropertyValue(property, (FaultTypes)enumValue);
                                        break;
                                    }
                                case Type enumType when enumType == typeof(FunctionTypes):
                                    {
                                        if (int.TryParse(value, out int enumValue))
                                            this.SetPropertyValue(property, (FunctionTypes)enumValue);
                                        break;
                                    }
                                case Type enumType when enumType == typeof(GlobalUpdates):
                                    {
                                        if (int.TryParse(value, out int enumValue))
                                            this.SetPropertyValue(property, (GlobalUpdates)enumValue);
                                        break;
                                    }
                                case Type enumType when enumType == typeof(HeatExchangerTypes):
                                    {
                                        if (int.TryParse(value, out int enumValue))
                                            this.SetPropertyValue(property, (HeatExchangerTypes)enumValue);
                                        break;
                                    }
                                case Type enumType when enumType == typeof(HeliosPortalAccess):
                                    {
                                        if (int.TryParse(value, out int enumValue))
                                            this.SetPropertyValue(property, (HeliosPortalAccess)enumValue);
                                        break;
                                    }
                                case Type enumType when enumType == typeof(KwlFTFConfig):
                                    {
                                        if (int.TryParse(value, out int enumValue))
                                            this.SetPropertyValue(property, (KwlFTFConfig)enumValue);
                                        break;
                                    }
                                case Type enumType when enumType == typeof(KwlSensorConfig):
                                    {
                                        if (int.TryParse(value, out int enumValue))
                                            this.SetPropertyValue(property, (KwlSensorConfig)enumValue);
                                        break;
                                    }
                                case Type enumType when enumType == typeof(MinimumFanLevels):
                                    {
                                        if (int.TryParse(value, out int enumValue))
                                            this.SetPropertyValue(property, (MinimumFanLevels)enumValue);
                                        break;
                                    }
                                case Type enumType when enumType == typeof(OperationModes):
                                    {
                                        if (int.TryParse(value, out int enumValue))
                                            this.SetPropertyValue(property, (OperationModes)enumValue);
                                        break;
                                    }
                                case Type enumType when enumType == typeof(PreheaterTypes):
                                    {
                                        if (int.TryParse(value, out int enumValue))
                                            this.SetPropertyValue(property, (PreheaterTypes)enumValue);
                                        break;
                                    }
                                case Type enumType when enumType == typeof(SensorStatus):
                                    {
                                        if (int.TryParse(value, out int enumValue))
                                            this.SetPropertyValue(property, (SensorStatus)enumValue);
                                        break;
                                    }
                                case Type enumType when enumType == typeof(StatusTypes):
                                    {
                                        if (int.TryParse(value, out int enumValue))
                                            this.SetPropertyValue(property, (StatusTypes)enumValue);
                                        break;
                                    }
                                case Type enumType when enumType == typeof(VacationOperations):
                                    {
                                        if (int.TryParse(value, out int enumValue))
                                            this.SetPropertyValue(property, (VacationOperations)enumValue);
                                        break;
                                    }
                                case Type enumType when enumType == typeof(WeeklyProfiles):
                                    {
                                        if (int.TryParse(value, out int enumValue))
                                            this.SetPropertyValue(property, (WeeklyProfiles)enumValue);
                                        break;
                                    }
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Property Helper

        /// <summary>
        /// Returns the Helios attribute of the property.
        /// </summary>
        /// <param name="property">The property name.</param>
        /// <returns>The Helios attribute.</returns>
        public static HeliosAttribute GetHeliosAttribute(string property)
            => HeliosAttribute.GetHeliosAttribute(typeof(HeliosData).GetProperty(property));

        /// <summary>
        /// Gets the property list for the HeliosData class.
        /// </summary>
        /// <returns>The property list.</returns>
        public static List<string> GetProperties()
            => typeof(HeliosData).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                .Select(p => p.Name).ToList();

        /// <summary>
        /// Returns true if property with the specified name is found in the HeliosData class.
        /// </summary>
        /// <param name="property">The property name.</param>
        /// <returns>Returns true if property is found.</returns>
        public static bool IsProperty(string property)
            => GetProperties().Contains(property);

        /// <summary>
        /// Gets the label list for the HeliosData class.
        /// </summary>
        /// <returns>The property list.</returns>
        public static List<string> GetLabels()
            => GetProperties()
                .Select(p => GetHeliosAttribute(p).Name).ToList();

        /// <summary>
        /// Returns the property value for a property with the specified Helios attribute (label). 
        /// </summary>
        /// <param name="label">The Helios label (vXXXXX)</param>
        /// <returns>The property value</returns>
        public object? GetHeliosValue(string label)
            => this.GetPropertyValue(GetProperties().Find(p => GetHeliosAttribute(p).Name == label) ?? string.Empty);

        /// <summary>
        /// Returns the property name for a property with the specified Helios attribute (label). 
        /// </summary>
        /// <param name="label">The Helios label (vXXXXX)</param>
        /// <returns>The property name</returns>
        public static string GetHeliosProperty(string label)
            => HeliosData.GetProperties().Find(p => HeliosData.GetHeliosAttribute(p).Name == label) ?? string.Empty;

        /// <summary>
        /// Returns true if property with the specified label (vXXXXX) is found in the HeliosData class.
        /// </summary>
        /// <param name="label">The Helios label (vXXXXX)</param>
        /// <returns>Returns true if property is found.</returns>
        public static bool IsHelios(string label)
            => GetProperties().Select(p => GetHeliosAttribute(p).Name).Contains(label);

        #endregion
    }
}
