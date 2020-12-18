// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadCommand.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace EM300LRApp.Commands
{
    #region Using Directives

    using System.Text.Json;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    using McMaster.Extensions.CommandLineUtils;

    using UtilityLib;
    using EM300LRLib;
    using EM300LRLib.Models;
    using EM300LRApp.Models;

    #endregion Using Directives

    /// <summary>
    /// Application command "read".
    /// </summary>
    [Command(Name = "read",
             FullName = "EM300LR Read Command",
             Description = "Reading data values from b-Control EM300LR energy manager.",
             ExtendedHelpText = "\nCopyright (c) 2020 Dr. Peter Trimmel - All rights reserved.")]
    public class ReadCommand : BaseCommand<ReadCommand, AppSettings>
    {
        #region Private Data Members

        private readonly JsonSerializerOptions _options = JsonExtensions.DefaultSerializerOptions;
        private readonly EM300LRGateway _gateway;

        #endregion Private Data Members

        #region Private Properties

        /// <summary>
        /// This is a reference to the parent command <see cref="RootCommand"/>.
        /// </summary>
        private RootCommand? Parent { get; }

        #endregion Private Properties

        #region Public Properties

        [Option("-d|--data", Description = "Reads all data.")]
        public bool Data { get; }

        [Option("-t|--total", Description = "Reads the total data.")]
        public bool Total { get; }

        [Option("-1|--phase1", Description = "Reads the phase 1 data.")]
        public bool Phase1 { get; }

        [Option("-2|--phase2", Description = "Reads the phase 2 data.")]
        public bool Phase2 { get; }

        [Option("-3|--phase3", Description = "Reads the phase 3 data.")]
        public bool Phase3 { get; }

        [Argument(0, Description = "Reads the named property.")]
        public string Property { get; } = string.Empty;

        [Option("--status", Description = "Shows the data status.")]
        public bool Status { get; }

        #endregion Public Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadCommand"/> class.
        /// </summary>
        /// <param name="gateway"></param>
        /// <param name="console"></param>
        /// <param name="settings"></param>
        /// <param name="config"></param>
        /// <param name="environment"></param>
        /// <param name="lifetime"></param>
        /// <param name="logger"></param>
        /// <param name="application"></param>
        public ReadCommand(EM300LRGateway gateway,
                           IConsole console,
                           AppSettings settings,
                           IConfiguration config,
                           IHostEnvironment environment,
                           IHostApplicationLifetime lifetime,
                           ILogger<ReadCommand> logger,
                           CommandLineApplication application)
            : base(console, settings, config, environment, lifetime, logger, application)
        {
            _logger?.LogDebug("ReadCommand()");

            // Setting the EM300LR instance.
            _gateway = gateway;
        }

        #endregion Constructors

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
                    // Overriding EM300LR options.
                    _settings.BaseAddress = Parent.BaseAddress;
                    _settings.Timeout = Parent.Timeout;
                    _settings.Password = Parent.Password;
                    _settings.SerialNumber = Parent.SerialNumber;
                    _gateway.UpdateClient();
                }

                if (Parent?.ShowSettings ?? false)
                {
                    _console.WriteLine(JsonSerializer.Serialize<AppSettings>(_settings, _options));
                }

                if (_gateway.ReadAll().IsGood)
                {
                    if (string.IsNullOrEmpty(Property))
                    {
                        if (Data)
                        {
                            _console.WriteLine($"Reading all data from EM300LR energy manager.");
                            _console.WriteLine($"Data:");
                            _console.WriteLine(JsonSerializer.Serialize<EM300LRData>(_gateway.Data, _options));
                        }

                        if (Total)
                        {
                            _console.WriteLine($"Reading all total data from EM300LR energy manager.");
                            _console.WriteLine($"Total:");
                            _console.WriteLine(JsonSerializer.Serialize<TotalData>(_gateway.TotalData, _options));
                        }

                        if (Phase1)
                        {
                            _console.WriteLine($"Reading all phase 1 data from EM300LR energy manager.");
                            _console.WriteLine($"Phase1:");
                            _console.WriteLine(JsonSerializer.Serialize<Phase1Data>(_gateway.Phase1Data, _options));
                        }

                        if (Phase2)
                        {
                            _console.WriteLine($"Reading all phase 2 data from EM300LR energy manager.");
                            _console.WriteLine($"Phase2:");
                            _console.WriteLine(JsonSerializer.Serialize<Phase2Data>(_gateway.Phase2Data, _options));
                        }

                        if (Phase3)
                        {
                            _console.WriteLine($"Reading all phase 3 data from EM300LR energy manager.");
                            _console.WriteLine($"Phase3:");
                            _console.WriteLine(JsonSerializer.Serialize<Phase3Data>(_gateway.Phase3Data, _options));
                        }
                    }
                    else
                    {
                        if (Data)
                        {
                            _console.WriteLine($"Value of EM300LR data property '{Property}' = {_gateway.Data.GetPropertyValue(Property)}");
                        }

                        if (Total)
                        {
                            _console.WriteLine($"Value of total data property '{Property}' = {_gateway.TotalData.GetPropertyValue(Property)}");
                        }

                        if (Phase1)
                        {
                            _console.WriteLine($"Value of phase 1 data property '{Property}' = {_gateway.Phase1Data.GetPropertyValue(Property)}");
                        }

                        if (Phase2)
                        {
                            _console.WriteLine($"Value of phase 2 data property '{Property}' = {_gateway.Phase2Data.GetPropertyValue(Property)}");
                        }

                        if (Phase3)
                        {
                            _console.WriteLine($"Value of phase 3 data property '{Property}' = {_gateway.Phase3Data.GetPropertyValue(Property)}");
                        }
                    }
                }
                else
                {
                    _console.WriteLine($"Error reading all data from EM300LR energy manager.");
                }

                if (Status)
                {
                    _console.WriteLine($"Status:");
                    _console.WriteLine(JsonSerializer.Serialize<DataStatus>(_gateway.Status, _options));
                }
            }
            catch
            {
                _logger.LogError("ReadCommand exception");
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

                if (Data) ++options;
                if (Total) ++options;
                if (Phase1) ++options;
                if (Phase2) ++options;
                if (Phase3) ++options;

                if (options != 1)
                {
                    _console.WriteLine("Please specifiy a single data option");
                    return false;
                }

                if (!string.IsNullOrEmpty(Property))
                {
                    if (Data)
                    {
                        if (typeof(EM300LRData).GetProperty(Property) is null)
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }
                    
                    if (Total)
                    {
                        if (typeof(TotalData).GetProperty(Property) is null)
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }
                    
                    if (Phase1)
                    {
                        if (typeof(Phase1Data).GetProperty(Property) is null)
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }
                    
                    if (Phase2)
                    {
                        if (typeof(Phase2Data).GetProperty(Property) is null)
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }
                    
                    if (Phase3)
                    {
                        if (typeof(Phase3Data).GetProperty(Property) is null)
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }
                }
            }

            return true;
        }

        #endregion
    }
}