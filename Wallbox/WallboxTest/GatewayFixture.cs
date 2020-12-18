// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GatewayFixture.cs" company="DTV-Online">
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

    using System;
    using System.Globalization;

    using Microsoft.Extensions.Logging;

    using WallboxLib;
    using WallboxLib.Models;

    #endregion

    public class GatewayFixture : IDisposable
    {
        #region Public Properties

        public WallboxGateway Gateway { get; private set; }
        public WallboxSettings Settings { get; private set; } = new WallboxSettings()
        {
            EndPoint = "10.0.1.9:7090",
            Port = 7090,
            Timeout = 1000
        };

    #endregion

    #region Constructors

    public GatewayFixture()
        {
            // Set the default culture.
            CultureInfo.CurrentCulture = new CultureInfo("en-US");

            var loggerFactory = new LoggerFactory();

            var client = new WallboxClient(Settings,
                                           loggerFactory.CreateLogger<WallboxClient>());

            Gateway = new WallboxGateway(client,
                                         Settings,
                                         loggerFactory.CreateLogger<WallboxGateway>());
            Gateway.Startup();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // free managed resources
            }
            // free native resources if there are any.
        }

        #endregion
    }
}
