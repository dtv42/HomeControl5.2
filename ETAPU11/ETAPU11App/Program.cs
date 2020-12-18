// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Program.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace ETAPU11App
{
    #region Using Directives

    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Configuration;

    using ModbusLib;
    using UtilityLib;

    using ETAPU11Lib;
    using ETAPU11Lib.Models;

    using ETAPU11App.Commands;
    using ETAPU11App.Models;
    using ModbusLib.Models;
    using System.Text.Json.Serialization;

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
                // Configure the ETAPU11 specific settings and the singleton ETAPU11 instances.
                // Add a singleton service using the application settings implementing ETAPU11 client settings.
                var settings = context.Configuration.GetSection("AppSettings").Get<ETAPU11Settings>().ValidateAndThrow();
                services.AddSingleton(settings);
                services.AddSingleton((ITcpClientSettings)settings);
                services.AddSingleton<TcpModbusClient>();
                services.AddSingleton<ETAPU11Client>();
                services.AddSingleton<ETAPU11Gateway>();
            })
            .BaseProgramRunAsync<AppSettings, RootCommand>(args);
    }
}
