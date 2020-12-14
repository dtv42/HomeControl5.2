// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ValidateCommand.cs" company="DTV-Online">
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
    using UtilityApp.Options;

    #endregion

    /// <summary>
    ///  Sample of a sub command with various options and validation.
    /// </summary>
    internal sealed class ValidateCommand : Command
    {
        #region Constructors

        /// <summary>
        ///  Initializes a new instance of the <see cref="ValidateCommand"/> class.
        /// </summary>
        /// <param name="logger"></param>
        public ValidateCommand(ILogger<ValidateCommand> logger)
            : base("validate", "A sample dotnet console application - validate command")
        {
            logger.LogDebug("ValidateCommand()");

            // Setup command options.
            AddOption(new Option<int> ("-d", "Default value"            ).Name("number").Default(1));
            AddOption(new Option<int> ("-v", "Required value"           ).Name("number").IsRequired());
            AddOption(new Option<int> ("-o", "Zero or one value"        ).Name("number").Arity(ArgumentArity.ZeroOrOne));
            AddOption(new Option<int> ("-n", "Number value"             ).Name("number").FromAmong(1, 2, 3));
            AddOption(new Option<int> ("-r", "Range value [0..10]"      ).Name("number").Range(0, 10));
            AddOption(new Option<long>("-l", "Range value [0, 10000000]").Name("number").Range(0, 10000000L));

            AddOption(new Option<string>("-s", "String value"           ).Name("string"));
            AddOption(new Option<string>("-m", "String value [max: 10]" ).Name("string").StringLength(10));
            AddOption(new Option<string>("-e", "Not empty value"        ).Name("string").NotEmpty());
            AddOption(new Option<string>("-w", "Not whitespace value"   ).Name("string").NotWhiteSpace());
            AddOption(new Option<string>("-x", "Regex value"            ).Name("string").Regex(@"^[a-zA-Z\.\-_]+@([a-zA-Z\.\-_]+\.)+[a-zA-Z]{2,4}$"));
            AddOption(new Option<string>("-g", "Guid value"             ).Name("guid").Guid());
            AddOption(new Option<string>("-a", "IP address value"       ).Name("address").IPAddress());
            AddOption(new Option<string>("-p", "IP endpoint value"      ).Name("endpoint").IPEndpoint());
            AddOption(new Option<string>("-u", "URI value"              ).Name("uri").Uri());

            // Setup execution handler.
            Handler = CommandHandler.Create<IConsole, bool, ValidateOptions>((console, verbose, options) =>
            {
                logger.LogDebug("Handler()");

                if (verbose)
                {
                    console.Out.WriteLine($"Commandline Application: {RootCommand.ExecutableName}");
                    console.Out.WriteLine();
                }

                console.Out.WriteLine($"D Integer Value: {options.IntegerD}");
                console.Out.WriteLine($"V Integer Value: {options.IntegerV}");
                console.Out.WriteLine($"O Integer Value: {options.IntegerO}");
                console.Out.WriteLine($"N Integer Value: {options.IntegerN}");
                console.Out.WriteLine($"R Integer Value: {options.IntegerR}");
                console.Out.WriteLine($"L Integer Value: {options.IntegerL}");

                console.Out.WriteLine($"S String  Value: '{options.StringS}'");
                console.Out.WriteLine($"M String  Value: '{options.StringM}'");
                console.Out.WriteLine($"E String  Value: '{options.StringE}'");
                console.Out.WriteLine($"W String  Value: '{options.StringW}'");
                console.Out.WriteLine($"X String  Value: '{options.StringX}'");
                console.Out.WriteLine($"G String  Value: '{options.StringG}'");
                console.Out.WriteLine($"A String  Value: '{options.StringA}'");
                console.Out.WriteLine($"P String  Value: '{options.StringP}'");
                console.Out.WriteLine($"U String  Value: '{options.StringU}'");

                console.Out.WriteLine();

                return (int)ExitCodes.SuccessfullyCompleted;
            });
        }

        #endregion
    }
}
