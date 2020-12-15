// --------------------------------------------------------------------------------------------------------------------
// <copyright file="LogCommand.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>10-12-2020 16:49</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace UtilityApp.Commands
{
    #region Using Directives

    using System.CommandLine;
    using System.CommandLine.IO;
    using System.CommandLine.Invocation;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using Serilog.Events;

    using UtilityLib;
    using UtilityLib.Console;

    #endregion Using Directives

    /// <summary>
    ///  Sample of a command using logging statements. Various log statements are generated.
    /// </summary>
    public sealed class LoggingCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        ///  Initializes a new instance of the <see cref="LoggingCommand"/> class.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        public LoggingCommand(IConfiguration configuration, ILogger<LoggingCommand> logger)
            : base(logger, "logging", "A sample dotnet console application - log command")
        {
            logger.LogDebug("LoggingCommand()");

            // Setup execution handler.
            Handler = CommandHandler.Create<IConsole, bool>((console, verbose) =>
            {
                logger.LogDebug("Handler()");

                if (verbose)
                {
                    console.Out.WriteLine($"Commandline Application: {RootCommand.ExecutableName}");
                    console.Out.WriteLine($"MinimumLevel Default:    {configuration.GetValue<LogEventLevel>("Serilog:MinimumLevel:Default")}");
                    console.Out.WriteLine($"MinimumLevel System:     {configuration.GetValue<LogEventLevel>("Serilog:MinimumLevel:Override:System")}");
                    console.Out.WriteLine($"MinimumLevel Microsoft:  {configuration.GetValue<LogEventLevel>("Serilog:MinimumLevel:Override:Microsoft")}");
                    console.Out.WriteLine();
                }

                logger.LogTrace("Trace Message");
                logger.LogDebug("Debug Message");
                logger.LogInformation("Information Message");
                logger.LogWarning("Warning Message");
                logger.LogError("Error Message");
                logger.LogCritical("Critical Message");

                console.Out.WriteLine();

                return (int)ExitCodes.SuccessfullyCompleted;
            });
        }

        #endregion Constructors
    }
}