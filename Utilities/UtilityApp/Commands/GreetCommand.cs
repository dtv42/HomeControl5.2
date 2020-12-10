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
namespace UtilityApp
{
    #region Using Directives

    using System;
    using System.CommandLine;
    using System.CommandLine.Invocation;

    using Microsoft.Extensions.Logging;

    using UtilityLib;
    using UtilityApp.Models;

    #endregion

    /// <summary>
    /// The "greet" sub command.
    /// </summary>
    public class GreetCommand : BaseCommand
    {
        #region Private Data Members

        /// <summary>
        /// The greeting option (configured using app settings only).
        /// </summary>
        private string _greeting = string.Empty;

        #endregion

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

            Handler = CommandHandler.Create((GlobalOptions globals, string name) 
                => HandleCommand(globals, name));

            _greeting = options.Greeting;
        }

        #endregion Constructors

        #region Private Methods

        /// <summary>
        /// The command handler for the greet sub command.
        /// </summary>
        /// <param name="options">The global options.</param>
        /// <param name="name">The name option.</param>
        /// <returns>Zero if sucessful.</returns>
        private int HandleCommand(GlobalOptions options, string name)
        {
            Program.LevelSwitch.MinimumLevel = options.LogLevel;

            try
            {
                if (options.Verbose)
                {
                    Console.WriteLine($"Password: {options.Password}");
                    Console.WriteLine($"Verbose:  {options.Verbose}");
                    Console.WriteLine($"LogLevel: {options.LogLevel}");
                    Console.WriteLine($"Uri:      {options.Uri}");
                }

                _logger.LogDebug($"Greeting:  {_greeting}");
                _logger.LogDebug($"Name:      {name}");

                Console.WriteLine($"{_greeting} {name}!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }

            return 0;
        }

        #endregion Private Methods
    }
}
