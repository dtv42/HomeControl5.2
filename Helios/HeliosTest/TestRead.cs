// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestRead.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
// Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace HeliosTest
{
    #region Using Directives

    using System.Globalization;
    using System.Threading.Tasks;

    using Xunit;

    using HeliosLib;

    #endregion

    /// <summary>
    /// XUnit testing class.
    /// </summary>
    [Collection("Helios Test Collection")]
    public class TestRead : IClassFixture<GatewayFixture>
    {
        #region Private Data Members

        private readonly HeliosGateway _gateway;

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
        public async Task TestHeliosRead()
        {
            await _gateway.ReadAllAsync();
            Assert.True(_gateway.Status.IsGood);
        }

        [Fact]
        public async Task TestReadBoosterData()
        {
            var status = await _gateway.ReadBoosterDataAsync();
            Assert.True(status.IsGood);
        }

        [Fact]
        public async Task TestReadDeviceData()
        {
            var status = await _gateway.ReadDeviceDataAsync();
            Assert.True(status.IsGood);
        }

        [Fact]
        public async Task TestReadDisplayData()
        {
            var status = await _gateway.ReadDisplayDataAsync();
            Assert.True(status.IsGood);
        }

        [Fact]
        public async Task TestReadErrorData()
        {
            var status = await _gateway.ReadErrorDataAsync();
            Assert.True(status.IsGood);
        }

        [Fact]
        public async Task TestReadFanData()
        {
            var status = await _gateway.ReadFanDataAsync();
            Assert.True(status.IsGood);
        }

        [Fact]
        public async Task TestReadHeaterData()
        {
            var status = await _gateway.ReadHeaterDataAsync();
            Assert.True(status.IsGood);
        }

        [Fact]
        public async Task TestReadInfoData()
        {
            var status = await _gateway.ReadInfoDataAsync();
            Assert.True(status.IsGood);
        }

        [Fact]
        public async Task TestReadNetworkData()
        {
            var status = await _gateway.ReadNetworkDataAsync();
            Assert.True(status.IsGood);
        }

        [Fact]
        public async Task TestReadOperationData()
        {
            var status = await _gateway.ReadOperationDataAsync();
            Assert.True(status.IsGood);
        }

        [Fact]
        public async Task TestReadSensorData()
        {
            var status = await _gateway.ReadSensorDataAsync();
            Assert.True(status.IsGood);
        }

        [Fact]
        public async Task TestReadSystemData()
        {
            var status = await _gateway.ReadSystemDataAsync();
            Assert.True(status.IsGood);
        }

        [Fact]
        public async Task TestReadTechnicalData()
        {
            var status = await _gateway.ReadTechnicalDataAsync();
            Assert.True(status.IsGood);
        }

        [Fact]
        public async Task TestReadVacationData()
        {
            var status = await _gateway.ReadVacationDataAsync();
            Assert.True(status.IsGood);
        }

        #endregion
    }
}
