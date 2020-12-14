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

    using Microsoft.Extensions.Logging;

    using UtilityLib;

    #endregion Using Directives

    /// <summary>
    ///  Sample of a command throwing an exception.
    /// </summary>
    public sealed class ErrorCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        ///  Initializes a new instance of the <see cref="ErrorCommand"/> class.
        /// </summary>
        /// <param name="logger"></param>
        public ErrorCommand(ILogger<ErrorCommand> logger)
            : base(logger, "error", "A sample dotnet console application - error command")
        {
            logger.LogDebug("ErrorCommand()");

            // Setup command options.
            AddOption(new Option<bool>(
                aliases: new string[] { "-x", "--exception" }, 
                description: "Throw an exception")
            );

            // Setup execution handler.
            Handler = CommandHandler.Create<IConsole, bool, bool>((console, verbose, exception) =>
            {
                logger.LogDebug("Handler()");

                if (verbose)
                {
                    console.Out.WriteLine($"Commandline Application: {RootCommand.ExecutableName}");
                    console.Out.WriteLine();
                }

                if (exception)
                {
                    throw new System.Exception("Application exception thrown");
                }

                console.Out.WriteLine();

                return (int)ExitCodes.NotSuccessfullyCompleted;
            });
        }

        #endregion Constructors
    }
}