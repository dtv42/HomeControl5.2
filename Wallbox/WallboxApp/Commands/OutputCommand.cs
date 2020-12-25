// --------------------------------------------------------------------------------------------------------------------
// <copyright file="OutputCommand.cs" company="DTV-Online">
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
    /// Application sub command "output".
    /// </summary>
    public class OutputCommand : BaseCommand
    {
        #region Private Data Members

        private readonly JsonSerializerOptions _serializerOptions = JsonExtensions.DefaultSerializerOptions;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentCommand"/> class.
        /// </summary>
        /// <param name="gateway"></param>
        /// <param name="logger">The logger instance.</param>
        public OutputCommand(WallboxGateway gateway, ILogger<OutputCommand> logger)
            : base(logger, "output", "Setting the output on the BMW Wallbox charging station.")
        {
            _logger?.LogDebug("OutputCommand()");

            // Setup command arguments and options.
            AddArgument(new Argument<ushort?>("output", "Output value (0: Close; 1: Open).").Arity(ArgumentArity.ExactlyOne).FromAmong(0,1));

            AddOption(new Option<bool>(new string[] { "-s", "--status" }, "Shows the data status"));

            // Setup execution handler.
            Handler = CommandHandler.Create<IConsole, GlobalOptions, ushort?, bool>
                ((console, globals, output, status) =>
                {
                    logger.LogDebug("Handler()");

                    if (globals.Verbose)
                    {
                        console.Out.WriteLine($"Commandline Application: {RootCommand.ExecutableName}");
                        console.Out.WriteLine($"Endpoint:  {globals.EndPoint}");
                        console.Out.WriteLine($"Port:      {globals.Port}");
                        console.Out.WriteLine($"Timeout:   {globals.Timeout}");
                        console.Out.WriteLine();
                    }

                    console.Out.WriteLine("Setting the output relay on BMW Wallbox charging station.");

                    if (output.HasValue)
                    {
                        gateway.SetOutput(output.Value);

                        if (gateway.Status.IsGood)
                        {
                            console.Out.WriteLine("OK");
                        }
                        else
                        {
                            console.Out.WriteLine("Error the output relay on BMW Wallbox charging station.");
                        }
                    }

                    if (status)
                    {
                        console.Out.WriteLine("Status:");
                        console.Out.WriteLine(JsonSerializer.Serialize<DataStatus>(gateway.Status, _serializerOptions));
                    }

                    if (gateway.Status.IsNotGood) return (int)ExitCodes.NotSuccessfullyCompleted;

                    return (int)ExitCodes.SuccessfullyCompleted;
                });
        }

        #endregion
    }
}
