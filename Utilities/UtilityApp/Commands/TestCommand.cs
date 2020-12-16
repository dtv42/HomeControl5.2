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
    using System.CommandLine.Parsing;

    using Microsoft.Extensions.Logging;

    using UtilityLib;
    using UtilityLib.Console;
    using UtilityApp.Options;

    #endregion

    /// <summary>
    /// The "test" sub command.
    /// </summary>
    public sealed class TestCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GreetCommand"/> class.
        /// </summary>
        /// <param name="options">The greet command options.</param>
        /// <param name="logger">The logger instance.</param>
        public TestCommand(TestOptions options, ILogger<TestCommand> logger)
           : base(logger, "test", "A sample dotnet console application - test command")
        {
            AddArgument(new Argument<string>(
                name: "name",
                () => options.Name,
                description: "The name argument.")
            );

            AddArgument(new Argument<string>(
                name: "value",
                () => options.Value,
                description: "The value argument.")
            );

            // The new help option is allowing the use of a -h option.
            AddOption(new Option<bool>(new string[] { "-?", "--help", "/?", "/help" }, "Show help and usage information"));

            AddOption(new Option<bool>(new string[] { "-a", "--aoption" }, "test option A"));
            AddOption(new Option<bool>(new string[] { "-b", "--boption" }, "test option B"));
            AddOption(new Option<bool>(new string[] { "-c", "--coption" }, "test option C"));
            AddOption(new Option<bool>(new string[] { "-h", "--hoption" }, "test option H"));

            // Add custom command validation.
            AddValidator(r =>
            {
                if ((r.Children["name"]?.Tokens.Count == 0) && (r.Children["value"]?.Tokens.Count == 0) && (r.OptionResult("--help") is null) &&
                    (r.OptionResult("-a") is null) && (r.OptionResult("-b") is null) && (r.OptionResult("-c") is null))
                {
                    return "Please select at least one option (-a|-b|-c) or specify an argument.";
                }

                return null;
            });

            Handler = CommandHandler.Create<IConsole, ParseResult, GlobalOptions, bool, TestOptions>((console, result, globals, help, options) =>
            {
                logger.LogDebug("Handler()");

                // Showing the command help output.
                if (help) { this.ShowHelp(console); return (int)ExitCodes.SuccessfullyCompleted; }

                if (globals.Verbose)
                {
                    console.Out.WriteLine($"Commandline Application: {RootCommand.ExecutableName}");
                    console.Out.WriteLine($"Name:      {options.Name}");
                    console.Out.WriteLine($"Value:     {options.Value}");
                    console.Out.WriteLine($"A Option:  {options.AOption}");
                    console.Out.WriteLine($"B Option:  {options.BOption}");
                    console.Out.WriteLine($"C Option:  {options.COption}");
                    console.Out.WriteLine($"H Option:  {options.HOption}");
                    console.Out.WriteLine();
                }

                if (result.HasArgument())   console.Out.WriteLine($"{result.ArgumentCount()} Argument(s) provided");
                if (result.HasOption("-a")) console.Out.WriteLine("Option A provided");
                if (result.HasOption("-b")) console.Out.WriteLine("Option B provided");
                if (result.HasOption("-c")) console.Out.WriteLine("Option C provided");
                if (result.HasOption("-h")) console.Out.WriteLine("Option H provided");

                return (int)ExitCodes.SuccessfullyCompleted;
            });
        }

        #endregion Constructors
    }
}
