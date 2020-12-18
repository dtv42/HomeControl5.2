// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MonitorCommand.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>22-4-2020 17:19</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace EM300LRApp.Commands
{
    #region Using Directives

    using System;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
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
    /// Application command "monitor".
    /// </summary>
    [Command(Name = "monitor",
             FullName = "EM300LR Monitor Command",
             Description = "Monitoring data values from b-Control EM300LR energy manager.",
             ExtendedHelpText = "\nCopyright (c) 2020 Dr. Peter Trimmel - All rights reserved.")]
    [HelpOption("-?|--help")]
    public class MonitorCommand : BaseCommand<MonitorCommand, AppSettings>
    {
        #region Private Data Members

        private readonly JsonSerializerOptions _options = JsonExtensions.DefaultSerializerOptions;
        private static readonly AutoResetEvent _closing = new AutoResetEvent(false);
        private readonly EM300LRGateway _gateway;

        #endregion Private Data Members

        #region Private Properties

        /// <summary>
        /// This is a reference to the parent command <see cref="RootCommand"/>.
        /// </summary>
        private RootCommand? Parent { get; set; }

        #endregion Private Properties

        #region Public Properties

        [Option("-d|--data", Description = "Monitors Fronius data.")]
        public bool Data { get; }

        [Option("-t|--total", Description = "Monitors the total data.")]
        public bool Total { get; }

        [Option("-1|--phase1", Description = "Monitors the phase 1 data.")]
        public bool Phase1 { get; }

        [Option("-2|--phase2", Description = "Monitors the phase 2 data.")]
        public bool Phase2 { get; }

        [Option("-3|--phase3", Description = "Monitors the phase 3 data.")]
        public bool Phase3 { get; }

        [Argument(0, Description = "Monitors the named property.")]
        public string Property { get; } = string.Empty;

        [Option("--repeat", Description = "The number of iterations (default: forever).")]
        public uint Repeat { get; set; } = 0;

        [Option("--interval", Description = "The seconds between times to read (default: 10).")]
        public uint Interval { get; set; } = 10;

        #endregion Public Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MonitorCommand"/> class.
        /// </summary>
        /// <param name="gateway"></param>
        /// <param name="console"></param>
        /// <param name="settings"></param>
        /// <param name="config"></param>
        /// <param name="environment"></param>
        /// <param name="lifetime"></param>
        /// <param name="logger"></param>
        /// <param name="application"></param>
        public MonitorCommand(EM300LRGateway gateway,
                              IConsole console,
                              AppSettings settings,
                              IConfiguration config,
                              IHostEnvironment environment,
                              IHostApplicationLifetime lifetime,
                              ILogger<MonitorCommand> logger,
                              CommandLineApplication application)
            : base(console, settings, config, environment, lifetime, logger, application)
        {
            _logger?.LogDebug("MonitorCommand()");

            // Setting the EM300LR instance.
            _gateway = gateway;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Runs when the commandline application command is executed.
        /// </summary>
        /// <returns>The exit code</returns>
        public async Task<int> OnExecuteAsync(CancellationToken cancellationToken)
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

            try
            {
                bool forever = (Repeat == 0);
                bool verbose = true;

                await Task.Factory.StartNew(async () =>
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        // Read the specified data.
                        var start = DateTime.UtcNow;
                        ReadingData(verbose);
                        // Only first call is verbose.
                        verbose = false;
                        var end = DateTime.UtcNow;
                        double delay = ((Interval * 1000.0) - (end - start).TotalMilliseconds) / 1000.0;

                        if (Interval > 0)
                        {
                            if (delay < 0)
                            {
                                _logger?.LogWarning($"Monitoring: no time between reads (duration: {((end - start).TotalMilliseconds / 1000.0):F2}).");
                            }
                            else
                            {
                                await Task.Delay(TimeSpan.FromSeconds(delay), cancellationToken);
                            }
                        }

                        if (!forever && (--Repeat <= 0))
                        {
                            _closing.Set();
                            break;
                        }
                    }

                }, cancellationToken);

                _console.CancelKeyPress += new ConsoleCancelEventHandler((sender, args) =>
                {
                    _console.WriteLine($"Monitoring cancelled.");
                    _closing.Set();
                });

                _closing.WaitOne();
            }
            catch (AggregateException aex) when (aex.InnerExceptions.All(e => e is OperationCanceledException))
            {
                _console.WriteLine($"Monitoring cancelled.");
            }
            catch (OperationCanceledException)
            {
                _console.WriteLine($"Monitoring cancelled.");
                throw;
            }
            catch
            {
                _logger.LogError("MonitorCommand exception");
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

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Reading the specified data.
        /// </summary>
        private void ReadingData(bool verbose = false)
        {
            DataStatus status = _gateway.ReadAll();

            if (status.IsGood)
            {
                if (string.IsNullOrEmpty(Property))
                {
                    if (Data)
                    {
                        if (verbose) _console.WriteLine($"Monitoring EM300LR data:");
                        _console.WriteLine(JsonSerializer.Serialize<EM300LRData>(_gateway.Data, _options));
                    }

                    if (Total)
                    {
                        if (verbose) _console.WriteLine($"Monitoring total data:");
                        _console.WriteLine(JsonSerializer.Serialize<TotalData>(_gateway.TotalData, _options));
                    }

                    if (Phase1)
                    {
                        if (verbose) _console.WriteLine($"Monitoring phase 1 data:");
                        _console.WriteLine(JsonSerializer.Serialize<Phase1Data>(_gateway.Phase1Data, _options));
                    }

                    if (Phase2)
                    {
                        if (verbose) _console.WriteLine($"Monitoring phase 2 data:");
                        _console.WriteLine(JsonSerializer.Serialize<Phase2Data>(_gateway.Phase2Data, _options));
                    }

                    if (Phase3)
                    {
                        if (verbose) _console.WriteLine($"Monitoring phase 3 data:");
                        _console.WriteLine(JsonSerializer.Serialize<Phase3Data>(_gateway.Phase3Data, _options));
                    }
                }
                else
                {
                    if (verbose) _console.WriteLine($"Monitoring property '{Property}':");

                    if (Data)
                    {
                        _console.WriteLine($"Value of property '{Property}' = {_gateway.Data.GetPropertyValue(Property)}");
                    }

                    if (Total)
                    {
                        _console.WriteLine($"Value of property '{Property}' = {_gateway.TotalData.GetPropertyValue(Property)}");
                    }

                    if (Phase1)
                    {
                        _console.WriteLine($"Value of property '{Property}' = {_gateway.Phase1Data.GetPropertyValue(Property)}");
                    }

                    if (Phase2)
                    {
                        _console.WriteLine($"Value of property '{Property}' = {_gateway.Phase2Data.GetPropertyValue(Property)}");
                    }

                    if (Phase3)
                    {
                        _console.WriteLine($"Value of property '{Property}' = {_gateway.Phase3Data.GetPropertyValue(Property)}");
                    }
                }
            }
            else
            {
                _console.WriteLine($"Error reading data from EM300LR energy manager.");
            }
        }

        #endregion Private Methods
    }
}