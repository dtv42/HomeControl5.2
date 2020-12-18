namespace WallboxTest
{
    #region Using Directives

    using System.Text.Json;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Testing;

    using Xunit;

    using UtilityLib;
    using WallboxLib.Models;
    using System.Collections.Generic;

    #endregion Using Directives

    /// <summary>
    /// XUnit testing class.
    /// </summary>
    [Collection("Helios Test Collection")]
    public class TestWeb : IClassFixture<WebApplicationFactory<WallboxWeb.Startup>>
    {
        private readonly WebApplicationFactory<WallboxWeb.Startup> _factory;
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions();

        public TestWeb(WebApplicationFactory<WallboxWeb.Startup> factory)
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
        [InlineData("/report1")]
        [InlineData("/report2")]
        [InlineData("/report3")]
        [InlineData("/report100")]
        [InlineData("/reports")]
        [InlineData("/reports/101")]
        [InlineData("/reports/102")]
        [InlineData("/reports/103")]
        [InlineData("/reports/104")]
        [InlineData("/reports/105")]
        [InlineData("/reports/106")]
        [InlineData("/reports/107")]
        [InlineData("/reports/108")]
        [InlineData("/reports/109")]
        [InlineData("/reports/110")]
        [InlineData("/reports/111")]
        [InlineData("/reports/112")]
        [InlineData("/reports/113")]
        [InlineData("/reports/114")]
        [InlineData("/reports/115")]
        [InlineData("/reports/116")]
        [InlineData("/reports/117")]
        [InlineData("/reports/118")]
        [InlineData("/reports/119")]
        [InlineData("/reports/120")]
        [InlineData("/reports/121")]
        [InlineData("/reports/122")]
        [InlineData("/reports/123")]
        [InlineData("/reports/124")]
        [InlineData("/reports/125")]
        [InlineData("/reports/126")]
        [InlineData("/reports/127")]
        [InlineData("/reports/128")]
        [InlineData("/reports/129")]
        [InlineData("/reports/130")]
        [InlineData("/info")]

        [InlineData("/report1/property/ID")]
        [InlineData("/report1/property/Product")]
        [InlineData("/report1/property/Serial")]
        [InlineData("/report1/property/Firmware")]
        [InlineData("/report1/property/ComModule")]
        [InlineData("/report1/property/Backend")]
        [InlineData("/report1/property/TimeQ")]
        [InlineData("/report1/property/DIPSwitch1")]
        [InlineData("/report1/property/DIPSwitch2")]
        [InlineData("/report1/property/Seconds")]

        [InlineData("/report2/property/ID")]
        [InlineData("/report2/property/State")]
        [InlineData("/report2/property/Error1")]
        [InlineData("/report2/property/Error2")]
        [InlineData("/report2/property/Plug")]
        [InlineData("/report2/property/AuthON")]
        [InlineData("/report2/property/AuthRequired")]
        [InlineData("/report2/property/EnableSystem")]
        [InlineData("/report2/property/EnableUser")]
        [InlineData("/report2/property/MaxCurrent")]
        [InlineData("/report2/property/DutyCycle")]
        [InlineData("/report2/property/CurrentHW")]
        [InlineData("/report2/property/CurrentUser")]
        [InlineData("/report2/property/CurrentFS")]
        [InlineData("/report2/property/TimeoutFS")]
        [InlineData("/report2/property/CurrentTimer")]
        [InlineData("/report2/property/TimeoutCT")]
        [InlineData("/report2/property/SetEnergy")]
        [InlineData("/report2/property/Output")]
        [InlineData("/report2/property/Input")]
        [InlineData("/report2/property/Serial")]
        [InlineData("/report2/property/Seconds")]

        [InlineData("/report3/property/ID")]
        [InlineData("/report3/property/VoltageL1N")]
        [InlineData("/report3/property/VoltageL2N")]
        [InlineData("/report3/property/VoltageL3N")]
        [InlineData("/report3/property/CurrentL1")]
        [InlineData("/report3/property/CurrentL2")]
        [InlineData("/report3/property/CurrentL3")]
        [InlineData("/report3/property/Power")]
        [InlineData("/report3/property/PowerFactor")]
        [InlineData("/report3/property/EnergyCharging")]
        [InlineData("/report3/property/EnergyTotal")]
        [InlineData("/report3/property/Serial")]
        [InlineData("/report3/property/Seconds")]

        [InlineData("/report100/property/ID")]
        [InlineData("/report100/property/SessionID")]
        [InlineData("/report100/property/CurrentHW")]
        [InlineData("/report100/property/EnergyConsumption")]
        [InlineData("/report100/property/EnergyTransferred")]
        [InlineData("/report100/property/StartedSeconds")]
        [InlineData("/report100/property/EndedSeconds")]
        [InlineData("/report100/property/Started")]
        [InlineData("/report100/property/Ended")]
        [InlineData("/report100/property/Reason")]
        [InlineData("/report100/property/TimeQ")]
        [InlineData("/report100/property/RFID")]
        [InlineData("/report100/property/Serial")]
        [InlineData("/report100/property/Seconds")]
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
        [InlineData("/report1/property/ID")]
        [InlineData("/report1/property/Product")]
        [InlineData("/report1/property/Serial")]
        [InlineData("/report1/property/Firmware")]
        [InlineData("/report1/property/ComModule")]
        [InlineData("/report1/property/Backend")]
        [InlineData("/report1/property/TimeQ")]
        [InlineData("/report1/property/DIPSwitch1")]
        [InlineData("/report1/property/DIPSwitch2")]
        [InlineData("/report1/property/Seconds")]
        public async Task TestReport1Property(string url)
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
                typeof(Report1Data).GetProperty(url.Split("/")[3]).PropertyType, _options);
            Assert.NotNull(data);
        }

        [Theory]
        [InlineData("/report2/property/ID")]
        [InlineData("/report2/property/State")]
        [InlineData("/report2/property/Error1")]
        [InlineData("/report2/property/Error2")]
        [InlineData("/report2/property/Plug")]
        [InlineData("/report2/property/AuthON")]
        [InlineData("/report2/property/AuthRequired")]
        [InlineData("/report2/property/EnableSystem")]
        [InlineData("/report2/property/EnableUser")]
        [InlineData("/report2/property/MaxCurrent")]
        [InlineData("/report2/property/DutyCycle")]
        [InlineData("/report2/property/CurrentHW")]
        [InlineData("/report2/property/CurrentUser")]
        [InlineData("/report2/property/CurrentFS")]
        [InlineData("/report2/property/TimeoutFS")]
        [InlineData("/report2/property/CurrentTimer")]
        [InlineData("/report2/property/TimeoutCT")]
        [InlineData("/report2/property/SetEnergy")]
        [InlineData("/report2/property/Output")]
        [InlineData("/report2/property/Input")]
        [InlineData("/report2/property/Serial")]
        [InlineData("/report2/property/Seconds")]
        public async Task TestReport2Property(string url)
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
                typeof(Report2Data).GetProperty(url.Split("/")[3]).PropertyType, _options);
            Assert.NotNull(data);
        }

        [Theory]
        [InlineData("/report3/property/ID")]
        [InlineData("/report3/property/VoltageL1N")]
        [InlineData("/report3/property/VoltageL2N")]
        [InlineData("/report3/property/VoltageL3N")]
        [InlineData("/report3/property/CurrentL1")]
        [InlineData("/report3/property/CurrentL2")]
        [InlineData("/report3/property/CurrentL3")]
        [InlineData("/report3/property/Power")]
        [InlineData("/report3/property/PowerFactor")]
        [InlineData("/report3/property/EnergyCharging")]
        [InlineData("/report3/property/EnergyTotal")]
        [InlineData("/report3/property/Serial")]
        [InlineData("/report3/property/Seconds")]
        public async Task TestReport3Property(string url)
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
                typeof(Report3Data).GetProperty(url.Split("/")[3]).PropertyType, _options);
            Assert.NotNull(data);
        }

        [Theory]
        [InlineData("/report100/property/ID")]
        [InlineData("/report100/property/SessionID")]
        [InlineData("/report100/property/CurrentHW")]
        [InlineData("/report100/property/EnergyConsumption")]
        [InlineData("/report100/property/EnergyTransferred")]
        [InlineData("/report100/property/StartedSeconds")]
        [InlineData("/report100/property/EndedSeconds")]
        [InlineData("/report100/property/Started")]
        [InlineData("/report100/property/Ended")]
        [InlineData("/report100/property/Reason")]
        [InlineData("/report100/property/TimeQ")]
        [InlineData("/report100/property/RFID")]
        [InlineData("/report100/property/Serial")]
        [InlineData("/report100/property/Seconds")]
        public async Task TestReport100Property(string url)
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
                typeof(ReportsData).GetProperty(url.Split("/")[3]).PropertyType, _options);
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
            var info = JsonSerializer.Deserialize<WallboxInfo>(json);
            Assert.True(info.Settings.EndPoint.Length > 0);
            Assert.Equal(7090, info.Settings.Port);
            Assert.Equal(1000, info.Settings.Timeout);
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
            var data = JsonSerializer.Deserialize<WallboxData>(json, _options);
            Assert.NotNull(data);
        }

        [Fact]
        public async Task TestReport1()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/report1");

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<Report1Data>(json, _options);
            Assert.NotNull(data);
        }

        [Fact]
        public async Task TestReport2()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/report2");

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<Report2Data>(json, _options);
            Assert.NotNull(data);
        }

        [Fact]
        public async Task TestReport3()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/report3");

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<Report3Data>(json, _options);
            Assert.NotNull(data);
        }

        [Fact]
        public async Task TestReport100()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/report100");

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<ReportsData>(json, _options);
            Assert.NotNull(data);
        }

        [Fact]
        public async Task TestReports()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/reports");

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<List<ReportsData>>(json, _options);
            Assert.NotNull(data);
            Assert.NotEmpty(data);
        }

        #endregion Test Methods
    }
}
