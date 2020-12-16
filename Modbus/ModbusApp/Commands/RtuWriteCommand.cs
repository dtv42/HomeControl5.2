// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RtuWriteCommand.cs" company="DTV-Online">
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

    internal sealed class RtuWriteCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RtuWriteCommand"/> class.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="logger"></param>
        public RtuWriteCommand(IRtuModbusClient client,
                              ILogger<RtuReadCommand> logger)
            : base(logger, "write", "Supporting Modbus RTU write operations.")
        {
            // The new help option is allowing the use of a -h option.
            AddOption(new Option<bool>  (new string[] { "-?", "--help"    }, "Show help and usage information"));

            // Setup command options.
            AddOption(new Option<string>(new string[] { "-c", "--coil"    }, "Write coil(s)."                 ).Name("Json"));
            AddOption(new Option<string>(new string[] { "-h", "--holding" }, "Writes holding register(s)."    ).Name("Json"));
            AddOption(new Option<bool>  (new string[] { "-x", "--hex"     }, "Writes the HEX values (string)" ));
            AddOption(new Option<ushort>(new string[] { "-o", "--offset"  }, "The offset of the first item."  ).Name("Offset").Default((ushort)0));
            AddOption(new Option<string>(new string[] { "-t", "--type"    }, "Reads the specified data type"  ).Name("Type")
                .FromAmong("bits", "string", "byte", "short", "ushort", "int", "uint", "float", "double", "long", "ulong"));

            // Add custom validation.
            AddValidator(r =>
            {
                var optionHelp = r.OptionResult("-?") is not null;
                var optionC    = r.OptionResult("-c") is not null;
                var optionH    = r.OptionResult("-h") is not null;
                var optionX    = r.OptionResult("-x") is not null;
                var optionO    = r.OptionResult("-o") is not null;
                var optionT    = r.OptionResult("-t") is not null;

                if ((!optionC && !optionH) || (optionC && optionH) || optionHelp)
                {
                    return "Specify a single write option (coils or holding registers).";
                }

                return null;
            });

            // Setup execution handler.
            Handler = CommandHandler.Create<IConsole, bool, bool, RtuWriteCommandOptions>((console, verbose, help, options) =>
            {
                logger.LogInformation("Handler()");

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
                        // Writing coils.
                        CommandHelper.WritingCoils(console,
                                                  client,
                                                  options.RtuSlave.ID,
                                                  options.Offset,
                                                  options.Coil);

                        // Writing holding registers.
                        CommandHelper.WritingHoldingRegisters(console,
                                                             client,
                                                             options.RtuSlave.ID,
                                                             options.Offset,
                                                             options.Holding,
                                                             options.Type,
                                                             options.Hex);
                    }
                    else
                    {
                        Console.WriteLine($"Modbus RTU slave not found at {options.RtuMaster.SerialPort}.");
                        return (int)ExitCodes.NotSuccessfullyCompleted;
                    }
                }
                catch (JsonException jex)
                {
                    console.Out.WriteLine(jex.Message);
                    return (int)ExitCodes.NotSuccessfullyCompleted;
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
