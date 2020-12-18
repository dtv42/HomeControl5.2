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
namespace WallboxApp
{
    #region Using Directives

    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Configuration;

    using UtilityLib;
    using WallboxLib;
    using WallboxLib.Models;

    using WallboxApp.Commands;
    using WallboxApp.Models;

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
                // Configure the Wallbox specific settings and the singleton Wallbox instances.
                // Add a singleton service using the application settings implementing Wallbox client settings.
                services.AddSingleton((WallboxSettings)context.Configuration.GetSection("AppSettings").Get<AppSettings>());
                services.AddSingleton<WallboxClient>();
                services.AddSingleton<WallboxGateway>();
            })
            .BaseProgramRunAsync<AppSettings, RootCommand>(args);
    }
}
