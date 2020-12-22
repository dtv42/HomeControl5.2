// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadCommand.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>25-4-2020 18:06</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace FroniusApp.Commands
{
    #region Using Directives

    using System.CommandLine;
    using System.CommandLine.Invocation;
    using System.CommandLine.IO;
    using System.Text.Json;

    using Microsoft.Extensions.Logging;

    using UtilityLib;
    using UtilityLib.Console;

    using FroniusLib;
    using FroniusLib.Models;

    using FroniusApp.Options;

    #endregion

    /// <summary>
    /// Application command "read".
    /// </summary>
    public class ReadCommand : BaseCommand
    {
        #region Private Data Members

        private readonly JsonSerializerOptions _serializerOptions = JsonExtensions.DefaultSerializerOptions;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadCommand"/> class.
        /// </summary>
        /// <param name="gateway"></param>
        /// <param name="logger"></param>
        public ReadCommand(FroniusGateway gateway, ILogger<ReadCommand> logger)
            : base(logger, "read", "Reading data info from Fronius Symo 8.2-3-M solar inverter.")
        {
            _logger?.LogDebug("ReadCommand()");

            // Setup command arguments and options.
            AddArgument(new Argument<string>("name", "The property name.").Arity(ArgumentArity.ZeroOrOne));

            AddOption(new Option<bool>(new string[] { "-d", "--data"     }, "Reads all data"));
            AddOption(new Option<bool>(new string[] { "-c", "--common"   }, "Reads the inverter common data"));
            AddOption(new Option<bool>(new string[] { "-i", "--inverter" }, "Reads the inverter info data"));
            AddOption(new Option<bool>(new string[] { "-m", "--minmax"   }, "Reads the inverter minmax data"));
            AddOption(new Option<bool>(new string[] { "-p", "--phase"    }, "Reads the inverter phase data"));
            AddOption(new Option<bool>(new string[] { "-l", "--logger"   }, "Reads the logger info data"));
            AddOption(new Option<bool>(new string[] { "-s", "--status"   }, "Shows the data status"));

            // Setup execution handler.
            Handler = CommandHandler.Create<IConsole, GlobalOptions, ReadOptions>
                ((console, globals, options) =>
                {
                    logger.LogDebug("Handler()");

                    if (!options.CheckOptions(console)) return (int)ExitCodes.IncorrectFunction;

                    if (globals.Verbose)
                    {
                        console.Out.WriteLine($"Commandline Application: {RootCommand.ExecutableName}");
                        console.Out.WriteLine($"Device ID:     {globals.DeviceID}");
                        console.Out.WriteLine($"Address:       {globals.Address}");
                        console.Out.WriteLine($"Timeout:       {globals.Timeout}");
                        console.Out.WriteLine();
                    }

                    if (gateway.ReadAll().IsGood)
                    {
                        if (string.IsNullOrEmpty(options.Name))
                        {
                            if (options.Data)
                            {
                                console.Out.WriteLine("Reading all data from Fronius solar inverter.");
                                console.Out.WriteLine("Data:");
                                console.Out.WriteLine(JsonSerializer.Serialize<FroniusData>(gateway.Data, _serializerOptions));
                            }

                            if (options.Common)
                            {
                                console.Out.WriteLine("Reading all common inverter data from Fronius solar inverter.");
                                console.Out.WriteLine("Common:");
                                console.Out.WriteLine(JsonSerializer.Serialize<CommonData>(gateway.CommonData, _serializerOptions));
                            }

                            if (options.Inverter)
                            {
                                console.Out.WriteLine("Reading all inverter info data from Fronius solar inverter.");
                                console.Out.WriteLine("Inverter:");
                                console.Out.WriteLine(JsonSerializer.Serialize<InverterInfo>(gateway.InverterInfo, _serializerOptions));
                            }

                            if (options.Logger)
                            {
                                console.Out.WriteLine("Reading all logger info data from Fronius solar inverter.");
                                console.Out.WriteLine("Logger:");
                                console.Out.WriteLine(JsonSerializer.Serialize<LoggerInfo>(gateway.LoggerInfo, _serializerOptions));
                            }

                            if (options.MinMax)
                            {
                                console.Out.WriteLine("Reading all minmax data from Fronius solar inverter.");
                                console.Out.WriteLine("MinMax:");
                                console.Out.WriteLine(JsonSerializer.Serialize<MinMaxData>(gateway.MinMaxData, _serializerOptions));
                            }

                            if (options.Phase)
                            {
                                console.Out.WriteLine("Reading all phase data from Fronius solar inverter.");
                                console.Out.WriteLine("Phase3:");
                                console.Out.WriteLine(JsonSerializer.Serialize<PhaseData>(gateway.PhaseData, _serializerOptions));
                            }
                        }
                        else
                        {
                            if (options.Data)
                            {
                                console.Out.WriteLine($"Value of Fronius data property '{options.Name}' = {gateway.Data.GetPropertyValue(options.Name)}");
                            }

                            if (options.Common)
                            {
                                console.Out.WriteLine($"Value of common inverter data property '{options.Name}' = {gateway.CommonData.GetPropertyValue(options.Name)}");
                            }

                            if (options.Inverter)
                            {
                                console.Out.WriteLine($"Value of inverter info data property '{options.Name}' = {gateway.InverterInfo.GetPropertyValue(options.Name)}");
                            }

                            if (options.Logger)
                            {
                                console.Out.WriteLine($"Value of logger info data property '{options.Name}' = {gateway.LoggerInfo.GetPropertyValue(options.Name)}");
                            }

                            if (options.MinMax)
                            {
                                console.Out.WriteLine($"Value of minmax data property '{options.Name}' = {gateway.MinMaxData.GetPropertyValue(options.Name)}");
                            }

                            if (options.Phase)
                            {
                                console.Out.WriteLine($"Value of phase data property '{options.Name}' = {gateway.PhaseData.GetPropertyValue(options.Name)}");
                            }
                        }
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading all data from Fronius solar inverter.");
                        return (int)ExitCodes.NotSuccessfullyCompleted;
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
