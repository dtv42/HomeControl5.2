// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TcpReadCommand.cs" company="DTV-Online">
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

    internal sealed class TcpReadCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TcpReadCommand"/> class.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="logger"></param>
        public TcpReadCommand(ITcpModbusClient client,
                              ILogger<TcpReadCommand> logger)
            : base(logger, "read", "Supporting Modbus TCP read operations.")
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
            Handler = CommandHandler.Create<IConsole, bool, bool, TcpReadCommandOptions>((console, verbose, help, options) =>
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
                        // Reading coils.
                        if (options.Coil)
                        {
                            CommandHelper.ReadingCoils(console,
                                                       client,
                                                       options.TcpSlave.ID,
                                                       options.Number,
                                                       options.Offset);
                        }

                        // Reading discrete inputs.
                        if (options.Discrete)
                        {
                            CommandHelper.ReadingDiscreteInputs(console,
                                                                client,
                                                                options.TcpSlave.ID,
                                                                options.Number,
                                                                options.Offset);
                        }

                        // Reading holding registers.
                        if (options.Holding)
                        {
                            CommandHelper.ReadingHoldingRegisters(console,
                                                                  client,
                                                                  options.TcpSlave.ID,
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
                                                                options.TcpSlave.ID,
                                                                options.Number,
                                                                options.Offset,
                                                                options.Type,
                                                                options.Hex);
                        }
                    }
                    else
                    {
                        console.Out.WriteLine($"Modbus TCP slave not found at {options.TcpSlave.Address}:{options.TcpSlave.Port}.");
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
