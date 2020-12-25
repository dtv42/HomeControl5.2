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
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using System.CommandLine.IO;
    using System.Linq;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using UtilityLib;
    using UtilityLib.Console;

    using WallboxLib;
    using WallboxLib.Models;

    using WallboxApp.Options;

    #endregion

    /// <summary>
    /// Application command "monitor".
    /// </summary>
    public class MonitorCommand : BaseCommand
    {
        #region Private Data Members

        private readonly JsonSerializerOptions _serializerOptions = JsonExtensions.DefaultSerializerOptions;
        private static readonly AutoResetEvent _closing = new AutoResetEvent(false);

        #endregion Private Data Members

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MonitorCommand"/> class.
        /// </summary>
        /// <param name="gateway"></param>
        /// <param name="logger"></param>
        public MonitorCommand(WallboxGateway gateway, ILogger<MonitorCommand> logger)
            : base(logger, "monitor", "Monitoring data values from from BMW Wallbox charging station.")
        {
            _logger?.LogDebug("MonitorCommand()");

            // Setup command arguments and options.
            AddArgument(new Argument<string>("name", "The property name.").Arity(ArgumentArity.ZeroOrOne));

            AddOption(new Option<bool>  (new string[] { "-1", "--report1" }, "Monitors the report 1 data"));
            AddOption(new Option<bool>  (new string[] { "-2", "--report2" }, "Monitors the report 2 data"));
            AddOption(new Option<bool>  (new string[] { "-3", "--report3" }, "Monitors the report 3 data"));
            AddOption(new Option<ushort>(new string[] { "-n", "--number"  }, "Monitors the specified charging report data (101 - 130)").Name("number").Range(101, 130));
            AddOption(new Option<bool>  (new string[] { "-l", "--last"    }, "Monitors the last charging report data (report 100)")); 
            AddOption(new Option<bool>  (new string[] { "-i", "--info"    }, "Monitors the info data"));
            AddOption(new Option<uint>  ("--repeat", "The number of iterations (default: forever)."));
            AddOption(new Option<uint>  ("--interval", "The seconds between times to read (default: 10)."));

            // Setup execution handler.
            Handler = CommandHandler.Create<IConsole, CancellationToken, GlobalOptions, MonitorOptions>
                (async (console, token, globals, options) =>
                {
                    logger.LogDebug("Handler()");

                    if (!options.CheckOptions(console)) return (int)ExitCodes.IncorrectFunction;

                    if (globals.Verbose)
                    {
                        console.Out.WriteLine($"Commandline Application: {RootCommand.ExecutableName}");
                        console.Out.WriteLine($"Endpoint:  {globals.EndPoint}");
                        console.Out.WriteLine($"Port:      {globals.Port}");
                        console.Out.WriteLine($"Timeout:   {globals.Timeout}");
                        console.Out.WriteLine();
                    }

                    try
                    {
                        bool forever = (options.Repeat == 0);
                        bool header = true;

                        await Task.Factory.StartNew(async () =>
                        {
                            while (!token.IsCancellationRequested)
                            {
                                // Read the specified data.
                                var start = DateTime.UtcNow;

                                console.Out.WriteLine(start.ToLongTimeString());

                                ReadingData(console, gateway, options, header);

                                // Only first call is showing the header.
                                header = false;
                                var end = DateTime.UtcNow;
                                var elapsed = (end - start).TotalMilliseconds;
                                double delay = ((options.Interval * 1000.0) - (end - start).TotalMilliseconds) / 1000.0;

                                console.Out.WriteLine($"Elapsed time: {(elapsed / 1000.0):F2}");

                                if (options.Interval > 0)
                                {
                                    if (delay < 0)
                                    {
                                        console.YellowWriteLine("Monitoring: no time between reads.");
                                    }
                                    else
                                    {
                                        await Task.Delay(TimeSpan.FromSeconds(delay), token);
                                    }
                                }

                                if (!forever && (--options.Repeat <= 0))
                                {
                                    _closing.Set();
                                    break;
                                }
                            }
                        }, token);

                        Console.CancelKeyPress += new ConsoleCancelEventHandler((sender, args) =>
                        {
                            console.Out.WriteLine($"Monitoring cancelled.");
                            _closing.Set();
                        });

                        _closing.WaitOne();
                    }
                    catch (AggregateException aex) when (aex.InnerExceptions.All(e => e is OperationCanceledException))
                    {
                        console.Out.WriteLine($"Monitoring cancelled.");
                        return (int)ExitCodes.OperationCanceled;
                    }
                    catch (OperationCanceledException)
                    {
                        console.Out.WriteLine($"Monitoring cancelled.");
                        return (int)ExitCodes.OperationCanceled;
                    }
                    catch (Exception)
                    {
                        console.Out.WriteLine($"Monitoring exception.");
                        return (int)ExitCodes.OperationCanceled;
                    }

                    return (int)ExitCodes.SuccessfullyCompleted;
                });
        }

        #endregion Constructors

        #region Private Methods

        /// <summary>
        /// Reading the specified data.
        /// </summary>
        /// <param name="console">The command line console.</param>
        /// <param name="gateway">The gateway instance.</param>
        /// <param name="options">The monitor options.</param>
        /// <param name="header">The haeder flag.</param>
        private void ReadingData(IConsole console, WallboxGateway gateway, MonitorOptions options, bool header = false)
        {
            if (string.IsNullOrEmpty(options.Name))
            {
                if (options.Report1)
                {
                    if (header) console.Out.WriteLine($"Monitoring report 1 data.");
                    DataStatus status = gateway.ReadReport1();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Report1:");
                        console.Out.WriteLine(JsonSerializer.Serialize<Report1Data>(gateway.Report1, _serializerOptions));
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading report 1 from BMW Wallbox charging station.");
                    }
                }

                if (options.Report2)
                {
                    if (header) console.Out.WriteLine($"Monitoring report 2 data.");
                    DataStatus status = gateway.ReadReport2();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Report2:");
                        console.Out.WriteLine(JsonSerializer.Serialize<Report2Data>(gateway.Report2, _serializerOptions));
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading report 2 from BMW Wallbox charging station.");
                    }
                }

                if (options.Report3)
                {
                    if (header) console.Out.WriteLine($"Monitoring report 3 data.");
                    DataStatus status = gateway.ReadReport3();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Report3:");
                        console.Out.WriteLine(JsonSerializer.Serialize<Report3Data>(gateway.Report3, _serializerOptions));
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading report 1 from BMW Wallbox charging station.");
                    }
                }

                if (options.Last)
                {
                    if (header) console.Out.WriteLine($"Monitoring last charging report data.");
                    DataStatus status = gateway.ReadReport100();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Report100:");
                        console.Out.WriteLine(JsonSerializer.Serialize<ReportsData>(gateway.Report100, _serializerOptions));
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading report 100 from BMW Wallbox charging station.");
                    }
                }

                if (options.Number > 0)
                {
                    int index = options.Number - WallboxGateway.REPORTS_ID - 1;
                    if (header) console.Out.WriteLine($"Monitoring report {index} data.");
                    DataStatus status = gateway.ReadReports();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Report{options.Number}:");
                        console.Out.WriteLine(JsonSerializer.Serialize<ReportsData>(gateway.Reports[index], _serializerOptions));
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading report from BMW Wallbox charging station.");
                    }
                }

                if (options.Info)
                {
                    console.Out.WriteLine($"Monitoring info data from BMW Wallbox charging station.");
                    DataStatus status = gateway.ReadInfo();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Info:");
                        console.Out.WriteLine(JsonSerializer.Serialize<InfoData>(gateway.Info, _serializerOptions));
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading info data from BMW Wallbox charging station.");
                    }
                }
            }
            else
            {
                if (header) console.Out.WriteLine($"Monitoring property '{options.Name}':");

                if (options.Report1)
                {
                    DataStatus status = gateway.ReadReport1();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.Report1.GetPropertyValue(options.Name)}");
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading report 1 data from BMW Wallbox charging station.");
                    }
                }

                if (options.Report2)
                {
                    DataStatus status = gateway.ReadReport2();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.Report2.GetPropertyValue(options.Name)}");
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading report 2 from BMW Wallbox charging station.");
                    }
                }

                if (options.Report3)
                {
                    DataStatus status = gateway.ReadReport3();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.Report3.GetPropertyValue(options.Name)}");
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading report 3 from BMW Wallbox charging station.");
                    }
                }

                if (options.Last)
                {
                    DataStatus status = gateway.ReadReport100();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.Report100.GetPropertyValue(options.Name)}");
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading report 100 from BMW Wallbox charging station.");
                    }
                }

                if (options.Number > 0)
                {
                    int index = options.Number - WallboxGateway.REPORTS_ID - 1;
                    DataStatus status = gateway.ReadReports();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.Reports[index].GetPropertyValue(options.Name)}");
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading report from BMW Wallbox charging station.");
                    }
                }

                if (options.Info)
                {
                    DataStatus status = gateway.ReadInfo();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.Info.GetPropertyValue(options.Name)}");
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading info data from BMW Wallbox charging station.");
                    }
                }
            }
        }

        #endregion
    }
}
