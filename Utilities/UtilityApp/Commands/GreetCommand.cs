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

    using Serilog.Core;
    using Serilog.Events;

    using UtilityApp.Models;

    #endregion

    public class GreetCommand : Command
    {
        private readonly GreetOptions _options;
        private readonly ILogger<GreetCommand> _logger;
        private readonly LoggingLevelSwitch _levelSwitch;

        public GreetCommand(ILogger<GreetCommand> logger, LoggingLevelSwitch levelSwitch, GreetOptions options)
           : base("greet", "Says a greeting to the specified person.")
        {
            AddArgument(new Argument<string>(
                name: "name",
                () => options.Name,
                description: "The name of the person to greet.")
            );

            Handler = CommandHandler.Create((Uri uri, bool verbose, LogEventLevel level, string password, string name) 
                => HandleCommand(uri, verbose, level, password, name));

            _options = options;
            _logger = logger;
            _levelSwitch = levelSwitch;
        }

        private int HandleCommand(Uri uri, bool verbose, LogEventLevel level, string password, string name)
        {
            try
            {
                _levelSwitch.MinimumLevel = level;

                if (verbose)
                {
                    Console.WriteLine($"Password: {password}");
                    Console.WriteLine($"Verbose:  {verbose}");
                    Console.WriteLine($"LogLevel: {level}");
                    Console.WriteLine($"Uri:      {uri}");
                }

                _logger.LogDebug($"Greeting:  {_options.Greeting}");
                _logger.LogDebug($"Name:      {_options.Name}");

                Console.WriteLine($"{_options.Greeting} {name}!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return 1;
            }

            return 0;
        }
    }
}
