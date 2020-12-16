﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RtuReadCommand.cs" company="DTV-Online">
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
    using System.Text.Json;

    using Microsoft.Extensions.Logging;

    using UtilityLib;
    using UtilityLib.Console;

    using ModbusLib;
    using ModbusLib.Models;
    using ModbusApp.Options;

    #endregion

    internal sealed class RtuReadCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RtuReadCommand"/> class.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="logger"></param>
        public RtuReadCommand(IRtuModbusClient client,
                              ILogger<RtuReadCommand> logger)
            : base(logger, "read", "Supporting Modbus RTU read operations.")
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
                .FromAmong("bits", "string", "byte", "short", "ushort", "int", "uint", "nfloat", "double", "long", "ulong"));

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
                    (optionC && (optionD || optionH || optionI))   ||
                    (optionD && (optionC || optionH || optionI))   ||
                    (optionH && (optionD || optionC || optionI))   ||
                    (optionI && (optionD || optionH || optionC))   || optionHelp)
                {
                    return "Specify a single read option (coils, discrete inputs, holding registers, input registers).";
                }

                return null;
            });

            // Setup execution handler.
            Handler = CommandHandler.Create<IConsole, bool, bool, RtuReadCommandOptions>((console, verbose, help, options) =>
            {
                logger.LogInformation("Handler()");

                // Showing the command help output.
                if (help) { this.ShowHelp(console); return (int)ExitCodes.SuccessfullyCompleted; }

                // Run additional checks on options.
                options.CheckOptions(console);

                if (options.Coil || options.Discrete)
                {
                    if ((options.Number < 1) || (options.Number > IModbusClient.MaxBooleanPoints))
                    {
                        throw new ArgumentOutOfRangeException($"Number {options.Number} is out of the range of valid values (1..{IModbusClient.MaxBooleanPoints}).");
                    }
                }

                if (options.Holding || options.Input)
                {
                    if (!string.IsNullOrEmpty(options.Type))
                    {
                        bool ok = options.Type switch
                        {
                            "bits"   => (options.Number > 1) ? throw new ArgumentOutOfRangeException("Only a single bit array value is supported.") : true,
                            "string" => ((options.Number < 1) || ((options.Number + 1) / 2 > IModbusClient.MaxRegisterPoints)) ? throw new ArgumentOutOfRangeException($"Reading string values: options.Number {options.Number} is out of the range (max. {IModbusClient.MaxRegisterPoints} registers).") : true,
                            "byte"   => ((options.Number < 1) || ((options.Number + 1) / 2 > IModbusClient.MaxRegisterPoints)) ? throw new ArgumentOutOfRangeException($"Reading byte values:   options.Number {options.Number} is out of the range (max. {IModbusClient.MaxRegisterPoints} registers).") : true,
                            "short"  => ((options.Number < 1) || (options.Number > IModbusClient.MaxRegisterPoints)) ? throw new ArgumentOutOfRangeException($"Reading short values:  options.Number {options.Number} is out of the range (max. {IModbusClient.MaxRegisterPoints} registers).") : true,
                            "ushort" => ((options.Number < 1) || (options.Number > IModbusClient.MaxRegisterPoints)) ? throw new ArgumentOutOfRangeException($"Reading ushort values: options.Number {options.Number} is out of the range (max. {IModbusClient.MaxRegisterPoints} registers).") : true,
                            "int"    => ((options.Number < 1) || (options.Number > IModbusClient.MaxRegisterPoints / 2)) ? throw new ArgumentOutOfRangeException($"Reading int values:    options.Number {options.Number} is out of the range of (max. {IModbusClient.MaxRegisterPoints} registers).") : true,
                            "uint"   => ((options.Number < 1) || (options.Number > IModbusClient.MaxRegisterPoints / 2)) ? throw new ArgumentOutOfRangeException($"Reading uint values:   options.Number {options.Number} is out of the range of (max. {IModbusClient.MaxRegisterPoints} registers).") : true,
                            "float"  => ((options.Number < 1) || (options.Number > IModbusClient.MaxRegisterPoints / 2)) ? throw new ArgumentOutOfRangeException($"Reading float values:  options.Number {options.Number} is out of the range of (max. {IModbusClient.MaxRegisterPoints} registers).") : true,
                            "double" => ((options.Number < 1) || (options.Number > IModbusClient.MaxRegisterPoints / 4)) ? throw new ArgumentOutOfRangeException($"Reading double values: options.Number {options.Number} is out of the range of (max. {IModbusClient.MaxRegisterPoints} registers).") : true,
                            "long"   => ((options.Number < 1) || (options.Number > IModbusClient.MaxRegisterPoints / 4)) ? throw new ArgumentOutOfRangeException($"Reading long values:   options.Number {options.Number} is out of the range of (max. {IModbusClient.MaxRegisterPoints} registers).") : true,
                            "ulong"  => ((options.Number < 1) || (options.Number > IModbusClient.MaxRegisterPoints / 4)) ? throw new ArgumentOutOfRangeException($"Reading ulong values:  options.Number {options.Number} is out of the range of (max. {IModbusClient.MaxRegisterPoints} registers).") : true,
                            _ => throw new ArgumentOutOfRangeException($"Unknown type '{options.Type}' (should not happen).")
                        };
                    }
                    else
                    {
                        if ((options.Number < 1) || (options.Number > IModbusClient.MaxRegisterPoints))
                        {
                            throw new ArgumentOutOfRangeException($"Number {options.Number} is out of the range of valid values (1..{IModbusClient.MaxBooleanPoints}).");
                        }
                    }
                }

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
                        // Reading coils.
                        if (options.Coil)
                        {
                            CommandHelper.ReadingCoils(console,
                                                       client,
                                                       options.RtuSlave.ID,
                                                       options.Number,
                                                       options.Offset);
                        }

                        // Reading discrete inputs.
                        if (options.Discrete)
                        {
                            CommandHelper.ReadingDiscreteInputs(console,
                                                                client,
                                                                client.RtuSlave.ID,
                                                                options.Number,
                                                                options.Offset);
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
                                                                  options.Hex);
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
                                                               options.Hex);
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
    }
}
