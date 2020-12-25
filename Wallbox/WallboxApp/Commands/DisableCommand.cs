// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DisableCommand.cs" company="DTV-Online">
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
    /// Application sub command "disable".
    /// </summary>
    public class DisableCommand : BaseCommand
    {
        #region Private Data Members

        private readonly JsonSerializerOptions _serializerOptions = JsonExtensions.DefaultSerializerOptions;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DisableCommand"/> class.
        /// </summary>
        /// <param name="gateway"></param>
        /// <param name="logger">The logger instance.</param>
        public DisableCommand(WallboxGateway gateway, ILogger<DisableCommand> logger)
            : base(logger, "disable", "Disable the BMW Wallbox charging station.")
        {
            _logger?.LogDebug("DisableCommand()");

            AddOption(new Option<bool>(new string[] { "-s", "--status" }, "Shows the data status"));

            // Setup execution handler.
            Handler = CommandHandler.Create<IConsole, GlobalOptions, bool>
                ((console, globals, status) =>
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

                    console.Out.WriteLine("Disable the system on the BMW Wallbox charging station.");

                    gateway.EnableCommand(0);

                    if (gateway.Status.IsGood)
                    {
                        console.Out.WriteLine("OK");
                    }
                    else
                    {
                        console.RedWriteLine("Error disabling the system on BMW Wallbox charging station.");
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
