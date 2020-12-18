namespace FroniusTest
{
    #region Using Directives

    using System.Text.Json;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Testing;

    using Xunit;

    using UtilityLib;
    using FroniusLib.Models;
    using System;

    #endregion Using Directives

    /// <summary>
    /// XUnit testing class.
    /// </summary>
    [Collection("Fronius Test Collection")]
    public class TestWeb : IClassFixture<WebApplicationFactory<FroniusWeb.Startup>>
    {
        private readonly WebApplicationFactory<FroniusWeb.Startup> _factory;
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions();

        public TestWeb(WebApplicationFactory<FroniusWeb.Startup> factory)
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
        [InlineData("/common")]
        [InlineData("/inverter")]
        [InlineData("/logger")]
        [InlineData("/minmax")]
        [InlineData("/phase")]
        [InlineData("/version")]

        [InlineData("/data/property/CommonData")]
        [InlineData("/data/property/InverterInfo")]
        [InlineData("/data/property/LoggerInfo")]
        [InlineData("/data/property/MinMaxData")]
        [InlineData("/data/property/PhaseData")]

        [InlineData("/common/property/Frequency")]
        [InlineData("/common/property/CurrentDC")]
        [InlineData("/common/property/CurrentAC")]
        [InlineData("/common/property/VoltageDC")]
        [InlineData("/common/property/VoltageAC")]
        [InlineData("/common/property/PowerAC")]
        [InlineData("/common/property/DailyEnergy")]
        [InlineData("/common/property/YearlyEnergy")]
        [InlineData("/common/property/TotalEnergy")]
        [InlineData("/common/property/StatusCode")]

        [InlineData("/inverter/property/Index")]
        [InlineData("/inverter/property/DeviceType")]
        [InlineData("/inverter/property/PVPower")]
        [InlineData("/inverter/property/CustomName")]
        [InlineData("/inverter/property/Show")]
        [InlineData("/inverter/property/UniqueID")]
        [InlineData("/inverter/property/ErrorCode")]
        [InlineData("/inverter/property/StatusCode")]

        [InlineData("/logger/property/UniqueID")]
        [InlineData("/logger/property/ProductID")]
        [InlineData("/logger/property/PlatformID")]
        [InlineData("/logger/property/HWVersion")]
        [InlineData("/logger/property/SWVersion")]
        [InlineData("/logger/property/TimezoneLocation")]
        [InlineData("/logger/property/TimezoneName")]
        [InlineData("/logger/property/UTCOffset")]
        [InlineData("/logger/property/DefaultLanguage")]
        [InlineData("/logger/property/CashFactor")]
        [InlineData("/logger/property/CashCurrency")]
        [InlineData("/logger/property/CO2Factor")]
        [InlineData("/logger/property/CO2Unit")]

        [InlineData("/minmax/property/DailyMaxVoltageDC")]
        [InlineData("/minmax/property/DailyMaxVoltageAC")]
        [InlineData("/minmax/property/DailyMinVoltageAC")]
        [InlineData("/minmax/property/YearlyMaxVoltageDC")]
        [InlineData("/minmax/property/YearlyMaxVoltageAC")]
        [InlineData("/minmax/property/YearlyMinVoltageAC")]
        [InlineData("/minmax/property/TotalMaxVoltageDC")]
        [InlineData("/minmax/property/TotalMaxVoltageAC")]
        [InlineData("/minmax/property/TotalMinVoltageAC")]
        [InlineData("/minmax/property/DailyMaxPower")]
        [InlineData("/minmax/property/YearlyMaxPower")]
        [InlineData("/minmax/property/TotalMaxPower")]

        [InlineData("/phase/property/CurrentL1")]
        [InlineData("/phase/property/CurrentL2")]
        [InlineData("/phase/property/CurrentL3")]
        [InlineData("/phase/property/VoltageL1N")]
        [InlineData("/phase/property/VoltageL2N")]
        [InlineData("/phase/property/VoltageL3N")]

        [InlineData("/version/property/APIVersion")]
        [InlineData("/version/property/BaseURL")]
        [InlineData("/version/property/CompatibilityRange")]
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
        [InlineData("/data/property/CommonData", typeof(CommonDeviceData))]
        [InlineData("/data/property/InverterInfo", typeof(InverterDeviceData))]
        [InlineData("/data/property/LoggerInfo", typeof(LoggerDeviceData))]
        [InlineData("/data/property/MinMaxData", typeof(MinMaxDeviceData))]
        [InlineData("/data/property/PhaseData", typeof(PhaseDeviceData))]
        public async Task TestDataProperty(string url, Type type)
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
            dynamic data = JsonSerializer.Deserialize(json, type, _options);
            Assert.NotNull(data);
        }

        [Theory]
        [InlineData("/common/property/Frequency", typeof(double))]
        [InlineData("/common/property/CurrentDC", typeof(double))]
        [InlineData("/common/property/CurrentAC", typeof(double))]
        [InlineData("/common/property/VoltageDC", typeof(double))]
        [InlineData("/common/property/VoltageAC", typeof(double))]
        [InlineData("/common/property/PowerAC", typeof(double))]
        [InlineData("/common/property/DailyEnergy", typeof(double))]
        [InlineData("/common/property/YearlyEnergy", typeof(double))]
        [InlineData("/common/property/TotalEnergy", typeof(double))]
        [InlineData("/common/property/StatusCode", typeof(StatusCodes))]
        public async Task TestCommonProperty(string url, Type type)
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
            dynamic data = JsonSerializer.Deserialize(json, type, _options);
            Assert.NotNull(data);
        }

        [Theory]
        [InlineData("/inverter/property/Index", typeof(string))]
        [InlineData("/inverter/property/DeviceType", typeof(int))]
        [InlineData("/inverter/property/PVPower", typeof(int))]
        [InlineData("/inverter/property/CustomName", typeof(string))]
        [InlineData("/inverter/property/Show", typeof(bool))]
        [InlineData("/inverter/property/UniqueID", typeof(string))]
        [InlineData("/inverter/property/ErrorCode", typeof(int))]
        [InlineData("/inverter/property/StatusCode", typeof(StatusCodes))]
        public async Task TestInverterProperty(string url, Type type)
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
            dynamic data = JsonSerializer.Deserialize(json, type, _options);
            Assert.NotNull(data);
        }

        [Theory]
        [InlineData("/logger/property/UniqueID", typeof(string))]
        [InlineData("/logger/property/ProductID", typeof(string))]
        [InlineData("/logger/property/PlatformID", typeof(string))]
        [InlineData("/logger/property/HWVersion", typeof(string))]
        [InlineData("/logger/property/SWVersion", typeof(string))]
        [InlineData("/logger/property/TimezoneLocation", typeof(string))]
        [InlineData("/logger/property/TimezoneName", typeof(string))]
        [InlineData("/logger/property/UTCOffset", typeof(int))]
        [InlineData("/logger/property/DefaultLanguage", typeof(string))]
        [InlineData("/logger/property/CashFactor", typeof(double))]
        [InlineData("/logger/property/CashCurrency", typeof(string))]
        [InlineData("/logger/property/CO2Factor", typeof(double))]
        [InlineData("/logger/property/CO2Unit", typeof(string))]
        public async Task TestLoggerProperty(string url, Type type)
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
            dynamic data = JsonSerializer.Deserialize(json, type, _options);
            Assert.NotNull(data);
        }

        [Theory]
        [InlineData("/minmax/property/DailyMaxVoltageDC")]
        [InlineData("/minmax/property/DailyMaxVoltageAC")]
        [InlineData("/minmax/property/DailyMinVoltageAC")]
        [InlineData("/minmax/property/YearlyMaxVoltageDC")]
        [InlineData("/minmax/property/YearlyMaxVoltageAC")]
        [InlineData("/minmax/property/YearlyMinVoltageAC")]
        [InlineData("/minmax/property/TotalMaxVoltageDC")]
        [InlineData("/minmax/property/TotalMaxVoltageAC")]
        [InlineData("/minmax/property/TotalMinVoltageAC")]
        [InlineData("/minmax/property/DailyMaxPower")]
        [InlineData("/minmax/property/YearlyMaxPower")]
        [InlineData("/minmax/property/TotalMaxPower")]
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
        [InlineData("/phase/property/CurrentL1")]
        [InlineData("/phase/property/CurrentL2")]
        [InlineData("/phase/property/CurrentL3")]
        [InlineData("/phase/property/VoltageL1N")]
        [InlineData("/phase/property/VoltageL2N")]
        [InlineData("/phase/property/VoltageL3N")]
        public async Task TestPhaseProperty(string url)
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
        [InlineData("/version/property/APIVersion")]
        [InlineData("/version/property/BaseURL")]
        [InlineData("/version/property/CompatibilityRange")]
        public async Task TestVersionProperty(string url)
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
            dynamic data = JsonSerializer.Deserialize(json, typeof(string), _options);
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
            var info = JsonSerializer.Deserialize<FroniusInfo>(json);
            Assert.True(info.Settings.BaseAddress.Length > 0);
            Assert.Equal(100, info.Settings.Timeout);
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
            var data = JsonSerializer.Deserialize<FroniusData>(json, _options);
            Assert.NotNull(data);
        }

        [Fact]
        public async Task TestCommon()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/common");

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<CommonData>(json, _options);
            Assert.NotNull(data);
        }

        [Fact]
        public async Task TestInverter()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/inverter");

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<InverterInfo>(json, _options);
            Assert.NotNull(data);
        }

        [Fact]
        public async Task TestLogger()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/logger");

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<LoggerInfo>(json, _options);
            Assert.NotNull(data);
        }

        [Fact]
        public async Task TestMinMax()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/minmax");

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<MinMaxData>(json, _options);
            Assert.NotNull(data);
        }

        [Fact]
        public async Task TestPhase()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/phase");

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<PhaseData>(json, _options);
            Assert.NotNull(data);
        }

        [Fact]
        public async Task TestVersion()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/version");

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<APIVersionData>(json, _options);
            Assert.NotNull(data);
        }

        #endregion Test Methods
    }
}
