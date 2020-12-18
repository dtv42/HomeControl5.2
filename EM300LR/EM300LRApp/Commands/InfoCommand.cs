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
    using System.Linq;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    using McMaster.Extensions.CommandLineUtils;

    using UtilityLib;
    using EM300LRLib;
    using EM300LRLib.Models;
    using EM300LRApp.Models;

    #endregion Using Directives

    /// <summary>
    /// Application command "info". Note that a gateway instance is not needed here.
    /// </summary>
    [Command(Name = "info",
             FullName = "EM300LR Info Command",
             Description = "Reading data values from b-Control EM300LR energy manager.",
             ExtendedHelpText = "\nCopyright (c) 2020 Dr. Peter Trimmel - All rights reserved.")]
    public class InfoCommand : BaseCommand<InfoCommand, AppSettings>
    {
        #region Private Properties

        /// <summary>
        /// This is a reference to the parent command <see cref="RootCommand"/>.
        /// </summary>
        private RootCommand? Parent { get; }

        #endregion Private Properties

        #region Public Properties

        [Option("-d|--data", Description = "Gets all data.")]
        public bool Data { get; }

        [Option("-t|--total", Description = "Get the total data.")]
        public bool Total { get; }

        [Option("-1|--phase1", Description = "Get the phase 1 data.")]
        public bool Phase1 { get; }

        [Option("-2|--phase2", Description = "Get the phase 2 data.")]
        public bool Phase2 { get; }

        [Option("-3|--phase3", Description = "Get the phase 3 data.")]
        public bool Phase3 { get; }

        [Argument(0, Description = "Specify the named property.")]
        public string Property { get; } = string.Empty;

        #endregion Public Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="InfoCommand"/> class.
        /// </summary>
        /// <param name="console"></param>
        /// <param name="settings"></param>
        /// <param name="config"></param>
        /// <param name="environment"></param>
        /// <param name="lifetime"></param>
        /// <param name="logger"></param>
        /// <param name="application"></param>
        public InfoCommand(IConsole console,
                           AppSettings settings,
                           IConfiguration config,
                           IHostEnvironment environment,
                           IHostApplicationLifetime lifetime,
                           ILogger<InfoCommand> logger,
                           CommandLineApplication application)
            : base(console, settings, config, environment, lifetime, logger, application)
        {
            _logger?.LogDebug("InfoCommand()");
        }

        #endregion Constructors

        #region Private Methods

        /// <summary>
        /// Runs when the commandline application command is executed.
        /// </summary>
        /// <returns>The exit code</returns>
        private int OnExecute()
        {
            try
            {
                if (string.IsNullOrEmpty(Property))
                {
                    if (Data)
                    {
                        _console.WriteLine($"Data:");
                        ShowProperties(typeof(EM300LRData));
                    }
                    
                    if (Total)
                    {
                        _console.WriteLine($"Total:");
                        ShowProperties(typeof(TotalData));
                    }
                    
                    if (Phase1)
                    {
                        _console.WriteLine($"Phase1:");
                        ShowProperties(typeof(Phase1Data));
                    }
                    
                    if (Phase2)
                    {
                        _console.WriteLine($"Phase2:");
                        ShowProperties(typeof(Phase2Data));
                    }
                    
                    if (Phase3)
                    {
                        _console.WriteLine($"Phase3:");
                        ShowProperties(typeof(Phase3Data));
                    }
                }
                else
                {
                    if (Data)
                    {
                        ShowProperty(typeof(EM300LRData), Property);
                    }
                    
                    if (Total)
                    {
                        ShowProperty(typeof(TotalData), Property);
                    }
                    
                    if (Phase1)
                    {
                        ShowProperty(typeof(Phase1Data), Property);
                    }
                    
                    if (Phase2)
                    {
                        ShowProperty(typeof(Phase2Data), Property);
                    }
                    
                    if (Phase3)
                    {
                        ShowProperty(typeof(Phase3Data), Property);
                    }
                }
            }
            catch
            {
                _logger.LogError("InfoCommand exception");
                throw;
            }

            return ExitCodes.SuccessfullyCompleted;
        }

        /// <summary>
        /// Helper method to check options.
        /// </summary>
        /// <returns>True if options are OK.</returns>
        public override bool CheckOptions()
        {
            if (Parent?.CheckOptions() ?? false)
            {
                int options = 0;

                if (Data) ++options;
                if (Total) ++options;
                if (Phase1) ++options;
                if (Phase2) ++options;
                if (Phase3) ++options;

                if (options != 1)
                {
                    _console.WriteLine("Please specifiy a single data option");
                    return false;
                }

                if (!string.IsNullOrEmpty(Property))
                {
                    if (Data)
                    {
                        if (typeof(EM300LRData).GetProperty(Property) is null)
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }

                    if (Total)
                    {
                        if (typeof(TotalData).GetProperty(Property) is null)
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }

                    if (Phase1)
                    {
                        if (typeof(Phase1Data).GetProperty(Property) is null)
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }

                    if (Phase2)
                    {
                        if (typeof(Phase2Data).GetProperty(Property) is null)
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }

                    if (Phase3)
                    {
                        if (typeof(Phase3Data).GetProperty(Property) is null)
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Displays a list of property names.
        /// </summary>
        /// <param name="type"></param>
        private void ShowProperties(Type type)
        {
            _console.WriteLine($"List of Properties:");
            var names = type.GetProperties().Select(p => p.Name);

            foreach (var name in names)
            {
                _console.WriteLine($"    {name}");
            }

            _console.WriteLine();
        }

        /// <summary>
        /// Displays selected property info data for a named property.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        private void ShowProperty(Type type, string name)
        {
            _console.WriteLine($"Property {name}:");
            var info = type.GetProperty(name);
            var pType = info?.PropertyType;

            _console.WriteLine($"   IsProperty:    {!(info is null)}");
            _console.WriteLine($"   CanRead:       {info?.CanRead}");
            _console.WriteLine($"   CanWrite:      {info?.CanWrite}");

            if (info?.PropertyType.IsArray ?? false)
            {
                _console.WriteLine($"   IsArray:       {pType?.IsArray}");
                _console.WriteLine($"   ElementType:   {pType?.GetElementType()}");
            }
            else if ((pType?.IsGenericType ?? false) && (pType?.GetGenericTypeDefinition() == typeof(List<>)))
            {
                _console.WriteLine($"   IsList:        List<ItempType>");
                _console.WriteLine($"   ItemType:      {pType?.GetGenericArguments().Single()}");
            }
            else
            {
                _console.WriteLine($"   PropertyType:  {pType?.Name}");
            }
            _console.WriteLine();
        }

        #endregion
    }
}