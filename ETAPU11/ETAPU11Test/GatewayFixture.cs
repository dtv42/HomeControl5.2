// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EM300LRFixture.cs" company="DTV-Online">
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

    using Microsoft.Extensions.Logging;

    using ModbusLib;
    using ModbusLib.Models;

    using ETAPU11Lib;
    using ETAPU11Lib.Models;

    #endregion Using Directives

    public class GatewayFixture : IDisposable
    {
        #region Public Properties

        public ETAPU11Gateway Gateway { get; private set; }

        public ETAPU11Settings Settings { get; private set; } = new ETAPU11Settings()
        {
            TcpSlave = new TcpSlaveData()
            {
                Address = "127.0.0.1",
                Port = 502,
                ID = 1
            }
        };

        #endregion Public Properties

        #region Constructors

        public GatewayFixture()
        {
            // Set the default culture.
            CultureInfo.CurrentCulture = new CultureInfo("en-US");

            var loggerFactory = new LoggerFactory();

            var client = new ETAPU11Client(new TcpModbusClient(Settings),
                                           loggerFactory.CreateLogger<ETAPU11Client>());

            Gateway = new ETAPU11Gateway(client,
                                         Settings,
                                         loggerFactory.CreateLogger<ETAPU11Gateway>());

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

        #endregion Constructors
    }
}