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
    using System.CommandLine.Parsing;

    using Microsoft.Extensions.Logging;

    using UtilityLib;
    using UtilityLib.Console;
    using UtilityApp.Options;

    #endregion

    /// <summary>
    ///  Sample of a sub command with various options and validation.
    /// </summary>
    public sealed class ValidateCommand : Command
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
            AddOption(new Option<int>   ("-d", "Default value"            ).Name("number"  ).Default(1));
            AddOption(new Option<int>   ("-v", "Required value"           ).Name("number"  ).IsRequired());
            AddOption(new Option<int>   ("-o", "Zero or one value"        ).Name("number"  ).Arity(ArgumentArity.ZeroOrOne));
            AddOption(new Option<int>   ("-n", "Number value"             ).Name("number"  ).FromAmong(1, 2, 3));
            AddOption(new Option<byte>  ("-b", "Byte value [0..255]"      ).Name("number"  ));
            AddOption(new Option<ushort>("-i", "Integer value [0..65535]" ).Name("number"  ));
            AddOption(new Option<int>   ("-r", "Range value [0..10]"      ).Name("number"  ).Range(0, 10));
            AddOption(new Option<long>  ("-l", "Range value [0, 10000000]").Name("number"  ).Range(0, 10000000L));
                                                                                           
            AddOption(new Option<char>  ("-c", "Character value"          ).Name("char"    ));
            AddOption(new Option<char>  ("-f", "Character value [A,B,C]"  ).Name("char"    ).FromAmong("ABC"));

            AddOption(new Option<string>("-s", "String value"             ).Name("string"  ));
            AddOption(new Option<string>("-m", "String value [max: 10]"   ).Name("string"  ).StringLength(10));
            AddOption(new Option<string>("-e", "Not empty value"          ).Name("string"  ).NotEmpty());
            AddOption(new Option<string>("-w", "Not whitespace value"     ).Name("string"  ).NotWhiteSpace());
            AddOption(new Option<string>("-x", "Regex value"              ).Name("string"  ).Regex(@"^[a-zA-Z\.\-_]+@([a-zA-Z\.\-_]+\.)+[a-zA-Z]{2,4}$"));
            AddOption(new Option<string>("-g", "Guid value"               ).Name("guid"    ).Guid());
            AddOption(new Option<string>("-a", "IP address value"         ).Name("address" ).IPAddress());
            AddOption(new Option<string>("-p", "IP endpoint value"        ).Name("endpoint").IPEndpoint());
            AddOption(new Option<string>("-u", "URI value"                ).Name("uri"     ).Uri());

            // Setup execution handler.
            Handler = CommandHandler.Create<IConsole, ParseResult, bool, ValidateOptions>((console, result, verbose, options) =>
            {
                logger.LogDebug("Handler()");

                if (verbose)
                {
                    console.Out.WriteLine($"Commandline Application: {RootCommand.ExecutableName}");
                    if (result.HasOption("-d")) console.Out.WriteLine("-d found: Default value");
                    if (result.HasOption("-v")) console.Out.WriteLine("-v found: Required value");
                    if (result.HasOption("-o")) console.Out.WriteLine("-o found: Zero or one value");
                    if (result.HasOption("-n")) console.Out.WriteLine("-n found: Number value");
                    if (result.HasOption("-b")) console.Out.WriteLine("-b found: Byte value [0..255]");
                    if (result.HasOption("-i")) console.Out.WriteLine("-i found: Integer value [0..65535]");
                    if (result.HasOption("-r")) console.Out.WriteLine("-r found: Range value [0..10]");
                    if (result.HasOption("-l")) console.Out.WriteLine("-l found: Range value [0, 10000000]");

                    if (result.HasOption("-c")) console.Out.WriteLine("-c found: Character value");
                    if (result.HasOption("-f")) console.Out.WriteLine("-f found: Character value [A,B,C]");

                    if (result.HasOption("-s")) console.Out.WriteLine("-s found: String value");
                    if (result.HasOption("-m")) console.Out.WriteLine("-m found: String value [max: 10]");
                    if (result.HasOption("-e")) console.Out.WriteLine("-e found: Not empty value");
                    if (result.HasOption("-w")) console.Out.WriteLine("-w found: Not whitespace value");
                    if (result.HasOption("-x")) console.Out.WriteLine("-x found: Regex value");
                    if (result.HasOption("-g")) console.Out.WriteLine("-g found: Guid value");
                    if (result.HasOption("-a")) console.Out.WriteLine("-a found: IP address value");
                    if (result.HasOption("-p")) console.Out.WriteLine("-p found: IP endpoint value");
                    if (result.HasOption("-u")) console.Out.WriteLine("-u found: URI value");
                    console.Out.WriteLine();
                }

                console.Out.WriteLine($"D Integer Value: {options.IntegerD}");
                console.Out.WriteLine($"V Integer Value: {options.IntegerV}");
                console.Out.WriteLine($"O Integer Value: {options.IntegerO}");
                console.Out.WriteLine($"N Integer Value: {options.IntegerN}");
                console.Out.WriteLine($"B Integer Value: {options.IntegerB}");
                console.Out.WriteLine($"I Integer Value: {options.IntegerI}");
                console.Out.WriteLine($"R Integer Value: {options.IntegerR}");
                console.Out.WriteLine($"L Integer Value: {options.IntegerL}");

                console.Out.WriteLine($"C Character Value: {options.CharacterC}");
                console.Out.WriteLine($"F Character Value: {options.CharacterF}");

                console.Out.WriteLine($"S String Value: '{options.StringS}'");
                console.Out.WriteLine($"M String Value: '{options.StringM}'");
                console.Out.WriteLine($"E String Value: '{options.StringE}'");
                console.Out.WriteLine($"W String Value: '{options.StringW}'");
                console.Out.WriteLine($"X String Value: '{options.StringX}'");
                console.Out.WriteLine($"G String Value: '{options.StringG}'");
                console.Out.WriteLine($"A String Value: '{options.StringA}'");
                console.Out.WriteLine($"P String Value: '{options.StringP}'");
                console.Out.WriteLine($"U String Value: '{options.StringU}'");

                console.Out.WriteLine();

                return (int)ExitCodes.SuccessfullyCompleted;
            });
        }

        #endregion
    }
}
