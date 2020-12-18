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
    [Command(Name = "start",
             FullName = "Wallbox Control Command",
             Description = "Authorize a charging session on the BMW Wallbox charging station.",
             ExtendedHelpText = "\nCopyright (c) 2020 Dr. Peter Trimmel - All rights reserved.")]
    public class StartCommand : BaseCommand<StartCommand, AppSettings>
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

        [Argument(0, "The RFID tag (8 byte HEX string).")]
        [Required]
        public string Tag { get; } = string.Empty;

        [Argument(1, "The RFID classifier (10 byte HEX string).")]
        [Required]
        public string Classifier { get; } = string.Empty;

        [Option("--status", Description = "Shows the data status.")]
        public bool Status { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StartCommand"/> class.
        /// </summary>
        /// <param name="gateway"></param>
        /// <param name="console"></param>
        /// <param name="settings"></param>
        /// <param name="config"></param>
        /// <param name="environment"></param>
        /// <param name="lifetime"></param>
        /// <param name="logger"></param>
        /// <param name="application"></param>
        public StartCommand(WallboxGateway gateway,
                            IConsole console,
                            AppSettings settings,
                            IConfiguration config,
                            IHostEnvironment environment,
                            IHostApplicationLifetime lifetime,
                            ILogger<StartCommand> logger,
                            CommandLineApplication application)
            : base(console, settings, config, environment, lifetime, logger, application)
        {
            _logger?.LogDebug("StartCommand()");

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

                _console.WriteLine($"authorize a charging session on BMW Wallbox charging station.");

                DataStatus status = _gateway.StartRFID(Tag, Classifier);

                if (status.IsGood)
                {
                    _console.WriteLine($"OK");
                }
                else
                {
                    _console.WriteLine($"Error authorizing a charging session  on BMW Wallbox charging station.");
                }

                if (Status)
                {
                    _console.WriteLine($"Status:");
                    _console.WriteLine(JsonSerializer.Serialize<DataStatus>(_gateway.Status, _options));
                }
            }
            catch
            {
                _logger.LogError("StartCommand exception");
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
                if (!_gateway.IsRFIDTagStringOk(Tag))
                {
                    _console.WriteLine($"Invalid RFID tag.");
                    return false;
                }

                if (!_gateway.IsRFIDClassifierStringOk(Classifier))
                {
                    _console.WriteLine($"Invalid RFID classifier.");
                    return false;
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
