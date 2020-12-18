// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MonitorCommand.cs" company="DTV-Online">
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

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Text.Json;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    using McMaster.Extensions.CommandLineUtils;

    using UtilityLib;
    using WallboxLib;
    using WallboxLib.Models;
    using WallboxApp.Models;

    #endregion

    /// <summary>
    /// Application command "monitor".
    /// </summary>
    [Command(Name = "monitor",
             FullName = "Wallbox Monitor Command",
             Description = "Monitoring data values from BMW Wallbox charging station.",
             ExtendedHelpText = "\nCopyright (c) 2020 Dr. Peter Trimmel - All rights reserved.")]
    public class MonitorCommand : BaseCommand<MonitorCommand, AppSettings>
    {
        #region Private Data Members

        private readonly JsonSerializerOptions _options = JsonExtensions.DefaultSerializerOptions;
        private static readonly AutoResetEvent _closing = new AutoResetEvent(false);
        private readonly WallboxGateway _gateway;

        #endregion

        #region Private Properties

        /// <summary>
        /// This is a reference to the parent command <see cref="RootCommand"/>.
        /// </summary>
        private RootCommand? Parent { get; }

        #endregion

        #region Public Properties

        [Option("-1|--report1", Description = "Monitors the report 1 data.")]
        public bool Report1 { get; }

        [Option("-2|--report2", Description = "Monitors the report 2 data.")]
        public bool Report2 { get; }

        [Option("-3|--report3", Description = "Monitors the report 3 data.")]
        public bool Report3 { get; }

        [Option("-100|--report100", Description = "Monitors the last charging report data (report 100).")]
        public bool Report100 { get; }

        [Option("-n|--number <NUMBER>", Description = "Reads the specified charging report (101 - 130).")]
        [Range(101, 130)]
        public int? Index { get; }

        [Option("-i|--info", Description = "Reads info data.")]
        public bool Info { get; }

        [Argument(0, "Monitors the named property.")]
        public string Property { get; set; } = string.Empty;

        [Option(Description = "The number of iterations (default: forever).")]
        public uint Repeat { get; set; } = 0;

        [Option(Description = "The seconds between times to read (default: 10).")]
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
        public MonitorCommand(WallboxGateway gateway,
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

            // Setting the Wallbox instance.
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
                // Overriding Wallbox options.
                _settings.EndPoint = Parent.EndPoint;
                _settings.Port = Parent.Port;

                if (Parent.ShowSettings)
                {
                    _console.WriteLine(JsonSerializer.Serialize<AppSettings>(_settings, _options));
                }
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

                if (Report1) ++options;
                if (Report2) ++options;
                if (Report3) ++options;
                if (Report100) ++options;
                if (Index.HasValue) ++options;
                if (Info) ++options;

                if (options != 1)
                {
                    _console.WriteLine("Please specifiy a single report option");
                    _application.ShowHint();
                    return false;
                }

                if (!string.IsNullOrEmpty(Property))
                {
                    if (Report1)
                    {
                        if (!typeof(Report1Data).IsProperty(Property))
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }

                    if (Report2)
                    {
                        if (!typeof(Report2Data).IsProperty(Property))
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }

                    if (Report3)
                    {
                        if (!typeof(Report3Data).IsProperty(Property))
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }

                    if (Report100 || Index.HasValue)
                    {
                        if (!typeof(ReportsData).IsProperty(Property))
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }

                    if (Info)
                    {
                        if (!typeof(InfoData).IsProperty(Property))
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

        #endregion

        #region Private Methods

        /// <summary>
        /// Reading the specified data.
        /// </summary>
        private void ReadingData(bool verbose = false)
        {
            if (string.IsNullOrEmpty(Property))
            {
                if (Report1)
                {
                    if (verbose) _console.WriteLine($"Monitoring report 1 data.");
                    DataStatus status = _gateway.ReadReport1();

                    if (status.IsGood)
                    {
                        _console.WriteLine($"Report1:");
                        _console.WriteLine(JsonSerializer.Serialize<Report1Data>(_gateway.Report1, _options));
                    }
                    else
                    {
                        _console.WriteLine($"Error reading report 1 from BMW Wallbox charging station.");
                    }
                }

                if (Report2)
                {
                    if (verbose) _console.WriteLine($"Monitoring report 2 data.");
                    DataStatus status = _gateway.ReadReport2();

                    if (status.IsGood)
                    {
                        _console.WriteLine($"Report2:");
                        _console.WriteLine(JsonSerializer.Serialize<Report2Data>(_gateway.Report2, _options));
                    }
                    else
                    {
                        _console.WriteLine($"Error reading report 2 from BMW Wallbox charging station.");
                    }
                }

                if (Report3)
                {
                    if (verbose) _console.WriteLine($"Monitoring report 3 data.");
                    DataStatus status = _gateway.ReadReport3();

                    if (status.IsGood)
                    {
                        _console.WriteLine($"Report3:");
                        _console.WriteLine(JsonSerializer.Serialize<Report3Data>(_gateway.Report3, _options));
                    }
                    else
                    {
                        _console.WriteLine($"Error reading report 1 from BMW Wallbox charging station.");
                    }
                }

                if (Report100)
                {
                    if (verbose) _console.WriteLine($"Monitoring report 100 data.");
                    DataStatus status = _gateway.ReadReport100();

                    if (status.IsGood)
                    {
                        _console.WriteLine($"Report100:");
                        _console.WriteLine(JsonSerializer.Serialize<ReportsData>(_gateway.Report100, _options));
                    }
                    else
                    {
                        _console.WriteLine($"Error reading report 100 from BMW Wallbox charging station.");
                    }
                }

                if (Index.HasValue)
                {
                    int index = Index.Value - WallboxGateway.REPORTS_ID - 1;
                    if (verbose) _console.WriteLine($"Monitoring report {index} data.");
                    DataStatus status = _gateway.ReadReports();

                    if (status.IsGood)
                    {
                        _console.WriteLine($"Report{Index.Value}:");
                        _console.WriteLine(JsonSerializer.Serialize<ReportsData>(_gateway.Reports[index], _options));
                    }
                    else
                    {
                        _console.WriteLine($"Error reading report from BMW Wallbox charging station.");
                    }
                }

                if (Info)
                {
                    _console.WriteLine($"Monitoring info data from BMW Wallbox charging station.");
                    DataStatus status = _gateway.ReadInfo();

                    if (status.IsGood)
                    {
                        _console.WriteLine($"Info:");
                        _console.WriteLine(JsonSerializer.Serialize<InfoData>(_gateway.Info, _options));
                    }
                    else
                    {
                        _console.WriteLine($"Error reading info data from BMW Wallbox charging station.");
                    }
                }
            }
            else
            {
                if (verbose) _console.WriteLine($"Monitoring property '{Property}':");

                if (Report1)
                {
                    DataStatus status = _gateway.ReadReport1();

                    if (status.IsGood)
                    {
                        _console.WriteLine($"Value of property '{Property}' = {_gateway.Report1.GetPropertyValue(Property)}");
                    }
                    else
                    {
                        _console.WriteLine($"Error reading report 1 data from BMW Wallbox charging station.");
                    }
                }

                if (Report2)
                {
                    DataStatus status = _gateway.ReadReport2();

                    if (status.IsGood)
                    {
                        _console.WriteLine($"Value of property '{Property}' = {_gateway.Report2.GetPropertyValue(Property)}");
                    }
                    else
                    {
                        _console.WriteLine($"Error reading report 2 from BMW Wallbox charging station.");
                    }
                }

                if (Report3)
                {
                    DataStatus status = _gateway.ReadReport3();

                    if (status.IsGood)
                    {
                        _console.WriteLine($"Value of property '{Property}' = {_gateway.Report3.GetPropertyValue(Property)}");
                    }
                    else
                    {
                        _console.WriteLine($"Error reading report 3 from BMW Wallbox charging station.");
                    }
                }

                if (Report100)
                {
                    DataStatus status = _gateway.ReadReport100();

                    if (status.IsGood)
                    {
                        _console.WriteLine($"Value of property '{Property}' = {_gateway.Report100.GetPropertyValue(Property)}");
                    }
                    else
                    {
                        _console.WriteLine($"Error reading report 100 from BMW Wallbox charging station.");
                    }
                }

                if (Index.HasValue)
                {
                    int index = Index.Value - WallboxGateway.REPORTS_ID - 1;
                    DataStatus status = _gateway.ReadReports();

                    if (status.IsGood)
                    {
                        _console.WriteLine($"Value of property '{Property}' = {_gateway.Reports[index].GetPropertyValue(Property)}");
                    }
                    else
                    {
                        _console.WriteLine($"Error reading report from BMW Wallbox charging station.");
                    }
                }

                if (Info)
                {
                    DataStatus status = _gateway.ReadInfo();

                    if (status.IsGood)
                    {
                        _console.WriteLine($"Value of property '{Property}' = {_gateway.Info.GetPropertyValue(Property)}");
                    }
                    else
                    {
                        _console.WriteLine($"Error reading info data from BMW Wallbox charging station.");
                    }
                }
            }
        }

        #endregion
    }
}
