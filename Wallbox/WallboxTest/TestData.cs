// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestData.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>26-4-2020 20:21</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace WallboxTest
{
    #region Using Directives

    using System.Globalization;

    using Xunit;

    using UtilityLib;

    using WallboxLib;
    using WallboxLib.Models;

    #endregion

    /// <summary>
    /// XUnit testing class.
    /// </summary>
    [Collection("Wallbox Test Collection")]
    public class TestData : IClassFixture<GatewayFixture>
    {
        #region Private Data Members

        private readonly WallboxGateway _gateway;

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

        [Fact]
        public void Test()
        {
            Assert.True(_gateway.IsStartupOk);
            Assert.True(_gateway.Status.IsGood);
        }

        [Theory]
        [InlineData("Settings")]
        [InlineData("Data")]
        [InlineData("Report1")]
        [InlineData("Report2")]
        [InlineData("Report3")]
        [InlineData("Report100")]
        [InlineData("Reports")]
        [InlineData("Info")]
        public void TestGatewayProperty(string property)
        {
            Assert.True(typeof(WallboxGateway).IsProperty(property));
            Assert.NotNull(_gateway.GetPropertyValue(property));
        }

        [Theory]
        [InlineData("Report1")]
        [InlineData("Report2")]
        [InlineData("Report3")]
        [InlineData("Report100")]
        [InlineData("Reports")]
        public void TestDataProperty(string property)
        {
            Assert.True(typeof(WallboxData).IsProperty(property));
            Assert.NotNull(_gateway.Data.GetPropertyValue(property));
        }

        [Theory]
        [InlineData("ID")]
        [InlineData("Product")]
        [InlineData("Serial")]
        [InlineData("Firmware")]
        [InlineData("ComModule")]
        [InlineData("Backend")]
        [InlineData("TimeQ")]
        [InlineData("DipSW1")]
        [InlineData("DipSW2")]
        [InlineData("Sec")]
        public void TestDataReport1Property(string property)
        {
            Assert.True(typeof(Report1Udp).IsProperty(property));
            Assert.NotNull(_gateway.Data.Report1.GetPropertyValue(property));
        }

        [Theory]
        [InlineData("ID")]
        [InlineData("State")]
        [InlineData("Error1")]
        [InlineData("Error2")]
        [InlineData("Plug")]
        [InlineData("AuthON")]
        [InlineData("AuthReq")]
        [InlineData("EnableSys")]
        [InlineData("EnableUser")]
        [InlineData("MaxCurr")]
        [InlineData("MaxCurrPercent")]
        [InlineData("CurrHW")]
        [InlineData("CurrUser")]
        [InlineData("CurrFS")]
        [InlineData("TmoFS")]
        [InlineData("CurrTimer")]
        [InlineData("TmoCT")]
        [InlineData("Setenergy")]
        [InlineData("Output")]
        [InlineData("Input")]
        [InlineData("Serial")]
        [InlineData("Sec")]
        public void TestDataReport2Property(string property)
        {
            Assert.True(typeof(Report2Udp).IsProperty(property));
            Assert.NotNull(_gateway.Data.Report2.GetPropertyValue(property));
        }

        [Theory]
        [InlineData("ID")]
        [InlineData("U1")]
        [InlineData("U2")]
        [InlineData("U3")]
        [InlineData("I1")]
        [InlineData("I2")]
        [InlineData("I3")]
        [InlineData("P")]
        [InlineData("PF")]
        [InlineData("Epres")]
        [InlineData("Etotal")]
        [InlineData("Serial")]
        [InlineData("Sec")]
        public void TestDataReport3Property(string property)
        {
            Assert.True(typeof(Report3Udp).IsProperty(property));
            Assert.NotNull(_gateway.Data.Report3.GetPropertyValue(property));
        }

        [Theory]
        [InlineData("ID")]
        [InlineData("SessionID")]
        [InlineData("CurrHW")]
        [InlineData("Estart")]
        [InlineData("Epres")]
        [InlineData("StartedSec")]
        [InlineData("EndedSec")]
        [InlineData("Started")]
        [InlineData("Ended")]
        [InlineData("Reason")]
        [InlineData("TimeQ")]
        [InlineData("RFIDclass")]
        [InlineData("Serial")]
        [InlineData("Sec")]
        public void TestDataReport100Property(string property)
        {
            Assert.True(typeof(ReportsUdp).IsProperty(property));
            Assert.NotNull(_gateway.Data.Report100.GetPropertyValue(property));
        }

        [Theory]
        [InlineData("ID")]
        [InlineData("Product")]
        [InlineData("Serial")]
        [InlineData("Firmware")]
        [InlineData("ComModule")]
        [InlineData("Backend")]
        [InlineData("TimeQ")]
        [InlineData("DIPSwitch1")]
        [InlineData("DIPSwitch2")]
        [InlineData("Seconds")]
        public void TestReport1Property(string property)
        {
            Assert.True(typeof(Report1Data).IsProperty(property));
            Assert.NotNull(_gateway.Report1.GetPropertyValue(property));
        }

        [Theory]
        [InlineData("ID")]
        [InlineData("State")]
        [InlineData("Error1")]
        [InlineData("Error2")]
        [InlineData("Plug")]
        [InlineData("AuthON")]
        [InlineData("AuthRequired")]
        [InlineData("EnableSystem")]
        [InlineData("EnableUser")]
        [InlineData("MaxCurrent")]
        [InlineData("DutyCycle")]
        [InlineData("CurrentHW")]
        [InlineData("CurrentUser")]
        [InlineData("CurrentFS")]
        [InlineData("TimeoutFS")]
        [InlineData("CurrentTimer")]
        [InlineData("TimeoutCT")]
        [InlineData("SetEnergy")]
        [InlineData("Output")]
        [InlineData("Input")]
        [InlineData("Serial")]
        [InlineData("Seconds")]
        public void TestReport2Property(string property)
        {
            Assert.True(typeof(Report2Data).IsProperty(property));
            Assert.NotNull(_gateway.Report2.GetPropertyValue(property));
        }

        [Theory]
        [InlineData("ID")]
        [InlineData("VoltageL1N")]
        [InlineData("VoltageL2N")]
        [InlineData("VoltageL3N")]
        [InlineData("CurrentL1")]
        [InlineData("CurrentL2")]
        [InlineData("CurrentL3")]
        [InlineData("Power")]
        [InlineData("PowerFactor")]
        [InlineData("EnergyCharging")]
        [InlineData("EnergyTotal")]
        [InlineData("Serial")]
        [InlineData("Seconds")]
        public void TestReport3Property(string property)
        {
            Assert.True(typeof(Report3Data).IsProperty(property));
            Assert.NotNull(_gateway.Report3.GetPropertyValue(property));
        }

        [Theory]
        [InlineData("ID")]
        [InlineData("SessionID")]
        [InlineData("CurrentHW")]
        [InlineData("EnergyConsumption")]
        [InlineData("EnergyTransferred")]
        [InlineData("StartedSeconds")]
        [InlineData("EndedSeconds")]
        [InlineData("Started")]
        [InlineData("Ended")]
        [InlineData("Reason")]
        [InlineData("TimeQ")]
        [InlineData("RFID")]
        [InlineData("Serial")]
        [InlineData("Seconds")]
        public void TestReport100Property(string property)
        {
            Assert.True(typeof(ReportsData).IsProperty(property));
            Assert.NotNull(_gateway.Report100.GetPropertyValue(property));
        }

        [Theory]
        [InlineData("ID")]
        [InlineData("SessionID")]
        [InlineData("CurrentHW")]
        [InlineData("EnergyConsumption")]
        [InlineData("EnergyTransferred")]
        [InlineData("StartedSeconds")]
        [InlineData("EndedSeconds")]
        [InlineData("Started")]
        [InlineData("Ended")]
        [InlineData("Reason")]
        [InlineData("TimeQ")]
        [InlineData("RFID")]
        [InlineData("Serial")]
        [InlineData("Seconds")]
        public void TestReportsProperty(string property)
        {
            Assert.True(typeof(ReportsData).IsProperty(property));

            foreach (var report in _gateway.Reports)
            {
                Assert.NotNull(report.GetPropertyValue(property));
            }
        }

        [Theory]
        [InlineData("Firmware")]
        public void TestInfoProperty(string property)
        {
            Assert.True(typeof(InfoData).IsProperty(property));
            Assert.NotNull(_gateway.Info.GetPropertyValue(property));
        }

        #endregion
    }
}
