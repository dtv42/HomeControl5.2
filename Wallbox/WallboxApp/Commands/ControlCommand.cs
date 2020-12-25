// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ControlCommand.cs" company="DTV-Online">
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

    using Microsoft.Extensions.Logging;

    using UtilityLib;
    using UtilityLib.Console;

    using WallboxApp.Options;

    #endregion

    /// <summary>
    /// Subcommand to control a device at the BMW Wallbox charging station.
    /// </summary>
    public class ControlCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlCommand"/> class.
        /// </summary>
        /// <param name="console"></param>
        /// <param name="currentCommand"></param>
        /// <param name="energyCommand"></param>
        /// <param name="outputCommand"></param>
        /// <param name="startCommand"></param>
        /// <param name="stopCommand"></param>
        /// <param name="disableCommand"></param>
        /// <param name="unlockCommand"></param>
        /// <param name="logger">The logger instance.</param>
        public ControlCommand(CurrentCommand currentCommand,
                              EnergyCommand energyCommand,
                              OutputCommand outputCommand,
                              StartCommand startCommand,
                              StopCommand stopCommand,
                              DisableCommand disableCommand,
                              UnlockCommand unlockCommand,
                              ILogger<ControlCommand> logger)
            : base(logger, "control", "Controlling the BMW Wallbox charging station.")
        {
            logger.LogDebug("ReadCommand()");

            // Adding sub commands.
            AddCommand(currentCommand);
            AddCommand(energyCommand);
            AddCommand(outputCommand);
            AddCommand(startCommand);
            AddCommand(stopCommand);
            AddCommand(disableCommand);
            AddCommand(unlockCommand);

            // Setup execution handler.
            Handler = CommandHandler.Create<IConsole, GlobalOptions>
                ((console, globals) =>
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

                    return this.Invoke("-h");
                });
        }

        #endregion  Constructors
    }
}
