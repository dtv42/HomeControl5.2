﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RtuMonitorCommand.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>20-4-2020 13:29</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace ModbusApp.Commands
{
    #region Using Directives

    using System;
    using System.CommandLine;
    using System.CommandLine.IO;
    using System.CommandLine.Invocation;
    using System.Linq;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using UtilityLib;
    using UtilityLib.Console;
    using ModbusLib;
    using ModbusLib.Models;
    using ModbusApp.Options;

    #endregion

    internal sealed class RtuMonitorCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RtuMonitorCommand"/> class.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="logger"></param>
        public RtuMonitorCommand(IRtuModbusClient client,
                                 ILogger<RtuMonitorCommand> logger)
            : base(logger, "monitor", "Supporting Modbus RTU monitor operations.")
        {
            // The new help option is allowing the use of a -h option.
            AddOption(new Option<bool>  (new string[] { "-?", "--help"     }, "Show help and usage information"));

            // Setup command options.                                                                                    
            AddOption(new Option<bool>  (new string[] { "-c", "--coil"     }, "Reads coil(s)"                  ));
            AddOption(new Option<bool>  (new string[] { "-d", "--discrete" }, "Reads discrete input(s)"        ));
            AddOption(new Option<bool>  (new string[] { "-h", "--holding"  }, "Reads holding register(s)"      ));
            AddOption(new Option<bool>  (new string[] { "-i", "--input"    }, "Reads input register(s)"        ));
            AddOption(new Option<bool>  (new string[] { "-x", "--hex"      }, "Displays the values in HEX"     ));
            AddOption(new Option<ushort>(new string[] { "-n", "--number"   }, "The number of items to read"    ).Name("Number").Default((ushort)1));
            AddOption(new Option<ushort>(new string[] { "-o", "--offset"   }, "The offset of the first item"   ).Name("Offset").Default((ushort)0));
            AddOption(new Option<string>(new string[] { "-t", "--type"     }, "Reads the specified data type"  ).Name("Type")
                .FromAmong("bits", "string", "byte", "short", "ushort", "int", "uint", "float", "double", "long", "ulong"));
            AddOption(new Option<uint>  (new string[] { "-r", "--repeat"   }, "The number of times to read"    ).Name("Repeat").Default((uint)10));
            AddOption(new Option<uint>  (new string[] { "-s", "--seconds"  }, "The seconds between read times" ).Name("Seconds").Default((uint)1));

            // Add custom validation.
            AddValidator(r =>
            {
                var optionHelp = r.OptionResult("-?") is not null;
                var optionC    = r.OptionResult("-c") is not null;
                var optionD    = r.OptionResult("-d") is not null;
                var optionH    = r.OptionResult("-h") is not null;
                var optionI    = r.OptionResult("-i") is not null;
                var optionX    = r.OptionResult("-x") is not null;
                var optionN    = r.OptionResult("-n") is not null;
                var optionO    = r.OptionResult("-o") is not null;
                var optionT    = r.OptionResult("-t") is not null;

                if ((!optionC && !optionD && !optionH && !optionI) ||
                     (optionC && (optionD || optionH || optionI))  ||
                     (optionD && (optionC || optionH || optionI))  ||
                     (optionH && (optionD || optionC || optionI))  ||
                     (optionI && (optionD || optionH || optionC))  || optionHelp)
                {
                    return "Specify a single read option (coils, discrete inputs, holding registers, input registers).";
                }

                return null;
            });

            // Setup execution handler.
            Handler = CommandHandler.Create<IConsole, CancellationToken, bool, bool, RtuMonitorCommandOptions>(async (console, token, verbose, help, options) =>
            {
                logger.LogInformation("Handler()");

                // Showing the command help output.
                if (help) { this.ShowHelp(console); return (int)ExitCodes.SuccessfullyCompleted; }

                // Run additional checks on options.
                options.CheckOptions(console);

                // Using RTU client options.
                client.RtuMaster.SerialPort   = options.RtuMaster.SerialPort;
                client.RtuMaster.Baudrate     = options.RtuMaster.Baudrate;
                client.RtuMaster.Parity       = options.RtuMaster.Parity;
                client.RtuMaster.DataBits     = options.RtuMaster.DataBits;
                client.RtuMaster.StopBits     = options.RtuMaster.StopBits;
                client.RtuMaster.ReadTimeout  = options.RtuMaster.ReadTimeout;
                client.RtuMaster.WriteTimeout = options.RtuMaster.WriteTimeout;
                client.RtuSlave.ID            = options.RtuSlave.ID;

                if (verbose)
                {
                    console.Out.WriteLine($"Modbus Commandline Application: {RootCommand.ExecutableName}");
                    console.Out.WriteLine();
                    console.Out.Write("RtuMasterData: ");
                    console.Out.WriteLine(JsonSerializer.Serialize<RtuMasterData>(client.RtuMaster, _jsonoptions));
                    console.Out.Write("RtuSlaveData: ");
                    console.Out.WriteLine(JsonSerializer.Serialize<RtuSlaveData>(client.RtuSlave, _jsonoptions));
                    console.Out.WriteLine();
                }

                try
                {
                    if (client.Connect())
                    {
                        try
                        {
                            bool forever = (options.Repeat == 0);
                            bool header = true;
                            var time = DateTime.UtcNow;

                            while (!token.IsCancellationRequested)
                            {
                                var start = DateTime.UtcNow;
                                if (verbose && !header) console.Out.WriteLine($"Time elapsed {start - time:d'.'hh':'mm':'ss'.'fff}");
                                ReadingData(client, console, logger, options, header);
                                // Only first call is printing the header.
                                header = false;
                                var end = DateTime.UtcNow;
                                double delay = options.Seconds - (end - start).TotalSeconds;

                                if (delay < 0)
                                {
                                    logger?.LogWarning($"Monitoring: no time between reads (min. {delay + options.Seconds}).");
                                }

                                if ((--options.Repeat > 0) && delay > 0)
                                {
                                    await Task.Delay(TimeSpan.FromSeconds(delay), token);
                                }

                                if (!forever && (options.Repeat == 0))
                                {
                                    break;
                                }
                            }
                        }
                        catch (AggregateException aex) when (aex.InnerExceptions.All(e => e is OperationCanceledException))
                        {
                            console.Out.WriteLine("Monitoring cancelled.");
                        }
                        catch (OperationCanceledException)
                        {
                            console.Out.WriteLine("Monitoring cancelled.");
                        }
                        catch (Exception ex)
                        {
                            console.Out.WriteLine($"Exception: {ex.Message}");
                            return (int)ExitCodes.UnhandledException;
                        }
                    }
                    else
                    {
                        console.Out.WriteLine($"Modbus RTU slave not found at {options.RtuMaster.SerialPort}.");
                        return (int)ExitCodes.NotSuccessfullyCompleted;
                    }
                }
                catch (Exception ex)
                {
                    console.Out.WriteLine($"Exception: {ex.Message}");
                    return (int)ExitCodes.UnhandledException;
                }
                finally
                {
                    if (client.Connected)
                    {
                        client.Disconnect();
                    }
                }

                return (int)ExitCodes.SuccessfullyCompleted;
            });
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Reading the specified data.
        /// </summary>
        private static void ReadingData(IRtuModbusClient client,
                                        IConsole console,
                                        ILogger<RtuMonitorCommand>? logger,
                                        RtuMonitorCommandOptions options,
                                        bool header)
        {
            logger?.LogDebug("TcpMonitor: Reading data...");

            // Reading coils.
            if (options.Coil)
            {
                CommandHelper.ReadingCoils(console,
                                           client,
                                           options.RtuSlave.ID,
                                           options.Number,
                                           options.Offset,
                                           header);
            }

            // Reading discrete inputs.
            if (options.Discrete)
            {
                CommandHelper.ReadingDiscreteInputs(console,
                                                    client,
                                                    options.RtuSlave.ID,
                                                    options.Number,
                                                    options.Offset,
                                                    header);
            }

            // Reading holding registers.
            if (options.Holding)
            {
                CommandHelper.ReadingHoldingRegisters(console,
                                                      client,
                                                      options.RtuSlave.ID,
                                                      options.Number,
                                                      options.Offset,
                                                      options.Type,
                                                      options.Hex,
                                                      header);
            }

            // Reading input registers.
            if (options.Input)
            {
                CommandHelper.ReadingInputRegisters(console,
                                                    client,
                                                    options.RtuSlave.ID,
                                                    options.Number,
                                                    options.Offset,
                                                    options.Type,
                                                    options.Hex,
                                                    header);
            }
        }

        #endregion
    }
}
