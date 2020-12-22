// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TestRead.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
// Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace EM300LRTest
{
    #region Using Directives

    using System.Globalization;
    using System.Threading.Tasks;

    using Xunit;

    using EM300LRLib;

    #endregion

    /// <summary>
    /// XUnit testing class.
    /// </summary>
    [Collection("EM300LR Test Collection")]
    public class TestRead : IClassFixture<GatewayFixture>
    {
        #region Private Data Members

        private readonly EM300LRGateway _gateway;

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
        public async Task TestEM300LRRead()
        {
            await _gateway.ReadAllAsync();
            Assert.True(_gateway.Status.IsGood);
        }

        #endregion
    }
}
