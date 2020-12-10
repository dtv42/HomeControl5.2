// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HostExtension.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>13-5-2020 16:32</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace UtilityLib
{
    #region Using Directives

    using System;
    using System.CommandLine;
    using System.CommandLine.Builder;
    using System.CommandLine.Parsing;
    using System.Diagnostics;
    using System.Globalization;
    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Serilog;

    #endregion

    /// <summary>
    ///  Extension methods for command line execution.
    /// </summary>
    public static class HostExtension
    {
        public static async Task<int> RunCommandLineAsync(this IHost host, string[] args)
        {
            // Setup command line parser.
            using IServiceScope scope = host.Services.CreateScope();
            CultureInfo.CurrentCulture = new CultureInfo("en-US");
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Binding application settings.
            var serviceProvider = host.Services;

            var rootCommand = serviceProvider.GetRequiredService<RootCommand>();
            var commandLineBuilder = new CommandLineBuilder(rootCommand);

            foreach (Command command in serviceProvider.GetServices<Command>())
            {
                if (rootCommand.GetType().Name != command.GetType().Name)
                {
                    commandLineBuilder.AddCommand(command);
                }
            }

            var parser = commandLineBuilder
                .UseParseErrorReporting()
                .UseVersionOption()
                .UseDefaults()
                .Build();

            try
            {
                return await parser.InvokeAsync(args).ConfigureAwait(false);
            }
            finally
            {
                stopWatch.Stop();
                Console.WriteLine($"Time elapsed {stopWatch.Elapsed}");
                Log.CloseAndFlush();
            }
        }
    }
}
