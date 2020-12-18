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
    using System.Linq;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    using McMaster.Extensions.CommandLineUtils;

    using UtilityLib;
    using WallboxLib.Models;
    using WallboxApp.Models;

    #endregion

    /// <summary>
    /// Application command "info".
    /// </summary>
    [Command(Name = "info",
             FullName = "Wallbox Info Command",
             Description = "Reading data values from BMW Wallbox charging station.",
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

        [Option("-1|--report1", Description = "Shows the report 1 data.")]
        public bool Report1 { get; }

        [Option("-2|--report2", Description = "Shows the report 2 data.")]
        public bool Report2 { get; }

        [Option("-3|--report3", Description = "Shows the report 3 data.")]
        public bool Report3 { get; }

        [Option("-r|--reports", Description = "Shows the charging report data (e.g. report 100 - 130).")]
        public bool Reports { get; }

        [Option("-i|--info", Description = "Shows the info data.")]
        public bool Info { get; }

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
                    if (Report1)
                    {
                        _console.WriteLine($"Report1:");
                        ShowProperties(typeof(Report1Data));
                    }

                    if (Report2)
                    {
                        _console.WriteLine($"Report1:");
                        ShowProperties(typeof(Report2Data));
                    }

                    if (Report3)
                    {
                        _console.WriteLine($"Report1:");
                        ShowProperties(typeof(Report3Data));
                    }

                    if (Reports)
                    {
                        _console.WriteLine($"Report100:");
                        ShowProperties(typeof(ReportsData));
                    }

                    if (Info)
                    {
                        _console.WriteLine($"Info:");
                        ShowProperties(typeof(InfoData));
                    }
                }
                else
                {
                    if (Report1)
                    {
                        ShowProperty(typeof(Report1Data), Property);
                    }

                    if (Report2)
                    {
                        ShowProperty(typeof(Report2Data), Property);
                    }

                    if (Report3)
                    {
                        ShowProperty(typeof(Report3Data), Property);
                    }

                    if (Reports)
                    {
                        ShowProperty(typeof(ReportsData), Property);
                    }

                    if (Info)
                    {
                        ShowProperty(typeof(InfoData), Property);
                    }
                }
            }
            catch
            {
                _logger.LogError("ReadCommand exception");
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

                if (Report1) ++options;
                if (Report2) ++options;
                if (Report3) ++options;
                if (Reports) ++options;

                if (options != 1)
                {
                    _console.WriteLine("Please specifiy a single data option");
                    _application.ShowHint();
                    return false;
                }

                if (!string.IsNullOrEmpty(Property))
                {
                    if (Report1)
                    {
                        if (!typeof(Report1Data).IsProperty(Property))
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }

                    if (Report2)
                    {
                        if (!typeof(Report2Data).IsProperty(Property))
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }

                    if (Report3)
                    {
                        if (!typeof(Report3Data).IsProperty(Property))
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }

                    if (Reports)
                    {
                        if (!typeof(ReportsData).IsProperty(Property))
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
