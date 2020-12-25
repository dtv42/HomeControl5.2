// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EnergyCommand.cs" company="DTV-Online">
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
    /// Application sub command "energy".
    /// </summary>
    public class EnergyCommand : BaseCommand
    {
        #region Private Data Members

        private readonly JsonSerializerOptions _serializerOptions = JsonExtensions.DefaultSerializerOptions;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EnergyCommand"/> class.
        /// </summary>
        /// <param name="gateway"></param>
        /// <param name="logger">The logger instance.</param>
        public EnergyCommand(WallboxGateway gateway, ILogger<EnergyCommand> logger)
            : base(logger, "energy", "Setting the energy limit on the BMW Wallbox charging station.")
        {
            _logger?.LogDebug("EnergyCommand()");

            // Setup command arguments and options.
            AddArgument(new Argument<uint?>("energy", "Energy value in 0.1 Wh (0; 1 - 999999999).   ").Arity(ArgumentArity.ExactlyOne).Name("number"));

            AddOption(new Option<bool>(new string[] { "-s", "--status" }, "Shows the data status"));

            // Setup execution handler.
            Handler = CommandHandler.Create<IConsole, GlobalOptions, uint?, bool>
                ((console, globals, energy, status) =>
                {
                    logger.LogDebug("Handler()");

                    if (!CheckOptions(console, energy)) return (int)ExitCodes.IncorrectFunction;

                    if (globals.Verbose)
                    {
                        console.Out.WriteLine($"Commandline Application: {RootCommand.ExecutableName}");
                        console.Out.WriteLine($"Endpoint:  {globals.EndPoint}");
                        console.Out.WriteLine($"Port:      {globals.Port}");
                        console.Out.WriteLine($"Timeout:   {globals.Timeout}");
                        console.Out.WriteLine();
                    }

                    console.Out.WriteLine("Setting the energy charging limit on BMW Wallbox charging station.");

                    if (energy.HasValue)
                    {
                        gateway.SetEnergy(energy.Value);

                        if (gateway.Status.IsGood)
                        {
                            console.Out.WriteLine("OK");
                        }
                        else
                        {
                            console.RedWriteLine("Error setting the energy charging limit on BMW Wallbox charging station.");
                        }
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
        public static bool CheckOptions(IConsole console, uint? energy)
        {
            if (energy.HasValue)
            {
                if (!WallboxGateway.IsEnergyValueOk(energy.Value))
                {
                    console.RedWriteLine("Energy value out of bounds (0; 1..999999999).");
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}
