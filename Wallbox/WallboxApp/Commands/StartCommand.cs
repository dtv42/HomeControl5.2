// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StartCommand.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>26-4-2020 20:18</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace WallboxApp.Commands
{
    #region Using Directives

    using System.CommandLine;
    using System.CommandLine.Invocation;
    using System.CommandLine.IO;
    using System.Text.Json;

    using Microsoft.Extensions.Logging;

    using UtilityLib;
    using UtilityLib.Console;

    using WallboxLib;

    using WallboxApp.Options;

    #endregion

    /// <summary>
    /// Application sub command "start".
    /// </summary>
    public class StartCommand : BaseCommand
    {
        #region Private Data Members

        private readonly JsonSerializerOptions _serializerOptions = JsonExtensions.DefaultSerializerOptions;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StartCommand"/> class.
        /// </summary>
        /// <param name="gateway"></param>
        /// <param name="logger">The logger instance.</param>
        public StartCommand(WallboxGateway gateway, ILogger<StartCommand> logger)
            : base(logger, "start", "Authorize a charging session on the BMW Wallbox charging station.")
        {
            _logger?.LogDebug("StartCommand()");

            // Setup command arguments and options.
            AddArgument(new Argument<string>("tag", "The RFID tag (8 byte HEX string).").Arity(ArgumentArity.ExactlyOne).StringLength(8));
            AddArgument(new Argument<string>("classifier", "The RFID classifier (10 byte HEX string).").Arity(ArgumentArity.ExactlyOne).StringLength(10));

            AddOption(new Option<bool>(new string[] { "-s", "--status" }, "Shows the data status"));

            // Setup execution handler.
            Handler = CommandHandler.Create<IConsole, GlobalOptions, string, string, bool>
                ((console, globals, tag, classifier, status) =>
                {
                    logger.LogDebug("Handler()");

                    if (!CheckOptions(console, tag, classifier)) return (int)ExitCodes.IncorrectFunction;

                    if (globals.Verbose)
                    {
                        console.Out.WriteLine($"Commandline Application: {RootCommand.ExecutableName}");
                        console.Out.WriteLine($"Endpoint:  {globals.EndPoint}");
                        console.Out.WriteLine($"Port:      {globals.Port}");
                        console.Out.WriteLine($"Timeout:   {globals.Timeout}");
                        console.Out.WriteLine();
                    }

                    console.Out.WriteLine("Authorize a charging session on BMW Wallbox charging station.");

                    gateway.StartRFID(tag, classifier);

                    if (gateway.Status.IsGood)
                    {
                        console.Out.WriteLine("OK");
                    }
                    else
                    {
                        console.RedWriteLine("Error authorizing a charging session  on BMW Wallbox charging station.");
                    }

                    if (status)
                    {
                        console.Out.WriteLine($"Status:");
                        console.Out.WriteLine(JsonSerializer.Serialize<DataStatus>(gateway.Status, _serializerOptions));
                    }

                    if (gateway.Status.IsNotGood) return (int)ExitCodes.NotSuccessfullyCompleted;

                    return (int)ExitCodes.SuccessfullyCompleted;
                });
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Helper method to check options.
        /// </summary>
        /// <returns>True if options are OK.</returns>
        public static bool CheckOptions(IConsole console, string tag, string classifier)
        {
            if (!WallboxGateway.IsRFIDTagStringOk(tag))
            {
                console.RedWriteLine("Invalid RFID tag.");
                return false;
            }

            if (!WallboxGateway.IsRFIDClassifierStringOk(classifier))
            {
                console.RedWriteLine("Invalid RFID classifier.");
                return false;
            }

            return true;
        }

        #endregion
    }
}
