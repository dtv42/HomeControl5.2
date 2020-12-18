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

    using System.ComponentModel.DataAnnotations;
    using System.Text.Json;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    using McMaster.Extensions.CommandLineUtils;

    using UtilityLib;
    using WallboxLib;
    using WallboxApp.Models;

    #endregion

    /// <summary>
    /// Application command "control".
    /// </summary>
    [Command(Name = "current",
             FullName = "Wallbox Control Command",
             Description = "Setting the current on the BMW Wallbox charging station.",
             ExtendedHelpText = "\nCopyright (c) 2020 Dr. Peter Trimmel - All rights reserved.")]
    public class CurrentCommand : BaseCommand<CurrentCommand, AppSettings>
    {
        #region Private Data Members

        private readonly JsonSerializerOptions _options = JsonExtensions.DefaultSerializerOptions;
        private readonly WallboxGateway _gateway;

        #endregion

        #region Private Properties

        /// <summary>
        /// This is a reference to the parent command <see cref="ControlCommand"/>.
        /// </summary>
        private ControlCommand? Parent { get; set; }

        #endregion

        #region Public Properties

        [Argument(0, "Current value in mA (0; 6000 - 63000).")]
        [Required]
        public uint? Current { get; }

        [Argument(1, "Optional delay in seconds(0; 1 - 860400).")]
        public uint? Delay { get; }

        [Option("--status", Description = "Shows the data status.")]
        public bool Status { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="CurrentCommand"/> class.
        /// </summary>
        /// <param name="gateway"></param>
        /// <param name="console"></param>
        /// <param name="settings"></param>
        /// <param name="config"></param>
        /// <param name="environment"></param>
        /// <param name="lifetime"></param>
        /// <param name="logger"></param>
        /// <param name="application"></param>
        public CurrentCommand(WallboxGateway gateway,
                              IConsole console,
                              AppSettings settings,
                              IConfiguration config,
                              IHostEnvironment environment,
                              IHostApplicationLifetime lifetime,
                              ILogger<CurrentCommand> logger,
                              CommandLineApplication application)
            : base(console, settings, config, environment, lifetime, logger, application)
        {
            _logger?.LogDebug("CurrentCommand()");

            // Setting the Wallbox instance.
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
                    if (!(Parent.Parent is null))
                    {
                        // Overriding Wallbox options.
                        _settings.EndPoint = Parent.Parent.EndPoint;
                        _settings.Port = Parent.Parent.Port;

                        if (Parent.Parent.ShowSettings)
                        {
                            _console.WriteLine(JsonSerializer.Serialize<AppSettings>(_settings, _options));
                        }
                    }
                }

                _console.WriteLine($"Setting the charging current on BMW Wallbox charging station.");

                if (Current.HasValue)
                {
                    if (Delay.HasValue)
                    {
                        DataStatus status = _gateway.SetCurrent(Current.Value, Delay.Value);

                        if (status.IsGood)
                        {
                            _console.WriteLine($"OK");
                        }
                        else
                        {
                            _console.WriteLine($"Error setting the charging current on BMW Wallbox charging station.");
                        }
                    }
                    else
                    {
                        DataStatus status = _gateway.SetCurrent(Current.Value);

                        if (status.IsGood)
                        {
                            _console.WriteLine($"OK");
                        }
                        else
                        {
                            _console.WriteLine($"Error setting the charging current on BMW Wallbox charging station.");
                        }
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
                _logger.LogError("CurrentCommand exception");
                throw;
            }

            return ExitCodes.SuccessfullyCompleted;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Helper method to check options.
        /// </summary>
        /// <returns>True if options are OK.</returns>
        public override bool CheckOptions()
        {
            if (Parent?.CheckOptions() ?? false)
            {
                if (Current.HasValue)
                {
                    if (!_gateway.IsCurrentValueOk(Current.Value))
                    {
                        _console.WriteLine($"Current value out of bounds (0; 6000..63000).");
                        return false;
                    }
                }

                if (Delay.HasValue)
                {
                    if (!_gateway.IsDelayValueOk(Delay.Value))
                    {
                        _console.WriteLine($"Delay value out of bounds (0; 1..860400).");
                        return false;
                    }
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
