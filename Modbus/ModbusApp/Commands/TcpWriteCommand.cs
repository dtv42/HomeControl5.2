// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TcpWriteCommand.cs" company="DTV-Online">
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

    internal sealed class TcpWriteCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TcpWriteCommand"/> class.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="logger"></param>
        public TcpWriteCommand(ITcpModbusClient client,
                               ILogger<TcpWriteCommand> logger)
            : base(logger, "write", "Supporting Modbus TCP write operations.")
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
                var optionN    = r.OptionResult("-n") is not null;
                var optionO    = r.OptionResult("-o") is not null;
                var optionT    = r.OptionResult("-t") is not null;

                if ((!optionC && !optionH) || (optionC && optionH) || optionHelp)
                {
                    return "Specify a single write option (coils or holding registers).";
                }

                return null;
            });

            // Setup execution handler.
            Handler = CommandHandler.Create<IConsole, bool, bool, TcpWriteCommandOptions>((console, verbose, help, options) =>
            {
                logger.LogInformation("Handler()");

                // Showing the command help output.
                if (help) { this.ShowHelp(console); return (int)ExitCodes.SuccessfullyCompleted; }

                // Run additional checks on options.
                options.CheckOptions(console);

                // Using TCP client options.
                client.TcpSlave.Address         = options.TcpSlave.Address;
                client.TcpSlave.Port            = options.TcpSlave.Port;
                client.TcpSlave.ID              = options.TcpSlave.ID;
                client.TcpMaster.ReceiveTimeout = options.TcpMaster.ReceiveTimeout;
                client.TcpMaster.SendTimeout    = options.TcpMaster.SendTimeout;

                if (verbose)
                {
                    console.Out.WriteLine($"Modbus Commandline Application: {RootCommand.ExecutableName}");
                    console.Out.WriteLine();
                    console.Out.Write("TcpMasterData: ");
                    console.Out.WriteLine(JsonSerializer.Serialize<TcpMasterData>(client.TcpMaster, _jsonoptions));
                    console.Out.Write("TcpSlaveData: ");
                    console.Out.WriteLine(JsonSerializer.Serialize<TcpSlaveData>(client.TcpSlave, _jsonoptions));
                    console.Out.WriteLine();
                }

                try
                {
                    if (client.Connect())
                    {
                        // Writing coils.
                        CommandHelper.WritingCoils(console,
                                                  client,
                                                  options.TcpSlave.ID,
                                                  options.Offset,
                                                  options.Coil);

                        // Writing holding registers.
                        CommandHelper.WritingHoldingRegisters(console,
                                                             client,
                                                             options.TcpSlave.ID,
                                                             options.Offset,
                                                             options.Holding,
                                                             options.Type,
                                                             options.Hex);
                    }
                    else
                    {
                        console.Out.WriteLine($"Modbus TCP slave not found at {options.TcpSlave.Address}:{options.TcpSlave.Port}.");
                        return (int)ExitCodes.NotSuccessfullyCompleted;
                    }
                }
                catch (JsonException jex)
                {
                    logger.LogError(jex, $"Exception parsing JSON data values.");
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
