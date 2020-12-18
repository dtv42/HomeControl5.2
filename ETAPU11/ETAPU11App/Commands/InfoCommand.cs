// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InfoCommand.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace ETAPU11App.Commands
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
    using ETAPU11Lib;
    using ETAPU11Lib.Models;
    using ETAPU11App.Models;

    #endregion

    /// <summary>
    /// Application command "info".
    /// </summary>
    [Command(Name = "info",
             FullName = "ETAPU11 Info Command",
             Description = "Reading data values from ETA PU 11 pellet boiler.",
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

        [Option("-b|--boiler", Description = "Gets the boiler data.")]
        public bool Boiler { get; }

        [Option("-w|--hotwater", Description = "Gets the hot water data.")]
        public bool Hotwater { get; }

        [Option("-h|--heating", Description = "Gets the heating circuit data.")]
        public bool Heating { get; }

        [Option("-s|--storage", Description = "Gets the storage data.")]
        public bool Storage { get; }

        [Option("-y|--system", Description = "Gets the system data.")]
        public bool System { get; }

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
                        ShowProperties(typeof(ETAPU11Data));
                    }
                    
                    if (Boiler)
                    {
                        _console.WriteLine($"Boiler:");
                        ShowProperties(typeof(BoilerData));
                    }
                    
                    if (Hotwater)
                    {
                        _console.WriteLine($"Hotwater:");
                        ShowProperties(typeof(HotwaterData));
                    }
                    
                    if (Heating)
                    {
                        _console.WriteLine($"Heating:");
                        ShowProperties(typeof(HeatingData));
                    }
                    
                    if (Storage)
                    {
                        _console.WriteLine($"Storage:");
                        ShowProperties(typeof(StorageData));
                    }
                    
                    if (System)
                    {
                        _console.WriteLine($"System:");
                        ShowProperties(typeof(SystemData));
                    }
                }
                else
                {
                    if (Data)
                    {
                        ShowProperty(typeof(ETAPU11Data), Property);
                    }
                    
                    if (Boiler)
                    {
                        ShowProperty(typeof(BoilerData), Property);
                    }
                    
                    if (Hotwater)
                    {
                        ShowProperty(typeof(HotwaterData), Property);
                    }
                    
                    if (Heating)
                    {
                        ShowProperty(typeof(HeatingData), Property);
                    }
                    
                    if (Storage)
                    {
                        ShowProperty(typeof(StorageData), Property);
                    }
                    
                    if (System)
                    {
                        ShowProperty(typeof(SystemData), Property);
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
                if (Boiler) ++options;
                if (Hotwater) ++options;
                if (Heating) ++options;
                if (Storage) ++options;
                if (System) ++options;

                if (options != 1)
                {
                    _console.WriteLine("Please specifiy a single data option");
                    return false;
                }

                if (!string.IsNullOrEmpty(Property))
                {
                    if (Data)
                    {
                        if (!typeof(ETAPU11Data).IsProperty(Property))
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }

                    if (Boiler)
                    {
                        if (!typeof(BoilerData).IsProperty(Property))
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }

                    if (Hotwater)
                    {
                        if (!typeof(HotwaterData).IsProperty(Property))
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }

                    if (Heating)
                    {
                        if (!typeof(HeatingData).IsProperty(Property))
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }

                    if (Storage)
                    {
                        if (!typeof(StorageData).IsProperty(Property))
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }

                    if (System)
                    {
                        if (!typeof(SystemData).IsProperty(Property))
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

        #endregion

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

            string access = string.Empty;

            if (ETAPU11Data.IsReadable(name) && ETAPU11Data.IsWritable(name)) access = "RW";
            if (!ETAPU11Data.IsReadable(name) && ETAPU11Data.IsWritable(name)) access = "WO";
            if (ETAPU11Data.IsReadable(name) && !ETAPU11Data.IsWritable(name)) access = "RO";
            if (!ETAPU11Data.IsReadable(name) && !ETAPU11Data.IsWritable(name)) access = "??";

            _console.WriteLine($"   Modbus:        {access}");
            _console.WriteLine();
        }

        #endregion
    }
}
