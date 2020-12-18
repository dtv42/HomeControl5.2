// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisplayData.cs" company="DTV-Online">
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
    public class DisplayData
    {
        #region Public Properties

        public StatusTypes PreheaterStatus { get; set; } = new StatusTypes();
        public SensorStatus HumidityControlStatus { get; set; } = new SensorStatus();
        public SensorStatus CO2ControlStatus { get; set; } = new SensorStatus();
        public SensorStatus VOCControlStatus { get; set; } = new SensorStatus();
        public FanLevels PartyVentilationLevel { get; set; } = new FanLevels();
        public StatusTypes PartyOperationActivate { get; set; } = new StatusTypes();
        public FanLevels StandbyVentilationLevel { get; set; } = new FanLevels();
        public StatusTypes StandbyOperationActivate { get; set; } = new StatusTypes();
        public OperationModes OperationMode { get; set; } = new OperationModes();
        public FanLevels VentilationLevel { get; set; } = new FanLevels();
        public int VentilationPercentage { get; set; }
        public double TemperatureOutdoor { get; set; }
        public double TemperatureSupply { get; set; }
        public double TemperatureExhaust { get; set; }
        public double TemperatureExtract { get; set; }
        public double TemperaturePreHeater { get; set; }
        public double TemperaturePostHeater { get; set; }
        public double ExternalHumiditySensor1 { get; set; }
        public double ExternalHumiditySensor2 { get; set; }
        public double ExternalHumiditySensor3 { get; set; }
        public double ExternalHumiditySensor4 { get; set; }
        public double ExternalHumiditySensor5 { get; set; }
        public double ExternalHumiditySensor6 { get; set; }
        public double ExternalHumiditySensor7 { get; set; }
        public double ExternalHumiditySensor8 { get; set; }
        public double ExternalHumidityTemperature1 { get; set; }
        public double ExternalHumidityTemperature2 { get; set; }
        public double ExternalHumidityTemperature3 { get; set; }
        public double ExternalHumidityTemperature4 { get; set; }
        public double ExternalHumidityTemperature5 { get; set; }
        public double ExternalHumidityTemperature6 { get; set; }
        public double ExternalHumidityTemperature7 { get; set; }
        public double ExternalHumidityTemperature8 { get; set; }
        public double ExternalCO2Sensor1 { get; set; }
        public double ExternalCO2Sensor2 { get; set; }
        public double ExternalCO2Sensor3 { get; set; }
        public double ExternalCO2Sensor4 { get; set; }
        public double ExternalCO2Sensor5 { get; set; }
        public double ExternalCO2Sensor6 { get; set; }
        public double ExternalCO2Sensor7 { get; set; }
        public double ExternalCO2Sensor8 { get; set; }
        public double ExternalVOCSensor1 { get; set; }
        public double ExternalVOCSensor2 { get; set; }
        public double ExternalVOCSensor3 { get; set; }
        public double ExternalVOCSensor4 { get; set; }
        public double ExternalVOCSensor5 { get; set; }
        public double ExternalVOCSensor6 { get; set; }
        public double ExternalVOCSensor7 { get; set; }
        public double ExternalVOCSensor8 { get; set; }
        public double TemperatureChannel { get; set; }
        public WeeklyProfiles WeeklyProfile { get; set; } = new WeeklyProfiles();
        public int SupplyFanSpeed { get; set; }
        public int ExhaustFanSpeed { get; set; }
        public VacationOperations VacationOperation { get; set; } = new VacationOperations();
        public FanLevels VacationVentilationLevel { get; set; } = new FanLevels();
        public ContactTypes ExternalContact { get; set; } = new ContactTypes();
        public FanLevels SupplyLevel { get; set; } = new FanLevels();
        public FanLevels ExhaustLevel { get; set; } = new FanLevels();
        public string SensorName1 { get; set; } = string.Empty;
        public string SensorName2 { get; set; } = string.Empty;
        public string SensorName3 { get; set; } = string.Empty;
        public string SensorName4 { get; set; } = string.Empty;
        public string SensorName5 { get; set; } = string.Empty;
        public string SensorName6 { get; set; } = string.Empty;
        public string SensorName7 { get; set; } = string.Empty;
        public string SensorName8 { get; set; } = string.Empty;
        public string CO2SensorName1 { get; set; } = string.Empty;
        public string CO2SensorName2 { get; set; } = string.Empty;
        public string CO2SensorName3 { get; set; } = string.Empty;
        public string CO2SensorName4 { get; set; } = string.Empty;
        public string CO2SensorName5 { get; set; } = string.Empty;
        public string CO2SensorName6 { get; set; } = string.Empty;
        public string CO2SensorName7 { get; set; } = string.Empty;
        public string CO2SensorName8 { get; set; } = string.Empty;
        public string VOCSensorName1 { get; set; } = string.Empty;
        public string VOCSensorName2 { get; set; } = string.Empty;
        public string VOCSensorName3 { get; set; } = string.Empty;
        public string VOCSensorName4 { get; set; } = string.Empty;
        public string VOCSensorName5 { get; set; } = string.Empty;
        public string VOCSensorName6 { get; set; } = string.Empty;
        public string VOCSensorName7 { get; set; } = string.Empty;
        public string VOCSensorName8 { get; set; } = string.Empty;
        public int NumberOfErrors { get; set; }
        public int NumberOfWarnings { get; set; }
        public int NumberOfInfos { get; set; }
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
            PreheaterStatus = data.PreheaterStatus;
            HumidityControlStatus = data.HumidityControlStatus;
            CO2ControlStatus = data.CO2ControlStatus;
            VOCControlStatus = data.VOCControlStatus;
            PartyVentilationLevel = data.PartyVentilationLevel;
            PartyOperationActivate = data.PartyOperationActivate;
            StandbyVentilationLevel = data.StandbyVentilationLevel;
            StandbyOperationActivate = data.StandbyOperationActivate;
            OperationMode = data.OperationMode;
            VentilationLevel = data.VentilationLevel;
            VentilationPercentage = data.VentilationPercentage;
            TemperatureOutdoor = data.TemperatureOutdoor;
            TemperatureSupply = data.TemperatureSupply;
            TemperatureExhaust = data.TemperatureExhaust;
            TemperatureExtract = data.TemperatureExtract;
            TemperaturePreHeater = data.TemperaturePreHeater;
            TemperaturePostHeater = data.TemperaturePostHeater;
            ExternalHumiditySensor1 = data.ExternalHumiditySensor1;
            ExternalHumiditySensor2 = data.ExternalHumiditySensor2;
            ExternalHumiditySensor3 = data.ExternalHumiditySensor3;
            ExternalHumiditySensor4 = data.ExternalHumiditySensor4;
            ExternalHumiditySensor5 = data.ExternalHumiditySensor5;
            ExternalHumiditySensor6 = data.ExternalHumiditySensor6;
            ExternalHumiditySensor7 = data.ExternalHumiditySensor7;
            ExternalHumiditySensor8 = data.ExternalHumiditySensor8;
            ExternalHumidityTemperature1 = data.ExternalHumidityTemperature1;
            ExternalHumidityTemperature2 = data.ExternalHumidityTemperature2;
            ExternalHumidityTemperature3 = data.ExternalHumidityTemperature3;
            ExternalHumidityTemperature4 = data.ExternalHumidityTemperature4;
            ExternalHumidityTemperature5 = data.ExternalHumidityTemperature5;
            ExternalHumidityTemperature6 = data.ExternalHumidityTemperature6;
            ExternalHumidityTemperature7 = data.ExternalHumidityTemperature7;
            ExternalHumidityTemperature8 = data.ExternalHumidityTemperature8;
            ExternalCO2Sensor1 = data.ExternalCO2Sensor1;
            ExternalCO2Sensor2 = data.ExternalCO2Sensor2;
            ExternalCO2Sensor3 = data.ExternalCO2Sensor3;
            ExternalCO2Sensor4 = data.ExternalCO2Sensor4;
            ExternalCO2Sensor5 = data.ExternalCO2Sensor5;
            ExternalCO2Sensor6 = data.ExternalCO2Sensor6;
            ExternalCO2Sensor7 = data.ExternalCO2Sensor7;
            ExternalCO2Sensor8 = data.ExternalCO2Sensor8;
            ExternalVOCSensor1 = data.ExternalVOCSensor1;
            ExternalVOCSensor2 = data.ExternalVOCSensor2;
            ExternalVOCSensor3 = data.ExternalVOCSensor3;
            ExternalVOCSensor4 = data.ExternalVOCSensor4;
            ExternalVOCSensor5 = data.ExternalVOCSensor5;
            ExternalVOCSensor6 = data.ExternalVOCSensor6;
            ExternalVOCSensor7 = data.ExternalVOCSensor7;
            ExternalVOCSensor8 = data.ExternalVOCSensor8;
            TemperatureChannel = data.TemperatureChannel;
            WeeklyProfile = data.WeeklyProfile;
            SupplyFanSpeed = data.SupplyFanSpeed;
            ExhaustFanSpeed = data.ExhaustFanSpeed;
            VacationOperation = data.VacationOperation;
            VacationVentilationLevel = data.VacationVentilationLevel;
            ExternalContact = data.ExternalContact;
            SupplyLevel = data.SupplyLevel;
            ExhaustLevel = data.ExhaustLevel;
            SensorName1 = data.SensorName1;
            SensorName2 = data.SensorName2;
            SensorName3 = data.SensorName3;
            SensorName4 = data.SensorName4;
            SensorName5 = data.SensorName5;
            SensorName6 = data.SensorName6;
            SensorName7 = data.SensorName7;
            SensorName8 = data.SensorName8;
            CO2SensorName1 = data.CO2SensorName1;
            CO2SensorName2 = data.CO2SensorName2;
            CO2SensorName3 = data.CO2SensorName3;
            CO2SensorName4 = data.CO2SensorName4;
            CO2SensorName5 = data.CO2SensorName5;
            CO2SensorName6 = data.CO2SensorName6;
            CO2SensorName7 = data.CO2SensorName7;
            CO2SensorName8 = data.CO2SensorName8;
            VOCSensorName1 = data.VOCSensorName1;
            VOCSensorName2 = data.VOCSensorName2;
            VOCSensorName3 = data.VOCSensorName3;
            VOCSensorName4 = data.VOCSensorName4;
            VOCSensorName5 = data.VOCSensorName5;
            VOCSensorName6 = data.VOCSensorName6;
            VOCSensorName7 = data.VOCSensorName7;
            VOCSensorName8 = data.VOCSensorName8;
            NumberOfErrors = data.NumberOfErrors;
            NumberOfWarnings = data.NumberOfWarnings;
            NumberOfInfos = data.NumberOfInfos;
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
