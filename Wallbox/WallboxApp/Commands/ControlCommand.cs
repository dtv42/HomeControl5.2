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

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    using McMaster.Extensions.CommandLineUtils;

    using UtilityLib;
    using WallboxApp.Models;
    using WallboxLib;

    #endregion

    /// <summary>
    /// Subcommand to control a device at the IKEA Trådfri gateway.
    /// </summary>
    [Command(Name = "control",
             FullName = "WallboxApp Control Command",
             Description = "Controlling the BMW Wallbox charging station.",
             ExtendedHelpText = "\nCopyright (c) 2020 Dr. Peter Trimmel - All rights reserved.")]
    [Subcommand(
        typeof(CurrentCommand),
        typeof(EnergyCommand),
        typeof(OutputCommand),
        typeof(StartCommand),
        typeof(StopCommand),
        typeof(DisableCommand),
        typeof(UnlockCommand))]
    public class ControlCommand : BaseCommand<ControlCommand, AppSettings>
    {
        #region Public Properties

        public RootCommand? Parent { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlCommand"/> class.
        /// </summary>
        /// <param name="gateway"></param>
        /// <param name="console"></param>
        /// <param name="settings"></param>
        /// <param name="config"></param>
        /// <param name="environment"></param>
        /// <param name="lifetime"></param>
        /// <param name="logger"></param>
        /// <param name="application"></param>
        public ControlCommand(WallboxGateway gateway,
                              IConsole console,
                              AppSettings settings,
                              IConfiguration config,
                              IHostEnvironment environment,
                              IHostApplicationLifetime lifetime,
                              ILogger<ControlCommand> logger,
                              CommandLineApplication application)
            : base(console, settings, config, environment, lifetime, logger, application)
        {}

        #endregion

        #region Public Methods

        /// <summary>
        /// Runs when the commandline application command is executed.
        /// </summary>
        /// <returns>The exit code</returns>
        public int OnExecute()
        {
            _application.ShowHelp();
            return ExitCodes.SuccessfullyCompleted;
        }

        /// <summary>
        /// Helper method to check options.
        /// </summary>
        /// <returns>True if options are OK.</returns>
        public override bool CheckOptions()
            => Parent?.CheckOptions() ?? false;

        #endregion
    }
}
