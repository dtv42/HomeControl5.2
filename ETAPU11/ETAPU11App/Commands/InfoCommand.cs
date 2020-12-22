// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InfoCommand.cs" company="DTV-Online">
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

    using System;
    using System.Collections.Generic;
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using System.CommandLine.IO;
    using System.Linq;

    using Microsoft.Extensions.Logging;

    using UtilityLib;
    using UtilityLib.Console;

    using ETAPU11Lib.Models;
    using ETAPU11App.Options;

    #endregion

    /// <summary>
    /// Application command "info". Note that a gateway instance is not needed here.
    /// </summary>
    public sealed class InfoCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        ///  Initializes a new instance of the <see cref="InfoCommand"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        public InfoCommand(ILogger<InfoCommand> logger)
            : base(logger, "info", "Showing data info from ETA PU 11 pellet boiler.")
        {
            logger.LogDebug("InfoCommand()");

            // Setup command arguments and options.
            AddArgument(new Argument<string>("name", "The property name.").Arity(ArgumentArity.ZeroOrOne));

            AddOption(new Option<bool>(new string[] { "-d", "--data"    }, "Gets all data"));
            AddOption(new Option<bool>(new string[] { "-b", "--boiler"  }, "Gets the boiler data."));
            AddOption(new Option<bool>(new string[] { "-w", "--water"   }, "Gets the hot water data."));
            AddOption(new Option<bool>(new string[] { "-c", "--circuit" }, "Gets the heating circuit data."));
            AddOption(new Option<bool>(new string[] { "-s", "--storage" }, "Gets the pellets storage data."));
            AddOption(new Option<bool>(new string[] { "-y", "--system"  }, "Gets the system data."));

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
                            ShowProperties(console, typeof(ETAPU11Data));
                        }

                        if (options.Boiler)
                        {
                            console.Out.WriteLine($"Boiler Data:");
                            ShowProperties(console, typeof(BoilerData));
                        }

                        if (options.Water)
                        {
                            console.Out.WriteLine($"Hot Water:");
                            ShowProperties(console, typeof(HotwaterData));
                        }

                        if (options.Circuit)
                        {
                            console.Out.WriteLine($"Heating Circuit:");
                            ShowProperties(console, typeof(HeatingData));
                        }

                        if (options.Storage)
                        {
                            console.Out.WriteLine($"Pellet Storage:");
                            ShowProperties(console, typeof(StorageData));
                        }

                        if (options.System)
                        {
                            console.Out.WriteLine($"System Info:");
                            ShowProperties(console, typeof(SystemData));
                        }
                    }
                    else
                    {
                        if (options.Data)
                        {
                            ShowProperty(console, typeof(ETAPU11Data), options.Name);
                        }

                        if (options.Boiler)
                        {
                            ShowProperty(console, typeof(BoilerData), options.Name);
                        }

                        if (options.Water)
                        {
                            ShowProperty(console, typeof(HotwaterData), options.Name);
                        }

                        if (options.Circuit)
                        {
                            ShowProperty(console, typeof(HeatingData), options.Name);
                        }

                        if (options.Storage)
                        {
                            ShowProperty(console, typeof(StorageData), options.Name);
                        }

                        if (options.System)
                        {
                            ShowProperty(console, typeof(SystemData), options.Name);
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

        #endregion
    }
}
