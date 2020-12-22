// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GatewayFixture.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>17-12-2020 12:51</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace EM300LRTest
{
    #region Using Directives

    using System;
    using System.Globalization;
    using System.Net.Http;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using EM300LRLib;
    using EM300LRLib.Models;

    #endregion Using Directives

    public class GatewayFixture : IDisposable
    {
        #region Public Properties

        public EM300LRGateway Gateway { get; private set; }
        public EM300LRSettings Settings { get; private set; } = new EM300LRSettings();

        #endregion Public Properties

        #region Constructors

        public GatewayFixture()
        {
            CultureInfo.CurrentCulture = new CultureInfo("en-US");

            var loggerFactory = new LoggerFactory();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddUserSecrets("15e9821a-836b-4bb0-96d6-e83cb4b42cd4")
                .Build();

            configuration.GetSection("AppSettings:GatewaySettings").Bind(Settings);

            var client = new EM300LRClient(new HttpClient()
                                           {
                                               BaseAddress = new Uri(Settings.Address),
                                               Timeout = TimeSpan.FromMilliseconds(Settings.Timeout)
                                           },
                                           loggerFactory.CreateLogger<EM300LRClient>());

            Gateway = new EM300LRGateway(client,
                                         Settings,
                                         loggerFactory.CreateLogger<EM300LRGateway>());

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