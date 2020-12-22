// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestRead.cs" company="DTV-Online">
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
    public class TestRead : IClassFixture<GatewayFixture>
    {
        #region Private Data Members

        private readonly ETAPU11Gateway _gateway;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TestRead"/> class.
        /// </summary>
        public TestRead(GatewayFixture fixture)
        {
            // Set the default culture.
            CultureInfo.CurrentCulture = new CultureInfo("en-US");

            _gateway = fixture.Gateway;
            _gateway.Settings.TcpSlave.Address = "10.0.1.4";
        }

        #endregion

        #region Test Methods

        [Fact]
        public async Task TestETAPU11Read()
        {
            await _gateway.ReadAllAsync();
            Assert.True(_gateway.Status.IsGood);
        }

        [Fact]
        public async Task TestETAPU11BlockRead()
        {
            var status = await _gateway.ReadBlockAllAsync();
            Assert.True(status.IsGood);
            Assert.True(_gateway.Status.IsGood);
        }

        [Fact]
        public async Task TestReadData()
        {
            var status = await _gateway.ReadBoilerDataAsync();
            Assert.True(status.IsGood);
            status = await _gateway.ReadHotwaterDataAsync();
            Assert.True(status.IsGood);
            status = await _gateway.ReadHeatingDataAsync();
            Assert.True(status.IsGood);
            status = await _gateway.ReadStorageDataAsync();
            Assert.True(status.IsGood);
            status = await _gateway.ReadSystemDataAsync();
            Assert.True(status.IsGood);
        }

        [Theory]
        [InlineData("FullLoadHours")]
        [InlineData("TotalConsumed")]
        [InlineData("BoilerState")]
        [InlineData("BoilerPressure")]
        [InlineData("BoilerTemperature")]
        [InlineData("BoilerTarget")]
        [InlineData("BoilerBottom")]
        [InlineData("FlowControlState")]
        [InlineData("DiverterValveState")]
        [InlineData("DiverterValveDemand")]
        [InlineData("DemandedOutput")]
        [InlineData("FlowMixValveTarget")]
        [InlineData("FlowMixValveState")]
        [InlineData("FlowMixValveCurrTemp")]
        [InlineData("FlowMixValvePosition")]
        [InlineData("BoilerPumpOutput")]
        [InlineData("BoilerPumpDemand")]
        [InlineData("FlueGasTemperature")]
        [InlineData("DraughtFanSpeed")]
        [InlineData("ResidualO2")]
        [InlineData("StokerScrewDemand")]
        [InlineData("StokerScrewClockRate")]
        [InlineData("StokerScrewState")]
        [InlineData("StokerScrewMotorCurr")]
        [InlineData("AshRemovalState")]
        [InlineData("AshRemovalStartIdleTime")]
        [InlineData("AshRemovalDurationIdleTime")]
        [InlineData("ConsumptionSinceDeAsh")]
        [InlineData("ConsumptionSinceAshBoxEmptied")]
        [InlineData("EmptyAshBoxAfter")]
        [InlineData("ConsumptionSinceMaintainence")]
        [InlineData("HopperState")]
        [InlineData("HopperFillUpPelletBin")]
        [InlineData("HopperPelletBinContents")]
        [InlineData("HopperFillUpTime")]
        [InlineData("HopperVacuumState")]
        [InlineData("HopperVacuumDemand")]
        [InlineData("OnOffButton")]
        [InlineData("DeAshButton")]
        [InlineData("HotwaterTankState")]
        [InlineData("ChargingTimesState")]
        [InlineData("ChargingTimesSwitchStatus")]
        [InlineData("ChargingTimesTemperature")]
        [InlineData("HotwaterSwitchonDiff")]
        [InlineData("HotwaterTarget")]
        [InlineData("HotwaterTemperature")]
        [InlineData("ChargeButton")]
        [InlineData("RoomSensor")]
        [InlineData("HeatingCircuitState")]
        [InlineData("RunningState")]
        [InlineData("HeatingTimes")]
        [InlineData("HeatingSwitchStatus")]
        [InlineData("HeatingTemperature")]
        [InlineData("RoomTemperature")]
        [InlineData("RoomTarget")]
        [InlineData("Flow")]
        [InlineData("HeatingCurve")]
        [InlineData("FlowAtMinus10")]
        [InlineData("FlowAtPlus10")]
        [InlineData("FlowSetBack")]
        [InlineData("OutsideTemperatureDelayed")]
        [InlineData("DayHeatingThreshold")]
        [InlineData("NightHeatingThreshold")]
        [InlineData("HeatingDayButton")]
        [InlineData("HeatingAutoButton")]
        [InlineData("HeatingNightButton")]
        [InlineData("HeatingOnOffButton")]
        [InlineData("HeatingHomeButton")]
        [InlineData("HeatingAwayButton")]
        [InlineData("HeatingHolidayStart")]
        [InlineData("HeatingHolidayEnd")]
        [InlineData("DischargeScrewDemand")]
        [InlineData("DischargeScrewClockRate")]
        [InlineData("DischargeScrewState")]
        [InlineData("DischargeScrewMotorCurr")]
        [InlineData("ConveyingSystem")]
        [InlineData("Stock")]
        [InlineData("StockWarningLimit")]
        [InlineData("OutsideTemperature")]
        [InlineData("FirebedState")]
        [InlineData("SupplyDemand")]
        [InlineData("IgnitionDemand")]
        [InlineData("FlowMixValveTemperature")]
        [InlineData("AirValveSetPosition")]
        [InlineData("AirValveCurrPosition")]
        public async Task TestETAPU11ReadProperty(string property)
        {
            Assert.True(ETAPU11Data.IsProperty(property));
            Assert.True(ETAPU11Data.IsReadable(property));
            var status = await _gateway.ReadPropertyAsync(property);
            Assert.True(status.IsGood);
        }

        #endregion
    }
}
