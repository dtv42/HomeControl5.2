// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FroniusFixture.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
// Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace FroniusTest
{
    #region Using Directives

    using System;
    using System.Globalization;
    using System.Net.Http;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using FroniusLib;
    using FroniusLib.Models;

    #endregion

    public class GatewayFixture : IDisposable
    {
        #region Public Properties

        public FroniusGateway Gateway { get; }
        public FroniusSettings Settings { get; private set; } = new FroniusSettings();

        #endregion

        #region Constructors
        public GatewayFixture()
        {
            // Set the default culture.
            CultureInfo.CurrentCulture = new CultureInfo("en-US");

            var loggerFactory = new LoggerFactory();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddUserSecrets("15e9821a-836b-4bb0-96d6-e83cb4b42cd4")
                .Build();

            configuration.GetSection("AppSettings:GatewaySettings").Bind(Settings);
            var client = new FroniusClient(new HttpClient()
                                           {
                                                BaseAddress = new Uri(Settings.Address),
                                                Timeout = TimeSpan.FromMilliseconds(Settings.Timeout)
                                           },
                                           loggerFactory.CreateLogger<FroniusClient>());

            Gateway = new FroniusGateway(client,
                                         Settings,
                                         loggerFactory.CreateLogger<FroniusGateway>());

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
