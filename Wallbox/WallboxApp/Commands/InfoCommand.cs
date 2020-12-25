// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InfoCommand.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>26-4-2020 20:18</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace WallboxApp.Commands
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

    using WallboxLib.Models;

    using WallboxApp.Options;

    #endregion

    /// <summary>
    /// Application command "info".
    /// </summary>
    public class InfoCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        ///  Initializes a new instance of the <see cref="InfoCommand"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        public InfoCommand(ILogger<InfoCommand> logger)
            : base(logger, "info", "Showing data info from BMW Wallbox charging station.")
        {
            logger.LogDebug("InfoCommand()");

            // Setup command arguments and options.
            AddArgument(new Argument<string>("name", "The property name.").Arity(ArgumentArity.ZeroOrOne));

            AddOption(new Option<bool>(new string[] { "-1", "--report1" }, "Gets the report 1 data info."));
            AddOption(new Option<bool>(new string[] { "-2", "--report2" }, "Gets the report 2 data info."));
            AddOption(new Option<bool>(new string[] { "-3", "--report3" }, "Gets the report 3 data info."));
            AddOption(new Option<bool>(new string[] { "-r", "--reports" }, "Gets the charging report data info (e.g. report 100 - 130)."));
            AddOption(new Option<bool>(new string[] { "-i", "--info"    }, "Gets the infodata info"));

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
                        if (options.Report1)
                        {
                            console.Out.WriteLine($"Report1:");
                            ShowProperties(console, typeof(Report1Data));
                        }

                        if (options.Report2)
                        {
                            console.Out.WriteLine($"Report2:");
                            ShowProperties(console, typeof(Report2Data));
                        }

                        if (options.Report3)
                        {
                            console.Out.WriteLine($"Report3:");
                            ShowProperties(console, typeof(Report3Data));
                        }

                        if (options.Reports)
                        {
                            console.Out.WriteLine($"Report100:");
                            ShowProperties(console, typeof(ReportsData));
                        }

                        if (options.Info)
                        {
                            console.Out.WriteLine($"Info:");
                            ShowProperties(console, typeof(InfoData));
                        }
                    }
                    else
                    {
                        if (options.Report1)
                        {
                            ShowProperty(console, typeof(Report1Data), options.Name);
                        }

                        if (options.Report2)
                        {
                            ShowProperty(console, typeof(Report2Data), options.Name);
                        }

                        if (options.Report3)
                        {
                            ShowProperty(console, typeof(Report3Data), options.Name);
                        }

                        if (options.Reports)
                        {
                            ShowProperty(console, typeof(ReportsData), options.Name);
                        }

                        if (options.Info)
                        {
                            ShowProperty(console, typeof(InfoData), options.Name);
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
