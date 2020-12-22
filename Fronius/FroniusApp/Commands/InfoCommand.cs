// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InfoCommand.cs" company="DTV-Online">
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

    using System;
    using System.Collections.Generic;
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using System.CommandLine.IO;
    using System.Linq;

    using Microsoft.Extensions.Logging;

    using UtilityLib;
    using UtilityLib.Console;

    using FroniusLib.Models;

    using FroniusApp.Options;

    #endregion

    /// <summary>
    /// Application command "info".
    /// </summary>
    public class InfoCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InfoCommand"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        public InfoCommand(ILogger<InfoCommand> logger)
            : base(logger, "info", "Showing data info from Fronius Symo 8.2-3-M solar inverter.")
        {
            _logger?.LogDebug("InfoCommand()");

            // Setup command arguments and options.
            AddArgument(new Argument<string>("name", "The property name.").Arity(ArgumentArity.ZeroOrOne));

            AddOption(new Option<bool>(new string[] { "-d", "--data"     }, "Gets all data"));
            AddOption(new Option<bool>(new string[] { "-c", "--common"   }, "Gets the inverter common data"));
            AddOption(new Option<bool>(new string[] { "-i", "--inverter" }, "Gets the inverter info data"));
            AddOption(new Option<bool>(new string[] { "-m", "--minmax"   }, "Gets the inverter minmax data"));
            AddOption(new Option<bool>(new string[] { "-p", "--phase"    }, "Gets the inverter phase data"));
            AddOption(new Option<bool>(new string[] { "-l", "--logger"   }, "Gets the logger info data"));

            // Setup execution handler.
            Handler = CommandHandler.Create<IConsole, GlobalOptions, InfoOptions>
                ((console, globals, options) =>
                {
                    logger.LogDebug("Handler()");

                    if (!options.CheckOptions(console)) return (int)ExitCodes.IncorrectFunction;

                    if (globals.Verbose)
                    {
                        console.Out.WriteLine($"Commandline Application: {RootCommand.ExecutableName}");
                        console.Out.WriteLine();
                    }

                    if (string.IsNullOrEmpty(options.Name))
                    {
                        if (options.Data)
                        {
                            console.Out.WriteLine($"Data:");
                            ShowProperties(console, typeof(FroniusData));
                        }

                        if (options.Common)
                        {
                            console.Out.WriteLine($"Common:");
                            ShowProperties(console, typeof(CommonData));
                        }

                        if (options.Inverter)
                        {
                            console.Out.WriteLine($"Inverter:");
                            ShowProperties(console, typeof(InverterInfo));
                        }

                        if (options.Logger)
                        {
                            console.Out.WriteLine($"Logger:");
                            ShowProperties(console, typeof(LoggerInfo));
                        }

                        if (options.MinMax)
                        {
                            console.Out.WriteLine($"MinMax:");
                            ShowProperties(console, typeof(MinMaxData));
                        }

                        if (options.Phase)
                        {
                            console.Out.WriteLine($"Phase:");
                            ShowProperties(console, typeof(PhaseData));
                        }
                    }
                    else
                    {
                        if (options.Data)
                        {
                            ShowProperty(console, typeof(FroniusData), options.Name);
                        }

                        if (options.Common)
                        {
                            ShowProperty(console, typeof(CommonData), options.Name);
                        }

                        if (options.Inverter)
                        {
                            ShowProperty(console, typeof(InverterInfo), options.Name);
                        }

                        if (options.Logger)
                        {
                            ShowProperty(console, typeof(LoggerInfo), options.Name);
                        }

                        if (options.MinMax)
                        {
                            ShowProperty(console, typeof(MinMaxData), options.Name);
                        }

                        if (options.Phase)
                        {
                            ShowProperty(console, typeof(PhaseData), options.Name);
                        }
                    }

                    return (int)ExitCodes.SuccessfullyCompleted;
                });
        }

        #endregion Constructors

        #region Private Methods

        /// <summary>
        /// Displays a list of property names.
        /// </summary>
        /// <param name="type"></param>
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
        /// <param name="type"></param>
        /// <param name="name"></param>
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
            console.Out.WriteLine();
        }

        #endregion Private Methods
    }
}
