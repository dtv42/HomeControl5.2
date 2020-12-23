// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MonitorCommand.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>26-4-2020 10:05</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace HeliosApp.Commands
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

    using HeliosLib;
    using HeliosLib.Models;

    using HeliosApp.Options;

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
        public MonitorCommand(HeliosGateway gateway, ILogger<ReadCommand> logger)
            : base(logger, "monitor", "Monitoring data values from Helios KWL EC 200 ventilation system.")
        {
            _logger?.LogDebug("MonitorCommand()");

            // Setup command arguments and options.
            AddArgument(new Argument<string>("name", "The property name.").Arity(ArgumentArity.ZeroOrOne));

            // The new help option is allowing the use of a -h option.
            AddOption(new Option<bool>(new string[] { "-?", "--help", "/?", "/help" }, "Show help and usage information"));

            AddOption(new Option<bool>(new string[] { "-a", "--alldata" }, "Reads all data."));
            AddOption(new Option<bool>(new string[] { "-b", "--booster" }, "Reads the booster data."));
            AddOption(new Option<bool>(new string[] { "-d", "--device" }, "Reads the device data."));
            AddOption(new Option<bool>(new string[] { "-e", "--error" }, "Reads the current error data."));
            AddOption(new Option<bool>(new string[] { "-f", "--fan" }, "Reads the fan data."));
            AddOption(new Option<bool>(new string[] { "-h", "--heater" }, "Reads the heater data."));
            AddOption(new Option<bool>(new string[] { "-i", "--info" }, "Reads the info data."));
            AddOption(new Option<bool>(new string[] { "-n", "--network" }, "Reads the network data."));
            AddOption(new Option<bool>(new string[] { "-o", "--operation" }, "Reads the initial operation data."));
            AddOption(new Option<bool>(new string[] { "-p", "--display" }, "Reads the current system status data."));
            AddOption(new Option<bool>(new string[] { "-s", "--sensor" }, "Reads the sensor data."));
            AddOption(new Option<bool>(new string[] { "-t", "--technical" }, "Reads the technical data."));
            AddOption(new Option<bool>(new string[] { "-v", "--vacation" }, "Reads the vacation data."));
            AddOption(new Option<bool>(new string[] { "-y", "--system" }, "Reads the system data."));
            AddOption(new Option<bool>(new string[] { "-l", "--label" }, "Reads the data by label."));
            AddOption(new Option<bool>("--status", "Shows the data status"));

            // Setup execution handler.
            Handler = CommandHandler.Create<IConsole, CancellationToken, GlobalOptions, bool, MonitorOptions>
                (async (console, token, globals, help, options) =>
                {
                    logger.LogDebug("Handler()");

                    // Showing the command help output.
                    if (help) { this.ShowHelp(console); return (int)ExitCodes.SuccessfullyCompleted; }

                    if (!options.CheckOptions(console)) return (int)ExitCodes.IncorrectFunction;

                    if (globals.Verbose)
                    {
                        console.Out.WriteLine($"Commandline Application: {RootCommand.ExecutableName}");
                        console.Out.WriteLine($"Password:      {globals.Password}");
                        console.Out.WriteLine($"Address:       {globals.Address}");
                        console.Out.WriteLine($"Timeout:       {globals.Timeout}");
                        console.Out.WriteLine();
                    }

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
        private void ReadingData(IConsole console, HeliosGateway gateway, MonitorOptions options, bool header = false)
        {
            if (string.IsNullOrEmpty(options.Name))
            {
                if (options.AllData)
                {
                    if (header) console.Out.WriteLine($"Monitoring Helios data.");
                    DataStatus status = gateway.ReadAll();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Data:");
                        console.Out.WriteLine(JsonSerializer.Serialize<HeliosData>(gateway.Data, _serializerOptions));
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading data from Helios solar inverter.");
                    }
                }

                if (options.Booster)
                {
                    if (header) console.Out.WriteLine($"Monitoring booster data from Helios ventilation system.");
                    DataStatus status = gateway.ReadBoosterData();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Booster:");
                        console.Out.WriteLine(JsonSerializer.Serialize<BoosterData>(gateway.BoosterData, _serializerOptions));
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading booster data from Helios ventilation system.");
                    }
                }

                if (options.Device)
                {
                    if (header) console.Out.WriteLine($"Monitoring device data from Helios ventilation system.");
                    DataStatus status = gateway.ReadDeviceData();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Device:");
                        console.Out.WriteLine(JsonSerializer.Serialize<DeviceData>(gateway.DeviceData, _serializerOptions));
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading device data from Helios ventilation system.");
                    }
                }

                if (options.Display)
                {
                    if (header) console.Out.WriteLine($"Monitoring display data from Helios ventilation system.");
                    DataStatus status = gateway.ReadDisplayData();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Status:");
                        console.Out.WriteLine(JsonSerializer.Serialize<DisplayData>(gateway.DisplayData, _serializerOptions));
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading status data from Helios ventilation system.");
                    }
                }

                if (options.Error)
                {
                    if (header) console.Out.WriteLine($"Monitoring error data from Helios ventilation system.");
                    DataStatus status = gateway.ReadErrorData();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Error:");
                        console.Out.WriteLine(JsonSerializer.Serialize<ErrorData>(gateway.ErrorData, _serializerOptions));
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading error data from Helios ventilation system.");
                    }
                }

                if (options.Fan)
                {
                    if (header) console.Out.WriteLine($"Monitoring fan data from Helios ventilation system.");
                    DataStatus status = gateway.ReadFanData();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Fan:");
                        console.Out.WriteLine(JsonSerializer.Serialize<FanData>(gateway.FanData, _serializerOptions));
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading fan data from Helios ventilation system.");
                    }
                }

                if (options.Heater)
                {
                    if (header) console.Out.WriteLine($"Monitoring heater data from Helios ventilation system.");
                    DataStatus status = gateway.ReadHeaterData();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Heater:");
                        console.Out.WriteLine(JsonSerializer.Serialize<HeaterData>(gateway.HeaterData, _serializerOptions));
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading heater data from Helios ventilation system.");
                    }
                }

                if (options.Info)
                {
                    if (header) console.Out.WriteLine($"Monitoring info data from Helios ventilation system.");
                    DataStatus status = gateway.ReadInfoData();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Info:");
                        console.Out.WriteLine(JsonSerializer.Serialize<InfoData>(gateway.InfoData, _serializerOptions));
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading info data from Helios ventilation system.");
                    }
                }

                if (options.Network)
                {
                    if (header) console.Out.WriteLine($"Monitoring network data from Helios ventilation system.");
                    DataStatus status = gateway.ReadNetworkData();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Network:");
                        console.Out.WriteLine(JsonSerializer.Serialize<NetworkData>(gateway.NetworkData, _serializerOptions));
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading network data from Helios ventilation system.");
                    }
                }

                if (options.Operation)
                {
                    if (header) console.Out.WriteLine($"Monitoring operation data from Helios ventilation system.");
                    DataStatus status = gateway.ReadOperationData();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Operation:");
                        console.Out.WriteLine(JsonSerializer.Serialize<OperationData>(gateway.OperationData, _serializerOptions));
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading operation data from Helios ventilation system.");
                    }
                }

                if (options.Sensor)
                {
                    if (header) console.Out.WriteLine($"Monitoring sensor data from Helios ventilation system.");
                    DataStatus status = gateway.ReadSensorData();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Sensor:");
                        console.Out.WriteLine(JsonSerializer.Serialize<SensorData>(gateway.SensorData, _serializerOptions));
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading sensor data from Helios ventilation system.");
                    }
                }

                if (options.System)
                {
                    if (header) console.Out.WriteLine($"Monitoring system data from Helios ventilation system.");
                    DataStatus status = gateway.ReadSystemData();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"System:");
                        console.Out.WriteLine(JsonSerializer.Serialize<SystemData>(gateway.SystemData, _serializerOptions));
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading system data from Helios ventilation system.");
                    }
                }

                if (options.Technical)
                {
                    if (header) console.Out.WriteLine($"Monitoring technical data from Helios ventilation system.");
                    DataStatus status = gateway.ReadTechnicalData();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Technical:");
                        console.Out.WriteLine(JsonSerializer.Serialize<TechnicalData>(gateway.TechnicalData, _serializerOptions));
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading technical data from Helios ventilation system.");
                    }
                }

                if (options.Vacation)
                {
                    if (header) console.Out.WriteLine($"Monitoring vacation data from Helios ventilation system.");
                    DataStatus status = gateway.ReadVacationData();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Vacation:");
                        console.Out.WriteLine(JsonSerializer.Serialize<VacationData>(gateway.VacationData, _serializerOptions));
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading vacation data from Helios ventilation system.");
                    }
                }
            }
            else
            {
                if (header) console.Out.WriteLine($"Monitoring property '{options.Name}':");

                if (options.AllData)
                {
                    DataStatus status = gateway.ReadAll();

                    if (status.IsGood)
                    {
                        if (options.Label)
                        {
                            console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.Data.GetHeliosValue(options.Name)}");
                        }
                        else
                        {
                            console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.Data.GetPropertyValue(options.Name)}");
                        }
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading data from Helios solar inverter.");
                    }
                }

                if (options.Booster)
                {
                    DataStatus status = gateway.ReadBoosterData();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.BoosterData.GetPropertyValue(options.Name)}");
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading booster data from Helios ventilation system.");
                    }
                }

                if (options.Device)
                {
                    DataStatus status = gateway.ReadDeviceData();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.DeviceData.GetPropertyValue(options.Name)}");
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading device data from Helios ventilation system.");
                    }
                }

                if (options.Display)
                {
                    DataStatus status = gateway.ReadDisplayData();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.DisplayData.GetPropertyValue(options.Name)}");
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading display data from Helios ventilation system.");
                    }
                }

                if (options.Error)
                {
                    DataStatus status = gateway.ReadErrorData();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.ErrorData.GetPropertyValue(options.Name)}");
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading error data from Helios ventilation system.");
                    }
                }

                if (options.Fan)
                {
                    DataStatus status = gateway.ReadFanData();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.FanData.GetPropertyValue(options.Name)}");
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading fan data from Helios ventilation system.");
                    }
                }

                if (options.Heater)
                {
                    DataStatus status = gateway.ReadHeaterData();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.HeaterData.GetPropertyValue(options.Name)}");
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading heater data from Helios ventilation system.");
                    }
                }

                if (options.Info)
                {
                    DataStatus status = gateway.ReadInfoData();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.InfoData.GetPropertyValue(options.Name)}");
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading info data from Helios ventilation system.");
                    }
                }

                if (options.Network)
                {
                    DataStatus status = gateway.ReadNetworkData();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.NetworkData.GetPropertyValue(options.Name)}");
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading network data from Helios ventilation system.");
                    }
                }

                if (options.Operation)
                {
                    DataStatus status = gateway.ReadOperationData();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.OperationData.GetPropertyValue(options.Name)}");
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading initial operation data from Helios ventilation system.");
                    }
                }

                if (options.Sensor)
                {
                    DataStatus status = gateway.ReadSensorData();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.SensorData.GetPropertyValue(options.Name)}");
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading sensor data from Helios ventilation system.");
                    }
                }

                if (options.System)
                {
                    DataStatus status = gateway.ReadSystemData();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.SystemData.GetPropertyValue(options.Name)}");
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading system data from Helios ventilation system.");
                    }
                }

                if (options.Technical)
                {
                    DataStatus status = gateway.ReadTechnicalData();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.TechnicalData.GetPropertyValue(options.Name)}");
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading technical data from Helios ventilation system.");
                    }
                }

                if (options.Vacation)
                {
                    DataStatus status = gateway.ReadVacationData();

                    if (status.IsGood)
                    {
                        console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.VacationData.GetPropertyValue(options.Name)}");
                    }
                    else
                    {
                        console.Out.WriteLine($"Error reading ^vacation data from Helios ventilation system.");
                    }
                }
            }
        }

        #endregion Private Methods
    }
}
