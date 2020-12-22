// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadCommand.cs" company="DTV-Online">
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

    using System.CommandLine;
    using System.CommandLine.Invocation;
    using System.CommandLine.IO;
    using System.Text.Json;

    using Microsoft.Extensions.Logging;

    using UtilityLib;
    using UtilityLib.Console;

    using ETAPU11Lib;
    using ETAPU11Lib.Models;

    using ETAPU11App.Options;

    #endregion

    /// <summary>
    /// Application command "read".
    /// </summary>
    public class ReadCommand : BaseCommand
    {
        #region Private Data Members

        private readonly JsonSerializerOptions _serializerOptions = JsonExtensions.DefaultSerializerOptions;

        #endregion Private Data Members

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadCommand"/> class.
        /// </summary>
        /// <param name="gateway"></param>
        /// <param name="logger"></param>
        public ReadCommand(ETAPU11Gateway gateway, ILogger<ReadCommand> logger)
            : base(logger, "read", "Reading data values from an ETA PU 11 pellet boiler.")
        {
            _logger?.LogDebug("ReadCommand()");

            // Setup command arguments and options.
            AddArgument(new Argument<string>("name", "The property name.").Arity(ArgumentArity.ZeroOrOne));

            AddOption(new Option<bool>(new string[] { "-d", "--data"    }, "Reads all data"));
            AddOption(new Option<bool>(new string[] { "-b", "--boiler"  }, "Reads the boiler data."));
            AddOption(new Option<bool>(new string[] { "-w", "--water"   }, "Reads the hot water data."));
            AddOption(new Option<bool>(new string[] { "-c", "--circuit" }, "Reads the heating circuit data."));
            AddOption(new Option<bool>(new string[] { "-s", "--storage" }, "Reads the pellets storage data."));
            AddOption(new Option<bool>(new string[] { "-y", "--system"  }, "Reads the system info data."));
            AddOption(new Option<bool>("--blockmode", "Using block mode read (only used when reading all data)."));
            AddOption(new Option<bool>("--status", "Shows the data status"));

            // Setup execution handler.
            Handler = CommandHandler.Create<IConsole, GlobalOptions, ReadOptions>
                ((console, globals, options) =>
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

                if (string.IsNullOrEmpty(options.Name))
                {
                    if (options.Data)
                    {
                        console.Out.WriteLine("Reading all data from ETAPU11 pellet boiler.");
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

                            console.Out.WriteLine("Data:");
                            console.Out.WriteLine(JsonSerializer.Serialize<ETAPU11Data>(gateway.Data, _serializerOptions));
                        }
                        else
                        {
                            console.Out.WriteLine($"Error reading ETAPU11 data from ETAPU11 pellet boiler.");
                            return (int)ExitCodes.NotSuccessfullyCompleted;
                        }
                    }

                    if (options.Boiler)
                    {
                        console.Out.WriteLine($"Reading boiler data from ETAPU11 pellet boiler.");
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
                            return (int)ExitCodes.NotSuccessfullyCompleted;
                        }
                    }

                    if (options.Water)
                    {
                        console.Out.WriteLine($"Reading hotwater data from ETAPU11 pellet boiler.");
                        DataStatus status = gateway.ReadHotwaterData();

                        if (status.IsGood)
                        {
                            console.Out.WriteLine(JsonSerializer.Serialize<HotwaterData>(gateway.HotwaterData, _serializerOptions));
                        }
                        else
                        {
                            console.Out.WriteLine($"Error reading hotwater data from ETAPU11 pellet boiler.");
                            return (int)ExitCodes.NotSuccessfullyCompleted;
                        }
                    }

                    if (options.Circuit)
                    {
                        console.Out.WriteLine($"Reading heating circuit data from ETAPU11 pellet boiler.");
                        DataStatus status = gateway.ReadHeatingData();

                        if (status.IsGood)
                        {
                            // Fix: Json Serializer cannot serialize NaN
                            if (double.IsNaN(gateway.Data.Flow)) gateway.Data.Flow = 0;

                            console.Out.WriteLine(JsonSerializer.Serialize<HeatingData>(gateway.HeatingData, _serializerOptions));
                        }
                        else
                        {
                            console.Out.WriteLine($"Error reading heating circuit data from ETAPU11 pellet boiler.");
                            return (int)ExitCodes.NotSuccessfullyCompleted;
                        }
                    }

                    if (options.Storage)
                    {
                        console.Out.WriteLine($"Reading pellet storage data from ETAPU11 pellet boiler.");
                        DataStatus status = gateway.ReadBoilerData();

                        if (status.IsGood)
                        {
                            console.Out.WriteLine(JsonSerializer.Serialize<StorageData>(gateway.StorageData, _serializerOptions));
                        }
                        else
                        {
                            console.Out.WriteLine($"Error reading pellet storage data from ETAPU11 pellet boiler.");
                            return (int)ExitCodes.NotSuccessfullyCompleted;
                        }
                    }

                    if (options.System)
                    {
                        console.Out.WriteLine($"Reading system info data from ETAPU11 pellet boiler.");
                        DataStatus status = gateway.ReadBoilerData();

                        if (status.IsGood)
                        {
                            console.Out.WriteLine(JsonSerializer.Serialize<SystemData>(gateway.SystemData, _serializerOptions));
                        }
                        else
                        {
                            console.Out.WriteLine($"Error reading system info data from ETAPU11 pellet boiler.");
                            return (int)ExitCodes.NotSuccessfullyCompleted;
                        }
                    }
                }
                else
                {
                    console.Out.WriteLine($"Reading property '{options.Name}' from ETAPU11 pellet boiler");
                    var status = gateway.ReadProperty(options.Name);

                    if (status.IsGood)
                    {
                        if (options.Data)
                        {
                            console.Out.WriteLine($"Value of EM300LR data property '{options.Name}' = {gateway.Data.GetPropertyValue(options.Name)}");
                        }

                        if (options.Boiler)
                        {
                            console.Out.WriteLine($"Value of total data property '{options.Name}' = {gateway.BoilerData.GetPropertyValue(options.Name)}");
                        }

                        if (options.Water)
                        {
                            console.Out.WriteLine($"Value of phase 1 data property '{options.Name}' = {gateway.HotwaterData.GetPropertyValue(options.Name)}");
                        }

                        if (options.Circuit)
                        {
                            console.Out.WriteLine($"Value of phase 2 data property '{options.Name}' = {gateway.HeatingData.GetPropertyValue(options.Name)}");
                        }

                        if (options.Storage)
                        {
                            console.Out.WriteLine($"Value of phase 3 data property '{options.Name}' = {gateway.StorageData.GetPropertyValue(options.Name)}");
                        }

                        if (options.System)
                        {
                            console.Out.WriteLine($"Value of phase 3 data property '{options.Name}' = {gateway.SystemData.GetPropertyValue(options.Name)}");
                        }
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading property '{options.Name}' from ETAPU11 pellet boiler.");
                        return (int)ExitCodes.NotSuccessfullyCompleted;
                    }
                }

                if (options.Status)
                {
                    console.Out.WriteLine($"Status:");
                    console.Out.WriteLine(JsonSerializer.Serialize<DataStatus>(gateway.Status, _serializerOptions));
                }

                return (int)ExitCodes.SuccessfullyCompleted;
            });
        }

        #endregion Constructors
    }
}
