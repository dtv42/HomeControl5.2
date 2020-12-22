// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestWeb.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>17-12-2020 12:51</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace EM300LRTest
{
    #region Using Directives

    using System.Text.Json;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc.Testing;

    using HealthChecks.UI.Core;

    using Xunit;

    using UtilityLib;

    using EM300LRLib.Models;

    #endregion Using Directives

    /// <summary>
    /// XUnit testing class.
    /// </summary>
    [Collection("EM300LR Test Collection")]
    public class TestWeb : IClassFixture<WebApplicationFactory<EM300LRWeb.Startup>>
    {
        private readonly WebApplicationFactory<EM300LRWeb.Startup> _factory;
        private readonly JsonSerializerOptions _options = new JsonSerializerOptions();

        public TestWeb(WebApplicationFactory<EM300LRWeb.Startup> factory)
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
        [InlineData("/total")]
        [InlineData("/phase1")]
        [InlineData("/phase2")]
        [InlineData("/phase3")]

        [InlineData("/data/property/Serial")]
        [InlineData("/data/property/ActivePowerPlus")]
        [InlineData("/data/property/ActiveEnergyPlus")]
        [InlineData("/data/property/ActivePowerMinus")]
        [InlineData("/data/property/ActiveEnergyMinus")]
        [InlineData("/data/property/ReactivePowerPlus")]
        [InlineData("/data/property/ReactiveEnergyPlus")]
        [InlineData("/data/property/ReactivePowerMinus")]
        [InlineData("/data/property/ReactiveEnergyMinus")]
        [InlineData("/data/property/ApparentPowerPlus")]
        [InlineData("/data/property/ApparentEnergyPlus")]
        [InlineData("/data/property/ApparentPowerMinus")]
        [InlineData("/data/property/ApparentEnergyMinus")]
        [InlineData("/data/property/PowerFactor")]
        [InlineData("/data/property/SupplyFrequency")]
        [InlineData("/data/property/ActivePowerPlusL1")]
        [InlineData("/data/property/ActiveEnergyPlusL1")]
        [InlineData("/data/property/ActivePowerMinusL1")]
        [InlineData("/data/property/ActiveEnergyMinusL1")]
        [InlineData("/data/property/ReactivePowerPlusL1")]
        [InlineData("/data/property/ReactiveEnergyPlusL1")]
        [InlineData("/data/property/ReactivePowerMinusL1")]
        [InlineData("/data/property/ReactiveEnergyMinusL1")]
        [InlineData("/data/property/ApparentPowerPlusL1")]
        [InlineData("/data/property/ApparentEnergyPlusL1")]
        [InlineData("/data/property/ApparentPowerMinusL1")]
        [InlineData("/data/property/ApparentEnergyMinusL1")]
        [InlineData("/data/property/CurrentL1")]
        [InlineData("/data/property/VoltageL1")]
        [InlineData("/data/property/PowerFactorL1")]
        [InlineData("/data/property/ActivePowerPlusL2")]
        [InlineData("/data/property/ActiveEnergyPlusL2")]
        [InlineData("/data/property/ActivePowerMinusL2")]
        [InlineData("/data/property/ActiveEnergyMinusL2")]
        [InlineData("/data/property/ReactivePowerPlusL2")]
        [InlineData("/data/property/ReactiveEnergyPlusL2")]
        [InlineData("/data/property/ReactivePowerMinusL2")]
        [InlineData("/data/property/ReactiveEnergyMinusL2")]
        [InlineData("/data/property/ApparentPowerPlusL2")]
        [InlineData("/data/property/ApparentEnergyPlusL2")]
        [InlineData("/data/property/ApparentPowerMinusL2")]
        [InlineData("/data/property/ApparentEnergyMinusL2")]
        [InlineData("/data/property/CurrentL2")]
        [InlineData("/data/property/VoltageL2")]
        [InlineData("/data/property/PowerFactorL2")]
        [InlineData("/data/property/ActivePowerPlusL3")]
        [InlineData("/data/property/ActiveEnergyPlusL3")]
        [InlineData("/data/property/ActivePowerMinusL3")]
        [InlineData("/data/property/ActiveEnergyMinusL3")]
        [InlineData("/data/property/ReactivePowerPlusL3")]
        [InlineData("/data/property/ReactiveEnergyPlusL3")]
        [InlineData("/data/property/ReactivePowerMinusL3")]
        [InlineData("/data/property/ReactiveEnergyMinusL3")]
        [InlineData("/data/property/ApparentPowerPlusL3")]
        [InlineData("/data/property/ApparentEnergyPlusL3")]
        [InlineData("/data/property/ApparentPowerMinusL3")]
        [InlineData("/data/property/ApparentEnergyMinusL3")]
        [InlineData("/data/property/CurrentL3")]
        [InlineData("/data/property/VoltageL3")]
        [InlineData("/data/property/PowerFactorL3")]

        [InlineData("/total/property/ActivePowerPlus")]
        [InlineData("/total/property/ActiveEnergyPlus")]
        [InlineData("/total/property/ActivePowerMinus")]
        [InlineData("/total/property/ActiveEnergyMinus")]
        [InlineData("/total/property/ReactivePowerPlus")]
        [InlineData("/total/property/ReactiveEnergyPlus")]
        [InlineData("/total/property/ReactivePowerMinus")]
        [InlineData("/total/property/ReactiveEnergyMinus")]
        [InlineData("/total/property/ApparentPowerPlus")]
        [InlineData("/total/property/ApparentEnergyPlus")]
        [InlineData("/total/property/ApparentPowerMinus")]
        [InlineData("/total/property/ApparentEnergyMinus")]
        [InlineData("/total/property/PowerFactor")]
        [InlineData("/total/property/SupplyFrequency")]

        [InlineData("/phase1/property/ActivePowerPlus")]
        [InlineData("/phase1/property/ActiveEnergyPlus")]
        [InlineData("/phase1/property/ActivePowerMinus")]
        [InlineData("/phase1/property/ActiveEnergyMinus")]
        [InlineData("/phase1/property/ReactivePowerPlus")]
        [InlineData("/phase1/property/ReactiveEnergyPlus")]
        [InlineData("/phase1/property/ReactivePowerMinus")]
        [InlineData("/phase1/property/ReactiveEnergyMinus")]
        [InlineData("/phase1/property/ApparentPowerPlus")]
        [InlineData("/phase1/property/ApparentEnergyPlus")]
        [InlineData("/phase1/property/ApparentPowerMinus")]
        [InlineData("/phase1/property/ApparentEnergyMinus")]
        [InlineData("/phase1/property/PowerFactor")]
        [InlineData("/phase1/property/Current")]
        [InlineData("/phase1/property/Voltage")]

        [InlineData("/phase2/property/ActivePowerPlus")]
        [InlineData("/phase2/property/ActiveEnergyPlus")]
        [InlineData("/phase2/property/ActivePowerMinus")]
        [InlineData("/phase2/property/ActiveEnergyMinus")]
        [InlineData("/phase2/property/ReactivePowerPlus")]
        [InlineData("/phase2/property/ReactiveEnergyPlus")]
        [InlineData("/phase2/property/ReactivePowerMinus")]
        [InlineData("/phase2/property/ReactiveEnergyMinus")]
        [InlineData("/phase2/property/ApparentPowerPlus")]
        [InlineData("/phase2/property/ApparentEnergyPlus")]
        [InlineData("/phase2/property/ApparentPowerMinus")]
        [InlineData("/phase2/property/ApparentEnergyMinus")]
        [InlineData("/phase2/property/PowerFactor")]
        [InlineData("/phase2/property/Current")]
        [InlineData("/phase2/property/Voltage")]

        [InlineData("/phase3/property/ActivePowerPlus")]
        [InlineData("/phase3/property/ActiveEnergyPlus")]
        [InlineData("/phase3/property/ActivePowerMinus")]
        [InlineData("/phase3/property/ActiveEnergyMinus")]
        [InlineData("/phase3/property/ReactivePowerPlus")]
        [InlineData("/phase3/property/ReactiveEnergyPlus")]
        [InlineData("/phase3/property/ReactivePowerMinus")]
        [InlineData("/phase3/property/ReactiveEnergyMinus")]
        [InlineData("/phase3/property/ApparentPowerPlus")]
        [InlineData("/phase3/property/ApparentEnergyPlus")]
        [InlineData("/phase3/property/ApparentPowerMinus")]
        [InlineData("/phase3/property/ApparentEnergyMinus")]
        [InlineData("/phase3/property/PowerFactor")]
        [InlineData("/phase3/property/Current")]
        [InlineData("/phase3/property/Voltage")]
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
        [InlineData("/data/property/ActivePowerPlus")]
        [InlineData("/data/property/ActiveEnergyPlus")]
        [InlineData("/data/property/ActivePowerMinus")]
        [InlineData("/data/property/ActiveEnergyMinus")]
        [InlineData("/data/property/ReactivePowerPlus")]
        [InlineData("/data/property/ReactiveEnergyPlus")]
        [InlineData("/data/property/ReactivePowerMinus")]
        [InlineData("/data/property/ReactiveEnergyMinus")]
        [InlineData("/data/property/ApparentPowerPlus")]
        [InlineData("/data/property/ApparentEnergyPlus")]
        [InlineData("/data/property/ApparentPowerMinus")]
        [InlineData("/data/property/ApparentEnergyMinus")]
        [InlineData("/data/property/PowerFactor")]
        [InlineData("/data/property/SupplyFrequency")]
        [InlineData("/data/property/ActivePowerPlusL1")]
        [InlineData("/data/property/ActiveEnergyPlusL1")]
        [InlineData("/data/property/ActivePowerMinusL1")]
        [InlineData("/data/property/ActiveEnergyMinusL1")]
        [InlineData("/data/property/ReactivePowerPlusL1")]
        [InlineData("/data/property/ReactiveEnergyPlusL1")]
        [InlineData("/data/property/ReactivePowerMinusL1")]
        [InlineData("/data/property/ReactiveEnergyMinusL1")]
        [InlineData("/data/property/ApparentPowerPlusL1")]
        [InlineData("/data/property/ApparentEnergyPlusL1")]
        [InlineData("/data/property/ApparentPowerMinusL1")]
        [InlineData("/data/property/ApparentEnergyMinusL1")]
        [InlineData("/data/property/CurrentL1")]
        [InlineData("/data/property/VoltageL1")]
        [InlineData("/data/property/PowerFactorL1")]
        [InlineData("/data/property/ActivePowerPlusL2")]
        [InlineData("/data/property/ActiveEnergyPlusL2")]
        [InlineData("/data/property/ActivePowerMinusL2")]
        [InlineData("/data/property/ActiveEnergyMinusL2")]
        [InlineData("/data/property/ReactivePowerPlusL2")]
        [InlineData("/data/property/ReactiveEnergyPlusL2")]
        [InlineData("/data/property/ReactivePowerMinusL2")]
        [InlineData("/data/property/ReactiveEnergyMinusL2")]
        [InlineData("/data/property/ApparentPowerPlusL2")]
        [InlineData("/data/property/ApparentEnergyPlusL2")]
        [InlineData("/data/property/ApparentPowerMinusL2")]
        [InlineData("/data/property/ApparentEnergyMinusL2")]
        [InlineData("/data/property/CurrentL2")]
        [InlineData("/data/property/VoltageL2")]
        [InlineData("/data/property/PowerFactorL2")]
        [InlineData("/data/property/ActivePowerPlusL3")]
        [InlineData("/data/property/ActiveEnergyPlusL3")]
        [InlineData("/data/property/ActivePowerMinusL3")]
        [InlineData("/data/property/ActiveEnergyMinusL3")]
        [InlineData("/data/property/ReactivePowerPlusL3")]
        [InlineData("/data/property/ReactiveEnergyPlusL3")]
        [InlineData("/data/property/ReactivePowerMinusL3")]
        [InlineData("/data/property/ReactiveEnergyMinusL3")]
        [InlineData("/data/property/ApparentPowerPlusL3")]
        [InlineData("/data/property/ApparentEnergyPlusL3")]
        [InlineData("/data/property/ApparentPowerMinusL3")]
        [InlineData("/data/property/ApparentEnergyMinusL3")]
        [InlineData("/data/property/CurrentL3")]
        [InlineData("/data/property/VoltageL3")]
        [InlineData("/data/property/PowerFactorL3")]
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
        [InlineData("/total/property/ActivePowerPlus")]
        [InlineData("/total/property/ActiveEnergyPlus")]
        [InlineData("/total/property/ActivePowerMinus")]
        [InlineData("/total/property/ActiveEnergyMinus")]
        [InlineData("/total/property/ReactivePowerPlus")]
        [InlineData("/total/property/ReactiveEnergyPlus")]
        [InlineData("/total/property/ReactivePowerMinus")]
        [InlineData("/total/property/ReactiveEnergyMinus")]
        [InlineData("/total/property/ApparentPowerPlus")]
        [InlineData("/total/property/ApparentEnergyPlus")]
        [InlineData("/total/property/ApparentPowerMinus")]
        [InlineData("/total/property/ApparentEnergyMinus")]
        [InlineData("/total/property/PowerFactor")]
        [InlineData("/total/property/SupplyFrequency")]
        public async Task TestTotalProperty(string url)
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
        [InlineData("/phase1/property/ActivePowerPlus")]
        [InlineData("/phase1/property/ActiveEnergyPlus")]
        [InlineData("/phase1/property/ActivePowerMinus")]
        [InlineData("/phase1/property/ActiveEnergyMinus")]
        [InlineData("/phase1/property/ReactivePowerPlus")]
        [InlineData("/phase1/property/ReactiveEnergyPlus")]
        [InlineData("/phase1/property/ReactivePowerMinus")]
        [InlineData("/phase1/property/ReactiveEnergyMinus")]
        [InlineData("/phase1/property/ApparentPowerPlus")]
        [InlineData("/phase1/property/ApparentEnergyPlus")]
        [InlineData("/phase1/property/ApparentPowerMinus")]
        [InlineData("/phase1/property/ApparentEnergyMinus")]
        [InlineData("/phase1/property/PowerFactor")]
        [InlineData("/phase1/property/Current")]
        [InlineData("/phase1/property/Voltage")]
        public async Task TestPhase1Property(string url)
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
        [InlineData("/phase2/property/ActivePowerPlus")]
        [InlineData("/phase2/property/ActiveEnergyPlus")]
        [InlineData("/phase2/property/ActivePowerMinus")]
        [InlineData("/phase2/property/ActiveEnergyMinus")]
        [InlineData("/phase2/property/ReactivePowerPlus")]
        [InlineData("/phase2/property/ReactiveEnergyPlus")]
        [InlineData("/phase2/property/ReactivePowerMinus")]
        [InlineData("/phase2/property/ReactiveEnergyMinus")]
        [InlineData("/phase2/property/ApparentPowerPlus")]
        [InlineData("/phase2/property/ApparentEnergyPlus")]
        [InlineData("/phase2/property/ApparentPowerMinus")]
        [InlineData("/phase2/property/ApparentEnergyMinus")]
        [InlineData("/phase2/property/PowerFactor")]
        [InlineData("/phase2/property/Current")]
        [InlineData("/phase2/property/Voltage")]
        public async Task TestPhase2Property(string url)
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
        [InlineData("/phase3/property/ActivePowerPlus")]
        [InlineData("/phase3/property/ActiveEnergyPlus")]
        [InlineData("/phase3/property/ActivePowerMinus")]
        [InlineData("/phase3/property/ActiveEnergyMinus")]
        [InlineData("/phase3/property/ReactivePowerPlus")]
        [InlineData("/phase3/property/ReactiveEnergyPlus")]
        [InlineData("/phase3/property/ReactivePowerMinus")]
        [InlineData("/phase3/property/ReactiveEnergyMinus")]
        [InlineData("/phase3/property/ApparentPowerPlus")]
        [InlineData("/phase3/property/ApparentEnergyPlus")]
        [InlineData("/phase3/property/ApparentPowerMinus")]
        [InlineData("/phase3/property/ApparentEnergyMinus")]
        [InlineData("/phase3/property/PowerFactor")]
        [InlineData("/phase3/property/Current")]
        [InlineData("/phase3/property/Voltage")]
        public async Task TestPhase3Property(string url)
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
            var info = JsonSerializer.Deserialize<EM300LRInfo>(json);
            Assert.True(info.Settings.Address.Length > 0);
            Assert.True(info.Settings.Timeout > 0);
            Assert.True(info.Settings.Password.Length > 0);
            Assert.True(info.Settings.SerialNumber.Length > 0);
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
            var data = JsonSerializer.Deserialize<EM300LRData>(json);
            Assert.NotNull(data);
        }

        [Fact]
        public async Task TestTotal()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/total");

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<TotalData>(json);
            Assert.NotNull(data);
        }

        [Fact]
        public async Task TestPhase1()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/phase1");

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<Phase1Data>(json);
            Assert.NotNull(data);
        }

        [Fact]
        public async Task TestPhase2()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/phase2");

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<Phase2Data>(json);
            Assert.NotNull(data);
        }

        [Fact]
        public async Task TestPhase3()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync("/phase3");

            // Assert
            Assert.NotNull(response);
            Assert.True(response.IsSuccessStatusCode);
            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType.ToString());
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<Phase3Data>(json);
            Assert.NotNull(data);
        }

        #endregion Test Methods
    }
}
