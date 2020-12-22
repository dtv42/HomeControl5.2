// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestData.cs" company="DTV-Online">
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

    using Xunit;

    using UtilityLib;

    using ETAPU11Lib;
    using ETAPU11Lib.Models;

    #endregion

    /// <summary>
    /// XUnit testing class.
    /// </summary>
    [Collection("ETAPU11 Test Collection")]
    public class TestData : IClassFixture<GatewayFixture>
    {
        #region Private Data Members

        private readonly ETAPU11Gateway _gateway;

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
        public void TestDataProperty(string property)
        {
            Assert.True(typeof(ETAPU11Data).IsProperty(property));
            Assert.NotNull(_gateway.Data.GetPropertyValue(property));
            var attribute = ETAPU11Data.GetEtaAttribute(property);
            Assert.NotNull(attribute);
        }

        [Theory]
        [InlineData("FullLoadHours")]
        [InlineData("TotalConsumed")]
        [InlineData("ConsumptionSinceDeAsh")]
        [InlineData("ConsumptionSinceAshBoxEmptied")]
        [InlineData("ConsumptionSinceMaintainence")]
        [InlineData("HopperFillUpPelletBin")]
        [InlineData("HopperPelletBinContents")]
        [InlineData("HopperFillUpTime")]
        [InlineData("BoilerState")]
        [InlineData("BoilerPressure")]
        [InlineData("BoilerTemperature")]
        [InlineData("BoilerTarget")]
        [InlineData("BoilerBottom")]
        [InlineData("FlueGasTemperature")]
        [InlineData("DraughtFanSpeed")]
        [InlineData("ResidualO2")]
        public void TestBoilerDataProperty(string property)
        {
            Assert.True(typeof(BoilerData).IsProperty(property));
            Assert.NotNull(_gateway.Data.GetPropertyValue(property));
        }

        [Theory]
        [InlineData("HotwaterTankState")]
        [InlineData("ChargingTimesState")]
        [InlineData("ChargingTimesSwitchStatus")]
        [InlineData("ChargingTimesTemperature")]
        [InlineData("HotwaterSwitchonDiff")]
        [InlineData("HotwaterTarget")]
        [InlineData("HotwaterTemperature")]
        public void TestHotwaterDataProperty(string property)
        {
            Assert.True(typeof(HotwaterData).IsProperty(property));
            Assert.NotNull(_gateway.HotwaterData.GetPropertyValue(property));
        }

        [Theory]
        [InlineData("RoomSensor")]
        [InlineData("HeatingCircuitState")]
        [InlineData("RunningState")]
        [InlineData("HeatingTimes")]
        [InlineData("HeatingSwitchStatus")]
        [InlineData("HeatingTemperature")]
        [InlineData("RoomTemperature")]
        [InlineData("RoomTarget")]
        [InlineData("Flow")]
        [InlineData("DayHeatingThreshold")]
        [InlineData("NightHeatingThreshold")]
        public void TestHeatingDataProperty(string property)
        {
            Assert.True(typeof(HeatingData).IsProperty(property));
            Assert.NotNull(_gateway.HeatingData.GetPropertyValue(property));
        }

        [Theory]
        [InlineData("DischargeScrewDemand")]
        [InlineData("DischargeScrewState")]
        [InlineData("DischargeScrewMotorCurr")]
        [InlineData("ConveyingSystem")]
        [InlineData("Stock")]
        [InlineData("StockWarningLimit")]
        public void TestStorageDataProperty(string property)
        {
            Assert.True(typeof(StorageData).IsProperty(property));
            Assert.NotNull(_gateway.StorageData.GetPropertyValue(property));
        }

        [Theory]
        [InlineData("OutsideTemperature")]
        public void TestSystemDataProperty(string property)
        {
            Assert.True(typeof(SystemData).IsProperty(property));
            Assert.NotNull(_gateway.SystemData.GetPropertyValue(property));
        }

        #endregion
    }
}
