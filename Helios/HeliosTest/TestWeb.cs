namespace HeliosTest
{
    #region Using Directives

    using System.Text.Json;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Testing;

    using HealthChecks.UI.Core;

    using Xunit;

    using UtilityLib;

    using HeliosLib.Models;

    #endregion Using Directives

    /// <summary>
    /// XUnit testing class.
    /// </summary>
    [Collection("Helios Test Collection")]
    public class TestWeb : IClassFixture<WebApplicationFactory<HeliosWeb.Startup>>
    {
        private readonly WebApplicationFactory<HeliosWeb.Startup> _factory;
        private readonly JsonSerializerOptions _options = new();

        public TestWeb(WebApplicationFactory<HeliosWeb.Startup> factory)
        {
            _factory = factory;
            _options.AddDefaultOptions();
            _options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        }

        #region Test Methods

        [Theory]
        [InlineData("/swagger")]
        [InlineData("/swagger/v1/swagger.json")]
        public async Task TestSwaggerEndpoints(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Theory]
        [InlineData("/healthchecks")]
        [InlineData("/healthchecks-ui")]
        public async Task TestHealthEndpoints(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        [Theory]
        [InlineData("/health-process")]
        [InlineData("/health-gateway")]
        public async Task TestHealthReport(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.Contains("application/json", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<UIHealthReport>(json, _options);
            Assert.NotNull(data);
            Assert.Equal(UIHealthStatus.Healthy, data.Status);
        }

        [Theory]
        [InlineData("/gateway")]
        [InlineData("/data")]
        [InlineData("/booster")]
        [InlineData("/device")]
        [InlineData("/display")]
        [InlineData("/error")]
        [InlineData("/fan")]
        [InlineData("/heater")]
        [InlineData("/info")]
        [InlineData("/network")]
        [InlineData("/operation")]
        [InlineData("/sensor")]
        [InlineData("/system")]
        [InlineData("/technical")]
        [InlineData("/vacation")]

        [InlineData("/data/property/ItemDescription")]
        [InlineData("/data/property/OrderNumber")]
        [InlineData("/data/property/MacAddress")]
        [InlineData("/data/property/Language")]
        [InlineData("/data/property/Date")]
        [InlineData("/data/property/Time")]
        [InlineData("/data/property/DayLightSaving")]
        [InlineData("/data/property/AutoUpdateEnabled")]
        [InlineData("/data/property/PortalAccessEnabled")]
        [InlineData("/data/property/ExhaustVentilatorVoltageLevel1")]
        [InlineData("/data/property/SupplyVentilatorVoltageLevel1")]
        [InlineData("/data/property/ExhaustVentilatorVoltageLevel2")]
        [InlineData("/data/property/SupplyVentilatorVoltageLevel2")]
        [InlineData("/data/property/ExhaustVentilatorVoltageLevel3")]
        [InlineData("/data/property/SupplyVentilatorVoltageLevel3")]
        [InlineData("/data/property/ExhaustVentilatorVoltageLevel4")]
        [InlineData("/data/property/SupplyVentilatorVoltageLevel4")]
        [InlineData("/data/property/MinimumVentilationLevel")]
        [InlineData("/data/property/KwlBeEnabled")]
        [InlineData("/data/property/KwlBecEnabled")]
        [InlineData("/data/property/DeviceConfiguration")]
        [InlineData("/data/property/PreheaterStatus")]
        [InlineData("/data/property/KwlFTFConfig0")]
        [InlineData("/data/property/KwlFTFConfig1")]
        [InlineData("/data/property/KwlFTFConfig2")]
        [InlineData("/data/property/KwlFTFConfig3")]
        [InlineData("/data/property/KwlFTFConfig4")]
        [InlineData("/data/property/KwlFTFConfig5")]
        [InlineData("/data/property/KwlFTFConfig6")]
        [InlineData("/data/property/KwlFTFConfig7")]
        [InlineData("/data/property/HumidityControlStatus")]
        [InlineData("/data/property/HumidityControlTarget")]
        [InlineData("/data/property/HumidityControlStep")]
        [InlineData("/data/property/HumidityControlStop")]
        [InlineData("/data/property/CO2ControlStatus")]
        [InlineData("/data/property/CO2ControlTarget")]
        [InlineData("/data/property/CO2ControlStep")]
        [InlineData("/data/property/VOCControlStatus")]
        [InlineData("/data/property/VOCControlTarget")]
        [InlineData("/data/property/VOCControlStep")]
        [InlineData("/data/property/ThermalComfortTemperature")]
        [InlineData("/data/property/TimeZoneOffset")]
        [InlineData("/data/property/DateFormat")]
        [InlineData("/data/property/HeatExchangerType")]
        [InlineData("/data/property/PartyOperationDuration")]
        [InlineData("/data/property/PartyVentilationLevel")]
        [InlineData("/data/property/PartyOperationRemaining")]
        [InlineData("/data/property/PartyOperationActivate")]
        [InlineData("/data/property/StandbyOperationDuration")]
        [InlineData("/data/property/StandbyVentilationLevel")]
        [InlineData("/data/property/StandbyOperationRemaining")]
        [InlineData("/data/property/StandbyOperationActivate")]
        [InlineData("/data/property/OperationMode")]
        [InlineData("/data/property/VentilationLevel")]
        [InlineData("/data/property/VentilationPercentage")]
        [InlineData("/data/property/TemperatureOutdoor")]
        [InlineData("/data/property/TemperatureSupply")]
        [InlineData("/data/property/TemperatureExhaust")]
        [InlineData("/data/property/TemperatureExtract")]
        [InlineData("/data/property/TemperaturePreHeater")]
        [InlineData("/data/property/V00109")]
        [InlineData("/data/property/TemperaturePostHeater")]
        [InlineData("/data/property/ExternalHumiditySensor1")]
        [InlineData("/data/property/ExternalHumiditySensor2")]
        [InlineData("/data/property/ExternalHumiditySensor3")]
        [InlineData("/data/property/ExternalHumiditySensor4")]
        [InlineData("/data/property/ExternalHumiditySensor5")]
        [InlineData("/data/property/ExternalHumiditySensor6")]
        [InlineData("/data/property/ExternalHumiditySensor7")]
        [InlineData("/data/property/ExternalHumiditySensor8")]
        [InlineData("/data/property/ExternalHumidityTemperature1")]
        [InlineData("/data/property/ExternalHumidityTemperature2")]
        [InlineData("/data/property/ExternalHumidityTemperature3")]
        [InlineData("/data/property/ExternalHumidityTemperature4")]
        [InlineData("/data/property/ExternalHumidityTemperature5")]
        [InlineData("/data/property/ExternalHumidityTemperature6")]
        [InlineData("/data/property/ExternalHumidityTemperature7")]
        [InlineData("/data/property/ExternalHumidityTemperature8")]
        [InlineData("/data/property/V00127")]
        [InlineData("/data/property/ExternalCO2Sensor1")]
        [InlineData("/data/property/ExternalCO2Sensor2")]
        [InlineData("/data/property/ExternalCO2Sensor3")]
        [InlineData("/data/property/ExternalCO2Sensor4")]
        [InlineData("/data/property/ExternalCO2Sensor5")]
        [InlineData("/data/property/ExternalCO2Sensor6")]
        [InlineData("/data/property/ExternalCO2Sensor7")]
        [InlineData("/data/property/ExternalCO2Sensor8")]
        [InlineData("/data/property/ExternalVOCSensor1")]
        [InlineData("/data/property/ExternalVOCSensor2")]
        [InlineData("/data/property/ExternalVOCSensor3")]
        [InlineData("/data/property/ExternalVOCSensor4")]
        [InlineData("/data/property/ExternalVOCSensor5")]
        [InlineData("/data/property/ExternalVOCSensor6")]
        [InlineData("/data/property/ExternalVOCSensor7")]
        [InlineData("/data/property/ExternalVOCSensor8")]
        [InlineData("/data/property/V00144")]
        [InlineData("/data/property/TemperatureChannel")]
        [InlineData("/data/property/WeeklyProfile")]
        [InlineData("/data/property/V00220")]
        [InlineData("/data/property/V00221")]
        [InlineData("/data/property/V00222")]
        [InlineData("/data/property/V00223")]
        [InlineData("/data/property/V00224")]
        [InlineData("/data/property/V00225")]
        [InlineData("/data/property/V00226")]
        [InlineData("/data/property/V00227")]
        [InlineData("/data/property/V00228")]
        [InlineData("/data/property/V00229")]
        [InlineData("/data/property/SerialNumber")]
        [InlineData("/data/property/ProductionCode")]
        [InlineData("/data/property/SecurityCode")]
        [InlineData("/data/property/SupplyFanSpeed")]
        [InlineData("/data/property/ExhaustFanSpeed")]
        [InlineData("/data/property/Password")]
        [InlineData("/data/property/Logout")]
        [InlineData("/data/property/VacationOperation")]
        [InlineData("/data/property/VacationVentilationLevel")]
        [InlineData("/data/property/VacationStartDate")]
        [InlineData("/data/property/VacationEndDate")]
        [InlineData("/data/property/VacationInterval")]
        [InlineData("/data/property/VacationDuration")]
        [InlineData("/data/property/V00901")]
        [InlineData("/data/property/V00920")]
        [InlineData("/data/property/V00921")]
        [InlineData("/data/property/V00922")]
        [InlineData("/data/property/V00923")]
        [InlineData("/data/property/V00924")]
        [InlineData("/data/property/V00925")]
        [InlineData("/data/property/V00926")]
        [InlineData("/data/property/V00927")]
        [InlineData("/data/property/V00928")]
        [InlineData("/data/property/V00929")]
        [InlineData("/data/property/UseDHCP")]
        [InlineData("/data/property/IPAddress")]
        [InlineData("/data/property/SubnetMask")]
        [InlineData("/data/property/Gateway")]
        [InlineData("/data/property/StandardDNS")]
        [InlineData("/data/property/FallbackDNS")]
        [InlineData("/data/property/HostName")]
        [InlineData("/data/property/PreheaterType")]
        [InlineData("/data/property/KwlFunctionType")]
        [InlineData("/data/property/HeaterAfterRunTime")]
        [InlineData("/data/property/ExternalContact")]
        [InlineData("/data/property/FaultTypeOutput")]
        [InlineData("/data/property/FilterChange")]
        [InlineData("/data/property/FilterChangeInterval")]
        [InlineData("/data/property/FilterChangeRemaining")]
        [InlineData("/data/property/BypassRoomTemperature")]
        [InlineData("/data/property/BypassOutdoorTemperature")]
        [InlineData("/data/property/BypassOutdoorTemperature2")]
        [InlineData("/data/property/RestartChangeInterval")]
        [InlineData("/data/property/StartReset")]
        [InlineData("/data/property/FactoryReset")]
        [InlineData("/data/property/SupplyLevel")]
        [InlineData("/data/property/ExhaustLevel")]
        [InlineData("/data/property/FanLevelRegion02")]
        [InlineData("/data/property/FanLevelRegion24")]
        [InlineData("/data/property/FanLevelRegion46")]
        [InlineData("/data/property/FanLevelRegion68")]
        [InlineData("/data/property/FanLevelRegion80")]
        [InlineData("/data/property/OffsetExhaust")]
        [InlineData("/data/property/FanLevelConfiguration")]
        [InlineData("/data/property/SensorName1")]
        [InlineData("/data/property/SensorName2")]
        [InlineData("/data/property/SensorName3")]
        [InlineData("/data/property/SensorName4")]
        [InlineData("/data/property/SensorName5")]
        [InlineData("/data/property/SensorName6")]
        [InlineData("/data/property/SensorName7")]
        [InlineData("/data/property/SensorName8")]
        [InlineData("/data/property/CO2SensorName1")]
        [InlineData("/data/property/CO2SensorName2")]
        [InlineData("/data/property/CO2SensorName3")]
        [InlineData("/data/property/CO2SensorName4")]
        [InlineData("/data/property/CO2SensorName5")]
        [InlineData("/data/property/CO2SensorName6")]
        [InlineData("/data/property/CO2SensorName7")]
        [InlineData("/data/property/CO2SensorName8")]
        [InlineData("/data/property/VOCSensorName1")]
        [InlineData("/data/property/VOCSensorName2")]
        [InlineData("/data/property/VOCSensorName3")]
        [InlineData("/data/property/VOCSensorName4")]
        [InlineData("/data/property/VOCSensorName5")]
        [InlineData("/data/property/VOCSensorName6")]
        [InlineData("/data/property/VOCSensorName7")]
        [InlineData("/data/property/VOCSensorName8")]
        [InlineData("/data/property/SoftwareVersion")]
        [InlineData("/data/property/OperatingHours")]
        [InlineData("/data/property/OperationMinutesSupply")]
        [InlineData("/data/property/OperationMinutesExhaust")]
        [InlineData("/data/property/OperationMinutesPreheater")]
        [InlineData("/data/property/OperationMinutesAfterheater")]
        [InlineData("/data/property/PowerPreheater")]
        [InlineData("/data/property/PowerAfterheater")]
        [InlineData("/data/property/ResetFlag")]
        [InlineData("/data/property/ErrorCode")]
        [InlineData("/data/property/WarningCode")]
        [InlineData("/data/property/InfoCode")]
        [InlineData("/data/property/ModbusActivated")]
        [InlineData("/data/property/NumberOfErrors")]
        [InlineData("/data/property/NumberOfWarnings")]
        [InlineData("/data/property/NumberOfInfos")]
        [InlineData("/data/property/Errors")]
        [InlineData("/data/property/Warnings")]
        [InlineData("/data/property/Infos")]
        [InlineData("/data/property/StatusFlags")]
        [InlineData("/data/property/GlobalUpdate")]
        [InlineData("/data/property/LastError")]
        [InlineData("/data/property/ClearError")]
        [InlineData("/data/property/SensorConfig1")]
        [InlineData("/data/property/SensorConfig2")]
        [InlineData("/data/property/SensorConfig3")]
        [InlineData("/data/property/SensorConfig4")]
        [InlineData("/data/property/SensorConfig5")]
        [InlineData("/data/property/SensorConfig6")]
        [InlineData("/data/property/SensorConfig7")]
        [InlineData("/data/property/SensorConfig8")]
        [InlineData("/data/property/V02103")]
        [InlineData("/data/property/DataExchange")]
        [InlineData("/data/property/V02115")]
        [InlineData("/data/property/ActivateAutoMode")]
        [InlineData("/data/property/V02117")]
        [InlineData("/data/property/V02118")]
        [InlineData("/data/property/V02119")]
        [InlineData("/data/property/V02120")]
        [InlineData("/data/property/V02121")]
        [InlineData("/data/property/V02122")]
        [InlineData("/data/property/V02123")]
        [InlineData("/data/property/V02128")]
        [InlineData("/data/property/V02129")]
        [InlineData("/data/property/V02130")]
        [InlineData("/data/property/V02131")]
        [InlineData("/data/property/CountryCode")]
        [InlineData("/data/property/V02136")]
        [InlineData("/data/property/V02137")]
        [InlineData("/data/property/V02142")]
        [InlineData("/data/property/V02143")]
        [InlineData("/data/property/V02144")]
        [InlineData("/data/property/V02145")]
        [InlineData("/data/property/V02146")]
        [InlineData("/data/property/V02147")]
        [InlineData("/data/property/V02148")]
        [InlineData("/data/property/V02149")]
        [InlineData("/data/property/V02150")]
        [InlineData("/data/property/V02151")]
        [InlineData("/data/property/V02152")]

        [InlineData("/booster/property/PartyOperationDuration")]
        [InlineData("/booster/property/PartyVentilationLevel")]
        [InlineData("/booster/property/PartyOperationRemaining")]
        [InlineData("/booster/property/PartyOperationActivate")]
        [InlineData("/booster/property/StandbyOperationDuration")]
        [InlineData("/booster/property/StandbyVentilationLevel")]
        [InlineData("/booster/property/StandbyOperationRemaining")]
        [InlineData("/booster/property/StandbyOperationActivate")]
        [InlineData("/booster/property/StatusFlags")]

        [InlineData("/device/property/KwlBeEnabled")]
        [InlineData("/device/property/KwlBecEnabled")]
        [InlineData("/device/property/DeviceConfiguration")]
        [InlineData("/device/property/PreheaterStatus")]
        [InlineData("/device/property/HeatExchangerType")]
        [InlineData("/device/property/WeeklyProfile")]
        [InlineData("/device/property/PreheaterType")]
        [InlineData("/device/property/KwlFunctionType")]
        [InlineData("/device/property/HeaterAfterRunTime")]
        [InlineData("/device/property/ExternalContact")]
        [InlineData("/device/property/FaultTypeOutput")]
        [InlineData("/device/property/FilterChange")]
        [InlineData("/device/property/FilterChangeInterval")]
        [InlineData("/device/property/FilterChangeRemaining")]
        [InlineData("/device/property/BypassRoomTemperature")]
        [InlineData("/device/property/BypassOutdoorTemperature")]
        [InlineData("/device/property/BypassOutdoorTemperature2")]
        [InlineData("/device/property/StartReset")]
        [InlineData("/device/property/FactoryReset")]
        [InlineData("/device/property/ModbusActivated")]
        [InlineData("/device/property/StatusFlags")]
        [InlineData("/device/property/V02115")]
        [InlineData("/device/property/V02120")]
        [InlineData("/device/property/V02121")]
        [InlineData("/device/property/V02128")]
        [InlineData("/device/property/V02129")]

        [InlineData("/display/property/PreheaterStatus")]
        [InlineData("/display/property/HumidityControlStatus")]
        [InlineData("/display/property/CO2ControlStatus")]
        [InlineData("/display/property/VOCControlStatus")]
        [InlineData("/display/property/PartyVentilationLevel")]
        [InlineData("/display/property/PartyOperationActivate")]
        [InlineData("/display/property/StandbyVentilationLevel")]
        [InlineData("/display/property/StandbyOperationActivate")]
        [InlineData("/display/property/OperationMode")]
        [InlineData("/display/property/VentilationLevel")]
        [InlineData("/display/property/VentilationPercentage")]
        [InlineData("/display/property/TemperatureOutdoor")]
        [InlineData("/display/property/TemperatureSupply")]
        [InlineData("/display/property/TemperatureExhaust")]
        [InlineData("/display/property/TemperatureExtract")]
        [InlineData("/display/property/TemperaturePreHeater")]
        [InlineData("/display/property/TemperaturePostHeater")]
        [InlineData("/display/property/ExternalHumiditySensor1")]
        [InlineData("/display/property/ExternalHumiditySensor2")]
        [InlineData("/display/property/ExternalHumiditySensor3")]
        [InlineData("/display/property/ExternalHumiditySensor4")]
        [InlineData("/display/property/ExternalHumiditySensor5")]
        [InlineData("/display/property/ExternalHumiditySensor6")]
        [InlineData("/display/property/ExternalHumiditySensor7")]
        [InlineData("/display/property/ExternalHumiditySensor8")]
        [InlineData("/display/property/ExternalHumidityTemperature1")]
        [InlineData("/display/property/ExternalHumidityTemperature2")]
        [InlineData("/display/property/ExternalHumidityTemperature3")]
        [InlineData("/display/property/ExternalHumidityTemperature4")]
        [InlineData("/display/property/ExternalHumidityTemperature5")]
        [InlineData("/display/property/ExternalHumidityTemperature6")]
        [InlineData("/display/property/ExternalHumidityTemperature7")]
        [InlineData("/display/property/ExternalHumidityTemperature8")]
        [InlineData("/display/property/ExternalCO2Sensor1")]
        [InlineData("/display/property/ExternalCO2Sensor2")]
        [InlineData("/display/property/ExternalCO2Sensor3")]
        [InlineData("/display/property/ExternalCO2Sensor4")]
        [InlineData("/display/property/ExternalCO2Sensor5")]
        [InlineData("/display/property/ExternalCO2Sensor6")]
        [InlineData("/display/property/ExternalCO2Sensor7")]
        [InlineData("/display/property/ExternalCO2Sensor8")]
        [InlineData("/display/property/ExternalVOCSensor1")]
        [InlineData("/display/property/ExternalVOCSensor2")]
        [InlineData("/display/property/ExternalVOCSensor3")]
        [InlineData("/display/property/ExternalVOCSensor4")]
        [InlineData("/display/property/ExternalVOCSensor5")]
        [InlineData("/display/property/ExternalVOCSensor6")]
        [InlineData("/display/property/ExternalVOCSensor7")]
        [InlineData("/display/property/ExternalVOCSensor8")]
        [InlineData("/display/property/TemperatureChannel")]
        [InlineData("/display/property/WeeklyProfile")]
        [InlineData("/display/property/SupplyFanSpeed")]
        [InlineData("/display/property/ExhaustFanSpeed")]
        [InlineData("/display/property/VacationOperation")]
        [InlineData("/display/property/VacationVentilationLevel")]
        [InlineData("/display/property/ExternalContact")]
        [InlineData("/display/property/SupplyLevel")]
        [InlineData("/display/property/ExhaustLevel")]
        [InlineData("/display/property/SensorName1")]
        [InlineData("/display/property/SensorName2")]
        [InlineData("/display/property/SensorName3")]
        [InlineData("/display/property/SensorName4")]
        [InlineData("/display/property/SensorName5")]
        [InlineData("/display/property/SensorName6")]
        [InlineData("/display/property/SensorName7")]
        [InlineData("/display/property/SensorName8")]
        [InlineData("/display/property/CO2SensorName1")]
        [InlineData("/display/property/CO2SensorName2")]
        [InlineData("/display/property/CO2SensorName3")]
        [InlineData("/display/property/CO2SensorName4")]
        [InlineData("/display/property/CO2SensorName5")]
        [InlineData("/display/property/CO2SensorName6")]
        [InlineData("/display/property/CO2SensorName7")]
        [InlineData("/display/property/CO2SensorName8")]
        [InlineData("/display/property/VOCSensorName1")]
        [InlineData("/display/property/VOCSensorName2")]
        [InlineData("/display/property/VOCSensorName3")]
        [InlineData("/display/property/VOCSensorName4")]
        [InlineData("/display/property/VOCSensorName5")]
        [InlineData("/display/property/VOCSensorName6")]
        [InlineData("/display/property/VOCSensorName7")]
        [InlineData("/display/property/VOCSensorName8")]
        [InlineData("/display/property/NumberOfErrors")]
        [InlineData("/display/property/NumberOfWarnings")]
        [InlineData("/display/property/NumberOfInfos")]
        [InlineData("/display/property/StatusFlags")]
        [InlineData("/display/property/SensorConfig1")]
        [InlineData("/display/property/SensorConfig2")]
        [InlineData("/display/property/SensorConfig3")]
        [InlineData("/display/property/SensorConfig4")]
        [InlineData("/display/property/SensorConfig5")]
        [InlineData("/display/property/SensorConfig6")]
        [InlineData("/display/property/SensorConfig7")]
        [InlineData("/display/property/SensorConfig8")]

        [InlineData("/error/property/NumberOfErrors")]
        [InlineData("/error/property/NumberOfWarnings")]
        [InlineData("/error/property/NumberOfInfos")]
        [InlineData("/error/property/Errors")]
        [InlineData("/error/property/Warnings")]
        [InlineData("/error/property/Infos")]
        [InlineData("/error/property/StatusFlags")]
        [InlineData("/error/property/DataExchange")]

        [InlineData("/fan/property/ExhaustVentilatorVoltageLevel1")]
        [InlineData("/fan/property/SupplyVentilatorVoltageLevel1")]
        [InlineData("/fan/property/ExhaustVentilatorVoltageLevel2")]
        [InlineData("/fan/property/SupplyVentilatorVoltageLevel2")]
        [InlineData("/fan/property/ExhaustVentilatorVoltageLevel3")]
        [InlineData("/fan/property/SupplyVentilatorVoltageLevel3")]
        [InlineData("/fan/property/ExhaustVentilatorVoltageLevel4")]
        [InlineData("/fan/property/SupplyVentilatorVoltageLevel4")]
        [InlineData("/fan/property/MinimumVentilationLevel")]
        [InlineData("/fan/property/SupplyLevel")]
        [InlineData("/fan/property/ExhaustLevel")]
        [InlineData("/fan/property/FanLevelRegion02")]
        [InlineData("/fan/property/FanLevelRegion24")]
        [InlineData("/fan/property/FanLevelRegion46")]
        [InlineData("/fan/property/FanLevelRegion68")]
        [InlineData("/fan/property/FanLevelRegion80")]
        [InlineData("/fan/property/OffsetExhaust")]
        [InlineData("/fan/property/FanLevelConfiguration")]
        [InlineData("/fan/property/StatusFlags")]

        [InlineData("/heater/property/ItemDescription")]
        [InlineData("/heater/property/OrderNumber")]
        [InlineData("/heater/property/MacAddress")]
        [InlineData("/heater/property/WeeklyProfile")]
        [InlineData("/heater/property/StatusFlags")]

        [InlineData("/info/property/ItemDescription")]
        [InlineData("/info/property/OrderNumber")]
        [InlineData("/info/property/MacAddress")]
        [InlineData("/info/property/PartyOperationRemaining")]
        [InlineData("/info/property/PartyOperationActivate")]
        [InlineData("/info/property/StandbyOperationRemaining")]
        [InlineData("/info/property/StandbyOperationActivate")]
        [InlineData("/info/property/OperationMode")]
        [InlineData("/info/property/VentilationLevel")]
        [InlineData("/info/property/VentilationPercentage")]
        [InlineData("/info/property/VacationOperation")]
        [InlineData("/info/property/VacationEndDate")]
        [InlineData("/info/property/ExternalContact")]
        [InlineData("/info/property/StatusFlags")]

        [InlineData("/network/property/UseDHCP")]
        [InlineData("/network/property/IPAddress")]
        [InlineData("/network/property/SubnetMask")]
        [InlineData("/network/property/Gateway")]
        [InlineData("/network/property/StandardDNS")]
        [InlineData("/network/property/FallbackDNS")]
        [InlineData("/network/property/HostName")]
        [InlineData("/network/property/StatusFlags")]

        [InlineData("/operation/property/ItemDescription")]
        [InlineData("/operation/property/OrderNumber")]
        [InlineData("/operation/property/MacAddress")]
        [InlineData("/operation/property/Language")]
        [InlineData("/operation/property/Date")]
        [InlineData("/operation/property/Time")]
        [InlineData("/operation/property/DayLightSaving")]
        [InlineData("/operation/property/AutoUpdateEnabled")]
        [InlineData("/operation/property/PortalAccessEnabled")]
        [InlineData("/operation/property/ExhaustVentilatorVoltageLevel1")]
        [InlineData("/operation/property/SupplyVentilatorVoltageLevel1")]
        [InlineData("/operation/property/ExhaustVentilatorVoltageLevel2")]
        [InlineData("/operation/property/SupplyVentilatorVoltageLevel2")]
        [InlineData("/operation/property/ExhaustVentilatorVoltageLevel3")]
        [InlineData("/operation/property/SupplyVentilatorVoltageLevel3")]
        [InlineData("/operation/property/ExhaustVentilatorVoltageLevel4")]
        [InlineData("/operation/property/SupplyVentilatorVoltageLevel4")]
        [InlineData("/operation/property/MinimumVentilationLevel")]
        [InlineData("/operation/property/KwlBeEnabled")]
        [InlineData("/operation/property/KwlBecEnabled")]
        [InlineData("/operation/property/DeviceConfiguration")]
        [InlineData("/operation/property/PreheaterStatus")]
        [InlineData("/operation/property/KwlFTFConfig0")]
        [InlineData("/operation/property/KwlFTFConfig1")]
        [InlineData("/operation/property/KwlFTFConfig2")]
        [InlineData("/operation/property/KwlFTFConfig3")]
        [InlineData("/operation/property/KwlFTFConfig4")]
        [InlineData("/operation/property/KwlFTFConfig5")]
        [InlineData("/operation/property/KwlFTFConfig6")]
        [InlineData("/operation/property/KwlFTFConfig7")]
        [InlineData("/operation/property/HumidityControlStatus")]
        [InlineData("/operation/property/HumidityControlTarget")]
        [InlineData("/operation/property/HumidityControlStep")]
        [InlineData("/operation/property/HumidityControlStop")]
        [InlineData("/operation/property/CO2ControlStatus")]
        [InlineData("/operation/property/CO2ControlTarget")]
        [InlineData("/operation/property/CO2ControlStep")]
        [InlineData("/operation/property/VOCControlStatus")]
        [InlineData("/operation/property/VOCControlTarget")]
        [InlineData("/operation/property/VOCControlStep")]
        [InlineData("/operation/property/ThermalComfortTemperature")]
        [InlineData("/operation/property/TimeZoneOffset")]
        [InlineData("/operation/property/DateFormat")]
        [InlineData("/operation/property/HeatExchangerType")]
        [InlineData("/operation/property/KwlFunctionType")]
        [InlineData("/operation/property/SensorName1")]
        [InlineData("/operation/property/SensorName2")]
        [InlineData("/operation/property/SensorName3")]
        [InlineData("/operation/property/SensorName4")]
        [InlineData("/operation/property/SensorName5")]
        [InlineData("/operation/property/SensorName6")]
        [InlineData("/operation/property/SensorName7")]
        [InlineData("/operation/property/SensorName8")]
        [InlineData("/operation/property/StatusFlags")]
        [InlineData("/operation/property/SensorConfig1")]
        [InlineData("/operation/property/SensorConfig2")]
        [InlineData("/operation/property/SensorConfig3")]
        [InlineData("/operation/property/SensorConfig4")]
        [InlineData("/operation/property/SensorConfig5")]
        [InlineData("/operation/property/SensorConfig6")]
        [InlineData("/operation/property/SensorConfig7")]
        [InlineData("/operation/property/SensorConfig8")]

        [InlineData("/sensor/property/KwlFTFConfig0")]
        [InlineData("/sensor/property/KwlFTFConfig1")]
        [InlineData("/sensor/property/KwlFTFConfig2")]
        [InlineData("/sensor/property/KwlFTFConfig3")]
        [InlineData("/sensor/property/KwlFTFConfig4")]
        [InlineData("/sensor/property/KwlFTFConfig5")]
        [InlineData("/sensor/property/KwlFTFConfig6")]
        [InlineData("/sensor/property/KwlFTFConfig7")]
        [InlineData("/sensor/property/HumidityControlStatus")]
        [InlineData("/sensor/property/HumidityControlTarget")]
        [InlineData("/sensor/property/HumidityControlStep")]
        [InlineData("/sensor/property/HumidityControlStop")]
        [InlineData("/sensor/property/CO2ControlStatus")]
        [InlineData("/sensor/property/CO2ControlTarget")]
        [InlineData("/sensor/property/CO2ControlStep")]
        [InlineData("/sensor/property/VOCControlStatus")]
        [InlineData("/sensor/property/VOCControlTarget")]
        [InlineData("/sensor/property/VOCControlStep")]
        [InlineData("/sensor/property/SensorName1")]
        [InlineData("/sensor/property/SensorName2")]
        [InlineData("/sensor/property/SensorName3")]
        [InlineData("/sensor/property/SensorName4")]
        [InlineData("/sensor/property/SensorName5")]
        [InlineData("/sensor/property/SensorName6")]
        [InlineData("/sensor/property/SensorName7")]
        [InlineData("/sensor/property/SensorName8")]
        [InlineData("/sensor/property/CO2SensorName1")]
        [InlineData("/sensor/property/CO2SensorName2")]
        [InlineData("/sensor/property/CO2SensorName3")]
        [InlineData("/sensor/property/CO2SensorName4")]
        [InlineData("/sensor/property/CO2SensorName5")]
        [InlineData("/sensor/property/CO2SensorName6")]
        [InlineData("/sensor/property/CO2SensorName7")]
        [InlineData("/sensor/property/CO2SensorName8")]
        [InlineData("/sensor/property/VOCSensorName1")]
        [InlineData("/sensor/property/VOCSensorName2")]
        [InlineData("/sensor/property/VOCSensorName3")]
        [InlineData("/sensor/property/VOCSensorName4")]
        [InlineData("/sensor/property/VOCSensorName5")]
        [InlineData("/sensor/property/VOCSensorName6")]
        [InlineData("/sensor/property/VOCSensorName7")]
        [InlineData("/sensor/property/VOCSensorName8")]
        [InlineData("/sensor/property/StatusFlags")]
        [InlineData("/sensor/property/V02137")]
        [InlineData("/sensor/property/V02142")]
        [InlineData("/sensor/property/V02143")]
        [InlineData("/sensor/property/V02144")]
        [InlineData("/sensor/property/V02145")]
        [InlineData("/sensor/property/V02146")]
        [InlineData("/sensor/property/V02147")]
        [InlineData("/sensor/property/V02148")]
        [InlineData("/sensor/property/V02149")]
        [InlineData("/sensor/property/V02150")]
        [InlineData("/sensor/property/V02151")]
        [InlineData("/sensor/property/V02152")]

        [InlineData("/system/property/Language")]
        [InlineData("/system/property/Date")]
        [InlineData("/system/property/Time")]
        [InlineData("/system/property/DayLightSaving")]
        [InlineData("/system/property/AutoUpdateEnabled")]
        [InlineData("/system/property/PortalAccessEnabled")]
        [InlineData("/system/property/TimeZoneOffset")]
        [InlineData("/system/property/DateFormat")]
        [InlineData("/system/property/SupplyFanSpeed")]
        [InlineData("/system/property/ExhaustFanSpeed")]
        [InlineData("/system/property/SoftwareVersion")]
        [InlineData("/system/property/OperationMinutesSupply")]
        [InlineData("/system/property/OperationMinutesExhaust")]
        [InlineData("/system/property/OperationMinutesPreheater")]
        [InlineData("/system/property/OperationMinutesAfterheater")]
        [InlineData("/system/property/PowerPreheater")]
        [InlineData("/system/property/PowerAfterheater")]
        [InlineData("/system/property/StatusFlags")]
        [InlineData("/system/property/ActivateAutoMode")]
        [InlineData("/system/property/CountryCode")]
        [InlineData("/system/property/V02103")]

        [InlineData("/technical/property/ItemDescription")]
        [InlineData("/technical/property/OrderNumber")]
        [InlineData("/technical/property/MacAddress")]
        [InlineData("/technical/property/SerialNumber")]
        [InlineData("/technical/property/ProductionCode")]
        [InlineData("/technical/property/SecurityCode")]
        [InlineData("/technical/property/StatusFlags")]

        [InlineData("/vacation/property/VacationOperation")]
        [InlineData("/vacation/property/VacationVentilationLevel")]
        [InlineData("/vacation/property/VacationStartDate")]
        [InlineData("/vacation/property/VacationEndDate")]
        [InlineData("/vacation/property/VacationInterval")]
        [InlineData("/vacation/property/VacationDuration")]
        [InlineData("/vacation/property/SupplyLevel")]
        [InlineData("/vacation/property/ExhaustLevel")]
        [InlineData("/vacation/property/StatusFlags")]
        public async Task TestEndpoints(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
        }

        [Theory]
        [InlineData("/data/property/ItemDescription")]
        [InlineData("/data/property/OrderNumber")]
        [InlineData("/data/property/MacAddress")]
        [InlineData("/data/property/Language")]
        [InlineData("/data/property/Date")]
        [InlineData("/data/property/Time")]
        [InlineData("/data/property/DayLightSaving")]
        [InlineData("/data/property/AutoUpdateEnabled")]
        [InlineData("/data/property/PortalAccessEnabled")]
        [InlineData("/data/property/ExhaustVentilatorVoltageLevel1")]
        [InlineData("/data/property/SupplyVentilatorVoltageLevel1")]
        [InlineData("/data/property/ExhaustVentilatorVoltageLevel2")]
        [InlineData("/data/property/SupplyVentilatorVoltageLevel2")]
        [InlineData("/data/property/ExhaustVentilatorVoltageLevel3")]
        [InlineData("/data/property/SupplyVentilatorVoltageLevel3")]
        [InlineData("/data/property/ExhaustVentilatorVoltageLevel4")]
        [InlineData("/data/property/SupplyVentilatorVoltageLevel4")]
        [InlineData("/data/property/MinimumVentilationLevel")]
        [InlineData("/data/property/KwlBeEnabled")]
        [InlineData("/data/property/KwlBecEnabled")]
        [InlineData("/data/property/DeviceConfiguration")]
        [InlineData("/data/property/PreheaterStatus")]
        [InlineData("/data/property/KwlFTFConfig0")]
        [InlineData("/data/property/KwlFTFConfig1")]
        [InlineData("/data/property/KwlFTFConfig2")]
        [InlineData("/data/property/KwlFTFConfig3")]
        [InlineData("/data/property/KwlFTFConfig4")]
        [InlineData("/data/property/KwlFTFConfig5")]
        [InlineData("/data/property/KwlFTFConfig6")]
        [InlineData("/data/property/KwlFTFConfig7")]
        [InlineData("/data/property/HumidityControlStatus")]
        [InlineData("/data/property/HumidityControlTarget")]
        [InlineData("/data/property/HumidityControlStep")]
        [InlineData("/data/property/HumidityControlStop")]
        [InlineData("/data/property/CO2ControlStatus")]
        [InlineData("/data/property/CO2ControlTarget")]
        [InlineData("/data/property/CO2ControlStep")]
        [InlineData("/data/property/VOCControlStatus")]
        [InlineData("/data/property/VOCControlTarget")]
        [InlineData("/data/property/VOCControlStep")]
        [InlineData("/data/property/ThermalComfortTemperature")]
        [InlineData("/data/property/TimeZoneOffset")]
        [InlineData("/data/property/DateFormat")]
        [InlineData("/data/property/HeatExchangerType")]
        [InlineData("/data/property/PartyOperationDuration")]
        [InlineData("/data/property/PartyVentilationLevel")]
        [InlineData("/data/property/PartyOperationRemaining")]
        [InlineData("/data/property/PartyOperationActivate")]
        [InlineData("/data/property/StandbyOperationDuration")]
        [InlineData("/data/property/StandbyVentilationLevel")]
        [InlineData("/data/property/StandbyOperationRemaining")]
        [InlineData("/data/property/StandbyOperationActivate")]
        [InlineData("/data/property/OperationMode")]
        [InlineData("/data/property/VentilationLevel")]
        [InlineData("/data/property/VentilationPercentage")]
        [InlineData("/data/property/TemperatureOutdoor")]
        [InlineData("/data/property/TemperatureSupply")]
        [InlineData("/data/property/TemperatureExhaust")]
        [InlineData("/data/property/TemperatureExtract")]
        [InlineData("/data/property/TemperaturePreHeater")]
        [InlineData("/data/property/V00109")]
        [InlineData("/data/property/TemperaturePostHeater")]
        [InlineData("/data/property/ExternalHumiditySensor1")]
        [InlineData("/data/property/ExternalHumiditySensor2")]
        [InlineData("/data/property/ExternalHumiditySensor3")]
        [InlineData("/data/property/ExternalHumiditySensor4")]
        [InlineData("/data/property/ExternalHumiditySensor5")]
        [InlineData("/data/property/ExternalHumiditySensor6")]
        [InlineData("/data/property/ExternalHumiditySensor7")]
        [InlineData("/data/property/ExternalHumiditySensor8")]
        [InlineData("/data/property/ExternalHumidityTemperature1")]
        [InlineData("/data/property/ExternalHumidityTemperature2")]
        [InlineData("/data/property/ExternalHumidityTemperature3")]
        [InlineData("/data/property/ExternalHumidityTemperature4")]
        [InlineData("/data/property/ExternalHumidityTemperature5")]
        [InlineData("/data/property/ExternalHumidityTemperature6")]
        [InlineData("/data/property/ExternalHumidityTemperature7")]
        [InlineData("/data/property/ExternalHumidityTemperature8")]
        [InlineData("/data/property/V00127")]
        [InlineData("/data/property/ExternalCO2Sensor1")]
        [InlineData("/data/property/ExternalCO2Sensor2")]
        [InlineData("/data/property/ExternalCO2Sensor3")]
        [InlineData("/data/property/ExternalCO2Sensor4")]
        [InlineData("/data/property/ExternalCO2Sensor5")]
        [InlineData("/data/property/ExternalCO2Sensor6")]
        [InlineData("/data/property/ExternalCO2Sensor7")]
        [InlineData("/data/property/ExternalCO2Sensor8")]
        [InlineData("/data/property/ExternalVOCSensor1")]
        [InlineData("/data/property/ExternalVOCSensor2")]
        [InlineData("/data/property/ExternalVOCSensor3")]
        [InlineData("/data/property/ExternalVOCSensor4")]
        [InlineData("/data/property/ExternalVOCSensor5")]
        [InlineData("/data/property/ExternalVOCSensor6")]
        [InlineData("/data/property/ExternalVOCSensor7")]
        [InlineData("/data/property/ExternalVOCSensor8")]
        [InlineData("/data/property/V00144")]
        [InlineData("/data/property/TemperatureChannel")]
        [InlineData("/data/property/WeeklyProfile")]
        [InlineData("/data/property/V00220")]
        [InlineData("/data/property/V00221")]
        [InlineData("/data/property/V00222")]
        [InlineData("/data/property/V00223")]
        [InlineData("/data/property/V00224")]
        [InlineData("/data/property/V00225")]
        [InlineData("/data/property/V00226")]
        [InlineData("/data/property/V00227")]
        [InlineData("/data/property/V00228")]
        [InlineData("/data/property/V00229")]
        [InlineData("/data/property/SerialNumber")]
        [InlineData("/data/property/ProductionCode")]
        [InlineData("/data/property/SecurityCode")]
        [InlineData("/data/property/SupplyFanSpeed")]
        [InlineData("/data/property/ExhaustFanSpeed")]
        [InlineData("/data/property/Password")]
        [InlineData("/data/property/Logout")]
        [InlineData("/data/property/VacationOperation")]
        [InlineData("/data/property/VacationVentilationLevel")]
        [InlineData("/data/property/VacationStartDate")]
        [InlineData("/data/property/VacationEndDate")]
        [InlineData("/data/property/VacationInterval")]
        [InlineData("/data/property/VacationDuration")]
        [InlineData("/data/property/V00901")]
        [InlineData("/data/property/V00920")]
        [InlineData("/data/property/V00921")]
        [InlineData("/data/property/V00922")]
        [InlineData("/data/property/V00923")]
        [InlineData("/data/property/V00924")]
        [InlineData("/data/property/V00925")]
        [InlineData("/data/property/V00926")]
        [InlineData("/data/property/V00927")]
        [InlineData("/data/property/V00928")]
        [InlineData("/data/property/V00929")]
        [InlineData("/data/property/UseDHCP")]
        [InlineData("/data/property/IPAddress")]
        [InlineData("/data/property/SubnetMask")]
        [InlineData("/data/property/Gateway")]
        [InlineData("/data/property/StandardDNS")]
        [InlineData("/data/property/FallbackDNS")]
        [InlineData("/data/property/HostName")]
        [InlineData("/data/property/PreheaterType")]
        [InlineData("/data/property/KwlFunctionType")]
        [InlineData("/data/property/HeaterAfterRunTime")]
        [InlineData("/data/property/ExternalContact")]
        [InlineData("/data/property/FaultTypeOutput")]
        [InlineData("/data/property/FilterChange")]
        [InlineData("/data/property/FilterChangeInterval")]
        [InlineData("/data/property/FilterChangeRemaining")]
        [InlineData("/data/property/BypassRoomTemperature")]
        [InlineData("/data/property/BypassOutdoorTemperature")]
        [InlineData("/data/property/BypassOutdoorTemperature2")]
        [InlineData("/data/property/RestartChangeInterval")]
        [InlineData("/data/property/StartReset")]
        [InlineData("/data/property/FactoryReset")]
        [InlineData("/data/property/SupplyLevel")]
        [InlineData("/data/property/ExhaustLevel")]
        [InlineData("/data/property/FanLevelRegion02")]
        [InlineData("/data/property/FanLevelRegion24")]
        [InlineData("/data/property/FanLevelRegion46")]
        [InlineData("/data/property/FanLevelRegion68")]
        [InlineData("/data/property/FanLevelRegion80")]
        [InlineData("/data/property/OffsetExhaust")]
        [InlineData("/data/property/FanLevelConfiguration")]
        [InlineData("/data/property/SensorName1")]
        [InlineData("/data/property/SensorName2")]
        [InlineData("/data/property/SensorName3")]
        [InlineData("/data/property/SensorName4")]
        [InlineData("/data/property/SensorName5")]
        [InlineData("/data/property/SensorName6")]
        [InlineData("/data/property/SensorName7")]
        [InlineData("/data/property/SensorName8")]
        [InlineData("/data/property/CO2SensorName1")]
        [InlineData("/data/property/CO2SensorName2")]
        [InlineData("/data/property/CO2SensorName3")]
        [InlineData("/data/property/CO2SensorName4")]
        [InlineData("/data/property/CO2SensorName5")]
        [InlineData("/data/property/CO2SensorName6")]
        [InlineData("/data/property/CO2SensorName7")]
        [InlineData("/data/property/CO2SensorName8")]
        [InlineData("/data/property/VOCSensorName1")]
        [InlineData("/data/property/VOCSensorName2")]
        [InlineData("/data/property/VOCSensorName3")]
        [InlineData("/data/property/VOCSensorName4")]
        [InlineData("/data/property/VOCSensorName5")]
        [InlineData("/data/property/VOCSensorName6")]
        [InlineData("/data/property/VOCSensorName7")]
        [InlineData("/data/property/VOCSensorName8")]
        [InlineData("/data/property/SoftwareVersion")]
        [InlineData("/data/property/OperatingHours")]
        [InlineData("/data/property/OperationMinutesSupply")]
        [InlineData("/data/property/OperationMinutesExhaust")]
        [InlineData("/data/property/OperationMinutesPreheater")]
        [InlineData("/data/property/OperationMinutesAfterheater")]
        [InlineData("/data/property/PowerPreheater")]
        [InlineData("/data/property/PowerAfterheater")]
        [InlineData("/data/property/ResetFlag")]
        [InlineData("/data/property/ErrorCode")]
        [InlineData("/data/property/WarningCode")]
        [InlineData("/data/property/InfoCode")]
        [InlineData("/data/property/ModbusActivated")]
        [InlineData("/data/property/NumberOfErrors")]
        [InlineData("/data/property/NumberOfWarnings")]
        [InlineData("/data/property/NumberOfInfos")]
        [InlineData("/data/property/Errors")]
        [InlineData("/data/property/Warnings")]
        [InlineData("/data/property/Infos")]
        [InlineData("/data/property/StatusFlags")]
        [InlineData("/data/property/GlobalUpdate")]
        [InlineData("/data/property/LastError")]
        [InlineData("/data/property/ClearError")]
        [InlineData("/data/property/SensorConfig1")]
        [InlineData("/data/property/SensorConfig2")]
        [InlineData("/data/property/SensorConfig3")]
        [InlineData("/data/property/SensorConfig4")]
        [InlineData("/data/property/SensorConfig5")]
        [InlineData("/data/property/SensorConfig6")]
        [InlineData("/data/property/SensorConfig7")]
        [InlineData("/data/property/SensorConfig8")]
        [InlineData("/data/property/V02103")]
        [InlineData("/data/property/DataExchange")]
        [InlineData("/data/property/V02115")]
        [InlineData("/data/property/ActivateAutoMode")]
        [InlineData("/data/property/V02117")]
        [InlineData("/data/property/V02118")]
        [InlineData("/data/property/V02119")]
        [InlineData("/data/property/V02120")]
        [InlineData("/data/property/V02121")]
        [InlineData("/data/property/V02122")]
        [InlineData("/data/property/V02123")]
        [InlineData("/data/property/V02128")]
        [InlineData("/data/property/V02129")]
        [InlineData("/data/property/V02130")]
        [InlineData("/data/property/V02131")]
        [InlineData("/data/property/CountryCode")]
        [InlineData("/data/property/V02136")]
        [InlineData("/data/property/V02137")]
        [InlineData("/data/property/V02142")]
        [InlineData("/data/property/V02143")]
        [InlineData("/data/property/V02144")]
        [InlineData("/data/property/V02145")]
        [InlineData("/data/property/V02146")]
        [InlineData("/data/property/V02147")]
        [InlineData("/data/property/V02148")]
        [InlineData("/data/property/V02149")]
        [InlineData("/data/property/V02150")]
        [InlineData("/data/property/V02151")]
        [InlineData("/data/property/V02152")]
        public async Task TestDataProperty(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            Assert.True(json.Length > 0);
            dynamic data = JsonSerializer.Deserialize(json,
                typeof(HeliosData).GetProperty(url.Split("/")[3]).PropertyType, _options);
            Assert.NotNull(data);
        }

        [Theory]
        [InlineData("/booster/property/PartyOperationDuration")]
        [InlineData("/booster/property/PartyVentilationLevel")]
        [InlineData("/booster/property/PartyOperationRemaining")]
        [InlineData("/booster/property/PartyOperationActivate")]
        [InlineData("/booster/property/StandbyOperationDuration")]
        [InlineData("/booster/property/StandbyVentilationLevel")]
        [InlineData("/booster/property/StandbyOperationRemaining")]
        [InlineData("/booster/property/StandbyOperationActivate")]
        [InlineData("/booster/property/StatusFlags")]
        public async Task TestBoosterProperty(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            Assert.True(json.Length > 0);
            dynamic data = JsonSerializer.Deserialize(json,
                typeof(BoosterData).GetProperty(url.Split("/")[3]).PropertyType, _options);
            Assert.NotNull(data);
        }

        [Theory]
        [InlineData("/device/property/KwlBeEnabled")]
        [InlineData("/device/property/KwlBecEnabled")]
        [InlineData("/device/property/DeviceConfiguration")]
        [InlineData("/device/property/PreheaterStatus")]
        [InlineData("/device/property/HeatExchangerType")]
        [InlineData("/device/property/WeeklyProfile")]
        [InlineData("/device/property/PreheaterType")]
        [InlineData("/device/property/KwlFunctionType")]
        [InlineData("/device/property/HeaterAfterRunTime")]
        [InlineData("/device/property/ExternalContact")]
        [InlineData("/device/property/FaultTypeOutput")]
        [InlineData("/device/property/FilterChange")]
        [InlineData("/device/property/FilterChangeInterval")]
        [InlineData("/device/property/FilterChangeRemaining")]
        [InlineData("/device/property/BypassRoomTemperature")]
        [InlineData("/device/property/BypassOutdoorTemperature")]
        [InlineData("/device/property/BypassOutdoorTemperature2")]
        [InlineData("/device/property/StartReset")]
        [InlineData("/device/property/FactoryReset")]
        [InlineData("/device/property/ModbusActivated")]
        [InlineData("/device/property/StatusFlags")]
        [InlineData("/device/property/V02115")]
        [InlineData("/device/property/V02120")]
        [InlineData("/device/property/V02121")]
        [InlineData("/device/property/V02128")]
        [InlineData("/device/property/V02129")]
        public async Task TestDeviceProperty(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            Assert.True(json.Length > 0);
            dynamic data = JsonSerializer.Deserialize(json,
                typeof(DeviceData).GetProperty(url.Split("/")[3]).PropertyType, _options);
            Assert.NotNull(data);
        }

        [Theory]
        [InlineData("/display/property/PreheaterStatus")]
        [InlineData("/display/property/HumidityControlStatus")]
        [InlineData("/display/property/CO2ControlStatus")]
        [InlineData("/display/property/VOCControlStatus")]
        [InlineData("/display/property/PartyVentilationLevel")]
        [InlineData("/display/property/PartyOperationActivate")]
        [InlineData("/display/property/StandbyVentilationLevel")]
        [InlineData("/display/property/StandbyOperationActivate")]
        [InlineData("/display/property/OperationMode")]
        [InlineData("/display/property/VentilationLevel")]
        [InlineData("/display/property/VentilationPercentage")]
        [InlineData("/display/property/TemperatureOutdoor")]
        [InlineData("/display/property/TemperatureSupply")]
        [InlineData("/display/property/TemperatureExhaust")]
        [InlineData("/display/property/TemperatureExtract")]
        [InlineData("/display/property/TemperaturePreHeater")]
        [InlineData("/display/property/TemperaturePostHeater")]
        [InlineData("/display/property/ExternalHumiditySensor1")]
        [InlineData("/display/property/ExternalHumiditySensor2")]
        [InlineData("/display/property/ExternalHumiditySensor3")]
        [InlineData("/display/property/ExternalHumiditySensor4")]
        [InlineData("/display/property/ExternalHumiditySensor5")]
        [InlineData("/display/property/ExternalHumiditySensor6")]
        [InlineData("/display/property/ExternalHumiditySensor7")]
        [InlineData("/display/property/ExternalHumiditySensor8")]
        [InlineData("/display/property/ExternalHumidityTemperature1")]
        [InlineData("/display/property/ExternalHumidityTemperature2")]
        [InlineData("/display/property/ExternalHumidityTemperature3")]
        [InlineData("/display/property/ExternalHumidityTemperature4")]
        [InlineData("/display/property/ExternalHumidityTemperature5")]
        [InlineData("/display/property/ExternalHumidityTemperature6")]
        [InlineData("/display/property/ExternalHumidityTemperature7")]
        [InlineData("/display/property/ExternalHumidityTemperature8")]
        [InlineData("/display/property/ExternalCO2Sensor1")]
        [InlineData("/display/property/ExternalCO2Sensor2")]
        [InlineData("/display/property/ExternalCO2Sensor3")]
        [InlineData("/display/property/ExternalCO2Sensor4")]
        [InlineData("/display/property/ExternalCO2Sensor5")]
        [InlineData("/display/property/ExternalCO2Sensor6")]
        [InlineData("/display/property/ExternalCO2Sensor7")]
        [InlineData("/display/property/ExternalCO2Sensor8")]
        [InlineData("/display/property/ExternalVOCSensor1")]
        [InlineData("/display/property/ExternalVOCSensor2")]
        [InlineData("/display/property/ExternalVOCSensor3")]
        [InlineData("/display/property/ExternalVOCSensor4")]
        [InlineData("/display/property/ExternalVOCSensor5")]
        [InlineData("/display/property/ExternalVOCSensor6")]
        [InlineData("/display/property/ExternalVOCSensor7")]
        [InlineData("/display/property/ExternalVOCSensor8")]
        [InlineData("/display/property/TemperatureChannel")]
        [InlineData("/display/property/WeeklyProfile")]
        [InlineData("/display/property/SupplyFanSpeed")]
        [InlineData("/display/property/ExhaustFanSpeed")]
        [InlineData("/display/property/VacationOperation")]
        [InlineData("/display/property/VacationVentilationLevel")]
        [InlineData("/display/property/ExternalContact")]
        [InlineData("/display/property/SupplyLevel")]
        [InlineData("/display/property/ExhaustLevel")]
        [InlineData("/display/property/SensorName1")]
        [InlineData("/display/property/SensorName2")]
        [InlineData("/display/property/SensorName3")]
        [InlineData("/display/property/SensorName4")]
        [InlineData("/display/property/SensorName5")]
        [InlineData("/display/property/SensorName6")]
        [InlineData("/display/property/SensorName7")]
        [InlineData("/display/property/SensorName8")]
        [InlineData("/display/property/CO2SensorName1")]
        [InlineData("/display/property/CO2SensorName2")]
        [InlineData("/display/property/CO2SensorName3")]
        [InlineData("/display/property/CO2SensorName4")]
        [InlineData("/display/property/CO2SensorName5")]
        [InlineData("/display/property/CO2SensorName6")]
        [InlineData("/display/property/CO2SensorName7")]
        [InlineData("/display/property/CO2SensorName8")]
        [InlineData("/display/property/VOCSensorName1")]
        [InlineData("/display/property/VOCSensorName2")]
        [InlineData("/display/property/VOCSensorName3")]
        [InlineData("/display/property/VOCSensorName4")]
        [InlineData("/display/property/VOCSensorName5")]
        [InlineData("/display/property/VOCSensorName6")]
        [InlineData("/display/property/VOCSensorName7")]
        [InlineData("/display/property/VOCSensorName8")]
        [InlineData("/display/property/NumberOfErrors")]
        [InlineData("/display/property/NumberOfWarnings")]
        [InlineData("/display/property/NumberOfInfos")]
        [InlineData("/display/property/StatusFlags")]
        [InlineData("/display/property/SensorConfig1")]
        [InlineData("/display/property/SensorConfig2")]
        [InlineData("/display/property/SensorConfig3")]
        [InlineData("/display/property/SensorConfig4")]
        [InlineData("/display/property/SensorConfig5")]
        [InlineData("/display/property/SensorConfig6")]
        [InlineData("/display/property/SensorConfig7")]
        [InlineData("/display/property/SensorConfig8")]
        public async Task TestDisplayProperty(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            Assert.True(json.Length > 0);
            dynamic data = JsonSerializer.Deserialize(json,
                typeof(DisplayData).GetProperty(url.Split("/")[3]).PropertyType, _options);
            Assert.NotNull(data);
        }

        [Theory]
        [InlineData("/error/property/NumberOfErrors")]
        [InlineData("/error/property/NumberOfWarnings")]
        [InlineData("/error/property/NumberOfInfos")]
        [InlineData("/error/property/Errors")]
        [InlineData("/error/property/Warnings")]
        [InlineData("/error/property/Infos")]
        [InlineData("/error/property/StatusFlags")]
        [InlineData("/error/property/DataExchange")]
        public async Task TestErrorProperty(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            Assert.True(json.Length > 0);
            dynamic data = JsonSerializer.Deserialize(json,
                typeof(ErrorData).GetProperty(url.Split("/")[3]).PropertyType, _options);
            Assert.NotNull(data);
        }

        [Theory]
        [InlineData("/fan/property/ExhaustVentilatorVoltageLevel1")]
        [InlineData("/fan/property/SupplyVentilatorVoltageLevel1")]
        [InlineData("/fan/property/ExhaustVentilatorVoltageLevel2")]
        [InlineData("/fan/property/SupplyVentilatorVoltageLevel2")]
        [InlineData("/fan/property/ExhaustVentilatorVoltageLevel3")]
        [InlineData("/fan/property/SupplyVentilatorVoltageLevel3")]
        [InlineData("/fan/property/ExhaustVentilatorVoltageLevel4")]
        [InlineData("/fan/property/SupplyVentilatorVoltageLevel4")]
        [InlineData("/fan/property/MinimumVentilationLevel")]
        [InlineData("/fan/property/SupplyLevel")]
        [InlineData("/fan/property/ExhaustLevel")]
        [InlineData("/fan/property/FanLevelRegion02")]
        [InlineData("/fan/property/FanLevelRegion24")]
        [InlineData("/fan/property/FanLevelRegion46")]
        [InlineData("/fan/property/FanLevelRegion68")]
        [InlineData("/fan/property/FanLevelRegion80")]
        [InlineData("/fan/property/OffsetExhaust")]
        [InlineData("/fan/property/FanLevelConfiguration")]
        [InlineData("/fan/property/StatusFlags")]
        public async Task TestFanProperty(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            Assert.True(json.Length > 0);
            dynamic data = JsonSerializer.Deserialize(json,
                typeof(FanData).GetProperty(url.Split("/")[3]).PropertyType, _options);
            Assert.NotNull(data);
        }

        [Theory]
        [InlineData("/heater/property/ItemDescription")]
        [InlineData("/heater/property/OrderNumber")]
        [InlineData("/heater/property/MacAddress")]
        [InlineData("/heater/property/WeeklyProfile")]
        [InlineData("/heater/property/StatusFlags")]
        public async Task TestHeaterProperty(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            Assert.True(json.Length > 0);
            dynamic data = JsonSerializer.Deserialize(json,
                typeof(HeaterData).GetProperty(url.Split("/")[3]).PropertyType, _options);
            Assert.NotNull(data);
        }

        [Theory]
        [InlineData("/info/property/ItemDescription")]
        [InlineData("/info/property/OrderNumber")]
        [InlineData("/info/property/MacAddress")]
        [InlineData("/info/property/PartyOperationRemaining")]
        [InlineData("/info/property/PartyOperationActivate")]
        [InlineData("/info/property/StandbyOperationRemaining")]
        [InlineData("/info/property/StandbyOperationActivate")]
        [InlineData("/info/property/OperationMode")]
        [InlineData("/info/property/VentilationLevel")]
        [InlineData("/info/property/VentilationPercentage")]
        [InlineData("/info/property/VacationOperation")]
        [InlineData("/info/property/VacationEndDate")]
        [InlineData("/info/property/ExternalContact")]
        [InlineData("/info/property/StatusFlags")]
        public async Task TestInfoProperty(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            Assert.True(json.Length > 0);
            dynamic data = JsonSerializer.Deserialize(json,
                typeof(InfoData).GetProperty(url.Split("/")[3]).PropertyType, _options);
            Assert.NotNull(data);
        }

        [Theory]
        [InlineData("/network/property/UseDHCP")]
        [InlineData("/network/property/IPAddress")]
        [InlineData("/network/property/SubnetMask")]
        [InlineData("/network/property/Gateway")]
        [InlineData("/network/property/StandardDNS")]
        [InlineData("/network/property/FallbackDNS")]
        [InlineData("/network/property/HostName")]
        [InlineData("/network/property/StatusFlags")]
        public async Task TestNetworkProperty(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            Assert.True(json.Length > 0);
            dynamic data = JsonSerializer.Deserialize(json,
                typeof(NetworkData).GetProperty(url.Split("/")[3]).PropertyType, _options);
            Assert.NotNull(data);
        }

        [Theory]
        [InlineData("/operation/property/ItemDescription")]
        [InlineData("/operation/property/OrderNumber")]
        [InlineData("/operation/property/MacAddress")]
        [InlineData("/operation/property/Language")]
        [InlineData("/operation/property/Date")]
        [InlineData("/operation/property/Time")]
        [InlineData("/operation/property/DayLightSaving")]
        [InlineData("/operation/property/AutoUpdateEnabled")]
        [InlineData("/operation/property/PortalAccessEnabled")]
        [InlineData("/operation/property/ExhaustVentilatorVoltageLevel1")]
        [InlineData("/operation/property/SupplyVentilatorVoltageLevel1")]
        [InlineData("/operation/property/ExhaustVentilatorVoltageLevel2")]
        [InlineData("/operation/property/SupplyVentilatorVoltageLevel2")]
        [InlineData("/operation/property/ExhaustVentilatorVoltageLevel3")]
        [InlineData("/operation/property/SupplyVentilatorVoltageLevel3")]
        [InlineData("/operation/property/ExhaustVentilatorVoltageLevel4")]
        [InlineData("/operation/property/SupplyVentilatorVoltageLevel4")]
        [InlineData("/operation/property/MinimumVentilationLevel")]
        [InlineData("/operation/property/KwlBeEnabled")]
        [InlineData("/operation/property/KwlBecEnabled")]
        [InlineData("/operation/property/DeviceConfiguration")]
        [InlineData("/operation/property/PreheaterStatus")]
        [InlineData("/operation/property/KwlFTFConfig0")]
        [InlineData("/operation/property/KwlFTFConfig1")]
        [InlineData("/operation/property/KwlFTFConfig2")]
        [InlineData("/operation/property/KwlFTFConfig3")]
        [InlineData("/operation/property/KwlFTFConfig4")]
        [InlineData("/operation/property/KwlFTFConfig5")]
        [InlineData("/operation/property/KwlFTFConfig6")]
        [InlineData("/operation/property/KwlFTFConfig7")]
        [InlineData("/operation/property/HumidityControlStatus")]
        [InlineData("/operation/property/HumidityControlTarget")]
        [InlineData("/operation/property/HumidityControlStep")]
        [InlineData("/operation/property/HumidityControlStop")]
        [InlineData("/operation/property/CO2ControlStatus")]
        [InlineData("/operation/property/CO2ControlTarget")]
        [InlineData("/operation/property/CO2ControlStep")]
        [InlineData("/operation/property/VOCControlStatus")]
        [InlineData("/operation/property/VOCControlTarget")]
        [InlineData("/operation/property/VOCControlStep")]
        [InlineData("/operation/property/ThermalComfortTemperature")]
        [InlineData("/operation/property/TimeZoneOffset")]
        [InlineData("/operation/property/DateFormat")]
        [InlineData("/operation/property/HeatExchangerType")]
        [InlineData("/operation/property/KwlFunctionType")]
        [InlineData("/operation/property/SensorName1")]
        [InlineData("/operation/property/SensorName2")]
        [InlineData("/operation/property/SensorName3")]
        [InlineData("/operation/property/SensorName4")]
        [InlineData("/operation/property/SensorName5")]
        [InlineData("/operation/property/SensorName6")]
        [InlineData("/operation/property/SensorName7")]
        [InlineData("/operation/property/SensorName8")]
        [InlineData("/operation/property/StatusFlags")]
        [InlineData("/operation/property/SensorConfig1")]
        [InlineData("/operation/property/SensorConfig2")]
        [InlineData("/operation/property/SensorConfig3")]
        [InlineData("/operation/property/SensorConfig4")]
        [InlineData("/operation/property/SensorConfig5")]
        [InlineData("/operation/property/SensorConfig6")]
        [InlineData("/operation/property/SensorConfig7")]
        [InlineData("/operation/property/SensorConfig8")]
        public async Task TestOperationProperty(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            Assert.True(json.Length > 0);
            dynamic data = JsonSerializer.Deserialize(json,
                typeof(OperationData).GetProperty(url.Split("/")[3]).PropertyType, _options);
            Assert.NotNull(data);
        }

        [Theory]
        [InlineData("/sensor/property/KwlFTFConfig0")]
        [InlineData("/sensor/property/KwlFTFConfig1")]
        [InlineData("/sensor/property/KwlFTFConfig2")]
        [InlineData("/sensor/property/KwlFTFConfig3")]
        [InlineData("/sensor/property/KwlFTFConfig4")]
        [InlineData("/sensor/property/KwlFTFConfig5")]
        [InlineData("/sensor/property/KwlFTFConfig6")]
        [InlineData("/sensor/property/KwlFTFConfig7")]
        [InlineData("/sensor/property/HumidityControlStatus")]
        [InlineData("/sensor/property/HumidityControlTarget")]
        [InlineData("/sensor/property/HumidityControlStep")]
        [InlineData("/sensor/property/HumidityControlStop")]
        [InlineData("/sensor/property/CO2ControlStatus")]
        [InlineData("/sensor/property/CO2ControlTarget")]
        [InlineData("/sensor/property/CO2ControlStep")]
        [InlineData("/sensor/property/VOCControlStatus")]
        [InlineData("/sensor/property/VOCControlTarget")]
        [InlineData("/sensor/property/VOCControlStep")]
        [InlineData("/sensor/property/SensorName1")]
        [InlineData("/sensor/property/SensorName2")]
        [InlineData("/sensor/property/SensorName3")]
        [InlineData("/sensor/property/SensorName4")]
        [InlineData("/sensor/property/SensorName5")]
        [InlineData("/sensor/property/SensorName6")]
        [InlineData("/sensor/property/SensorName7")]
        [InlineData("/sensor/property/SensorName8")]
        [InlineData("/sensor/property/CO2SensorName1")]
        [InlineData("/sensor/property/CO2SensorName2")]
        [InlineData("/sensor/property/CO2SensorName3")]
        [InlineData("/sensor/property/CO2SensorName4")]
        [InlineData("/sensor/property/CO2SensorName5")]
        [InlineData("/sensor/property/CO2SensorName6")]
        [InlineData("/sensor/property/CO2SensorName7")]
        [InlineData("/sensor/property/CO2SensorName8")]
        [InlineData("/sensor/property/VOCSensorName1")]
        [InlineData("/sensor/property/VOCSensorName2")]
        [InlineData("/sensor/property/VOCSensorName3")]
        [InlineData("/sensor/property/VOCSensorName4")]
        [InlineData("/sensor/property/VOCSensorName5")]
        [InlineData("/sensor/property/VOCSensorName6")]
        [InlineData("/sensor/property/VOCSensorName7")]
        [InlineData("/sensor/property/VOCSensorName8")]
        [InlineData("/sensor/property/StatusFlags")]
        [InlineData("/sensor/property/V02137")]
        [InlineData("/sensor/property/V02142")]
        [InlineData("/sensor/property/V02143")]
        [InlineData("/sensor/property/V02144")]
        [InlineData("/sensor/property/V02145")]
        [InlineData("/sensor/property/V02146")]
        [InlineData("/sensor/property/V02147")]
        [InlineData("/sensor/property/V02148")]
        [InlineData("/sensor/property/V02149")]
        [InlineData("/sensor/property/V02150")]
        [InlineData("/sensor/property/V02151")]
        [InlineData("/sensor/property/V02152")]
        public async Task TestSensorProperty(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            Assert.True(json.Length > 0);
            dynamic data = JsonSerializer.Deserialize(json,
                typeof(SensorData).GetProperty(url.Split("/")[3]).PropertyType, _options);
            Assert.NotNull(data);
        }

        [Theory]
        [InlineData("/system/property/Language")]
        [InlineData("/system/property/Date")]
        [InlineData("/system/property/Time")]
        [InlineData("/system/property/DayLightSaving")]
        [InlineData("/system/property/AutoUpdateEnabled")]
        [InlineData("/system/property/PortalAccessEnabled")]
        [InlineData("/system/property/TimeZoneOffset")]
        [InlineData("/system/property/DateFormat")]
        [InlineData("/system/property/SupplyFanSpeed")]
        [InlineData("/system/property/ExhaustFanSpeed")]
        [InlineData("/system/property/SoftwareVersion")]
        [InlineData("/system/property/OperationMinutesSupply")]
        [InlineData("/system/property/OperationMinutesExhaust")]
        [InlineData("/system/property/OperationMinutesPreheater")]
        [InlineData("/system/property/OperationMinutesAfterheater")]
        [InlineData("/system/property/PowerPreheater")]
        [InlineData("/system/property/PowerAfterheater")]
        [InlineData("/system/property/StatusFlags")]
        [InlineData("/system/property/ActivateAutoMode")]
        [InlineData("/system/property/CountryCode")]
        [InlineData("/system/property/V02103")]
        public async Task TestSystemProperty(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            Assert.True(json.Length > 0);
            dynamic data = JsonSerializer.Deserialize(json,
                typeof(SystemData).GetProperty(url.Split("/")[3]).PropertyType, _options);
            Assert.NotNull(data);
        }

        [Theory]
        [InlineData("/technical/property/ItemDescription")]
        [InlineData("/technical/property/OrderNumber")]
        [InlineData("/technical/property/MacAddress")]
        [InlineData("/technical/property/SerialNumber")]
        [InlineData("/technical/property/ProductionCode")]
        [InlineData("/technical/property/SecurityCode")]
        [InlineData("/technical/property/StatusFlags")]
        public async Task TestTechnicalProperty(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            Assert.True(json.Length > 0);
            dynamic data = JsonSerializer.Deserialize(json,
                typeof(TechnicalData).GetProperty(url.Split("/")[3]).PropertyType, _options);
            Assert.NotNull(data);
        }

        [Theory]
        [InlineData("/vacation/property/VacationOperation")]
        [InlineData("/vacation/property/VacationVentilationLevel")]
        [InlineData("/vacation/property/VacationStartDate")]
        [InlineData("/vacation/property/VacationEndDate")]
        [InlineData("/vacation/property/VacationInterval")]
        [InlineData("/vacation/property/VacationDuration")]
        [InlineData("/vacation/property/SupplyLevel")]
        [InlineData("/vacation/property/ExhaustLevel")]
        [InlineData("/vacation/property/StatusFlags")]
        public async Task TestVacationProperty(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            Assert.True(json.Length > 0);
            dynamic data = JsonSerializer.Deserialize(json,
                typeof(VacationData).GetProperty(url.Split("/")[3]).PropertyType, _options);
            Assert.NotNull(data);
        }

        [Theory]
        [InlineData("/data/label/v00000")]
        [InlineData("/data/label/v00001")]
        [InlineData("/data/label/v00002")]
        [InlineData("/data/label/v00003")]
        [InlineData("/data/label/v00004")]
        [InlineData("/data/label/v00005")]
        [InlineData("/data/label/v00006")]
        [InlineData("/data/label/v00007")]
        [InlineData("/data/label/v00008")]
        [InlineData("/data/label/v00012")]
        [InlineData("/data/label/v00013")]
        [InlineData("/data/label/v00014")]
        [InlineData("/data/label/v00015")]
        [InlineData("/data/label/v00016")]
        [InlineData("/data/label/v00017")]
        [InlineData("/data/label/v00018")]
        [InlineData("/data/label/v00019")]
        [InlineData("/data/label/v00020")]
        [InlineData("/data/label/v00021")]
        [InlineData("/data/label/v00022")]
        [InlineData("/data/label/v00023")]
        [InlineData("/data/label/v00024")]
        [InlineData("/data/label/v00025")]
        [InlineData("/data/label/v00026")]
        [InlineData("/data/label/v00027")]
        [InlineData("/data/label/v00028")]
        [InlineData("/data/label/v00029")]
        [InlineData("/data/label/v00030")]
        [InlineData("/data/label/v00031")]
        [InlineData("/data/label/v00032")]
        [InlineData("/data/label/v00033")]
        [InlineData("/data/label/v00034")]
        [InlineData("/data/label/v00035")]
        [InlineData("/data/label/v00036")]
        [InlineData("/data/label/v00037")]
        [InlineData("/data/label/v00038")]
        [InlineData("/data/label/v00039")]
        [InlineData("/data/label/v00040")]
        [InlineData("/data/label/v00041")]
        [InlineData("/data/label/v00042")]
        [InlineData("/data/label/v00043")]
        [InlineData("/data/label/v00051")]
        [InlineData("/data/label/v00052")]
        [InlineData("/data/label/v00053")]
        [InlineData("/data/label/v00091")]
        [InlineData("/data/label/v00092")]
        [InlineData("/data/label/v00093")]
        [InlineData("/data/label/v00094")]
        [InlineData("/data/label/v00096")]
        [InlineData("/data/label/v00097")]
        [InlineData("/data/label/v00098")]
        [InlineData("/data/label/v00099")]
        [InlineData("/data/label/v00101")]
        [InlineData("/data/label/v00102")]
        [InlineData("/data/label/v00103")]
        [InlineData("/data/label/v00104")]
        [InlineData("/data/label/v00105")]
        [InlineData("/data/label/v00106")]
        [InlineData("/data/label/v00107")]
        [InlineData("/data/label/v00108")]
        [InlineData("/data/label/v00109")]
        [InlineData("/data/label/v00110")]
        [InlineData("/data/label/v00111")]
        [InlineData("/data/label/v00112")]
        [InlineData("/data/label/v00113")]
        [InlineData("/data/label/v00114")]
        [InlineData("/data/label/v00115")]
        [InlineData("/data/label/v00116")]
        [InlineData("/data/label/v00117")]
        [InlineData("/data/label/v00118")]
        [InlineData("/data/label/v00119")]
        [InlineData("/data/label/v00120")]
        [InlineData("/data/label/v00121")]
        [InlineData("/data/label/v00122")]
        [InlineData("/data/label/v00123")]
        [InlineData("/data/label/v00124")]
        [InlineData("/data/label/v00125")]
        [InlineData("/data/label/v00126")]
        [InlineData("/data/label/v00127")]
        [InlineData("/data/label/v00128")]
        [InlineData("/data/label/v00129")]
        [InlineData("/data/label/v00130")]
        [InlineData("/data/label/v00131")]
        [InlineData("/data/label/v00132")]
        [InlineData("/data/label/v00133")]
        [InlineData("/data/label/v00134")]
        [InlineData("/data/label/v00135")]
        [InlineData("/data/label/v00136")]
        [InlineData("/data/label/v00137")]
        [InlineData("/data/label/v00138")]
        [InlineData("/data/label/v00139")]
        [InlineData("/data/label/v00140")]
        [InlineData("/data/label/v00141")]
        [InlineData("/data/label/v00142")]
        [InlineData("/data/label/v00143")]
        [InlineData("/data/label/v00144")]
        [InlineData("/data/label/v00146")]
        [InlineData("/data/label/v00201")]
        [InlineData("/data/label/v00220")]
        [InlineData("/data/label/v00221")]
        [InlineData("/data/label/v00222")]
        [InlineData("/data/label/v00223")]
        [InlineData("/data/label/v00224")]
        [InlineData("/data/label/v00225")]
        [InlineData("/data/label/v00226")]
        [InlineData("/data/label/v00227")]
        [InlineData("/data/label/v00228")]
        [InlineData("/data/label/v00229")]
        [InlineData("/data/label/v00303")]
        [InlineData("/data/label/v00304")]
        [InlineData("/data/label/v00343")]
        [InlineData("/data/label/v00348")]
        [InlineData("/data/label/v00349")]
        [InlineData("/data/label/v00402")]
        [InlineData("/data/label/v00403")]
        [InlineData("/data/label/v00601")]
        [InlineData("/data/label/v00602")]
        [InlineData("/data/label/v00603")]
        [InlineData("/data/label/v00604")]
        [InlineData("/data/label/v00605")]
        [InlineData("/data/label/v00606")]
        [InlineData("/data/label/v00901")]
        [InlineData("/data/label/v00920")]
        [InlineData("/data/label/v00921")]
        [InlineData("/data/label/v00922")]
        [InlineData("/data/label/v00923")]
        [InlineData("/data/label/v00924")]
        [InlineData("/data/label/v00925")]
        [InlineData("/data/label/v00926")]
        [InlineData("/data/label/v00927")]
        [InlineData("/data/label/v00928")]
        [InlineData("/data/label/v00929")]
        [InlineData("/data/label/v01001")]
        [InlineData("/data/label/v01002")]
        [InlineData("/data/label/v01003")]
        [InlineData("/data/label/v01004")]
        [InlineData("/data/label/v01005")]
        [InlineData("/data/label/v01006")]
        [InlineData("/data/label/v01007")]
        [InlineData("/data/label/v01010")]
        [InlineData("/data/label/v01017")]
        [InlineData("/data/label/v01019")]
        [InlineData("/data/label/v01020")]
        [InlineData("/data/label/v01021")]
        [InlineData("/data/label/v01031")]
        [InlineData("/data/label/v01032")]
        [InlineData("/data/label/v01033")]
        [InlineData("/data/label/v01035")]
        [InlineData("/data/label/v01036")]
        [InlineData("/data/label/v01037")]
        [InlineData("/data/label/v01038")]
        [InlineData("/data/label/v01041")]
        [InlineData("/data/label/v01042")]
        [InlineData("/data/label/v01050")]
        [InlineData("/data/label/v01051")]
        [InlineData("/data/label/v01061")]
        [InlineData("/data/label/v01062")]
        [InlineData("/data/label/v01063")]
        [InlineData("/data/label/v01064")]
        [InlineData("/data/label/v01065")]
        [InlineData("/data/label/v01066")]
        [InlineData("/data/label/v01068")]
        [InlineData("/data/label/v01071")]
        [InlineData("/data/label/v01072")]
        [InlineData("/data/label/v01073")]
        [InlineData("/data/label/v01074")]
        [InlineData("/data/label/v01075")]
        [InlineData("/data/label/v01076")]
        [InlineData("/data/label/v01077")]
        [InlineData("/data/label/v01078")]
        [InlineData("/data/label/v01081")]
        [InlineData("/data/label/v01082")]
        [InlineData("/data/label/v01083")]
        [InlineData("/data/label/v01084")]
        [InlineData("/data/label/v01085")]
        [InlineData("/data/label/v01086")]
        [InlineData("/data/label/v01087")]
        [InlineData("/data/label/v01088")]
        [InlineData("/data/label/v01091")]
        [InlineData("/data/label/v01092")]
        [InlineData("/data/label/v01093")]
        [InlineData("/data/label/v01094")]
        [InlineData("/data/label/v01095")]
        [InlineData("/data/label/v01096")]
        [InlineData("/data/label/v01097")]
        [InlineData("/data/label/v01098")]
        [InlineData("/data/label/v01101")]
        [InlineData("/data/label/v01102")]
        [InlineData("/data/label/v01103")]
        [InlineData("/data/label/v01104")]
        [InlineData("/data/label/v01105")]
        [InlineData("/data/label/v01106")]
        [InlineData("/data/label/v01108")]
        [InlineData("/data/label/v01109")]
        [InlineData("/data/label/v01120")]
        [InlineData("/data/label/v01123")]
        [InlineData("/data/label/v01124")]
        [InlineData("/data/label/v01125")]
        [InlineData("/data/label/v01200")]
        [InlineData("/data/label/v01300")]
        [InlineData("/data/label/v01301")]
        [InlineData("/data/label/v01302")]
        [InlineData("/data/label/v01303")]
        [InlineData("/data/label/v01304")]
        [InlineData("/data/label/v01305")]
        [InlineData("/data/label/v01306")]
        [InlineData("/data/label/v02013")]
        [InlineData("/data/label/v02014")]
        [InlineData("/data/label/v02015")]
        [InlineData("/data/label/v02020")]
        [InlineData("/data/label/v02021")]
        [InlineData("/data/label/v02022")]
        [InlineData("/data/label/v02023")]
        [InlineData("/data/label/v02024")]
        [InlineData("/data/label/v02025")]
        [InlineData("/data/label/v02026")]
        [InlineData("/data/label/v02027")]
        [InlineData("/data/label/v02103")]
        [InlineData("/data/label/v02104")]
        [InlineData("/data/label/v02115")]
        [InlineData("/data/label/v02116")]
        [InlineData("/data/label/v02117")]
        [InlineData("/data/label/v02118")]
        [InlineData("/data/label/v02119")]
        [InlineData("/data/label/v02120")]
        [InlineData("/data/label/v02121")]
        [InlineData("/data/label/v02122")]
        [InlineData("/data/label/v02123")]
        [InlineData("/data/label/v02128")]
        [InlineData("/data/label/v02129")]
        [InlineData("/data/label/v02130")]
        [InlineData("/data/label/v02131")]
        [InlineData("/data/label/v02134")]
        [InlineData("/data/label/v02136")]
        [InlineData("/data/label/v02137")]
        [InlineData("/data/label/v02142")]
        [InlineData("/data/label/v02143")]
        [InlineData("/data/label/v02144")]
        [InlineData("/data/label/v02145")]
        [InlineData("/data/label/v02146")]
        [InlineData("/data/label/v02147")]
        [InlineData("/data/label/v02148")]
        [InlineData("/data/label/v02149")]
        [InlineData("/data/label/v02150")]
        [InlineData("/data/label/v02151")]
        [InlineData("/data/label/v02152")]
        public async Task TestLabelProperty(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            Assert.True(json.Length > 0);
            dynamic data = JsonSerializer.Deserialize(json,
                typeof(HeliosData).GetProperty(HeliosData.GetHeliosProperty(url.Split("/")[3])).PropertyType, _options);
            Assert.NotNull(data);
        }

        [Fact]
        public async Task TestGateway()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/gateway");

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            var info = JsonSerializer.Deserialize<HeliosInfo>(json);
            Assert.True(info.Settings.Address.Length > 0);
            Assert.True(info.Settings.Timeout > 0);
            Assert.True(info.IsStartupOk);
            Assert.Equal(DataStatus.Good.Code, info.Status.Code);
            Assert.Equal(DataStatus.Good.Name, info.Status.Name);
            Assert.Equal(DataStatus.Good.IsGood, info.Status.IsGood);
            Assert.Equal(DataStatus.Good.IsNotGood, info.Status.IsNotGood);
            Assert.Equal(DataStatus.Good.IsUncertain, info.Status.IsUncertain);
            Assert.Equal(DataStatus.Good.IsNotUncertain, info.Status.IsNotUncertain);
            Assert.Equal(DataStatus.Good.IsBad, info.Status.IsBad);
            Assert.Equal(DataStatus.Good.IsNotBad, info.Status.IsNotBad);
            Assert.Equal(DataStatus.Good.Explanation, info.Status.Explanation);
        }

        [Fact]
        public async Task TestData()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/data");

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<HeliosData>(json, _options);
            Assert.NotNull(data);
        }

        [Fact]
        public async Task TestBooster()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/booster");

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<BoosterData>(json, _options);
            Assert.NotNull(data);
        }

        [Fact]
        public async Task TestDevice()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/device");

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<DeviceData>(json, _options);
            Assert.NotNull(data);
        }

        [Fact]
        public async Task TestDisplay()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/display");

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<DisplayData>(json, _options);
            Assert.NotNull(data);
        }

        [Fact]
        public async Task TestError()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/error");

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<ErrorData>(json, _options);
            Assert.NotNull(data);
        }

        [Fact]
        public async Task TestFan()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/fan");

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<FanData>(json, _options);
            Assert.NotNull(data);
        }

        [Fact]
        public async Task TestInfo()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/info");

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<InfoData>(json, _options);
            Assert.NotNull(data);
        }

        [Fact]
        public async Task TestNetwork()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/heater");

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<NetworkData>(json, _options);
            Assert.NotNull(data);
        }

        [Fact]
        public async Task TestOperation()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/operation");

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<OperationData>(json, _options);
            Assert.NotNull(data);
        }

        [Fact]
        public async Task TestSensor()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/sensor");

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<SensorData>(json, _options);
            Assert.NotNull(data);
        }

        [Fact]
        public async Task TestSystem()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/system");

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<SystemData>(json, _options);
            Assert.NotNull(data);
        }

        [Fact]
        public async Task TestTechnical()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/technical");

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<TechnicalData>(json, _options);
            Assert.NotNull(data);
        }

        [Fact]
        public async Task TestVacation()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/vacation");

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<VacationData>(json, _options);
            Assert.NotNull(data);
        }

        [Fact]
        public async Task TestHeater()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/heater");

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<HeaterData>(json, _options);
            Assert.NotNull(data);
        }

        #endregion Test Methods
    }
}
