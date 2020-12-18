// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestRead.cs" company="DTV-Online">
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
    using System.Threading.Tasks;

    using Xunit;

    using FroniusLib;

    #endregion

    /// <summary>
    /// XUnit testing class.
    /// </summary>
    [Collection("Fronius Test Collection")]
    public class TestRead : IClassFixture<GatewayFixture>
    {
        #region Private Data Members

        private readonly FroniusGateway _gateway;

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
        }

        #endregion

        #region Test Methods

        [Fact]
        public async Task TestFroniusRead()
        {
            await _gateway.ReadAllAsync();
            Assert.True(_gateway.Status.IsGood);
        }

        [Fact]
        public async Task TestReadData()
        {
            var status = await _gateway.ReadCommonDataAsync();
            Assert.True(status.IsGood);
            status = await _gateway.ReadInverterInfoAsync();
            Assert.True(status.IsGood);
            status = await _gateway.ReadLoggerInfoAsync();
            Assert.True(status.IsGood);
            status = await _gateway.ReadMinMaxDataAsync();
            Assert.True(status.IsGood);
            status = await _gateway.ReadPhaseDataAsync();
            Assert.True(status.IsGood);
        }

        #endregion
    }
}
