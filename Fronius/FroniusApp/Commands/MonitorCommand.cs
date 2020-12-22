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

    using FroniusLib;
    using FroniusLib.Models;

    using FroniusApp.Options;

    #endregion

    /// <summary>
    /// Application command "monitor".
    /// </summary>
    public class MonitorCommand : BaseCommand
    {
        #region Private Data Members

        private readonly JsonSerializerOptions _serializerOptions = JsonExtensions.DefaultSerializerOptions;
        private static readonly AutoResetEvent _closing = new AutoResetEvent(false);

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="MonitorCommand"/> class.
        /// </summary>
        /// <param name="gateway"></param>
        /// <param name="logger"></param>
        public MonitorCommand(FroniusGateway gateway, ILogger<ReadCommand> logger)
            : base(logger, "monitor", "Monitoring data info from Fronius Symo 8.2-3-M solar inverter.")
        {
            _logger?.LogDebug("MonitorCommand()");

            // Setup command arguments and options.
            AddArgument(new Argument<string>("name", "The property name.").Arity(ArgumentArity.ZeroOrOne));

            AddOption(new Option<bool>(new string[] { "-d", "--data"     }, "Monitors all data"));
            AddOption(new Option<bool>(new string[] { "-c", "--common"   }, "Monitors the inverter common data"));
            AddOption(new Option<bool>(new string[] { "-i", "--inverter" }, "Monitors the inverter info data"));
            AddOption(new Option<bool>(new string[] { "-m", "--minmax"   }, "Monitors the inverter minmax data"));
            AddOption(new Option<bool>(new string[] { "-p", "--phase"    }, "Monitors the inverter phase data"));
            AddOption(new Option<bool>(new string[] { "-l", "--logger"   }, "Monitors the logger info data"));
            AddOption(new Option<uint>("--repeat", "The number of iterations (default: forever)."));
            AddOption(new Option<uint>("--interval", "The seconds between times to read (default: 10)."));

            // Setup execution handler.
            Handler = CommandHandler.Create<IConsole, CancellationToken, GlobalOptions, MonitorOptions>
                (async (console, token, globals, options) =>
                {
                    logger.LogDebug("Handler()");

                    if (!options.CheckOptions(console)) return (int)ExitCodes.IncorrectFunction;

                    if (globals.Verbose)
                    {
                        console.Out.WriteLine($"Commandline Application: {RootCommand.ExecutableName}");
                        console.Out.WriteLine($"Device ID:     {globals.DeviceID}");
                        console.Out.WriteLine($"Address:       {globals.Address}");
                        console.Out.WriteLine($"Timeout:       {globals.Timeout}");
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
        private void ReadingData(IConsole console, FroniusGateway gateway, MonitorOptions options, bool header = false)
        {
            if (string.IsNullOrEmpty(options.Name))
            {
                if (options.Data)
                {
                    if (header) console.Out.WriteLine($"Monitoring Fronius data.");
                    DataStatus status = gateway.ReadAll();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Data:");
                        console.Out.WriteLine(JsonSerializer.Serialize<FroniusData>(gateway.Data, _serializerOptions));
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading data from Fronius solar inverter.");
                    }
                }

                if (options.Common)
                {
                    if (header) console.Out.WriteLine($"Monitoring common inverter data.");
                    DataStatus status = gateway.ReadCommonData();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Common:");
                        console.Out.WriteLine(JsonSerializer.Serialize<CommonData>(gateway.CommonData, _serializerOptions));
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading common inverter data from Fronius solar inverter.");
                    }
                }

                if (options.Inverter)
                {
                    if (header) console.Out.WriteLine($"Monitoring inverter info data.");
                    DataStatus status = gateway.ReadInverterInfo();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Inverter:");
                        console.Out.WriteLine(JsonSerializer.Serialize<InverterInfo>(gateway.InverterInfo, _serializerOptions));
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading hotwater data from Fronius solar inverter.");
                    }
                }

                if (options.Logger)
                {
                    if (header) console.Out.WriteLine($"Monitoring logger info data.");
                    DataStatus status = gateway.ReadLoggerInfo();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Logger:");
                        console.Out.WriteLine(JsonSerializer.Serialize<LoggerInfo>(gateway.LoggerInfo, _serializerOptions));
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading logger info from Fronius solar inverter.");
                    }
                }

                if (options.MinMax)
                {
                    if (header) console.Out.WriteLine($"Monitoring minmax data.");
                    DataStatus status = gateway.ReadMinMaxData();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"MinMax:");
                        console.Out.WriteLine(JsonSerializer.Serialize<MinMaxData>(gateway.MinMaxData, _serializerOptions));
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading minmax data from Fronius solar inverter.");
                    }
                }

                if (options.Phase)
                {
                    if (header) console.Out.WriteLine($"Monitoring phase data.");
                    DataStatus status = gateway.ReadPhaseData();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Phase:");
                        console.Out.WriteLine(JsonSerializer.Serialize<PhaseData>(gateway.PhaseData, _serializerOptions));
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading phase data from Fronius solar inverter.");
                    }
                }
            }
            else
            {
                if (header) console.Out.WriteLine($"Monitoring property '{options.Name}':");

                if (options.Data)
                {
                    DataStatus status = gateway.ReadAll();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.Data.GetPropertyValue(options.Name)}");
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading data from Fronius solar inverter.");
                    }
                }

                if (options.Common)
                {
                    DataStatus status = gateway.ReadCommonData();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.CommonData.GetPropertyValue(options.Name)}");
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading common inverter data from Fronius solar inverter.");
                    }
                }

                if (options.Inverter)
                {
                    DataStatus status = gateway.ReadInverterInfo();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.InverterInfo.GetPropertyValue(options.Name)}");
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading inverter info from Fronius solar inverter.");
                    }
                }

                if (options.Logger)
                {
                    DataStatus status = gateway.ReadLoggerInfo();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.LoggerInfo.GetPropertyValue(options.Name)}");
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading logger info from Fronius solar inverter.");
                    }
                }

                if (options.MinMax)
                {
                    DataStatus status = gateway.ReadMinMaxData();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.MinMaxData.GetPropertyValue(options.Name)}");
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading minmax data from Fronius solar inverter.");
                    }
                }

                if (options.Phase)
                {
                    DataStatus status = gateway.ReadPhaseData();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.PhaseData.GetPropertyValue(options.Name)}");
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading phase data from Fronius solar inverter.");
                    }
                }
            }
        }

        #endregion Private Methods
    }
}
