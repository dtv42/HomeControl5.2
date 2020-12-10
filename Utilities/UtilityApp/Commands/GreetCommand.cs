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

    public class GreetCommand : BaseCommand
    {
        private string _greeting = string.Empty;
        
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

        private int HandleCommand(GlobalOptions globals, string name)
        {
            Program.LevelSwitch.MinimumLevel = globals.LogLevel;

            try
            {
                if (globals.Verbose)
                {
                    Console.WriteLine($"Password: {globals.Password}");
                    Console.WriteLine($"Verbose:  {globals.Verbose}");
                    Console.WriteLine($"LogLevel: {globals.LogLevel}");
                    Console.WriteLine($"Uri:      {globals.Uri}");
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
    }
}
