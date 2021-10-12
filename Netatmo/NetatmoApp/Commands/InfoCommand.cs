// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InfoCommand.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>17-12-2020 12:51</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace NetatmoApp.Commands
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

    using NetatmoLib.Models;
    using NetatmoApp.Options;

    #endregion Using Directives

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
            : base(logger, "info", "Showing data info from the Netatmo web service.")
        {
            logger.LogDebug("InfoCommand()");

            // Setup command arguments and options.
            AddArgument(new Argument<string>("name", "The property name.").Arity(ArgumentArity.ZeroOrOne));

            AddOption(new Option<bool>(new string[] { "-d", "--data"    }, "Shows all data info"         ));
            AddOption(new Option<bool>(new string[] { "-m", "--main"    }, "Shows main station data info"));
            AddOption(new Option<bool>(new string[] { "-o", "--outdoor" }, "Shows the outdoor data info" ));
            AddOption(new Option<bool>(new string[] { "-i", "--indoor"  }, "Shows the indoor data info"  ));
            AddOption(new Option<bool>(new string[] { "-r", "--rain"    }, "Shows rain station data info"));
            AddOption(new Option<bool>(new string[] { "-w", "--wind"    }, "Shows wind station data info"));

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
                        console.Out.WriteLine($"Netatmo Data:");
                        ShowProperties(console, typeof(NetatmoData));
                    }

                    if (options.Main)
                    {
                        console.Out.WriteLine($"Main Data:");
                        ShowProperties(console, typeof(MainData));
                    }

                    if (options.Outdoor)
                    {
                        console.Out.WriteLine($"Outdoor Data:");
                        ShowProperties(console, typeof(OutdoorData));
                    }

                    if (options.Indoor)
                    {
                        console.Out.WriteLine($"Indoor Data:");
                        ShowProperties(console, typeof(IndoorData));
                    }

                    if (options.Rain)
                    {
                        console.Out.WriteLine($"Rain Data:");
                        ShowProperties(console, typeof(RainData));
                    }

                    if (options.Wind)
                    {
                        console.Out.WriteLine($"Wind Data:");
                        ShowProperties(console, typeof(WindData));
                    }
                }
                else
                {
                    if (options.Data)
                    {
                        ShowProperty(console, typeof(NetatmoData), options.Name);
                    }

                    if (options.Main)
                    {
                        ShowProperty(console, typeof(MainData), options.Name);
                    }

                    if (options.Outdoor)
                    {
                        ShowProperty(console, typeof(OutdoorData), options.Name);
                    }

                    if (options.Indoor)
                    {
                        ShowProperty(console, typeof(IndoorData), options.Name);
                    }

                    if (options.Rain)
                    {
                        ShowProperty(console, typeof(RainData), options.Name);
                    }

                    if (options.Wind)
                    {
                        ShowProperty(console, typeof(WindData), options.Name);
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
        /// <param name="type">The type to be used.</param>
        /// <param name="name">The property name</param>
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