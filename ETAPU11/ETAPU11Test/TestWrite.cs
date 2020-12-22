// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestWrite.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
// Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace ETAPU11Test
{
    #region Using Directives

    using System.Globalization;
    using System.Threading.Tasks;

    using Xunit;

    using ETAPU11Lib;
    using ETAPU11Lib.Models;

    #endregion

    /// <summary>
    /// XUnit testing class.
    /// </summary>
    [Collection("ETAPU11 Test Collection")]
    public class TestWrite : IClassFixture<GatewayFixture>
    {
        #region Private Data Members

        private readonly ETAPU11Gateway _gateway;

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="TestWrite"/> class.
        /// </summary>
        public TestWrite(GatewayFixture fixture)
        {
            // Set the default culture.
            CultureInfo.CurrentCulture = new CultureInfo("en-US");

            _gateway = fixture.Gateway;
            _gateway.Settings.TcpSlave.Address = "127.0.0.1";
        }

        [Theory]
        [InlineData("AshRemovalStartIdleTime", "21:00:00")]
        [InlineData("AshRemovalDurationIdleTime", "10:00:00")]
        [InlineData("HeatingHolidayStart", "2018-07-26T00:00:00")]
        [InlineData("HeatingHolidayEnd", "2018-08-02T23:59:00")]
        [InlineData("EmptyAshBoxAfter", "1000.0")]
        [InlineData("HopperPelletBinContents", "30.0")]
        [InlineData("HopperFillUpTime", "19:00:00")]
        [InlineData("HotwaterSwitchonDiff", "15.0")]
        [InlineData("HeatingTemperature", "20.0")]
        [InlineData("FlowAtMinus10", "55.0")]
        [InlineData("FlowAtPlus10", "35.0")]
        [InlineData("FlowSetBack", "15.0")]
        [InlineData("DayHeatingThreshold", "16.0")]
        [InlineData("NightHeatingThreshold", "2.0")]
        [InlineData("Stock", "2000.0")]
        [InlineData("StockWarningLimit", "800.0")]
        [InlineData("OutsideTemperature", "22.0")]
        [InlineData("HopperFillUpPelletBin", "Yes")]
        [InlineData("OnOffButton", "On")]
        [InlineData("DeAshButton", "On")]
        [InlineData("ChargeButton", "On")]
        [InlineData("HeatingDayButton", "On")]
        [InlineData("HeatingAutoButton", "On")]
        [InlineData("HeatingNightButton", "On")]
        [InlineData("HeatingOnOffButton", "On")]
        [InlineData("HeatingHomeButton", "On")]
        [InlineData("HeatingAwayButton", "On")]
        public async Task TestETAPU11WriteProperty(string property, string data)
        {
            Assert.True(ETAPU11Data.IsProperty(property));
            Assert.True(ETAPU11Data.IsWritable(property));
            var status = await _gateway.WritePropertyAsync(property, data);
            Assert.True(status.IsGood);
        }
    }
}
