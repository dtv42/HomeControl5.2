// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CurrentCommand.cs" company="DTV-Online">
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
    /// Application sub command "current".
    /// </summary>
    public class CurrentCommand : BaseCommand
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
        public CurrentCommand(WallboxGateway gateway, ILogger<CurrentCommand> logger)
            : base(logger, "current", "Setting the current on the BMW Wallbox charging station.")
        {
            _logger?.LogDebug("CurrentCommand()");

            // Setup command arguments and options.
            AddArgument(new Argument<uint?>("current", "Current value in mA (0; 6000 - 63000).").Arity(ArgumentArity.ExactlyOne));
            AddArgument(new Argument<uint?>("delay",   "Optional delay in seconds (0; 1 - 860400).").Arity(ArgumentArity.ZeroOrOne));

            AddOption(new Option<bool>(new string[] { "-s", "--status" }, "Shows the data status"));

            // Setup execution handler.
            Handler = CommandHandler.Create<IConsole, GlobalOptions, uint?, uint?, bool>
                ((console, globals, current, delay, status) =>
                {
                    logger.LogDebug("Handler()");

                    if (!CheckOptions(console, current, delay)) return (int)ExitCodes.IncorrectFunction;

                    if (globals.Verbose)
                    {
                        console.Out.WriteLine($"Commandline Application: {RootCommand.ExecutableName}");
                        console.Out.WriteLine($"Endpoint:  {globals.EndPoint}");
                        console.Out.WriteLine($"Port:      {globals.Port}");
                        console.Out.WriteLine($"Timeout:   {globals.Timeout}");
                        console.Out.WriteLine();
                    }

                    console.Out.WriteLine("Setting the charging current on BMW Wallbox charging station.");

                    if (current.HasValue)
                    {
                        if (delay.HasValue)
                        {
                            gateway.SetCurrent(current.Value, delay.Value);

                            if (gateway.Status.IsGood)
                            {
                                console.Out.WriteLine("OK");
                            }
                            else
                            {
                                console.RedWriteLine("Error setting the charging current on BMW Wallbox charging station.");
                            }
                        }
                        else
                        {
                            gateway.SetCurrent(current.Value);

                            if (gateway.Status.IsGood)
                            {
                                console.Out.WriteLine("OK");
                            }
                            else
                            {
                                console.RedWriteLine("Error setting the charging current on BMW Wallbox charging station.");
                            }
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

        #region Private Methods

        /// <summary>
        /// Helper method to check options.
        /// </summary>
        /// <returns>True if options are OK.</returns>
        public static bool CheckOptions(IConsole console, uint? current, uint? delay)
        {
            if (current.HasValue)
            {
                if (!WallboxGateway.IsCurrentValueOk(current.Value))
                {
                    console.RedWriteLine("Current value out of bounds (0; 6000..63000).");
                    return false;
                }
            }

            if (delay.HasValue)
            {
                if (!WallboxGateway.IsDelayValueOk(delay.Value))
                {
                    console.RedWriteLine("Delay value out of bounds (0; 1..860400).");
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}
