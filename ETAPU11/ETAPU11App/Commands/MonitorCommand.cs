// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MonitorCommand.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace ETAPU11App.Commands
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
    using ETAPU11Lib;
    using ETAPU11Lib.Models;
    using ETAPU11App.Models;

    #endregion

    /// <summary>
    /// Application command "monitor".
    /// </summary>
    [Command(Name = "monitor",
             FullName = "ETAPU11 Monitor Command",
             Description = "Monitoring data values from ETA PU 11 pellet boiler.",
             ExtendedHelpText = "\nCopyright (c) 2020 Dr. Peter Trimmel - All rights reserved.")]
    public class MonitorCommand : BaseCommand<MonitorCommand, AppSettings>
    {
        #region Private Data Members

        private readonly JsonSerializerOptions _options = JsonExtensions.DefaultSerializerOptions;
        private static readonly AutoResetEvent _closing = new AutoResetEvent(false);
        private readonly ETAPU11Gateway _gateway;

        #endregion

        #region Private Properties

        /// <summary>
        /// This is a reference to the parent command <see cref="RootCommand"/>.
        /// </summary>
        private RootCommand? Parent { get; }

        #endregion

        #region Public Properties

        [Option("-d|--data", Description = "Gets all data.")]
        public bool Data { get; }

        [Option("-b|--boiler", Description = "Gets the boiler data.")]
        public bool Boiler { get; }

        [Option("-w|--hotwater", Description = "Gets the hot water data.")]
        public bool Hotwater { get; }

        [Option("-h|--heating", Description = "Gets the heating circuit data.")]
        public bool Heating { get; }

        [Option("-s|--storage", Description = "Gets the storage data.")]
        public bool Storage { get; }

        [Option("-y|--system", Description = "Gets the system data.")]
        public bool System { get; }

        [Argument(0, Description = "Reads the named property.")]
        public string Property { get; } = string.Empty;

        [Option("--block", Description = "Using block mode read (only when reading all data).")]
        public bool Block { get; }

        [Option("--repeat", Description = "The number of iterations (default: forever).")]
        public uint Repeat { get; private set; } = 0;

        [Option("--interval", Description = "The seconds between times to read (default: 10).")]
        public uint Interval { get; } = 10;

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
        public MonitorCommand(ETAPU11Gateway gateway,
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

            // Setting the ETAPU11 instance.
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
                // Overriding ETAPU11 options.
                _gateway.Settings.TcpSlave.Address = Parent.Address;
                _gateway.Settings.TcpSlave.Port = Parent.Port;
                _gateway.Settings.TcpSlave.ID = Parent.SlaveID;
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
                if (Boiler) ++options;
                if (Hotwater) ++options;
                if (Heating) ++options;
                if (Storage) ++options;
                if (System) ++options;

                if (options != 1)
                {
                    _console.WriteLine("Please specifiy a single data option");
                    return false;
                }

                if (Block && !Data)
                {
                    _console.WriteLine("Block read option is ignored.");
                }

                if (!string.IsNullOrEmpty(Property))
                {
                    if (Data)
                    {
                        if (!typeof(ETAPU11Data).IsProperty(Property))
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }

                    if (Boiler)
                    {
                        if (!typeof(BoilerData).IsProperty(Property))
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }

                    if (Hotwater)
                    {
                        if (!typeof(HotwaterData).IsProperty(Property))
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }

                    if (Heating)
                    {
                        if (!typeof(HeatingData).IsProperty(Property))
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }

                    if (Storage)
                    {
                        if (!typeof(StorageData).IsProperty(Property))
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }

                    if (System)
                    {
                        if (!typeof(SystemData).IsProperty(Property))
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }

                    if (!ETAPU11Data.IsReadable(Property))
                    {
                        _logger?.LogError($"The property '{Property}' is not readable.");
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
                    if (verbose) _console.WriteLine($"Monitoring ETAPU11 data.");
                    DataStatus status;

                    if (Block)
                    {
                        status = _gateway.ReadBlockAll();
                    }
                    else
                    {
                        status = _gateway.ReadAll();
                    }

                    if (status.IsGood)
                    {
                        // Fix: Json Serializer cannot serialize NaN
                        if (double.IsNaN(_gateway.Data.Flow)) _gateway.Data.Flow = 0;
                        if (double.IsNaN(_gateway.Data.ResidualO2)) _gateway.Data.ResidualO2 = 0;

                        _console.WriteLine(JsonSerializer.Serialize<ETAPU11Data>(_gateway.Data, _options));
                    }
                    else
                    {
                        _console.WriteLine($"Error reading boiler data from ETAPU11 pellet boiler.");
                    }
                }

                if (Boiler)
                {
                    if (verbose) _console.WriteLine($"Monitoring boiler data.");
                    DataStatus status = _gateway.ReadBoilerData();

                    if (status.IsGood)
                    {
                        // Fix: Json Serializer cannot serialize NaN
                        if (double.IsNaN(_gateway.Data.ResidualO2)) _gateway.Data.ResidualO2 = 0;

                        _console.WriteLine(JsonSerializer.Serialize<BoilerData>(_gateway.BoilerData, _options));
                    }
                    else
                    {
                        _console.WriteLine($"Error reading boiler data from ETAPU11 pellet boiler.");
                    }
                }

                if (Hotwater)
                {
                    if (verbose) _console.WriteLine($"Monitoring hotwater data.");
                    DataStatus status = _gateway.ReadHotwaterData();

                    if (status.IsGood)
                    {
                        _console.WriteLine(JsonSerializer.Serialize<HotwaterData>(_gateway.HotwaterData, _options));
                    }
                    else
                    {
                        _console.WriteLine($"Error reading hotwater data from ETAPU11 pellet boiler.");
                    }
                }

                if (Heating)
                {
                    if (verbose) _console.WriteLine($"Monitoring heating data.");
                    DataStatus status = _gateway.ReadHeatingData();

                    if (status.IsGood)
                    {
                        // Fix: Json Serializer cannot serialize NaN
                        if (double.IsNaN(_gateway.Data.Flow)) _gateway.Data.Flow = 0;

                        _console.WriteLine(JsonSerializer.Serialize<HeatingData>(_gateway.HeatingData, _options));
                    }
                    else
                    {
                        _console.WriteLine($"Error reading heating data from ETAPU11 pellet boiler.");
                    }
                }

                if (Storage)
                {
                    if (verbose) _console.WriteLine($"Monitoring storage data.");
                    DataStatus status = _gateway.ReadBoilerData();

                    if (status.IsGood)
                    {
                        _console.WriteLine(JsonSerializer.Serialize<StorageData>(_gateway.StorageData, _options));
                    }
                    else
                    {
                        _console.WriteLine($"Error reading storage data from ETAPU11 pellet boiler.");
                    }
                }

                if (System)
                {
                    if (verbose) _console.WriteLine($"Monitoring system data.");
                    DataStatus status = _gateway.ReadBoilerData();

                    if (status.IsGood)
                    {
                        _console.WriteLine(JsonSerializer.Serialize<SystemData>(_gateway.SystemData, _options));
                    }
                    else
                    {
                        _console.WriteLine($"Error reading system data from ETAPU11 pellet boiler.");
                    }
                }
            }
            else
            {
                if (verbose) _console.WriteLine($"Monitoring property '{Property}':");
                var status = _gateway.ReadProperty(Property);

                if (status.IsGood)
                {
                    if (Data)
                    {
                        _console.WriteLine($"Value of property '{Property}' = {_gateway.Data.GetPropertyValue(Property)}");
                    }

                    if (Boiler)
                    {
                        _console.WriteLine($"Value of property '{Property}' = {_gateway.BoilerData.GetPropertyValue(Property)}");
                    }
                    
                    if (Hotwater)
                    {
                        _console.WriteLine($"Value of property '{Property}' = {_gateway.HotwaterData.GetPropertyValue(Property)}");
                    }
                    
                    if (Heating)
                    {
                        _console.WriteLine($"Value of property '{Property}' = {_gateway.HeatingData.GetPropertyValue(Property)}");
                    }
                    
                    if (Storage)
                    {
                        _console.WriteLine($"Value of property '{Property}' = {_gateway.StorageData.GetPropertyValue(Property)}");
                    }
                    
                    if (System)
                    {
                        _console.WriteLine($"Value of property '{Property}' = {_gateway.SystemData.GetPropertyValue(Property)}");
                    }
                }
                else
                {
                    _console.WriteLine($"Error reading property '{Property}' from ETAPU11 pellet boiler.");
                }
            }
        }

        #endregion
    }
}
