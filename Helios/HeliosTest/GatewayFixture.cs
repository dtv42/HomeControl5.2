// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeliosFixture.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
// Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace HeliosTest
{
    #region Using Directives

    using System;
    using System.Globalization;
    using System.Net.Http;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using HeliosLib;
    using HeliosLib.Models;

    #endregion

    public class GatewayFixture : IDisposable
    {
        #region Public Properties

        public HeliosGateway Gateway { get; }
        public HeliosSettings Settings { get; private set; } = new HeliosSettings();

        #endregion

        #region Constructors
        public GatewayFixture()
        {
            // Set the default culture.
            CultureInfo.CurrentCulture = new CultureInfo("en-US");

            var loggerFactory = new LoggerFactory();

            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddUserSecrets("64c846d9-0a3d-48a4-a821-af02bb1f8764")
                .Build();

            configuration.GetSection("AppSettings:GatewaySettings").Bind(Settings);

            var client = new HeliosClient(new HttpClient()
                                          {
                                              BaseAddress = new Uri(Settings.Address),
                                              Timeout = TimeSpan.FromMilliseconds(Settings.Timeout)
                                          },
                                          loggerFactory.CreateLogger<HeliosClient>());

            Gateway = new HeliosGateway(client,
                                        Settings,
                                        loggerFactory.CreateLogger<HeliosGateway>());
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
