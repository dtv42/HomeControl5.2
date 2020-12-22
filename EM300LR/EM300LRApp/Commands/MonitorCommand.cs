// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MonitorCommand.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>17-12-2020 12:51</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace EM300LRApp.Commands
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

    using EM300LRLib;
    using EM300LRLib.Models;

    using EM300LRApp.Options;

    #endregion Using Directives

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
        public MonitorCommand(EM300LRGateway gateway, ILogger<ReadCommand> logger)
            : base(logger, "monitor", "Monitoring data values from b-Control EM300LR energy manager.")
        {
            _logger?.LogDebug("MonitorCommand()");

            // Setup command arguments and options.
            AddArgument(new Argument<string>("name", "The property name.").Arity(ArgumentArity.ZeroOrOne));

            AddOption(new Option<bool>(new string[] { "-d", "--data"     }, "Monitors all data"));
            AddOption(new Option<bool>(new string[] { "-t", "--total"    }, "Monitors the total data"));
            AddOption(new Option<bool>(new string[] { "-1", "--phase1"   }, "Monitors the phase 1 data"));
            AddOption(new Option<bool>(new string[] { "-2", "--phase2"   }, "Monitors the phase 2 data"));
            AddOption(new Option<bool>(new string[] { "-3", "--phase3"   }, "Monitors the phase 3 data"));
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
                    console.Out.WriteLine($"Password:      {globals.Password}");
                    console.Out.WriteLine($"Serialnumber:  {globals.SerialNumber}");
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
        private void ReadingData(IConsole console, EM300LRGateway gateway, MonitorOptions options, bool header = false)
        {
            DataStatus status = gateway.ReadAll();

            if (status.IsGood)
            {
                if (string.IsNullOrEmpty(options.Name))
                {
                    if (options.Data)
                    {
                        if (header) console.Out.WriteLine($"Monitoring EM300LR data:");
                        console.Out.WriteLine(JsonSerializer.Serialize<EM300LRData>(gateway.Data, _serializerOptions));
                    }

                    if (options.Total)
                    {
                        if (header) console.Out.WriteLine($"Monitoring total data:");
                        console.Out.WriteLine(JsonSerializer.Serialize<TotalData>(gateway.TotalData, _serializerOptions));
                    }

                    if (options.Phase1)
                    {
                        if (header) console.Out.WriteLine($"Monitoring phase 1 data:");
                        console.Out.WriteLine(JsonSerializer.Serialize<Phase1Data>(gateway.Phase1Data, _serializerOptions));
                    }

                    if (options.Phase2)
                    {
                        if (header) console.Out.WriteLine($"Monitoring phase 2 data:");
                        console.Out.WriteLine(JsonSerializer.Serialize<Phase2Data>(gateway.Phase2Data, _serializerOptions));
                    }

                    if (options.Phase3)
                    {
                        if (header) console.Out.WriteLine($"Monitoring phase 3 data:");
                        console.Out.WriteLine(JsonSerializer.Serialize<Phase3Data>(gateway.Phase3Data, _serializerOptions));
                    }
                }
                else
                {
                    if (header) console.Out.WriteLine($"Monitoring property '{options.Name}':");

                    if (options.Data)
                    {
                        console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.Data.GetPropertyValue(options.Name)}");
                    }

                    if (options.Total)
                    {
                        console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.TotalData.GetPropertyValue(options.Name)}");
                    }

                    if (options.Phase1)
                    {
                        console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.Phase1Data.GetPropertyValue(options.Name)}");
                    }

                    if (options.Phase2)
                    {
                        console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.Phase2Data.GetPropertyValue(options.Name)}");
                    }

                    if (options.Phase3)
                    {
                        console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.Phase3Data.GetPropertyValue(options.Name)}");
                    }
                }
            }
            else
            {
                console.Out.WriteLine($"Error reading data from EM300LR energy manager.");
            }
        }

        #endregion Private Methods
    }
}