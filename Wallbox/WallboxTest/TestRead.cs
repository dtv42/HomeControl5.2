// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestRead.cs" company="DTV-Online">
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
    using System.Threading.Tasks;

    using Xunit;

    using WallboxLib;

    #endregion

    /// <summary>
    /// XUnit testing class.
    /// </summary>
    [Collection("Wallbox Test Collection")]
    public class TestRead : IClassFixture<GatewayFixture>
    {
        #region Private Data Members

        private readonly WallboxGateway _gateway;

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
        public async Task TestWallboxRead()
        {
            await _gateway.ReadAllAsync();
            Assert.True(_gateway.Status.IsGood);
        }

        [Fact]
        public async Task TestReadData()
        {
            var status = await _gateway.ReadReport1Async();
            Assert.True(status.IsGood);
            status = await _gateway.ReadReport2Async();
            Assert.True(status.IsGood);
            status = await _gateway.ReadReport3Async();
            Assert.True(status.IsGood);
            status = await _gateway.ReadReport100Async();
            Assert.True(status.IsGood);
            status = await _gateway.ReadReportsAsync();
            Assert.True(status.IsGood);
            status = await _gateway.ReadInfoAsync();
            Assert.True(status.IsGood);
        }

        #endregion
    }
}
