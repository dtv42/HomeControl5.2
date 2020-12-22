// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MonitorCommand.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>17-12-2020 12:52</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace ETAPU11App.Commands
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

    using ETAPU11Lib;
    using ETAPU11Lib.Models;

    using ETAPU11App.Options;

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
        public MonitorCommand(ETAPU11Gateway gateway, ILogger<ReadCommand> logger)
            : base(logger, "monitor", "Monitoring data values from an ETA PU 11 pellet boiler.")
        {
            _logger?.LogDebug("MonitorCommand()");

            // Setup command arguments and options.
            AddArgument(new Argument<string>("name", "The property name.").Arity(ArgumentArity.ZeroOrOne));

            AddOption(new Option<bool>(new string[] { "-d", "--data"     }, "Monitors all data"));
            AddOption(new Option<bool>(new string[] { "-b", "--boiler"   }, "Monitors the boiler data."));
            AddOption(new Option<bool>(new string[] { "-w", "--water"    }, "Monitors the hot water data."));
            AddOption(new Option<bool>(new string[] { "-c", "--circuit"  }, "Monitors the heating circuit data."));
            AddOption(new Option<bool>(new string[] { "-s", "--storage"  }, "Monitors the pellets storage data."));
            AddOption(new Option<bool>(new string[] { "-y", "--system"   }, "Monitors the system info data."));
            AddOption(new Option<bool>("--blockmode", "Using block mode read (only used when reading all data)."));
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
                    console.Out.WriteLine($"Slave Address: {globals.TcpSlave.Address}");
                    console.Out.WriteLine($"Slave Port:    {globals.TcpSlave.Port}");
                    console.Out.WriteLine($"Slave ID:      {globals.TcpSlave.ID}");
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
        private void ReadingData(IConsole console, ETAPU11Gateway gateway, MonitorOptions options, bool header = false)
        {
            if (string.IsNullOrEmpty(options.Name))
            {
                if (options.Data)
                {
                    if (header) console.Out.WriteLine($"Monitoring ETAPU11 data.");
                    DataStatus status;

                    if (options.Blockmode)
                    {
                        status = gateway.ReadBlockAll();
                    }
                    else
                    {
                        status = gateway.ReadAll();
                    }

                    if (status.IsGood)
                    {
                        // Fix: Json Serializer cannot serialize NaN
                        if (double.IsNaN(gateway.Data.Flow)) gateway.Data.Flow = 0;
                        if (double.IsNaN(gateway.Data.ResidualO2)) gateway.Data.ResidualO2 = 0;

                        console.Out.WriteLine(JsonSerializer.Serialize<ETAPU11Data>(gateway.Data, _serializerOptions));
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading boiler data from ETAPU11 pellet boiler.");
                    }
                }

                if (options.Boiler)
                {
                    if (header) console.Out.WriteLine($"Monitoring boiler data.");
                    DataStatus status = gateway.ReadBoilerData();

                    if (status.IsGood)
                    {
                        // Fix: Json Serializer cannot serialize NaN
                        if (double.IsNaN(gateway.Data.ResidualO2)) gateway.Data.ResidualO2 = 0;

                        console.Out.WriteLine(JsonSerializer.Serialize<BoilerData>(gateway.BoilerData, _serializerOptions));
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading boiler data from ETAPU11 pellet boiler.");
                    }
                }

                if (options.Water)
                {
                    if (header) console.Out.WriteLine($"Monitoring hotwater data.");
                    DataStatus status = gateway.ReadHotwaterData();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine(JsonSerializer.Serialize<HotwaterData>(gateway.HotwaterData, _serializerOptions));
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading hotwater data from ETAPU11 pellet boiler.");
                    }
                }

                if (options.Circuit)
                {
                    if (header) console.Out.WriteLine($"Monitoring heating data.");
                    DataStatus status = gateway.ReadHeatingData();

                    if (status.IsGood)
                    {
                        // Fix: Json Serializer cannot serialize NaN
                        if (double.IsNaN(gateway.Data.Flow)) gateway.Data.Flow = 0;

                        console.Out.WriteLine(JsonSerializer.Serialize<HeatingData>(gateway.HeatingData, _serializerOptions));
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading heating data from ETAPU11 pellet boiler.");
                    }
                }

                if (options.Storage)
                {
                    if (header) console.Out.WriteLine($"Monitoring storage data.");
                    DataStatus status = gateway.ReadBoilerData();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine(JsonSerializer.Serialize<StorageData>(gateway.StorageData, _serializerOptions));
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading storage data from ETAPU11 pellet boiler.");
                    }
                }

                if (options.System)
                {
                    if (header) console.Out.WriteLine($"Monitoring system data.");
                    DataStatus status = gateway.ReadBoilerData();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine(JsonSerializer.Serialize<SystemData>(gateway.SystemData, _serializerOptions));
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading system data from ETAPU11 pellet boiler.");
                    }
                }
            }
            else
            {
                if (header) console.Out.WriteLine($"Monitoring property '{options.Name}':");
                var status = gateway.ReadProperty(options.Name);

                if (status.IsGood)
                {
                    if (options.Data)
                    {
                        console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.Data.GetPropertyValue(options.Name)}");
                    }

                    if (options.Boiler)
                    {
                        console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.BoilerData.GetPropertyValue(options.Name)}");
                    }
                    
                    if (options.Water)
                    {
                        console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.HotwaterData.GetPropertyValue(options.Name)}");
                    }
                    
                    if (options.Circuit)
                    {
                        console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.HeatingData.GetPropertyValue(options.Name)}");
                    }
                    
                    if (options.Storage)
                    {
                        console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.StorageData.GetPropertyValue(options.Name)}");
                    }
                    
                    if (options.System)
                    {
                        console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.SystemData.GetPropertyValue(options.Name)}");
                    }
                }
                else
                {
                    console.Out.WriteLine($"Error reading property '{options.Name}' from ETAPU11 pellet boiler.");
                }
            }
        }

        #endregion
    }
}
