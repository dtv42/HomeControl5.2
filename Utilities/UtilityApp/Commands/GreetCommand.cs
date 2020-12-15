// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GreetCommand.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>2-12-2020 11:57</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace UtilityApp.Commands
{
    #region Using Directives

    using System.CommandLine;
    using System.CommandLine.Invocation;
    using System.CommandLine.IO;

    using Microsoft.Extensions.Logging;

    using UtilityLib;
    using UtilityLib.Console;
    using UtilityApp.Options;

    #endregion

    /// <summary>
    /// The "greet" sub command.
    /// </summary>
    public sealed class GreetCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GreetCommand"/> class.
        /// </summary>
        /// <param name="options">The greet command options.</param>
        /// <param name="logger">The logger instance.</param>
        public GreetCommand(GreetOptions options, ILogger<GreetCommand> logger)
           : base(logger, "greet", "Says a greeting to the specified person.")
        {
            AddArgument(new Argument<string>(
                name: "name",
                () => options.Name,
                description: "The name of the person to greet.")
            );

            Handler = CommandHandler.Create<IConsole, GlobalOptions, string>((console, globals, name) =>
            {
                logger.LogDebug("Handler()");

                if (globals.Verbose)
                {
                    console.Out.WriteLine($"Commandline Application: {RootCommand.ExecutableName}");
                    console.Out.WriteLine($"Password: {globals.Password}");
                    console.Out.WriteLine($"Verbose:  {globals.Verbose}");
                    console.Out.WriteLine($"Host:     {globals.Host}");
                }

                _logger.LogDebug($"Greeting:  {options.Greeting}");
                _logger.LogDebug($"Name:      {name}");

                console.Out.WriteLine($"{options.Greeting} {name}!");

                return (int)ExitCodes.SuccessfullyCompleted;
            });
        }

        #endregion Constructors
    }
}
