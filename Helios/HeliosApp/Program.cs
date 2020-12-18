// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>26-4-2020 10:01</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace HeliosApp
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
    using HeliosLib;
    using HeliosLib.Models;

    using HeliosApp.Commands;
    using HeliosApp.Models;

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
                // Configure the Helios specific settings and the singleton Helios instances.
                // Add a singleton service using the application settings implementing Helios client settings.
                services.AddSingleton((HeliosSettings)context.Configuration.GetSection("AppSettings").Get<AppSettings>());

                services.AddHttpClient<HeliosClient>()
                    .ConfigureHttpMessageHandlerBuilder(config => new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
                    })
                    .AddPolicyHandler(HttpPolicyExtensions
                        .HandleTransientHttpError()
                        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                            .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt))));

                services.AddSingleton<HeliosGateway>();
            })
            .BaseProgramRunAsync<AppSettings, RootCommand>(args);
    }
}
