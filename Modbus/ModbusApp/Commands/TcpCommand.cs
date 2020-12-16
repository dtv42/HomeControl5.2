// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TcpCommand.cs" company="DTV-Online">
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

    /// <summary>
    /// This class implements the TCP command.
    /// </summary>
    internal sealed class TcpCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="TcpCommand"/> class.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="tcpReadCommand"></param>
        /// <param name="tcpWriteCommand"></param>
        /// <param name="tcpMonitorCommand"></param>
        /// <param name="settings"></param>
        /// <param name="logger"></param>
        public TcpCommand(ITcpClientSettings options, 
                          ITcpModbusClient client,                         
                          TcpReadCommand tcpReadCommand,
                          TcpWriteCommand tcpWriteCommand,
                          TcpMonitorCommand tcpMonitorCommand,
                          ILogger<TcpCommand> logger)
            : base(logger, "tcp", "Subcommand supporting standard Modbus TCP operations.")
        {
            // Setup command options.
            AddGlobalOption(new Option<string>("--address", "Sets the Modbus slave IP address").Name("Address"       ).Default(options.TcpSlave.Address).IPAddress());
            AddGlobalOption(new Option<int>   ("--port", "Sets the Modbus slave IP port"      ).Name("Port"          ).Default(options.TcpSlave.Port).Range(0, 65535));
            AddGlobalOption(new Option<byte>  ("--slaveid", "Sets the Modbus slave ID"        ).Name("SlaveID"       ).Default(options.TcpSlave.ID));
            AddGlobalOption(new Option<int>   ("--receive-timeout", "Sets the receive timeout").Name("ReceiveTimeout").Default(options.TcpMaster.ReceiveTimeout).Range(0, Int32.MaxValue).IsHidden());
            AddGlobalOption(new Option<int>   ("--send-timeout", "Sets the send timeout"      ).Name("SendTimeout"   ).Default(options.TcpMaster.SendTimeout).Range(0, Int32.MaxValue).IsHidden());

            // Add sub commands.
            AddCommand(tcpReadCommand);
            AddCommand(tcpWriteCommand);
            AddCommand(tcpMonitorCommand);

            // Setup execution handler.
            Handler = CommandHandler.Create<IConsole, bool, TcpCommandOptions>((console, verbose, options) =>
            {
                logger.LogInformation("Handler()");

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
                        console.Out.WriteLine($"Modbus TCP slave found at {options.TcpSlave.Address}:{options.TcpSlave.Port}.");
                        return (int)ExitCodes.SuccessfullyCompleted;
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
            });
        }

        #endregion
    }
}
