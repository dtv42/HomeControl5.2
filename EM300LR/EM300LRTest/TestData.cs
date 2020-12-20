// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestEM300LRData.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>22-4-2020 16:30</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace EM300LRTest
{
    #region Using Directives

    using Xunit;

    using EM300LRLib;
    using EM300LRLib.Models;

    #endregion Using Directives

    /// <summary>
    /// XUnit testing class.
    /// </summary>
    [Collection("EM300LR Test Collection")]
    public class TestData : IClassFixture<GatewayFixture>
    {
        #region Private Data Members

        private readonly EM300LRGateway _gateway;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TestData"/> class.
        /// </summary>
        /// <param name="outputHelper"></param>
        public TestData(GatewayFixture fixture)
        {
            _gateway = fixture.Gateway;
        }

        #endregion Constructors

        #region Test Methods

        [Theory]
        [InlineData("ActivePowerPlus")]
        [InlineData("ActiveEnergyPlus")]
        [InlineData("ActivePowerMinus")]
        [InlineData("ActiveEnergyMinus")]
        [InlineData("ReactivePowerPlus")]
        [InlineData("ReactiveEnergyPlus")]
        [InlineData("ReactivePowerMinus")]
        [InlineData("ReactiveEnergyMinus")]
        [InlineData("ApparentPowerPlus")]
        [InlineData("ApparentEnergyPlus")]
        [InlineData("ApparentPowerMinus")]
        [InlineData("ApparentEnergyMinus")]
        [InlineData("PowerFactor")]
        [InlineData("SupplyFrequency")]
        [InlineData("ActivePowerPlusL1")]
        [InlineData("ActiveEnergyPlusL1")]
        [InlineData("ActivePowerMinusL1")]
        [InlineData("ActiveEnergyMinusL1")]
        [InlineData("ReactivePowerPlusL1")]
        [InlineData("ReactiveEnergyPlusL1")]
        [InlineData("ReactivePowerMinusL1")]
        [InlineData("ReactiveEnergyMinusL1")]
        [InlineData("ApparentPowerPlusL1")]
        [InlineData("ApparentEnergyPlusL1")]
        [InlineData("ApparentPowerMinusL1")]
        [InlineData("ApparentEnergyMinusL1")]
        [InlineData("CurrentL1")]
        [InlineData("VoltageL1")]
        [InlineData("PowerFactorL1")]
        [InlineData("ActivePowerPlusL2")]
        [InlineData("ActiveEnergyPlusL2")]
        [InlineData("ActivePowerMinusL2")]
        [InlineData("ActiveEnergyMinusL2")]
        [InlineData("ReactivePowerPlusL2")]
        [InlineData("ReactiveEnergyPlusL2")]
        [InlineData("ReactivePowerMinusL2")]
        [InlineData("ReactiveEnergyMinusL2")]
        [InlineData("ApparentPowerPlusL2")]
        [InlineData("ApparentEnergyPlusL2")]
        [InlineData("ApparentPowerMinusL2")]
        [InlineData("ApparentEnergyMinusL2")]
        [InlineData("CurrentL2")]
        [InlineData("VoltageL2")]
        [InlineData("PowerFactorL2")]
        [InlineData("ActivePowerPlusL3")]
        [InlineData("ActiveEnergyPlusL3")]
        [InlineData("ActivePowerMinusL3")]
        [InlineData("ActiveEnergyMinusL3")]
        [InlineData("ReactivePowerPlusL3")]
        [InlineData("ReactiveEnergyPlusL3")]
        [InlineData("ReactivePowerMinusL3")]
        [InlineData("ReactiveEnergyMinusL3")]
        [InlineData("ApparentPowerPlusL3")]
        [InlineData("ApparentEnergyPlusL3")]
        [InlineData("ApparentPowerMinusL3")]
        [InlineData("ApparentEnergyMinusL3")]
        [InlineData("CurrentL3")]
        [InlineData("VoltageL3")]
        [InlineData("PowerFactorL3")]
        public void TestDataProperty(string property)
        {
            Assert.NotNull(typeof(EM300LRData).GetProperty(property));
            Assert.NotNull(typeof(EM300LRData).GetProperty(property).GetValue(_gateway.Data));
        }

        [Theory]
        [InlineData("ActivePowerPlus")]
        [InlineData("ActiveEnergyPlus")]
        [InlineData("ActivePowerMinus")]
        [InlineData("ActiveEnergyMinus")]
        [InlineData("ReactivePowerPlus")]
        [InlineData("ReactiveEnergyPlus")]
        [InlineData("ReactivePowerMinus")]
        [InlineData("ReactiveEnergyMinus")]
        [InlineData("ApparentPowerPlus")]
        [InlineData("ApparentEnergyPlus")]
        [InlineData("ApparentPowerMinus")]
        [InlineData("ApparentEnergyMinus")]
        [InlineData("PowerFactor")]
        [InlineData("SupplyFrequency")]
        public void TestTotalProperty(string property)
        {
            Assert.NotNull(typeof(TotalData).GetProperty(property));
            Assert.NotNull(typeof(TotalData).GetProperty(property).GetValue(_gateway.TotalData));
        }

        [Theory]
        [InlineData("ActivePowerPlus")]
        [InlineData("ActiveEnergyPlus")]
        [InlineData("ActivePowerMinus")]
        [InlineData("ActiveEnergyMinus")]
        [InlineData("ReactivePowerPlus")]
        [InlineData("ReactiveEnergyPlus")]
        [InlineData("ReactivePowerMinus")]
        [InlineData("ReactiveEnergyMinus")]
        [InlineData("ApparentPowerPlus")]
        [InlineData("ApparentEnergyPlus")]
        [InlineData("ApparentPowerMinus")]
        [InlineData("ApparentEnergyMinus")]
        [InlineData("Current")]
        [InlineData("Voltage")]
        [InlineData("PowerFactor")]
        public void TestPhase1Property(string property)
        {
            Assert.NotNull(typeof(Phase1Data).GetProperty(property));
            Assert.NotNull(typeof(Phase1Data).GetProperty(property).GetValue(_gateway.Phase1Data));
        }

        [Theory]
        [InlineData("ActivePowerPlus")]
        [InlineData("ActiveEnergyPlus")]
        [InlineData("ActivePowerMinus")]
        [InlineData("ActiveEnergyMinus")]
        [InlineData("ReactivePowerPlus")]
        [InlineData("ReactiveEnergyPlus")]
        [InlineData("ReactivePowerMinus")]
        [InlineData("ReactiveEnergyMinus")]
        [InlineData("ApparentPowerPlus")]
        [InlineData("ApparentEnergyPlus")]
        [InlineData("ApparentPowerMinus")]
        [InlineData("ApparentEnergyMinus")]
        [InlineData("Current")]
        [InlineData("Voltage")]
        [InlineData("PowerFactor")]
        public void TestPhase2Property(string property)
        {
            Assert.NotNull(typeof(Phase2Data).GetProperty(property));
            Assert.NotNull(typeof(Phase2Data).GetProperty(property).GetValue(_gateway.Phase2Data));
        }

        [Theory]
        [InlineData("ActivePowerPlus")]
        [InlineData("ActiveEnergyPlus")]
        [InlineData("ActivePowerMinus")]
        [InlineData("ActiveEnergyMinus")]
        [InlineData("ReactivePowerPlus")]
        [InlineData("ReactiveEnergyPlus")]
        [InlineData("ReactivePowerMinus")]
        [InlineData("ReactiveEnergyMinus")]
        [InlineData("ApparentPowerPlus")]
        [InlineData("ApparentEnergyPlus")]
        [InlineData("ApparentPowerMinus")]
        [InlineData("ApparentEnergyMinus")]
        [InlineData("Current")]
        [InlineData("Voltage")]
        [InlineData("PowerFactor")]
        public void TestPhase3Property(string property)
        {
            Assert.NotNull(typeof(Phase3Data).GetProperty(property));
            Assert.NotNull(typeof(Phase3Data).GetProperty(property).GetValue(_gateway.Phase3Data));
        }

        #endregion Test Methods
    }
}
