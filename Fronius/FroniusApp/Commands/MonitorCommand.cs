// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MonitorCommand.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>25-4-2020 18:06</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace FroniusApp.Commands
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
    using FroniusLib;
    using FroniusLib.Models;

    using FroniusApp.Models;

    #endregion

    /// <summary>
    /// Application command "monitor".
    /// </summary>
    [Command(Name = "monitor",
             FullName = "Fronius Monitor Command",
             Description = "Monitoring data values from Fronius Symo 8.2-3-M solar inverter.",
             ExtendedHelpText = "\nCopyright (c) 2020 Dr. Peter Trimmel - All rights reserved.")]
    public class MonitorCommand : BaseCommand<MonitorCommand, AppSettings>
    {
        #region Private Data Members

        private readonly JsonSerializerOptions _options = JsonExtensions.DefaultSerializerOptions;
        private static readonly AutoResetEvent _closing = new AutoResetEvent(false);
        private readonly FroniusGateway _gateway;

        #endregion

        #region Private Properties

        /// <summary>
        /// This is a reference to the parent command <see cref="RootCommand"/>.
        /// </summary>
        private RootCommand? Parent { get; }

        #endregion

        #region Public Properties

        [Option("-d|--data", Description = "Reads all data.")]
        public bool Data { get; }

        [Option("-c|--common", Description = "Get the inverter common data.")]
        public bool Common { get; }

        [Option("-i|--inverter", Description = "Get the inverter info.")]
        public bool Inverter { get; }

        [Option("-l|--logger", Description = "Get the data logger info.")]
        public bool Logger { get; }

        [Option("-m|--minmax", Description = "Get the inverter minmax data.")]
        public bool MinMax { get; }

        [Option("-p|--phase", Description = "Get the inverter phase data.")]
        public bool Phase { get; }

        [Argument(0, Description = "Monitors the named property.")]
        public string Property { get; } = string.Empty;

        [Option("--repeat", Description = "The number of iterations (default: forever).")]
        public uint Repeat { get; set; } = 0;

        [Option("--interval", Description = "The seconds between times to read (default: 10).")]
        public uint Seconds { get; set; } = 10;

        #endregion

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
        public MonitorCommand(FroniusGateway gateway,
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

            // Setting the Fronius instance.
            _gateway = gateway;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Runs when the commandline application command is executed.
        /// </summary>
        /// <returns>The exit code</returns>
        public async Task<int> OnExecuteAsync(CancellationToken cancellationToken)
        {
            if (!(Parent is null))
            {
                // Overriding Fronius options.
                _settings.BaseAddress = Parent.BaseAddress;
                _settings.Timeout = Parent.Timeout;
                _settings.DeviceID = Parent.DeviceID;
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
                        double delay = ((Seconds * 1000.0) - (end - start).TotalMilliseconds) / 1000.0;

                        if (Seconds > 0)
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
                if (Common) ++options;
                if (Inverter) ++options;
                if (Logger) ++options;
                if (MinMax) ++options;
                if (Phase) ++options;

                if (options != 1)
                {
                    _console.WriteLine("Please specifiy a single data option");
                    return false;
                }

                if (!string.IsNullOrEmpty(Property))
                {
                    if (Data)
                    {
                        if (!typeof(FroniusData).IsProperty(Property))
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }

                    if (Common)
                    {
                        if (!typeof(CommonData).IsProperty(Property))
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }

                    if (Inverter)
                    {
                        if (!typeof(InverterInfo).IsProperty(Property))
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }

                    if (Logger)
                    {
                        if (!typeof(LoggerInfo).IsProperty(Property))
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }

                    if (MinMax)
                    {
                        if (!typeof(MinMaxData).IsProperty(Property))
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }

                    if (Phase)
                    {
                        if (!typeof(PhaseData).IsProperty(Property))
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }
                }
            }
            else
            {
                return false;
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
            if (string.IsNullOrEmpty(Property))
            {
                if (Data)
                {
                    if (verbose) _console.WriteLine($"Monitoring Fronius data.");
                    DataStatus status = _gateway.ReadAll();

                    if (status.IsGood)
                    {
                        _console.WriteLine($"Data:");
                        _console.WriteLine(JsonSerializer.Serialize<FroniusData>(_gateway.Data, _options));
                    }
                    else
                    {
                        _console.WriteLine($"Error reading data from Fronius solar inverter.");
                    }
                }

                if (Common)
                {
                    if (verbose) _console.WriteLine($"Monitoring common inverter data.");
                    DataStatus status = _gateway.ReadCommonData();

                    if (status.IsGood)
                    {
                        _console.WriteLine($"Common:");
                        _console.WriteLine(JsonSerializer.Serialize<CommonData>(_gateway.CommonData, _options));
                    }
                    else
                    {
                        _console.WriteLine($"Error reading common inverter data from Fronius solar inverter.");
                    }
                }

                if (Inverter)
                {
                    if (verbose) _console.WriteLine($"Monitoring inverter info data.");
                    DataStatus status = _gateway.ReadInverterInfo();

                    if (status.IsGood)
                    {
                        _console.WriteLine($"Inverter:");
                        _console.WriteLine(JsonSerializer.Serialize<InverterInfo>(_gateway.InverterInfo, _options));
                    }
                    else
                    {
                        _console.WriteLine($"Error reading hotwater data from Fronius solar inverter.");
                    }
                }

                if (Logger)
                {
                    if (verbose) _console.WriteLine($"Monitoring logger info data.");
                    DataStatus status = _gateway.ReadLoggerInfo();

                    if (status.IsGood)
                    {
                        _console.WriteLine($"Logger:");
                        _console.WriteLine(JsonSerializer.Serialize<LoggerInfo>(_gateway.LoggerInfo, _options));
                    }
                    else
                    {
                        _console.WriteLine($"Error reading logger info from Fronius solar inverter.");
                    }
                }

                if (MinMax)
                {
                    if (verbose) _console.WriteLine($"Monitoring minmax data.");
                    DataStatus status = _gateway.ReadMinMaxData();

                    if (status.IsGood)
                    {
                        _console.WriteLine($"MinMax:");
                        _console.WriteLine(JsonSerializer.Serialize<MinMaxData>(_gateway.MinMaxData, _options));
                    }
                    else
                    {
                        _console.WriteLine($"Error reading minmax data from Fronius solar inverter.");
                    }
                }

                if (Phase)
                {
                    if (verbose) _console.WriteLine($"Monitoring phase data.");
                    DataStatus status = _gateway.ReadPhaseData();

                    if (status.IsGood)
                    {
                        _console.WriteLine($"Phase:");
                        _console.WriteLine(JsonSerializer.Serialize<PhaseData>(_gateway.PhaseData, _options));
                    }
                    else
                    {
                        _console.WriteLine($"Error reading phase data from Fronius solar inverter.");
                    }
                }
            }
            else
            {
                if (verbose) _console.WriteLine($"Monitoring property '{Property}':");

                if (Data)
                {
                    DataStatus status = _gateway.ReadAll();

                    if (status.IsGood)
                    {
                        _console.WriteLine($"Value of property '{Property}' = {_gateway.Data.GetPropertyValue(Property)}");
                    }
                    else
                    {
                        _console.WriteLine($"Error reading data from Fronius solar inverter.");
                    }
                }

                if (Common)
                {
                    DataStatus status = _gateway.ReadCommonData();

                    if (status.IsGood)
                    {
                        _console.WriteLine($"Value of property '{Property}' = {_gateway.CommonData.GetPropertyValue(Property)}");
                    }
                    else
                    {
                        _console.WriteLine($"Error reading common inverter data from Fronius solar inverter.");
                    }
                }

                if (Inverter)
                {
                    DataStatus status = _gateway.ReadInverterInfo();

                    if (status.IsGood)
                    {
                        _console.WriteLine($"Value of property '{Property}' = {_gateway.InverterInfo.GetPropertyValue(Property)}");
                    }
                    else
                    {
                        _console.WriteLine($"Error reading inverter info from Fronius solar inverter.");
                    }
                }

                if (Logger)
                {
                    DataStatus status = _gateway.ReadLoggerInfo();

                    if (status.IsGood)
                    {
                        _console.WriteLine($"Value of property '{Property}' = {_gateway.LoggerInfo.GetPropertyValue(Property)}");
                    }
                    else
                    {
                        _console.WriteLine($"Error reading logger info from Fronius solar inverter.");
                    }
                }

                if (MinMax)
                {
                    DataStatus status = _gateway.ReadMinMaxData();

                    if (status.IsGood)
                    {
                        _console.WriteLine($"Value of property '{Property}' = {_gateway.MinMaxData.GetPropertyValue(Property)}");
                    }
                    else
                    {
                        _console.WriteLine($"Error reading minmax data from Fronius solar inverter.");
                    }
                }

                if (Phase)
                {
                    DataStatus status = _gateway.ReadPhaseData();

                    if (status.IsGood)
                    {
                        _console.WriteLine($"Value of property '{Property}' = {_gateway.PhaseData.GetPropertyValue(Property)}");
                    }
                    else
                    {
                        _console.WriteLine($"Error reading phase data from Fronius solar inverter.");
                    }
                }
            }
        }

        #endregion Private Methods
    }
}
