// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ControlCommand.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>26-4-2020 10:05</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace HeliosApp.Commands
{
    #region Using Directives

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    using McMaster.Extensions.CommandLineUtils;

    using UtilityLib;
    using HeliosLib;
    using HeliosLib.Models;
    using HeliosApp.Models;

    #endregion

    /// <summary>
    /// Subcommand to control the Helios ventilation system.
    /// </summary>
    [Command(Name = "control",
             FullName = "Helios Control Command",
             Description = "Control the Helios KWL EC 200 ventilation system.",
             ExtendedHelpText = "\nCopyright (c) 2020 Dr. Peter Trimmel - All rights reserved.")]
    public class ControlCommand : BaseCommand<ControlCommand, AppSettings>
    {
        #region Private Data Members

        private readonly JsonSerializerOptions _options = JsonExtensions.DefaultSerializerOptions;
        private readonly HeliosGateway _gateway;

        #endregion

        #region Private Properties

        /// <summary>
        /// This is a reference to the parent command <see cref="RootCommand"/>.
        /// </summary>
        private RootCommand? Parent { get; }

        #endregion

        #region Public Properties

        [Option("-o|--operation <VALUE>", Description = "Set the operation mode (auto, manual)")]
        [AllowedValues("auto", "manual", IgnoreCase = true)]
        public (bool HasValue, string Value) Operation { get; }

        [Option("-b|--booster", Description = "Set the booster operation")]
        public bool Booster { get; }

        [Option("-s|--standby", Description = "Set the standby operation")]
        public bool Standby { get; }

        [Option("-f|--fan <NUMBER>", Description = "Set the fan ventilation level (0..4)")]
        public int? Fan { get; }

        [Option("-m|--mode <STRING>", Description = "The mode (on|off)")]
        [AllowedValues("on", "off", IgnoreCase = true)]
        public (bool HasValue, string Value) Mode { get; }

        [Option("-l|--level <NUMBER>", Description = "The ventilation level (0..4)")]
        [Range(0, 4)]
        public int? Level { get; }

        [Option("-d|--duration <NUMBER>", Description = "Set the duration (5..180)")]
        [Range(5, 180)]
        public int? Duration { get; }

        [Option("--status", Description = "Shows the data status.")]
        public bool Status { get; }

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
        public ControlCommand(HeliosGateway gateway,
                              IConsole console,
                              AppSettings settings,
                              IConfiguration config,
                              IHostEnvironment environment,
                              IHostApplicationLifetime lifetime,
                              ILogger<ControlCommand> logger,
                              CommandLineApplication application)
            : base(console, settings, config, environment, lifetime, logger, application)
        {
            _logger?.LogDebug("ControlCommand()");

            // Setting the Helios instance.
            _gateway = gateway;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Runs when the commandline application command is executed.
        /// </summary>
        /// <returns>The exit code</returns>
        public int OnExecute()
        {
            try
            {
                if (!(Parent is null))
                {
                    // Overriding Helios options.
                    _settings.BaseAddress = Parent.BaseAddress;
                    _settings.Timeout = Parent.Timeout;
                    _settings.Password = Parent.Password;
                    _gateway.UpdateClient();

                    if (Parent.ShowSettings)
                    {
                        _console.WriteLine(JsonSerializer.Serialize<AppSettings>(_settings, _options));
                    }
                }

                if (Operation.HasValue)
                {
                    if (Operation.Value == "auto")
                    {
                        _gateway.SetOperationMode(OperationModes.Automatic);
                    }
                    else if (Operation.Value == "manual")
                    {
                        _gateway.SetOperationMode(OperationModes.Manual);
                    }
                }
                else if (Fan.HasValue)
                {
                    _gateway.SetFanLevel((FanLevels)Fan.Value);
                }
                else if (Booster)
                {
                    if (Mode.HasValue && !Level.HasValue && !Duration.HasValue)
                    {
                        _gateway.SetBoosterMode((Mode.Value == "on") ? true : false);
                    }
                    
                    if (Mode.HasValue && Level.HasValue && Duration.HasValue)
                    {
                        var data = new VentilationData()
                        {
                            Mode = (Mode.Value == "on") ? true : false,
                            Level = Level.HasValue ? (FanLevels)Level.Value : FanLevels.Level4,
                            Duration = Duration.HasValue ? Duration.Value : 120
                        };

                        _gateway.SetBooster(data);
                    }

                    if (Mode.HasValue)
                    {
                        _gateway.SetBoosterMode((Mode.Value == "on") ? true : false);
                    }

                    if (Level.HasValue)
                    {
                        _gateway.SetBoosterLevel((FanLevels)Level.Value);
                    }

                    if (Duration.HasValue)
                    {
                        _gateway.SetBoosterDuration(Duration.Value);
                    }
                }
                else if (Standby)
                {
                    if (Mode.HasValue && !Level.HasValue && !Duration.HasValue)
                    {
                        _gateway.SetStandbyMode((Mode.Value == "on") ? true : false);
                    }

                    if (Mode.HasValue && Level.HasValue && Duration.HasValue)
                    {
                        var data = new VentilationData()
                        {
                            Mode = (Mode.Value == "on") ? true : false,
                            Level = Level.HasValue ? (FanLevels)Level.Value : FanLevels.Level4,
                            Duration = Duration.HasValue ? Duration.Value : 120
                        };

                        _gateway.SetStandby(data);
                    }

                    if (Mode.HasValue)
                    {
                        _gateway.SetStandbyMode((Mode.Value == "on") ? true : false);
                    }

                    if (Level.HasValue)
                    {
                        _gateway.SetStandbyLevel((FanLevels)Level.Value);
                    }

                    if (Duration.HasValue)
                    {
                        _gateway.SetStandbyDuration(Duration.Value);
                    }
                }

                if (Status)
                {
                    _console.WriteLine($"Status:");
                    _console.WriteLine(JsonSerializer.Serialize<DataStatus>(_gateway.Status, _options));
                }
            }
            catch
            {
                _logger.LogError("ControlCommand exception");
                throw;
            }

            return ExitCodes.SuccessfullyCompleted;
        }

        /// <summary>
        /// Helper method to check options.
        /// </summary>
        /// <returns>True if options are OK.</returns>
        public override bool CheckOptions()
        {
            if (Parent?.CheckOptions() ?? false)
            {
                int options = 0;

                if (Operation.HasValue) ++options;
                if (Booster) ++options;
                if (Standby) ++options;
                if (Fan.HasValue) ++options;

                if (options != 1)
                {
                    _console.WriteLine("Please specifiy a single data option (Operating | Booster | Standby | Fan)");
                    return false;
                }

                if (Booster && (!Mode.HasValue && !Level.HasValue && !Duration.HasValue))
                {
                    _console.WriteLine("Please specifiy a booster operation data (Mode | Level | Duration)");
                    return false;
                }

                if (Standby && (!Mode.HasValue && !Level.HasValue && !Duration.HasValue))
                {
                    _console.WriteLine("Please specifiy a standby operation data (Mode | Level | Duration)");
                    return false;
                }

                if (Level.HasValue && (Operation.HasValue || Fan.HasValue))
                {
                    _console.WriteLine("Level data are ignored");
                }

                if (Duration.HasValue && (Operation.HasValue || Fan.HasValue))
                {
                    _console.WriteLine("Duration data are ignored");
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        #endregion
    }
}
