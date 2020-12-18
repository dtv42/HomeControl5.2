// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadCommand.cs" company="DTV-Online">
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

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    using McMaster.Extensions.CommandLineUtils;

    using UtilityLib;
    using WallboxLib;
    using WallboxLib.Models;

    using WallboxApp.Models;

    #endregion

    /// <summary>
    /// Application command "read".
    /// </summary>
    [Command(Name = "read",
             FullName = "Wallbox Read Command",
             Description = "Reading data values from BMW Wallbox charging station.",
             ExtendedHelpText = "\nCopyright (c) 2020 Dr. Peter Trimmel - All rights reserved.")]
    public class ReadCommand : BaseCommand<ReadCommand, AppSettings>
    {
        #region Private Data Members

        private readonly JsonSerializerOptions _options = JsonExtensions.DefaultSerializerOptions;
        private readonly WallboxGateway _gateway;

        #endregion

        #region Private Properties

        /// <summary>
        /// This is a reference to the parent command <see cref="RootCommand"/>.
        /// </summary>
        private RootCommand? Parent { get; set; }

        #endregion

        #region Public Properties

        [Option("-d|--data", Description = "Reads all report data.")]
        public bool Data { get; }

        [Option("-1|--report1", Description = "Reads the report 1.")]
        public bool Report1 { get; }

        [Option("-2|--report2", Description = "Reads the report 2.")]
        public bool Report2 { get; }

        [Option("-3|--report3", Description = "Reads the report 3.")]
        public bool Report3 { get; }

        [Option("-100|--report100", Description = "Reads the last charging report (report 100).")]
        public bool Report100 { get; }

        [Option("-r|--reports", Description = "Reads all charging reports.")]
        public bool Reports { get; }

        [Option("-n|--number <NUMBER>", Description = "Reads the specified charging report (101 - 130).")]
        [Range(101, 130)]
        public int? Index { get; }

        [Option("-i|--info", Description = "Reads info data.")]
        public bool Info { get; }

        [Argument(0, "Reads the named property.")]
        public string Property { get; set; } = string.Empty;

        [Option("--status", Description = "Shows the data status.")]
        public bool Status { get; }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadCommand"/> class.
        /// </summary>
        /// <param name="gateway"></param>
        /// <param name="console"></param>
        /// <param name="settings"></param>
        /// <param name="config"></param>
        /// <param name="environment"></param>
        /// <param name="lifetime"></param>
        /// <param name="logger"></param>
        /// <param name="application"></param>
        public ReadCommand(WallboxGateway gateway,
                           IConsole console,
                           AppSettings settings,
                           IConfiguration config,
                           IHostEnvironment environment,
                           IHostApplicationLifetime lifetime,
                           ILogger<ReadCommand> logger,
                           CommandLineApplication application)
            : base(console, settings, config, environment, lifetime, logger, application)
        {
            _logger?.LogDebug("ReadCommand()");

            // Setting the Wallbox instance.
            _gateway = gateway;
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
                if (!(Parent is null))
                {
                    // Overriding Wallbox options.
                    _settings.EndPoint = Parent.EndPoint;
                    _settings.Port = Parent.Port;

                    if (Parent.ShowSettings)
                    {
                        _console.WriteLine(JsonSerializer.Serialize<AppSettings>(_settings, _options));
                    }
                }

                if (string.IsNullOrEmpty(Property))
                {
                    if (Data)
                    {
                        _console.WriteLine($"Reading all data from BMW Wallbox charging station.");
                        DataStatus status = _gateway.ReadAll();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Data:");
                            _console.WriteLine(JsonSerializer.Serialize<WallboxData>(_gateway.Data, _options));
                        }
                        else
                        {
                            _console.WriteLine($"Error reading all data from BMW Wallbox charging station.");
                        }
                    }

                    if (Report1)
                    {
                        _console.WriteLine($"Reading report 1 from BMW Wallbox charging station.");
                        DataStatus status = _gateway.ReadReport1();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Report1:");
                            _console.WriteLine(JsonSerializer.Serialize<Report1Data>(_gateway.Report1, _options));
                        }
                        else
                        {
                            _console.WriteLine($"Error reading report 1 from BMW Wallbox charging station.");
                        }
                    }

                    if (Report2)
                    {
                        _console.WriteLine($"Reading report 2 from BMW Wallbox charging station.");
                        DataStatus status = _gateway.ReadReport2();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Report2:");
                            _console.WriteLine(JsonSerializer.Serialize<Report2Data>(_gateway.Report2, _options));
                        }
                        else
                        {
                            _console.WriteLine($"Error reading report 2 from BMW Wallbox charging station.");
                        }
                    }

                    if (Report3)
                    {
                        _console.WriteLine($"Reading report 3 from BMW Wallbox charging station.");
                        DataStatus status = _gateway.ReadReport3();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Report3:");
                            _console.WriteLine(JsonSerializer.Serialize<Report3Data>(_gateway.Report3, _options));
                        }
                        else
                        {
                            _console.WriteLine($"Error reading report 1 from BMW Wallbox charging station.");
                        }
                    }

                    if (Report100)
                    {
                        _console.WriteLine($"Reading report 100 from BMW Wallbox charging station.");
                        DataStatus status = _gateway.ReadReport1();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Report100:");
                            _console.WriteLine(JsonSerializer.Serialize<ReportsData>(_gateway.Report100, _options));
                        }
                        else
                        {
                            _console.WriteLine($"Error reading report 100 from BMW Wallbox charging station.");
                        }
                    }

                    if (Reports)
                    {
                        _console.WriteLine($"Reading reports from BMW Wallbox charging station.");
                        DataStatus status = _gateway.ReadReports();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Reports:");
                            _console.WriteLine(JsonSerializer.Serialize<List<ReportsData>>(_gateway.Reports, _options));
                        }
                        else
                        {
                            _console.WriteLine($"Error reading reportsfrom BMW Wallbox charging station.");
                        }
                    }

                    if (Index.HasValue)
                    {
                        _console.WriteLine($"Reading report from BMW Wallbox charging station.");
                        DataStatus status = _gateway.ReadReports();

                        if (status.IsGood)
                        {
                            int index = Index.Value - WallboxGateway.REPORTS_ID - 1;
                            _console.WriteLine($"Report{Index.Value}:");
                            _console.WriteLine(JsonSerializer.Serialize<ReportsData>(_gateway.Reports[index], _options));
                        }
                        else
                        {
                            _console.WriteLine($"Error reading report from BMW Wallbox charging station.");
                        }
                    }

                    if (Info)
                    {
                        _console.WriteLine($"Reading info data from BMW Wallbox charging station.");
                        DataStatus status = _gateway.ReadInfo();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Info:");
                            _console.WriteLine(JsonSerializer.Serialize<InfoData>(_gateway.Info, _options));
                        }
                        else
                        {
                            _console.WriteLine($"Error reading info data from BMW Wallbox charging station.");
                        }
                    }
                }
                else 
                {
                    if (Report1)
                    {
                        DataStatus status = _gateway.ReadReport1();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Value of property '{Property}' = {_gateway.Report1.GetPropertyValue(Property)}");
                        }
                        else
                        {
                            _console.WriteLine($"Error reading report 1 data from BMW Wallbox charging station.");
                        }
                    }

                    if (Report2)
                    {
                        DataStatus status = _gateway.ReadReport2();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Value of property '{Property}' = {_gateway.Report2.GetPropertyValue(Property)}");
                        }
                        else
                        {
                            _console.WriteLine($"Error reading report 2 from BMW Wallbox charging station.");
                        }
                    }

                    if (Report3)
                    {
                        DataStatus status = _gateway.ReadReport3();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Value of property '{Property}' = {_gateway.Report3.GetPropertyValue(Property)}");
                        }
                        else
                        {
                            _console.WriteLine($"Error reading report 3 from BMW Wallbox charging station.");
                        }
                    }

                    if (Report100)
                    {
                        DataStatus status = _gateway.ReadReport100();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Value of property '{Property}' = {_gateway.Report100.GetPropertyValue(Property)}");
                        }
                        else
                        {
                            _console.WriteLine($"Error reading report 100 from BMW Wallbox charging station.");
                        }
                    }

                    if (Index.HasValue)
                    {
                        int index = Index.Value - WallboxGateway.REPORTS_ID - 1;
                        DataStatus status = _gateway.ReadReports();

                        if (status.IsGood)
                        {
                            _console.WriteLine($"Value of property '{Property}' = {_gateway.Reports[index].GetPropertyValue(Property)}");
                        }
                        else
                        {
                            _console.WriteLine($"Error reading report from BMW Wallbox charging station.");
                        }
                    }
                }

                if (Status)
                {
                    _console.WriteLine($"Status:");
                    _console.WriteLine(JsonSerializer.Serialize<DataStatus>(_gateway.Status, _options));
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

                if (Data) ++options;
                if (Report1) ++options;
                if (Report2) ++options;
                if (Report3) ++options;
                if (Report100) ++options;
                if (Reports) ++options;
                if (Index.HasValue) ++options;
                if (Info) ++options;

                if (options != 1)
                {
                    _console.WriteLine("Please specifiy a single report option");
                    _application.ShowHint();
                    return false;
                }

                if (!string.IsNullOrEmpty(Property))
                {
                    if (Data)
                    {
                        _console.WriteLine("The data option (-d|--data) can not be used with a property argument.");
                        return false;
                    }

                    if (Reports)
                    {
                        _console.WriteLine("The reports option (-r|--reports) can not be used with a property argument.");
                        return false;
                    }

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

                    if (Report100 || Index.HasValue)
                    {
                        if (!typeof(ReportsData).IsProperty(Property))
                        {
                            _logger?.LogError($"The property '{Property}' has not been found.");
                            return false;
                        }
                    }

                    if (Info)
                    {
                        if (!typeof(InfoData).IsProperty(Property))
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
    }
}
