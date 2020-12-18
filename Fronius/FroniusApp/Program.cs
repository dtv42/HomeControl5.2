// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>25-4-2020 17:57</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace FroniusApp
{
    #region Using Directives

    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Configuration;

    using Polly;
    using Polly.Extensions.Http;

    using UtilityLib;
    using FroniusLib;
    using FroniusLib.Models;

    using FroniusApp.Commands;
    using FroniusApp.Models;

    #endregion Using Directives

    /// <summary>
    /// Class providing the main application entry point.
    /// </summary>
    public class Program : BaseProgram<AppSettings, RootCommand>
    {
        /// <summary>
        /// The main console application entry point.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <returns>The exit code.</returns>
        static async Task<int> Main(string[] args)
            => await CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                // Configure the Fronius specific settings and the singleton Fronius instances.
                // Add a singleton service using the application settings implementing Fronius client settings.
                services.AddSingleton((FroniusSettings)context.Configuration.GetSection("AppSettings").Get<AppSettings>());

                services.AddHttpClient<FroniusClient>()
                    .ConfigureHttpMessageHandlerBuilder(config => new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
                    })
                    .AddPolicyHandler(HttpPolicyExtensions
                        .HandleTransientHttpError()
                        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                            .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt))));

                services.AddSingleton<FroniusGateway>();
            })
            .BaseProgramRunAsync<AppSettings, RootCommand>(args);
    }
}
