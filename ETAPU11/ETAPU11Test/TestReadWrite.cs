// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestReadWrite.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
// Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace ETAPU11Test
{
    #region Using Directives

    using System;
    using System.Globalization;
    using System.Threading.Tasks;

    using Xunit;

    using UtilityLib;
    using ETAPU11Lib;
    using ETAPU11Lib.Models;

    #endregion

    /// <summary>
    /// XUnit testing class.
    /// </summary>
    [Collection("ETAPU11 Test Collection")]
    public class TestReadWrite : IClassFixture<GatewayFixture>
    {
        #region Private Data Members

        private readonly ETAPU11Gateway _gateway;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="TestReadWrite"/> class.
        /// </summary>
        public TestReadWrite(GatewayFixture fixture)
        {
            // Set the default culture.
            CultureInfo.CurrentCulture = new CultureInfo("en-US");

            _gateway = fixture.Gateway;
            _gateway.Settings.TcpSlave.Address = "127.0.0.1";
        }

        [Theory]
        [InlineData("HopperFillUpTime", "19:00:00")]
        [InlineData("AshRemovalStartIdleTime", "21:00:00")]
        [InlineData("AshRemovalDurationIdleTime", "10:00:00")]
        public async Task TestETAPU11ReadWriteTimeSpan(string property, string data)
        {
            var status = await _gateway.WritePropertyAsync(property, data);
            Assert.True(status.IsGood);
            status = await _gateway.ReadPropertyAsync(property);
            Assert.True(status.IsGood);
            Assert.Equal(data, ((TimeSpan)_gateway.Data.GetPropertyValue(property)).ToString());
        }

        [Theory]
        [InlineData("HeatingHolidayStart", "2018-07-26T00:00:00")]
        [InlineData("HeatingHolidayEnd", "2018-08-02T23:59:00")]
        public async Task TestETAPU11ReadWriteDateTime(string property, string data)
        {
            var status = await _gateway.WritePropertyAsync(property, data);
            Assert.True(status.IsGood);
            status = await _gateway.ReadPropertyAsync(property);
            Assert.True(status.IsGood);
            Assert.Equal(data, ((DateTimeOffset)_gateway.Data.GetPropertyValue(property)).ToLocalTime().ToString("yyyy-MM-ddTHH:mm:ss"));
        }

        [Theory]
        [InlineData("EmptyAshBoxAfter", 1000.0)]
        [InlineData("HopperPelletBinContents", 30.0)]
        [InlineData("HotwaterSwitchonDiff", 15.0)]
        [InlineData("HeatingTemperature", 20.0)]
        [InlineData("FlowAtMinus10", 55.0)]
        [InlineData("FlowAtPlus10", 35.0)]
        [InlineData("FlowSetBack", 15.0)]
        [InlineData("DayHeatingThreshold", 16.0)]
        [InlineData("NightHeatingThreshold", 2.0)]
        [InlineData("Stock", 2000.0)]
        [InlineData("StockWarningLimit", 800.0)]
        [InlineData("OutsideTemperature", 22.0)]
        public async Task TestETAPU11ReadWriteDouble(string property, double data)
        {
            var status = await _gateway.WritePropertyAsync(property, data.ToString());
            Assert.True(status.IsGood);
            status = await _gateway.ReadPropertyAsync(property);
            Assert.True(status.IsGood);
            Assert.Equal(data, (double)_gateway.Data.GetPropertyValue(property));
        }

        [Theory]
        [InlineData("HopperFillUpPelletBin", ETAPU11Data.StartValues.Yes)]
        [InlineData("OnOffButton", ETAPU11Data.OnOffStates.On)]
        [InlineData("DeAshButton", ETAPU11Data.OnOffStates.On)]
        [InlineData("ChargeButton", ETAPU11Data.OnOffStates.On)]
        [InlineData("HeatingDayButton", ETAPU11Data.OnOffStates.On)]
        [InlineData("HeatingAutoButton", ETAPU11Data.OnOffStates.On)]
        [InlineData("HeatingNightButton", ETAPU11Data.OnOffStates.On)]
        [InlineData("HeatingOnOffButton", ETAPU11Data.OnOffStates.On)]
        [InlineData("HeatingHomeButton", ETAPU11Data.OnOffStates.On)]
        [InlineData("HeatingAwayButton", ETAPU11Data.OnOffStates.On)]
        public async Task TestETAPU11ReadWriteEnum(string property, dynamic data)
        {
            var status = await _gateway.WritePropertyAsync(property, data.ToString());
            Assert.True(status.IsGood);
            status = await _gateway.ReadPropertyAsync(property);
            Assert.True(status.IsGood);
            Assert.Equal((int)data, (int)_gateway.Data.GetPropertyValue(property));
        }
    }
}
