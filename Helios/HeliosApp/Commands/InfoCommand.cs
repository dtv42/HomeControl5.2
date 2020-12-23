// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InfoCommand.cs" company="DTV-Online">
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
    using System.Collections.Generic;
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using System.CommandLine.IO;
    using System.Linq;

    using Microsoft.Extensions.Logging;

    using UtilityLib;
    using UtilityLib.Console;

    using HeliosLib.Models;
    using HeliosApp.Options;

    #endregion

    /// <summary>
    /// Application command "info".
    /// </summary>
    public sealed class InfoCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        ///  Initializes a new instance of the <see cref="InfoCommand"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        public InfoCommand(ILogger<InfoCommand> logger)
            : base(logger, "info", "Showing data info from Helios KWL EC 200 ventilation system.")
        {
            logger.LogDebug("InfoCommand()");

            // Setup command arguments and options.
            AddArgument(new Argument<string>("name", "The property name.").Arity(ArgumentArity.ZeroOrOne));

            // The new help option is allowing the use of a -h option.
            AddOption(new Option<bool>(new string[] { "-?", "--help", "/?", "/help" }, "Show help and usage information"));

            AddOption(new Option<bool>(new string[] { "-a", "--alldata"   }, "Gets all data."));
            AddOption(new Option<bool>(new string[] { "-b", "--booster"   }, "Gets the booster data."));
            AddOption(new Option<bool>(new string[] { "-d", "--device"    }, "Gets the device data."));
            AddOption(new Option<bool>(new string[] { "-e", "--error"     }, "Gets the current error data."));
            AddOption(new Option<bool>(new string[] { "-f", "--fan"       }, "Gets the fan data."));
            AddOption(new Option<bool>(new string[] { "-h", "--heater"    }, "Gets the heater data."));
            AddOption(new Option<bool>(new string[] { "-i", "--info"      }, "Gets the info data."));
            AddOption(new Option<bool>(new string[] { "-n", "--network"   }, "Gets the network data."));
            AddOption(new Option<bool>(new string[] { "-o", "--operation" }, "Gets the initial operation data."));
            AddOption(new Option<bool>(new string[] { "-p", "--display"   }, "Gets the current system status data."));
            AddOption(new Option<bool>(new string[] { "-s", "--sensor"    }, "Gets the sensor data."));
            AddOption(new Option<bool>(new string[] { "-t", "--technical" }, "Gets the technical data."));
            AddOption(new Option<bool>(new string[] { "-v", "--vacation"  }, "Gets the vacation data."));
            AddOption(new Option<bool>(new string[] { "-y", "--system"    }, "Gets the system data."));
            AddOption(new Option<bool>(new string[] { "-l", "--label"     }, "Gets the data by label."));

            // Setup execution handler.
            Handler = CommandHandler.Create<IConsole, GlobalOptions, bool, InfoOptions>
                ((console, globals, help, options) =>
                {
                    logger.LogDebug("Handler()");

                    // Showing the command help output.
                    if (help) { this.ShowHelp(console); return (int)ExitCodes.SuccessfullyCompleted; }

                    if (!options.CheckOptions(console)) return (int)ExitCodes.IncorrectFunction;

                    if (globals.Verbose)
                    {
                        console.Out.WriteLine($"Commandline Application: {RootCommand.ExecutableName}");
                        console.Out.WriteLine();
                    }

                    if (string.IsNullOrEmpty(options.Name))
                    {
                        if (options.AllData)
                        {
                            console.Out.WriteLine($"Data:");

                            if (options.Label)
                            {
                                ShowLabels(console);
                            }
                            else
                            {
                                ShowProperties(console, typeof(HeliosData));
                            }
                        }

                        if (options.Booster)
                        {
                            console.Out.WriteLine($"Booster:");
                            ShowProperties(console, typeof(BoosterData));
                        }
                        if (options.Device)
                        {
                            console.Out.WriteLine($"Device:");
                            ShowProperties(console, typeof(DeviceData));
                        }
                        if (options.Display)
                        {
                            console.Out.WriteLine($"Display:");
                            ShowProperties(console, typeof(DisplayData));
                        }
                        if (options.Error)
                        {
                            console.Out.WriteLine($"Error:");
                            ShowProperties(console, typeof(ErrorData));
                        }
                        if (options.Fan)
                        {
                            console.Out.WriteLine($"Fan:");
                            ShowProperties(console, typeof(FanData));
                        }
                        if (options.Heater)
                        {
                            console.Out.WriteLine($"Heater:");
                            ShowProperties(console, typeof(HeaterData));
                        }
                        if (options.Info)
                        {
                            console.Out.WriteLine($"Info:");
                            ShowProperties(console, typeof(InfoData));
                        }
                        if (options.Network)
                        {
                            console.Out.WriteLine($"Network:");
                            ShowProperties(console, typeof(NetworkData));
                        }
                        if (options.Operation)
                        {
                            console.Out.WriteLine($"Operation:");
                            ShowProperties(console, typeof(OperationData));
                        }
                        if (options.Sensor)
                        {
                            console.Out.WriteLine($"Sensor:");
                            ShowProperties(console, typeof(SensorData));
                        }
                        if (options.System)
                        {
                            console.Out.WriteLine($"System:");
                            ShowProperties(console, typeof(SystemData));
                        }
                        if (options.Technical)
                        {
                            console.Out.WriteLine($"Technical:");
                            ShowProperties(console, typeof(TechnicalData));
                        }
                        if (options.Vacation)
                        {
                            console.Out.WriteLine($"Vacation:");
                            ShowProperties(console, typeof(VacationData));
                        }
                    }
                    else
                    {
                        if (options.AllData)
                        {
                            if (options.Label)
                            {
                                ShowProperty(console, typeof(HeliosData), HeliosData.GetHeliosProperty(options.Name));
                            }
                            else
                            {
                                ShowProperty(console, typeof(HeliosData), options.Name);
                            }
                        }

                        if (options.Booster)
                        {
                            ShowProperty(console, typeof(BoosterData), options.Name);
                        }
                        if (options.Device)
                        {
                            ShowProperty(console, typeof(DeviceData), options.Name);
                        }
                        if (options.Display)
                        {
                            ShowProperty(console, typeof(DisplayData), options.Name);
                        }
                        if (options.Error)
                        {
                            ShowProperty(console, typeof(ErrorData), options.Name);
                        }
                        if (options.Fan)
                        {
                            ShowProperty(console, typeof(FanData), options.Name);
                        }
                        if (options.Heater)
                        {
                            ShowProperty(console, typeof(HeaterData), options.Name);
                        }
                        if (options.Info)
                        {
                            ShowProperty(console, typeof(InfoData), options.Name);
                        }
                        if (options.Network)
                        {
                            ShowProperty(console, typeof(NetworkData), options.Name);
                        }
                        if (options.Operation)
                        {
                            ShowProperty(console, typeof(OperationData), options.Name);
                        }
                        if (options.Sensor)
                        {
                            ShowProperty(console, typeof(SensorData), options.Name);
                        }
                        if (options.System)
                        {
                            ShowProperty(console, typeof(SystemData), options.Name);
                        }
                        if (options.Technical)
                        {
                            ShowProperty(console, typeof(TechnicalData), options.Name);
                        }
                        if (options.Vacation)
                        {
                            ShowProperty(console, typeof(VacationData), options.Name);
                        }
                    }
                    
                    return (int)ExitCodes.SuccessfullyCompleted;
                });
        }

        #endregion Constructors

        #region Private Methods

        /// <summary>
        /// Displays a list of Helios Labels.
        /// </summary>
        /// <param name="console">The command line console.</param>
        private static void ShowLabels(IConsole console)
        {
            console.Out.WriteLine($"List of Helios Labels:");
            var names = HeliosData.GetLabels();

            foreach (var name in names)
            {
                console.Out.WriteLine($"    {name}");
            }

            console.Out.WriteLine();
        }

        /// <summary>
        /// Displays a list of property names.
        /// </summary>
        /// <param name="console">The command line console.</param>
        /// <param name="type">The type to be used.</param>
        private static void ShowProperties(IConsole console, Type type)
        {
            console.Out.WriteLine($"List of Properties:");
            var names = type.GetProperties().Select(p => p.Name);

            foreach (var name in names)
            {
                console.Out.WriteLine($"    {name}");
            }

            console.Out.WriteLine();
        }

        /// <summary>
        /// Displays selected property info data for a named property.
        /// </summary>
        /// <param name="console">The command line console.</param>
        /// <param name="type">THe type to be used.</param>
        /// <param name="name">The property name.</param>
        private static void ShowProperty(IConsole console, Type type, string name)
        {
            console.Out.WriteLine($"Property {name}:");
            var info = type.GetProperty(name);
            var pType = info?.PropertyType;

            console.Out.WriteLine($"   IsProperty:    {!(info is null)}");
            console.Out.WriteLine($"   CanRead:       {info?.CanRead}");
            console.Out.WriteLine($"   CanWrite:      {info?.CanWrite}");

            if (info?.PropertyType.IsArray ?? false)
            {
                console.Out.WriteLine($"   IsArray:       {pType?.IsArray}");
                console.Out.WriteLine($"   ElementType:   {pType?.GetElementType()}");
            }
            else if ((pType?.IsGenericType ?? false) && (pType?.GetGenericTypeDefinition() == typeof(List<>)))
            {
                console.Out.WriteLine($"   IsList:        List<ItempType>");
                console.Out.WriteLine($"   ItemType:      {pType?.GetGenericArguments().Single()}");
            }
            else
            {
                console.Out.WriteLine($"   PropertyType:  {pType?.Name}");
            }

            console.Out.WriteLine($"   Helios Label:  {HeliosData.GetHeliosAttribute(name).Name}");

            console.Out.WriteLine();
        }

        #endregion
    }
}
