// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AsyncCommand.cs" company="DTV-Online">
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
    using System.CommandLine.Invocation;
    using System.CommandLine.IO;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using UtilityLib;
    using UtilityLib.Console;

    #endregion Using Directives

    /// <summary>
    ///  Sample of a command using async execution. The command simply waits for the specified delay.
    ///  Note that configuration or application settings are not used here. The global option verbose is available.
    /// </summary>
    public sealed class AsyncCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        ///  Initializes a new instance of the <see cref="AsyncCommand"/> class.
        /// </summary>
        /// <param name="logger"></param>
        public AsyncCommand(ILogger<AsyncCommand> logger)
            : base(logger, "async", "A sample dotnet console application - async command")
        {
            logger.LogDebug("AsyncCommand()");

            // Setup command options.
            AddOption(new Option<int>(
                aliases: new string[] { "-d", "--delay" },
                description: "The delay in seconds")
                .Default(10)
                .Range(1, 100));

            // Setup execution handler.
            Handler = CommandHandler.Create<IConsole, CancellationToken, bool, int>(async (console, token, verbose, delay) =>
            {
                logger.LogDebug("Handler()");

                if (verbose)
                {
                    console.Out.WriteLine($"Commandline Application: {RootCommand.ExecutableName}");
                    console.Out.WriteLine();
                    console.Out.WriteLine($"Verbose:       {verbose}");
                    console.Out.WriteLine($"Delay:         {delay}");
                }

                try
                {
                    console.Out.WriteLine($"Waiting for {delay} seconds...");
                    await Task.Delay(1000 * delay, token);
                    return (int)ExitCodes.SuccessfullyCompleted;
                }
                catch (TaskCanceledException tcx)
                {
                    logger.LogDebug(tcx, "Operation canceled");
                    return (int)ExitCodes.OperationCanceled;
                }
            });
        }

        #endregion Constructors
    }
}