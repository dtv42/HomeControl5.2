// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HostExtension.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>10-12-2020 15:55</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace UtilityLib
{
    #region Using Directives

    using System;
    using System.CommandLine;
    using System.CommandLine.Builder;
    using System.CommandLine.IO;
    using System.CommandLine.Parsing;
    using System.Diagnostics;
    using System.Globalization;
    using System.Threading.Tasks;

    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Serilog;

    #endregion Using Directives

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
            int code = (int)ExitCodes.SuccessfullyCompleted;
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            // Setup commandline builder.
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
                .UseVersionOption()
                .UseHelp()
                .UseEnvironmentVariableDirective()
                .UseParseDirective()
                .UseDebugDirective()
                .UseSuggestDirective()
                .RegisterWithDotnetSuggest()
                .UseTypoCorrections()
                .UseParseErrorReporting()
                .UseExceptionHandler()
                .CancelOnProcessTermination()
                .UseExceptionHandler((exception, context) =>
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    
                    if (exception.InnerException is not null)
                    {
                        context.Console.Error.WriteLine($"Exception: {exception.InnerException.Message}");
                        context.ResultCode = (int)ExitCodes.InvalidData;
                    }
                    else
                    {
                        context.Console.Error.WriteLine($"Unhandled exception: {exception.Message}");
                        context.ResultCode = (int)ExitCodes.UnhandledException;
                    }

                    Console.ResetColor();
                })
                .Build();

            try
            {
                code = await parser.InvokeAsync(args).ConfigureAwait(false);
                return code;
            }
            finally
            {
                stopWatch.Stop();

                if (code != (int)ExitCodes.SuccessfullyCompleted)
                {
                    Console.WriteLine($"Exit code: {(ExitCodes.Codes)code}");
                }
                else
                {
                    Console.WriteLine($"Time elapsed {stopWatch.Elapsed}");
                }

                Log.CloseAndFlush();
            }
        }
    }
}
