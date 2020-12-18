namespace ETAPU11Test
{
    #region Using Directives

    using System.Text.Json;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Testing;

    using Xunit;

    using UtilityLib;
    using ETAPU11Lib.Models;

    #endregion Using Directives

    /// <summary>
    /// XUnit testing class.
    /// </summary>
    [Collection("ETAPU11 Test Collection")]
    public class TestWeb : IClassFixture<WebApplicationFactory<ETAPU11Web.Startup>>
    {
        private readonly WebApplicationFactory<ETAPU11Web.Startup> _factory;
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions();

        public TestWeb(WebApplicationFactory<ETAPU11Web.Startup> factory)
        {
            _factory = factory;
            _options.AddDefaultOptions();
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
        [InlineData("/health")]
        [InlineData("/health?access=true")]
        [InlineData("/health?timeout=10")]
        [InlineData("/health?access=true&timeout=10")]
        public async Task TestHealthEndpoints(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<UIHealthReport>(json, _options);
            Assert.NotNull(data);
            Assert.Equal(UIHealthStatus.Healthy, data.Status);
        }

        [Theory]
        [InlineData("/gateway")]
        [InlineData("/data")]
        [InlineData("/boiler")]
        [InlineData("/heating")]
        [InlineData("/hotwater")]
        [InlineData("/storage")]
        [InlineData("/system")]

        [InlineData("/data/property/FullLoadHours")]
        [InlineData("/data/property/TotalConsumed")]
        [InlineData("/data/property/BoilerState")]
        [InlineData("/data/property/BoilerPressure")]
        [InlineData("/data/property/BoilerTemperature")]
        [InlineData("/data/property/BoilerTarget")]
        [InlineData("/data/property/BoilerBottom")]
        [InlineData("/data/property/FlowControlState")]
        [InlineData("/data/property/DiverterValveState")]
        [InlineData("/data/property/DiverterValveDemand")]
        [InlineData("/data/property/DemandedOutput")]
        [InlineData("/data/property/FlowMixValveTarget")]
        [InlineData("/data/property/FlowMixValveState")]
        [InlineData("/data/property/FlowMixValveCurrTemp")]
        [InlineData("/data/property/FlowMixValvePosition")]
        [InlineData("/data/property/BoilerPumpOutput")]
        [InlineData("/data/property/BoilerPumpDemand")]
        [InlineData("/data/property/FlueGasTemperature")]
        [InlineData("/data/property/DraughtFanSpeed")]
        [InlineData("/data/property/ResidualO2")]
        [InlineData("/data/property/StokerScrewDemand")]
        [InlineData("/data/property/StokerScrewClockRate")]
        [InlineData("/data/property/StokerScrewState")]
        [InlineData("/data/property/StokerScrewMotorCurr")]
        [InlineData("/data/property/AshRemovalState")]
        [InlineData("/data/property/AshRemovalStartIdleTime")]
        [InlineData("/data/property/AshRemovalDurationIdleTime")]
        [InlineData("/data/property/ConsumptionSinceDeAsh")]
        [InlineData("/data/property/ConsumptionSinceAshBoxEmptied")]
        [InlineData("/data/property/EmptyAshBoxAfter")]
        [InlineData("/data/property/ConsumptionSinceMaintainence")]
        [InlineData("/data/property/HopperState")]
        [InlineData("/data/property/HopperFillUpPelletBin")]
        [InlineData("/data/property/HopperPelletBinContents")]
        [InlineData("/data/property/HopperFillUpTime")]
        [InlineData("/data/property/HopperVacuumState")]
        [InlineData("/data/property/HopperVacuumDemand")]
        [InlineData("/data/property/OnOffButton")]
        [InlineData("/data/property/DeAshButton")]
        [InlineData("/data/property/HotwaterTankState")]
        [InlineData("/data/property/ChargingTimesState")]
        [InlineData("/data/property/ChargingTimesSwitchStatus")]
        [InlineData("/data/property/ChargingTimesTemperature")]
        [InlineData("/data/property/HotwaterSwitchonDiff")]
        [InlineData("/data/property/HotwaterTarget")]
        [InlineData("/data/property/HotwaterTemperature")]
        [InlineData("/data/property/ChargeButton")]
        [InlineData("/data/property/RoomSensor")]
        [InlineData("/data/property/HeatingCircuitState")]
        [InlineData("/data/property/RunningState")]
        [InlineData("/data/property/HeatingTimes")]
        [InlineData("/data/property/HeatingSwitchStatus")]
        [InlineData("/data/property/HeatingTemperature")]
        [InlineData("/data/property/RoomTemperature")]
        [InlineData("/data/property/RoomTarget")]
        [InlineData("/data/property/Flow")]
        [InlineData("/data/property/HeatingCurve")]
        [InlineData("/data/property/FlowAtMinus10")]
        [InlineData("/data/property/FlowAtPlus10")]
        [InlineData("/data/property/FlowSetBack")]
        [InlineData("/data/property/OutsideTemperatureDelayed")]
        [InlineData("/data/property/DayHeatingThreshold")]
        [InlineData("/data/property/NightHeatingThreshold")]
        [InlineData("/data/property/HeatingDayButton")]
        [InlineData("/data/property/HeatingAutoButton")]
        [InlineData("/data/property/HeatingNightButton")]
        [InlineData("/data/property/HeatingOnOffButton")]
        [InlineData("/data/property/HeatingHomeButton")]
        [InlineData("/data/property/HeatingAwayButton")]
        [InlineData("/data/property/HeatingHolidayStart")]
        [InlineData("/data/property/HeatingHolidayEnd")]
        [InlineData("/data/property/DischargeScrewDemand")]
        [InlineData("/data/property/DischargeScrewClockRate")]
        [InlineData("/data/property/DischargeScrewState")]
        [InlineData("/data/property/DischargeScrewMotorCurr")]
        [InlineData("/data/property/ConveyingSystem")]
        [InlineData("/data/property/Stock")]
        [InlineData("/data/property/StockWarningLimit")]
        [InlineData("/data/property/OutsideTemperature")]
        [InlineData("/data/property/FirebedState")]
        [InlineData("/data/property/SupplyDemand")]
        [InlineData("/data/property/IgnitionDemand")]
        [InlineData("/data/property/FlowMixValveTemperature")]
        [InlineData("/data/property/AirValveSetPosition")]
        [InlineData("/data/property/AirValveCurrPosition")]

        [InlineData("/boiler/property/FullLoadHours")]
        [InlineData("/boiler/property/TotalConsumed")]
        [InlineData("/boiler/property/ConsumptionSinceDeAsh")]
        [InlineData("/boiler/property/ConsumptionSinceAshBoxEmptied")]
        [InlineData("/boiler/property/ConsumptionSinceMaintainence")]
        [InlineData("/boiler/property/HopperFillUpPelletBin")]
        [InlineData("/boiler/property/HopperPelletBinContents")]
        [InlineData("/boiler/property/HopperFillUpTime")]
        [InlineData("/boiler/property/BoilerState")]
        [InlineData("/boiler/property/BoilerPressure")]
        [InlineData("/boiler/property/BoilerTemperature")]
        [InlineData("/boiler/property/BoilerTarget")]
        [InlineData("/boiler/property/BoilerBottom")]
        [InlineData("/boiler/property/FlueGasTemperature")]
        [InlineData("/boiler/property/DraughtFanSpeed")]
        [InlineData("/boiler/property/ResidualO2")]

        [InlineData("hotwater/property/HotwaterTankState")]
        [InlineData("hotwater/property/ChargingTimesState")]
        [InlineData("hotwater/property/ChargingTimesSwitchStatus")]
        [InlineData("hotwater/property/ChargingTimesTemperature")]
        [InlineData("hotwater/property/HotwaterSwitchonDiff")]
        [InlineData("hotwater/property/HotwaterTarget")]
        [InlineData("hotwater/property/HotwaterTemperature")]

        [InlineData("heating/property/RoomSensor")]
        [InlineData("heating/property/HeatingCircuitState")]
        [InlineData("heating/property/RunningState")]
        [InlineData("heating/property/HeatingTimes")]
        [InlineData("heating/property/HeatingSwitchStatus")]
        [InlineData("heating/property/HeatingTemperature")]
        [InlineData("heating/property/RoomTemperature")]
        [InlineData("heating/property/RoomTarget")]
        [InlineData("heating/property/Flow")]
        [InlineData("heating/property/DayHeatingThreshold")]
        [InlineData("heating/property/NightHeatingThreshold")]

        [InlineData("/storage/property/DischargeScrewDemand")]
        [InlineData("/storage/property/DischargeScrewState")]
        [InlineData("/storage/property/DischargeScrewMotorCurr")]
        [InlineData("/storage/property/ConveyingSystem")]
        [InlineData("/storage/property/Stock")]
        [InlineData("/storage/property/StockWarningLimit")]

        [InlineData("/system/property/OutsideTemperature")]
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
        [InlineData("/data/property/FullLoadHours")]
        [InlineData("/data/property/TotalConsumed")]
        [InlineData("/data/property/BoilerState")]
        [InlineData("/data/property/BoilerPressure")]
        [InlineData("/data/property/BoilerTemperature")]
        [InlineData("/data/property/BoilerTarget")]
        [InlineData("/data/property/BoilerBottom")]
        [InlineData("/data/property/FlowControlState")]
        [InlineData("/data/property/DiverterValveState")]
        [InlineData("/data/property/DiverterValveDemand")]
        [InlineData("/data/property/DemandedOutput")]
        [InlineData("/data/property/FlowMixValveTarget")]
        [InlineData("/data/property/FlowMixValveState")]
        [InlineData("/data/property/FlowMixValveCurrTemp")]
        [InlineData("/data/property/FlowMixValvePosition")]
        [InlineData("/data/property/BoilerPumpOutput")]
        [InlineData("/data/property/BoilerPumpDemand")]
        [InlineData("/data/property/FlueGasTemperature")]
        [InlineData("/data/property/DraughtFanSpeed")]
        [InlineData("/data/property/ResidualO2")]
        [InlineData("/data/property/StokerScrewDemand")]
        [InlineData("/data/property/StokerScrewClockRate")]
        [InlineData("/data/property/StokerScrewState")]
        [InlineData("/data/property/StokerScrewMotorCurr")]
        [InlineData("/data/property/AshRemovalState")]
        [InlineData("/data/property/AshRemovalStartIdleTime")]
        [InlineData("/data/property/AshRemovalDurationIdleTime")]
        [InlineData("/data/property/ConsumptionSinceDeAsh")]
        [InlineData("/data/property/ConsumptionSinceAshBoxEmptied")]
        [InlineData("/data/property/EmptyAshBoxAfter")]
        [InlineData("/data/property/ConsumptionSinceMaintainence")]
        [InlineData("/data/property/HopperState")]
        [InlineData("/data/property/HopperFillUpPelletBin")]
        [InlineData("/data/property/HopperPelletBinContents")]
        [InlineData("/data/property/HopperFillUpTime")]
        [InlineData("/data/property/HopperVacuumState")]
        [InlineData("/data/property/HopperVacuumDemand")]
        [InlineData("/data/property/OnOffButton")]
        [InlineData("/data/property/DeAshButton")]
        [InlineData("/data/property/HotwaterTankState")]
        [InlineData("/data/property/ChargingTimesState")]
        [InlineData("/data/property/ChargingTimesSwitchStatus")]
        [InlineData("/data/property/ChargingTimesTemperature")]
        [InlineData("/data/property/HotwaterSwitchonDiff")]
        [InlineData("/data/property/HotwaterTarget")]
        [InlineData("/data/property/HotwaterTemperature")]
        [InlineData("/data/property/ChargeButton")]
        [InlineData("/data/property/RoomSensor")]
        [InlineData("/data/property/HeatingCircuitState")]
        [InlineData("/data/property/RunningState")]
        [InlineData("/data/property/HeatingTimes")]
        [InlineData("/data/property/HeatingSwitchStatus")]
        [InlineData("/data/property/HeatingTemperature")]
        [InlineData("/data/property/RoomTemperature")]
        [InlineData("/data/property/RoomTarget")]
        [InlineData("/data/property/Flow")]
        [InlineData("/data/property/HeatingCurve")]
        [InlineData("/data/property/FlowAtMinus10")]
        [InlineData("/data/property/FlowAtPlus10")]
        [InlineData("/data/property/FlowSetBack")]
        [InlineData("/data/property/OutsideTemperatureDelayed")]
        [InlineData("/data/property/DayHeatingThreshold")]
        [InlineData("/data/property/NightHeatingThreshold")]
        [InlineData("/data/property/HeatingDayButton")]
        [InlineData("/data/property/HeatingAutoButton")]
        [InlineData("/data/property/HeatingNightButton")]
        [InlineData("/data/property/HeatingOnOffButton")]
        [InlineData("/data/property/HeatingHomeButton")]
        [InlineData("/data/property/HeatingAwayButton")]
        [InlineData("/data/property/HeatingHolidayStart")]
        [InlineData("/data/property/HeatingHolidayEnd")]
        [InlineData("/data/property/DischargeScrewDemand")]
        [InlineData("/data/property/DischargeScrewClockRate")]
        [InlineData("/data/property/DischargeScrewState")]
        [InlineData("/data/property/DischargeScrewMotorCurr")]
        [InlineData("/data/property/ConveyingSystem")]
        [InlineData("/data/property/Stock")]
        [InlineData("/data/property/StockWarningLimit")]
        [InlineData("/data/property/OutsideTemperature")]
        [InlineData("/data/property/FirebedState")]
        [InlineData("/data/property/SupplyDemand")]
        [InlineData("/data/property/IgnitionDemand")]
        [InlineData("/data/property/FlowMixValveTemperature")]
        [InlineData("/data/property/AirValveSetPosition")]
        [InlineData("/data/property/AirValveCurrPosition")]
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
            dynamic data = JsonSerializer.Deserialize(json, typeof(double), _options);
            Assert.NotNull(data);
        }

        [Theory]
        [InlineData("/boiler/property/FullLoadHours")]
        [InlineData("/boiler/property/TotalConsumed")]
        [InlineData("/boiler/property/ConsumptionSinceDeAsh")]
        [InlineData("/boiler/property/ConsumptionSinceAshBoxEmptied")]
        [InlineData("/boiler/property/ConsumptionSinceMaintainence")]
        [InlineData("/boiler/property/HopperFillUpPelletBin")]
        [InlineData("/boiler/property/HopperPelletBinContents")]
        [InlineData("/boiler/property/HopperFillUpTime")]
        [InlineData("/boiler/property/BoilerState")]
        [InlineData("/boiler/property/BoilerPressure")]
        [InlineData("/boiler/property/BoilerTemperature")]
        [InlineData("/boiler/property/BoilerTarget")]
        [InlineData("/boiler/property/BoilerBottom")]
        [InlineData("/boiler/property/FlueGasTemperature")]
        [InlineData("/boiler/property/DraughtFanSpeed")]
        [InlineData("/boiler/property/ResidualO2")]
        public async Task TestBoilerProperty(string url)
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
            dynamic data = JsonSerializer.Deserialize(json, typeof(double), _options);
            Assert.NotNull(data);
        }

        [Theory]
        [InlineData("hotwater/property/HotwaterTankState")]
        [InlineData("hotwater/property/ChargingTimesState")]
        [InlineData("hotwater/property/ChargingTimesSwitchStatus")]
        [InlineData("hotwater/property/ChargingTimesTemperature")]
        [InlineData("hotwater/property/HotwaterSwitchonDiff")]
        [InlineData("hotwater/property/HotwaterTarget")]
        [InlineData("hotwater/property/HotwaterTemperature")]
        public async Task TestHotwaterProperty(string url)
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
            dynamic data = JsonSerializer.Deserialize(json, typeof(double), _options);
            Assert.NotNull(data);
        }

        [Theory]
        [InlineData("heating/property/RoomSensor")]
        [InlineData("heating/property/HeatingCircuitState")]
        [InlineData("heating/property/RunningState")]
        [InlineData("heating/property/HeatingTimes")]
        [InlineData("heating/property/HeatingSwitchStatus")]
        [InlineData("heating/property/HeatingTemperature")]
        [InlineData("heating/property/RoomTemperature")]
        [InlineData("heating/property/RoomTarget")]
        [InlineData("heating/property/Flow")]
        [InlineData("heating/property/DayHeatingThreshold")]
        [InlineData("heating/property/NightHeatingThreshold")]
        public async Task TestHeatingProperty(string url)
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
            dynamic data = JsonSerializer.Deserialize(json, typeof(double), _options);
            Assert.NotNull(data);
        }

        [Theory]
        [InlineData("/storage/property/DischargeScrewDemand")]
        [InlineData("/storage/property/DischargeScrewState")]
        [InlineData("/storage/property/DischargeScrewMotorCurr")]
        [InlineData("/storage/property/ConveyingSystem")]
        [InlineData("/storage/property/Stock")]
        [InlineData("/storage/property/StockWarningLimit")]
        public async Task TestStorageProperty(string url)
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
            dynamic data = JsonSerializer.Deserialize(json, typeof(double), _options);
            Assert.NotNull(data);
        }

        [Theory]
        [InlineData("/system/property/OutsideTemperature")]
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
            dynamic data = JsonSerializer.Deserialize(json, typeof(double), _options);
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
            var info = JsonSerializer.Deserialize<ETAPU11Info>(json);
            Assert.True(info.Settings.TcpSlave.Address.Length > 0);
            Assert.Equal(502, info.Settings.TcpSlave.Port);
            Assert.Equal(1, info.Settings.TcpSlave.ID);
            Assert.Equal(1000, info.Settings.TcpMaster.ReceiveTimeout);
            Assert.Equal(1000, info.Settings.TcpMaster.SendTimeout);
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
            var data = JsonSerializer.Deserialize<ETAPU11Data>(json, _options);
            Assert.NotNull(data);
        }

        [Fact]
        public async Task TestBoiler()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/boiler");

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<BoilerData>(json, _options);
            Assert.NotNull(data);
        }

        [Fact]
        public async Task TestHeating()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/heating");

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<HeatingData>(json, _options);
            Assert.NotNull(data);
        }

        [Fact]
        public async Task TestHotwater()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/hotwater");

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<HotwaterData>(json, _options);
            Assert.NotNull(data);
        }

        [Fact]
        public async Task TestStorage()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/storage");

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<StorageData>(json, _options);
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

        #endregion Test Methods
    }
}
