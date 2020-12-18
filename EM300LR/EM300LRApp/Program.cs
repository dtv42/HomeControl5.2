// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>22-4-2020 12:42</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace EM300LRApp
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
    using EM300LRLib;
    using EM300LRLib.Models;

    using EM300LRApp.Commands;
    using EM300LRApp.Models;

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
                // Configure the EM300LR specific settings and the singleton EM300LR instances.
                // Add a singleton service using the application settings implementing EM300LR client settings.
                services.AddSingleton((EM300LRSettings)context.Configuration.GetSection("AppSettings").Get<AppSettings>());

                services.AddHttpClient<EM300LRClient>()
                    .ConfigureHttpMessageHandlerBuilder(config => new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; }
                    })
                    .AddPolicyHandler(HttpPolicyExtensions
                        .HandleTransientHttpError()
                        .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.NotFound)
                            .WaitAndRetryAsync(3, attempt => TimeSpan.FromSeconds(Math.Pow(2, attempt))));

                services.AddSingleton<EM300LRGateway>();
            })
            .BaseProgramRunAsync<AppSettings, RootCommand>(args);
    }
}
