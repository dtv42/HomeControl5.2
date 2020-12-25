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

    using System.CommandLine;
    using System.CommandLine.Invocation;
    using System.CommandLine.IO;
    using System.Collections.Generic;
    using System.Text.Json;

    using Microsoft.Extensions.Logging;

    using UtilityLib;
    using UtilityLib.Console;

    using WallboxLib;
    using WallboxLib.Models;

    using WallboxApp.Options;

    #endregion

    /// <summary>
    /// Application command "read".
    /// </summary>
    public class ReadCommand : BaseCommand
    {
        #region Private Data Members

        private readonly JsonSerializerOptions _serializerOptions = JsonExtensions.DefaultSerializerOptions;

        #endregion Private Data Members

        #region Constructors

        /// <summary>
        ///  Initializes a new instance of the <see cref="ReadCommand"/> class.
        /// </summary>
        /// <param name="gateway"></param>
        /// <param name="logger">The logger instance.</param>
        public ReadCommand(WallboxGateway gateway, ILogger<ReadCommand> logger)
            : base(logger, "read", "Reading data values from BMW Wallbox charging station.")
        {
            logger.LogDebug("ReadCommand()");

            // Setup command arguments and options.
            AddArgument(new Argument<string>("name", "The property name.").Arity(ArgumentArity.ZeroOrOne));

            AddOption(new Option<bool>  (new string[] { "-d", "--data"    }, "Reads all data"));
            AddOption(new Option<bool>  (new string[] { "-1", "--report1" }, "Reads the report 1 data."));
            AddOption(new Option<bool>  (new string[] { "-2", "--report2" }, "Reads the report 2 data."));
            AddOption(new Option<bool>  (new string[] { "-3", "--report3" }, "Reads the report 3 data."));
            AddOption(new Option<bool>  (new string[] { "-r", "--reports" }, "Reads all charging reports data."));
            AddOption(new Option<ushort>(new string[] { "-n", "--number"  }, "Reads the specified charging report (101 - 130).").Name("number").Range(101, 130));
            AddOption(new Option<bool>  (new string[] { "-l", "--last"    }, "Reads the last charging report (report 100)."));
            AddOption(new Option<bool>  (new string[] { "-i", "--info"    }, "Reads the info data"));
            AddOption(new Option<bool>  (new string[] { "-s", "--status"  }, "Shows the data status"));

            // Setup execution handler.
            Handler = CommandHandler.Create<IConsole, GlobalOptions, ReadOptions>
                ((console, globals, options) =>
                {
                    logger.LogDebug("Handler()");

                    if (!options.CheckOptions(console)) return (int)ExitCodes.IncorrectFunction;

                    if (globals.Verbose)
                    {
                        console.Out.WriteLine($"Commandline Application: {RootCommand.ExecutableName}");
                        console.Out.WriteLine($"Endpoint:  {globals.EndPoint}");
                        console.Out.WriteLine($"Port:      {globals.Port}");
                        console.Out.WriteLine($"Timeout:   {globals.Timeout}");
                        console.Out.WriteLine();
                    }

                    if (string.IsNullOrEmpty(options.Name))
                    {
                        if (options.Data)
                        {
                            console.Out.WriteLine($"Reading all data from BMW Wallbox charging station.");
                            DataStatus status = gateway.ReadAll();

                            if (status.IsGood)
                            {
                                console.Out.WriteLine($"Data:");
                                console.Out.WriteLine(JsonSerializer.Serialize<WallboxData>(gateway.Data, _serializerOptions));
                            }
                            else
                            {
                                console.Out.WriteLine($"Error reading all data from BMW Wallbox charging station.");
                            }
                        }

                        if (options.Report1)
                        {
                            console.Out.WriteLine($"Reading report 1 from BMW Wallbox charging station.");
                            DataStatus status = gateway.ReadReport1();

                            if (status.IsGood)
                            {
                                console.Out.WriteLine($"Report1:");
                                console.Out.WriteLine(JsonSerializer.Serialize<Report1Data>(gateway.Report1, _serializerOptions));
                            }
                            else
                            {
                                console.Out.WriteLine($"Error reading report 1 from BMW Wallbox charging station.");
                            }
                        }

                        if (options.Report2)
                        {
                            console.Out.WriteLine($"Reading report 2 from BMW Wallbox charging station.");
                            DataStatus status = gateway.ReadReport2();

                            if (status.IsGood)
                            {
                                console.Out.WriteLine($"Report2:");
                                console.Out.WriteLine(JsonSerializer.Serialize<Report2Data>(gateway.Report2, _serializerOptions));
                            }
                            else
                            {
                                console.Out.WriteLine($"Error reading report 2 from BMW Wallbox charging station.");
                            }
                        }

                        if (options.Report3)
                        {
                            console.Out.WriteLine($"Reading report 3 from BMW Wallbox charging station.");
                            DataStatus status = gateway.ReadReport3();

                            if (status.IsGood)
                            {
                                console.Out.WriteLine($"Report3:");
                                console.Out.WriteLine(JsonSerializer.Serialize<Report3Data>(gateway.Report3, _serializerOptions));
                            }
                            else
                            {
                                console.Out.WriteLine($"Error reading report 1 from BMW Wallbox charging station.");
                            }
                        }

                        if (options.Last)
                        {
                            console.Out.WriteLine($"Reading last report (100) from BMW Wallbox charging station.");
                            DataStatus status = gateway.ReadReport1();

                            if (status.IsGood)
                            {
                                console.Out.WriteLine($"Report100:");
                                console.Out.WriteLine(JsonSerializer.Serialize<ReportsData>(gateway.Report100, _serializerOptions));
                            }
                            else
                            {
                                console.Out.WriteLine($"Error reading report 100 from BMW Wallbox charging station.");
                            }
                        }

                        if (options.Reports)
                        {
                            console.Out.WriteLine($"Reading reports from BMW Wallbox charging station.");
                            DataStatus status = gateway.ReadReports();

                            if (status.IsGood)
                            {
                                console.Out.WriteLine($"Reports:");
                                console.Out.WriteLine(JsonSerializer.Serialize<List<ReportsData>>(gateway.Reports, _serializerOptions));
                            }
                            else
                            {
                                console.Out.WriteLine($"Error reading reportsfrom BMW Wallbox charging station.");
                            }
                        }

                        if (options.Number > 0)
                        {
                            console.Out.WriteLine($"Reading report from BMW Wallbox charging station.");
                            DataStatus status = gateway.ReadReports();

                            if (status.IsGood)
                            {
                                int index = options.Number - WallboxGateway.REPORTS_ID - 1;
                                console.Out.WriteLine($"Report{options.Number}:");
                                console.Out.WriteLine(JsonSerializer.Serialize<ReportsData>(gateway.Reports[index], _serializerOptions));
                            }
                            else
                            {
                                console.Out.WriteLine($"Error reading report from BMW Wallbox charging station.");
                            }
                        }

                        if (options.Info)
                        {
                            console.Out.WriteLine($"Reading info data from BMW Wallbox charging station.");
                            DataStatus status = gateway.ReadInfo();

                            if (status.IsGood)
                            {
                                console.Out.WriteLine($"Info:");
                                console.Out.WriteLine(JsonSerializer.Serialize<InfoData>(gateway.Info, _serializerOptions));
                            }
                            else
                            {
                                console.Out.WriteLine($"Error reading info data from BMW Wallbox charging station.");
                            }
                        }
                    }
                    else
                    {
                        if (options.Report1)
                        {
                            DataStatus status = gateway.ReadReport1();

                            if (status.IsGood)
                            {
                                console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.Report1.GetPropertyValue(options.Name)}");
                            }
                            else
                            {
                                console.Out.WriteLine($"Error reading report 1 data from BMW Wallbox charging station.");
                            }
                        }

                        if (options.Report2)
                        {
                            DataStatus status = gateway.ReadReport2();

                            if (status.IsGood)
                            {
                                console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.Report2.GetPropertyValue(options.Name)}");
                            }
                            else
                            {
                                console.Out.WriteLine($"Error reading report 2 from BMW Wallbox charging station.");
                            }
                        }

                        if (options.Report3)
                        {
                            DataStatus status = gateway.ReadReport3();

                            if (status.IsGood)
                            {
                                console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.Report3.GetPropertyValue(options.Name)}");
                            }
                            else
                            {
                                console.Out.WriteLine($"Error reading report 3 from BMW Wallbox charging station.");
                            }
                        }

                        if (options.Last)
                        {
                            DataStatus status = gateway.ReadReport100();

                            if (status.IsGood)
                            {
                                console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.Report100.GetPropertyValue(options.Name)}");
                            }
                            else
                            {
                                console.Out.WriteLine($"Error reading report 100 from BMW Wallbox charging station.");
                            }
                        }

                        if (options.Number > 0)
                        {
                            int index = options.Number - WallboxGateway.REPORTS_ID - 1;
                            DataStatus status = gateway.ReadReports();

                            if (status.IsGood)
                            {
                                console.Out.WriteLine($"Value of property '{options.Name}' = {gateway.Reports[index].GetPropertyValue(options.Name)}");
                            }
                            else
                            {
                                console.Out.WriteLine($"Error reading report from BMW Wallbox charging station.");
                            }
                        }
                    }

                    if (options.Status)
                    {
                        console.Out.WriteLine($"Status:");
                        console.Out.WriteLine(JsonSerializer.Serialize<DataStatus>(gateway.Status, _serializerOptions));
                    }

                    return (int)ExitCodes.SuccessfullyCompleted;
                });
        }

        #endregion Constructors
    }
}
