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
namespace NetatmoApp.Commands
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

    using NetatmoLib;
    using NetatmoLib.Models;

    using NetatmoApp.Options;

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
        public MonitorCommand(NetatmoGateway gateway, ILogger<ReadCommand> logger)
            : base(logger, "monitor", "Monitoring data values from the Netatmo web service.")
        {
            _logger?.LogDebug("MonitorCommand()");

            // Setup command arguments and options.
            AddArgument(new Argument<string>("name", "The property name.").Arity(ArgumentArity.ZeroOrOne));

            AddOption(new Option<bool>(new string[] { "-d", "--data"    }, "Monitors all data"));
            AddOption(new Option<bool>(new string[] { "-m", "--main"    }, "Monitors main station data"));
            AddOption(new Option<bool>(new string[] { "-o", "--outdoor" }, "Monitors the outdoor data"));
            AddOption(new Option<bool>(new string[] { "-1", "--indoor1" }, "Monitors the indoor 1 data"));
            AddOption(new Option<bool>(new string[] { "-2", "--indoor2" }, "Monitors the indoor 2 data"));
            AddOption(new Option<bool>(new string[] { "-3", "--indoor3" }, "Monitors the indoor 3 data"));
            AddOption(new Option<bool>(new string[] { "-r", "--rain"    }, "Monitors rain station data"));
            AddOption(new Option<bool>(new string[] { "-w", "--wind"    }, "Monitors wind station data"));
            AddOption(new Option<uint>("--repeat", "The number of iterations (default: forever)."));
            AddOption(new Option<uint>("--interval", "The seconds between times to read.").Default(10));

            // Setup execution handler.
            Handler = CommandHandler.Create<IConsole, CancellationToken, GlobalOptions, MonitorOptions>
                (async (console, token, globals, options) =>
            {
                logger.LogDebug("Handler()");

                if (!options.CheckOptions(console)) return (int)ExitCodes.IncorrectFunction;

                if (globals.Verbose)
                {
                    console.Out.WriteLine($"Commandline Application: {RootCommand.ExecutableName}");
                    console.Out.WriteLine($"User:          {globals.User}");
                    console.Out.WriteLine($"Password:      {globals.Password}");
                    console.Out.WriteLine($"ClientID:      {globals.ClientID}");
                    console.Out.WriteLine($"ClientSecret:  {globals.ClientSecret}");
                    console.Out.WriteLine($"Address:       {globals.Address}");
                    console.Out.WriteLine($"Timeout:       {globals.Timeout}");
                    console.Out.WriteLine();
                }

                // Update settings with options.
                gateway.Settings.Address      = globals.Address;
                gateway.Settings.Timeout      = globals.Timeout;
                gateway.Settings.User         = globals.User;
                gateway.Settings.Password     = globals.Password;
                gateway.Settings.ClientID     = globals.ClientID;
                gateway.Settings.ClientSecret = globals.ClientSecret;

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
        private void ReadingData(IConsole console, NetatmoGateway gateway, MonitorOptions options, bool header = false)
        {
            DataStatus status = gateway.ReadAll();

            if (status.IsGood)
            {
                if (string.IsNullOrEmpty(options.Name))
                {
                    if (options.Data)
                    {
                        if (header) console.Out.WriteLine($"Monitoring Netatmo data:");
                        console.Out.WriteLine(JsonSerializer.Serialize<NetatmoData>(gateway.Data, _serializerOptions));
                    }

                    if (options.Main)
                    {
                        if (header) console.Out.WriteLine($"Monitoring Netatmo main data:");
                        console.Out.WriteLine(JsonSerializer.Serialize<MainData>(gateway.Main, _serializerOptions));
                    }

                    if (options.Outdoor)
                    {
                        if (header) console.Out.WriteLine($"Monitoring Netatmo outdoor data:");
                        console.Out.WriteLine(JsonSerializer.Serialize<OutdoorData>(gateway.Outdoor, _serializerOptions));
                    }

                    if (options.Indoor1)
                    {
                        if (header) console.Out.WriteLine($"Monitoring Netatmo indoor 1 data:");
                        console.Out.WriteLine(JsonSerializer.Serialize<IndoorData>(gateway.Indoor1, _serializerOptions));
                    }

                    if (options.Indoor2)
                    {
                        if (header) console.Out.WriteLine($"Monitoring Netatmo indoor 2 data:");
                        console.Out.WriteLine(JsonSerializer.Serialize<IndoorData>(gateway.Indoor2, _serializerOptions));
                    }

                    if (options.Indoor3)
                    {
                        if (header) console.Out.WriteLine($"Monitoring Netatmo indoor 3 data:");
                        console.Out.WriteLine(JsonSerializer.Serialize<IndoorData>(gateway.Indoor3, _serializerOptions));
                    }

                    if (options.Rain)
                    {
                        if (header) console.Out.WriteLine($"Monitoring Netatmo rain data:");
                        console.Out.WriteLine(JsonSerializer.Serialize<RainData>(gateway.Rain, _serializerOptions));
                    }

                    if (options.Wind)
                    {
                        if (header) console.Out.WriteLine($"Monitoring Netatmo wind data:");
                        console.Out.WriteLine(JsonSerializer.Serialize<WindData>(gateway.Wind, _serializerOptions));
                    }
                }
                else
                {
                    if (header) console.Out.WriteLine($"Monitoring property '{options.Name}':");

                    if (options.Data)
                    {
                        console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.Data.GetPropertyValue(options.Name)}");
                    }

                    if (options.Main)
                    {
                        console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.Main.GetPropertyValue(options.Name)}");
                    }

                    if (options.Outdoor)
                    {
                        console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.Outdoor.GetPropertyValue(options.Name)}");
                    }

                    if (options.Indoor1)
                    {
                        console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.Indoor1.GetPropertyValue(options.Name)}");
                    }

                    if (options.Indoor2)
                    {
                        console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.Indoor2.GetPropertyValue(options.Name)}");
                    }

                    if (options.Indoor3)
                    {
                        console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.Indoor3.GetPropertyValue(options.Name)}");
                    }

                    if (options.Rain)
                    {
                        console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.Rain.GetPropertyValue(options.Name)}");
                    }

                    if (options.Wind)
                    {
                        console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.Wind.GetPropertyValue(options.Name)}");
                    }
                }
            }
            else
            {
                console.Out.WriteLine($"Error reading data from Netatmo web service.");
            }
        }

        #endregion Private Methods
    }
}