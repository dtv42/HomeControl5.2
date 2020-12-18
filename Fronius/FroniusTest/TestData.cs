// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestData.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
// Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace FroniusTest
{
    #region Using Directives

    using System.Globalization;

    using Xunit;

    using UtilityLib;
    using FroniusLib;
    using FroniusLib.Models;

    #endregion

    /// <summary>
    /// XUnit testing class.
    /// </summary>
    [Collection("Fronius Test Collection")]
    public class TestData : IClassFixture<GatewayFixture>
    {
        #region Private Data Members

        private readonly FroniusGateway _gateway;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TestData"/> class.
        /// </summary>
        public TestData(GatewayFixture fixture)
        {
            // Set the default culture.
            CultureInfo.CurrentCulture = new CultureInfo("en-US");

            _gateway = fixture.Gateway;
        }

        #endregion

        #region Test Methods

        [Theory]
        [InlineData("CommonData")]
        [InlineData("InverterInfo")]
        [InlineData("LoggerInfo")]
        [InlineData("PhaseData")]
        [InlineData("MinMaxData")]
        [InlineData("VersionInfo")]
        public void TestProperty(string property)
        {
            Assert.True(typeof(FroniusGateway).IsProperty(property));
            Assert.NotNull(_gateway.GetPropertyValue(property));
        }

        [Theory]
        [InlineData("Frequency")]
        [InlineData("CurrentDC")]
        [InlineData("CurrentAC")]
        [InlineData("VoltageDC")]
        [InlineData("VoltageAC")]
        [InlineData("PowerAC")]
        [InlineData("DailyEnergy")]
        [InlineData("YearlyEnergy")]
        [InlineData("TotalEnergy")]
        [InlineData("StatusCode")]
        public void TestCommonDataProperty(string property)
        {
            Assert.True(typeof(CommonData).IsProperty(property));
            Assert.NotNull(_gateway.CommonData.GetPropertyValue(property));
        }

        [Theory]
        [InlineData("Index")]
        [InlineData("DeviceType")]
        [InlineData("PVPower")]
        [InlineData("CustomName")]
        [InlineData("Show")]
        [InlineData("UniqueID")]
        [InlineData("ErrorCode")]
        [InlineData("StatusCode")]
        public void TestInverterInfoProperty(string property)
        {
            Assert.True(typeof(InverterInfo).IsProperty(property));
            Assert.NotNull(_gateway.InverterInfo.GetPropertyValue(property));
        }

        [Theory]
        [InlineData("UniqueID")]
        [InlineData("ProductID")]
        [InlineData("PlatformID")]
        [InlineData("HWVersion")]
        [InlineData("SWVersion")]
        [InlineData("TimezoneLocation")]
        [InlineData("TimezoneName")]
        [InlineData("UTCOffset")]
        [InlineData("DefaultLanguage")]
        [InlineData("CashFactor")]
        [InlineData("CashCurrency")]
        [InlineData("CO2Factor")]
        [InlineData("CO2Unit")]
        public void TestLoggerDataProperty(string property)
        {
            Assert.True(typeof(LoggerInfo).IsProperty(property));
            Assert.NotNull(_gateway.LoggerInfo.GetPropertyValue(property));
        }

        [Theory]
        [InlineData("DailyMaxVoltageDC")]
        [InlineData("DailyMaxVoltageAC")]
        [InlineData("DailyMinVoltageAC")]
        [InlineData("YearlyMaxVoltageDC")]
        [InlineData("YearlyMaxVoltageAC")]
        [InlineData("YearlyMinVoltageAC")]
        [InlineData("TotalMaxVoltageDC")]
        [InlineData("TotalMaxVoltageAC")]
        [InlineData("TotalMinVoltageAC")]
        [InlineData("DailyMaxPower")]
        [InlineData("YearlyMaxPower")]
        [InlineData("TotalMaxPower")]
        public void TestMinMaxDataProperty(string property)
        {
            Assert.True(typeof(MinMaxData).IsProperty(property));
            Assert.NotNull(_gateway.MinMaxData.GetPropertyValue(property));
        }

        [Theory]
        [InlineData("CurrentL1")]
        [InlineData("CurrentL2")]
        [InlineData("CurrentL3")]
        [InlineData("VoltageL1N")]
        [InlineData("VoltageL2N")]
        [InlineData("VoltageL3N")]
        public void TestPhaseDataProperty(string property)
        {
            Assert.True(typeof(PhaseData).IsProperty(property));
            Assert.NotNull(_gateway.PhaseData.GetPropertyValue(property));
        }

        [Theory]
        [InlineData("APIVersion")]
        [InlineData("BaseURL")]
        [InlineData("CompatibilityRange")]
        public void TestVersionProperty(string property)
        {
            Assert.True(typeof(APIVersionData).IsProperty(property));
            Assert.NotNull(_gateway.VersionInfo.GetPropertyValue(property));
        }

        #endregion
    }
}
