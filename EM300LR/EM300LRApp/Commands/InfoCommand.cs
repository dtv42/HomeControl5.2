// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InfoCommand.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>22-4-2020 17:19</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace EM300LRApp.Commands
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

    using EM300LRLib.Models;
    using EM300LRApp.Options;

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
            : base(logger, "info", "Reading data values from b-Control EM300LR energy manager.")
        {
            logger.LogDebug("InfoCommand()");

            // Setup command arguments and options.
            AddArgument(new Argument<string>("name", "The property name.").Arity(ArgumentArity.ZeroOrOne));

            AddOption(new Option<bool>(new string[] { "-d", "--data"   }, "Gets all data"        ));
            AddOption(new Option<bool>(new string[] { "-t", "--total"  }, "Gets the total data"  ));
            AddOption(new Option<bool>(new string[] { "-1", "--phase1" }, "Gets the phase 1 data"));
            AddOption(new Option<bool>(new string[] { "-2", "--phase2" }, "Gets the phase 2 data"));
            AddOption(new Option<bool>(new string[] { "-3", "--phase3" }, "Gets the phase 3 data"));

            // Setup execution handler.
            Handler = CommandHandler.Create<IConsole, GlobalOptions, InfoOptions>
                ((console, globals, options) =>
            {
                logger.LogDebug("Handler()");

                if (!options.CheckOptions(console)) return (int)ExitCodes.IncorrectFunction;

                if (globals.Verbose)
                {
                    console.Out.WriteLine($"Commandline Application: {RootCommand.ExecutableName}");
                    console.Out.WriteLine($"Password:      {globals.Password}");
                    console.Out.WriteLine($"Serialnumber:  {globals.SerialNumber}");
                    console.Out.WriteLine($"Address:       {globals.Address}");
                    console.Out.WriteLine($"Timeout:       {globals.Timeout}");
                    console.Out.WriteLine();
                }

                if (string.IsNullOrEmpty(options.Name))
                {
                    if (options.Data)
                    {
                        console.Out.WriteLine($"Data:");
                        ShowProperties(console, typeof(EM300LRData));
                    }

                    if (options.Total)
                    {
                        console.Out.WriteLine($"Total:");
                        ShowProperties(console, typeof(TotalData));
                    }

                    if (options.Phase1)
                    {
                        console.Out.WriteLine($"Phase1:");
                        ShowProperties(console, typeof(Phase1Data));
                    }

                    if (options.Phase2)
                    {
                        console.Out.WriteLine($"Phase2:");
                        ShowProperties(console, typeof(Phase2Data));
                    }

                    if (options.Phase3)
                    {
                        console.Out.WriteLine($"Phase3:");
                        ShowProperties(console, typeof(Phase3Data));
                    }
                }
                else
                {
                    if (options.Data)
                    {
                        ShowProperty(console, typeof(EM300LRData), options.Name);
                    }

                    if (options.Total)
                    {
                        ShowProperty(console, typeof(TotalData), options.Name);
                    }

                    if (options.Phase1)
                    {
                        ShowProperty(console, typeof(Phase1Data), options.Name);
                    }

                    if (options.Phase2)
                    {
                        ShowProperty(console, typeof(Phase2Data), options.Name);
                    }

                    if (options.Phase3)
                    {
                        ShowProperty(console, typeof(Phase3Data), options.Name);
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