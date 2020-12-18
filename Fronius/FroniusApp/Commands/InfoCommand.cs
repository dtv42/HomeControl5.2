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
    using System.Linq;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    using McMaster.Extensions.CommandLineUtils;

    using UtilityLib;
    using FroniusLib.Models;
    using FroniusApp.Models;

    #endregion

    /// <summary>
    /// Application command "info".
    /// </summary>
    [Command(Name = "info",
             FullName = "Fronius Info Command",
             Description = "Reading data values from Fronius Symo 8.2-3-M solar inverter.",
             ExtendedHelpText = "\nCopyright (c) 2020 Dr. Peter Trimmel - All rights reserved.")]
    public class InfoCommand : BaseCommand<InfoCommand, AppSettings>
    {
        #region Private Properties

        /// <summary>
        /// This is a reference to the parent command <see cref="RootCommand"/>.
        /// </summary>
        private RootCommand? Parent { get; }

        #endregion

        #region Public Properties

        [Option("-d|--data", Description = "Gets all data.")]
        public bool Data { get; }

        [Option("-c|--common", Description = "Get the inverter common data.")]
        public bool Common { get; }

        [Option("-i|--inverter", Description = "Get the inverter info.")]
        public bool Inverter { get; }

        [Option("-l|--logger", Description = "Get the data logger info.")]
        public bool Logger { get; }

        [Option("-m|--minmax", Description = "Get the inverter minmax data.")]
        public bool MinMax { get; }

        [Option("-p|--phase", Description = "Get the inverter phase data.")]
        public bool Phase { get; }

        [Argument(0, Description = "Specify the named property.")]
        public string Property { get; } = string.Empty;

        #endregion

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

        #endregion

        #region Public Methods

        /// <summary>
        /// Runs when the commandline application command is executed.
        /// </summary>
        /// <returns>The exit code</returns>
        public int OnExecute()
        {
            try
            {
                if (string.IsNullOrEmpty(Property))
                {
                    if (Data)
                    {
                        _console.WriteLine($"Data:");
                        ShowProperties(typeof(FroniusData));
                    }

                    if (Common)
                    {
                        _console.WriteLine($"Common:");
                        ShowProperties(typeof(CommonData));
                    }

                    if (Inverter)
                    {
                        _console.WriteLine($"Inverter:");
                        ShowProperties(typeof(InverterInfo));
                    }

                    if (Logger)
                    {
                        _console.WriteLine($"Logger:");
                        ShowProperties(typeof(LoggerInfo));
                    }

                    if (MinMax)
                    {
                        _console.WriteLine($"MinMax:");
                        ShowProperties(typeof(MinMaxData));
                    }

                    if (Phase)
                    {
                        _console.WriteLine($"Phase:");
                        ShowProperties(typeof(PhaseData));
                    }
                }
                else
                {
                    if (Data)
                    {
                        ShowProperty(typeof(FroniusData), Property);
                    }

                    if (Common)
                    {
                        ShowProperty(typeof(CommonData), Property);
                    }

                    if (Inverter)
                    {
                        ShowProperty(typeof(InverterInfo), Property);
                    }

                    if (Logger)
                    {
                        ShowProperty(typeof(LoggerInfo), Property);
                    }

                    if (MinMax)
                    {
                        ShowProperty(typeof(MinMaxData), Property);
                    }

                    if (Phase)
                    {
                        ShowProperty(typeof(PhaseData), Property);
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

        #endregion

        #region Private Methods

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
                if (Common) ++options;
                if (Inverter) ++options;
                if (Logger) ++options;
                if (MinMax) ++options;
                if (Phase) ++options;

                if (options != 1)
                {
                    _console.WriteLine("Please specifiy a single data option");
                    return false;
                }

                if (!string.IsNullOrEmpty(Property))
                {
                    if (Data)
                    {
                        if (!typeof(FroniusData).IsProperty(Property))
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }

                    if (Common)
                    {
                        if (!typeof(CommonData).IsProperty(Property))
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }

                    if (Inverter)
                    {
                        if (!typeof(InverterInfo).IsProperty(Property))
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }

                    if (Logger)
                    {
                        if (!typeof(LoggerInfo).IsProperty(Property))
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }

                    if (MinMax)
                    {
                        if (!typeof(MinMaxData).IsProperty(Property))
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }

                    if (Phase)
                    {
                        if (!typeof(PhaseData).IsProperty(Property))
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
