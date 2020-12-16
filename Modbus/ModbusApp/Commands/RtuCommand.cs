// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RtuCommand.cs" company="DTV-Online">
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
    using System.IO.Ports;
    using System.Text.Json;

    using Microsoft.Extensions.Logging;

    using UtilityLib;
    using UtilityLib.Console;

    using ModbusLib;
    using ModbusLib.Models;
    using ModbusApp.Options;

    #endregion

    /// <summary>
    /// This class implements the RTU command.
    /// </summary>
    internal sealed class RtuCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RtuCommand"/> class.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="rtuReadCommand"></param>
        /// <param name="rtuWriteCommand"></param>
        /// <param name="rtuMonitorCommand"></param>
        /// <param name="settings"></param>
        /// <param name="logger"></param>
        public RtuCommand(IRtuClientSettings options,
                          IRtuModbusClient client,
                          RtuReadCommand rtuReadCommand,
                          RtuWriteCommand rtuWriteCommand,
                          RtuMonitorCommand rtuMonitorCommand,
                          ILogger<RtuCommand> logger)
            : base(logger, "rtu", "Subcommand supporting standard Modbus RTU operations.")
        {
            // Setup command options.
            AddGlobalOption(new Option<string>  ("--serialport",    "Sets the Modbus master COM port"   ).Name("SerialPort"  ).Default(options.RtuMaster.SerialPort  ));
            AddGlobalOption(new Option<int>     ("--baudrate",      "Sets the Modbus COM port baud rate").Name("Baudrate"    ).Default(options.RtuMaster.Baudrate    ).FromAmong(new int[] { 110, 300, 600, 1200, 2400, 4800, 9600, 14400, 19200, 38400, 57600, 115200, 128000, 256000 }));
            AddGlobalOption(new Option<Parity>  ("--parity",        "Sets the Modbus COM port parity"   ).Name("Parity"      ).Default(options.RtuMaster.Parity      ));
            AddGlobalOption(new Option<int>     ("--databits",      "Sets the Modbus COM port data bits").Name("DataBits"    ).Default(options.RtuMaster.DataBits    ).Range(5, 8));
            AddGlobalOption(new Option<StopBits>("--stopbits",      "Sets the Modbus COM port stop bits").Name("StopBits"    ).Default(options.RtuMaster.StopBits    ));
            AddGlobalOption(new Option<byte>    ("--slaveid",       "Sets the Modbus slave ID"          ).Name("SlaveID"     ).Default(options.RtuSlave.ID           ));
            AddGlobalOption(new Option<int>     ("--read-timeout",  "Sets the read timeout"             ).Name("ReadTimeout" ).Default(options.RtuMaster.ReadTimeout ).Range(-1, Int32.MaxValue).IsHidden());
            AddGlobalOption(new Option<int>     ("--write-timeout", "Sets the read timeout"             ).Name("WriteTimeout").Default(options.RtuMaster.WriteTimeout).Range(-1, Int32.MaxValue).IsHidden());

            // Add sub commands.
            AddCommand(rtuReadCommand);
            AddCommand(rtuWriteCommand);
            AddCommand(rtuMonitorCommand);

            // Setup execution handler.
            Handler = CommandHandler.Create<IConsole, bool, RtuCommandOptions>((console, verbose, options) =>
            {
                logger.LogInformation("Handler()");

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
                        Console.WriteLine($"RTU serial port found at {options.RtuMaster.SerialPort}.");
                        return (int)ExitCodes.SuccessfullyCompleted;
                    }
                    else
                    {
                        Console.WriteLine($"RTU serial port not found at {options.RtuMaster.SerialPort}.");
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
            });
        }

        #endregion
    }
}
