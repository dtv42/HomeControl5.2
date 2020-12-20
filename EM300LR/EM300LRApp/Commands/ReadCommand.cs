// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadCommand.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace EM300LRApp.Commands
{
    #region Using Directives

    using System.CommandLine;
    using System.CommandLine.Invocation;
    using System.CommandLine.IO;
    using System.Text.Json;

    using Microsoft.Extensions.Logging;

    using UtilityLib;
    using UtilityLib.Console;

    using EM300LRLib;
    using EM300LRLib.Models;

    using EM300LRApp.Options;

    #endregion Using Directives

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
        /// Initializes a new instance of the <see cref="ReadCommand"/> class.
        /// </summary>
        /// <param name="gateway"></param>
        /// <param name="logger"></param>
        public ReadCommand(EM300LRGateway gateway, ILogger<ReadCommand> logger)
            : base(logger, "read", "Reading data values from b-Control EM300LR energy manager.")
        {
            _logger?.LogDebug("ReadCommand()");

            // Setup command arguments and options.
            AddArgument(new Argument<string>("name", "The property name.").Arity(ArgumentArity.ZeroOrOne));

            AddOption(new Option<bool>(new string[] { "-d", "--data"   }, "Reads all data"));
            AddOption(new Option<bool>(new string[] { "-t", "--total"  }, "Reads the total data"));
            AddOption(new Option<bool>(new string[] { "-1", "--phase1" }, "Reads the phase 1 data"));
            AddOption(new Option<bool>(new string[] { "-2", "--phase2" }, "Reads the phase 2 data"));
            AddOption(new Option<bool>(new string[] { "-3", "--phase3" }, "Reads the phase 3 data"));
            AddOption(new Option<bool>(new string[] { "-s", "--status" }, "Shows the data status"));

            // Setup execution handler.
            Handler = CommandHandler.Create<IConsole, GlobalOptions, ReadOptions>
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

                if (gateway.ReadAll().IsGood)
                {
                    if (string.IsNullOrEmpty(options.Name))
                    {
                        if (options.Data)
                        {
                            console.Out.WriteLine($"Reading all data from EM300LR energy manager.");
                            console.Out.WriteLine($"Data:");
                            console.Out.WriteLine(JsonSerializer.Serialize<EM300LRData>(gateway.Data, _serializerOptions));
                        }

                        if (options.Total)
                        {
                            console.Out.WriteLine($"Reading all total data from EM300LR energy manager.");
                            console.Out.WriteLine($"Total:");
                            console.Out.WriteLine(JsonSerializer.Serialize<TotalData>(gateway.TotalData, _serializerOptions));
                        }

                        if (options.Phase1)
                        {
                            console.Out.WriteLine($"Reading all phase 1 data from EM300LR energy manager.");
                            console.Out.WriteLine($"Phase1:");
                            console.Out.WriteLine(JsonSerializer.Serialize<Phase1Data>(gateway.Phase1Data, _serializerOptions));
                        }

                        if (options.Phase2)
                        {
                            console.Out.WriteLine($"Reading all phase 2 data from EM300LR energy manager.");
                            console.Out.WriteLine($"Phase2:");
                            console.Out.WriteLine(JsonSerializer.Serialize<Phase2Data>(gateway.Phase2Data, _serializerOptions));
                        }

                        if (options.Phase3)
                        {
                            console.Out.WriteLine($"Reading all phase 3 data from EM300LR energy manager.");
                            console.Out.WriteLine($"Phase3:");
                            console.Out.WriteLine(JsonSerializer.Serialize<Phase3Data>(gateway.Phase3Data, _serializerOptions));
                        }
                    }
                    else
                    {
                        if (options.Data)
                        {
                            console.Out.WriteLine($"Value of EM300LR data property '{options.Name}' = {gateway.Data.GetPropertyValue(options.Name)}");
                        }

                        if (options.Total)
                        {
                            console.Out.WriteLine($"Value of total data property '{options.Name}' = {gateway.TotalData.GetPropertyValue(options.Name)}");
                        }

                        if (options.Phase1)
                        {
                            console.Out.WriteLine($"Value of phase 1 data property '{options.Name}' = {gateway.Phase1Data.GetPropertyValue(options.Name)}");
                        }

                        if (options.Phase2)
                        {
                            console.Out.WriteLine($"Value of phase 2 data property '{options.Name}' = {gateway.Phase2Data.GetPropertyValue(options.Name)}");
                        }

                        if (options.Phase3)
                        {
                            console.Out.WriteLine($"Value of phase 3 data property '{options.Name}' = {gateway.Phase3Data.GetPropertyValue(options.Name)}");
                        }
                    }
                }
                else
                {
                    console.Out.WriteLine($"Error reading all data from EM300LR energy manager.");
                }

                if (options.Status)
                {
                    console.Out.WriteLine($"Status:");
                    console.Out.WriteLine(JsonSerializer.Serialize<DataStatus>(gateway.Status, _serializerOptions));
                }

                return (int)ExitCodes.SuccessfullyCompleted;
            });
        }
    }

    #endregion Constructors
}