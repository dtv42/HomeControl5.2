// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReadCommand.cs" company="DTV-Online">
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

    using System.CommandLine;
    using System.CommandLine.Invocation;
    using System.CommandLine.IO;
    using System.Text.Json;

    using Microsoft.Extensions.Logging;

    using UtilityLib;
    using UtilityLib.Console;

    using NetatmoLib;
    using NetatmoLib.Models;

    using NetatmoApp.Options;

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
        public ReadCommand(NetatmoGateway gateway, ILogger<ReadCommand> logger)
            : base(logger, "read", "Reading data values from the Netatmo web service.")
        {
            _logger?.LogDebug("ReadCommand()");

            // Setup command arguments and options.
            AddArgument(new Argument<string>("name", "The property name.").Arity(ArgumentArity.ZeroOrOne));

            AddOption(new Option<bool>(new string[] { "-d", "--data"    }, "Reads all data"));
            AddOption(new Option<bool>(new string[] { "-m", "--main"    }, "Reads main station data"));
            AddOption(new Option<bool>(new string[] { "-o", "--outdoor" }, "Reads the outdoor data"));
            AddOption(new Option<bool>(new string[] { "-1", "--indoor1" }, "Reads the indoor 1 data"));
            AddOption(new Option<bool>(new string[] { "-2", "--indoor2" }, "Reads the indoor 2 data"));
            AddOption(new Option<bool>(new string[] { "-3", "--indoor3" }, "Reads the indoor 3 data"));
            AddOption(new Option<bool>(new string[] { "-r", "--rain"    }, "Reads rain station data"));
            AddOption(new Option<bool>(new string[] { "-w", "--wind"    }, "Reads wind station data"));
            AddOption(new Option<bool>(new string[] { "-s", "--status"  }, "Shows the data status"));

            // Setup execution handler.
            Handler = CommandHandler.Create<IConsole, GlobalOptions, ReadOptions>
                ((console, globals, options) =>
            {
                logger.LogDebug("Handler()");

                if (!options.CheckOptions(console)) return (int)ExitCodes.IncorrectFunction;

                if (globals.Verbose)
                {
                    console.Out.WriteLine($"Commandline Application: {RootCommand.ExecutableName}");
                    console.Out.WriteLine($"User:          {globals.User}");
                    console.Out.WriteLine($"Password:      {globals.Password}");
                    console.Out.WriteLine($"ClientID:      {globals.ClientID}");
                    console.Out.WriteLine($"ClientSecret:  {globals.ClientSecret}");
                    console.Out.WriteLine($"Address:       {globals.Address}");
                    console.Out.WriteLine($"Timeout:       {globals.Timeout}");
                    console.Out.WriteLine();
                }

                // Update settings with options.
                gateway.Settings.Address      = globals.Address;
                gateway.Settings.Timeout      = globals.Timeout;
                gateway.Settings.User         = globals.User;
                gateway.Settings.Password     = globals.Password;
                gateway.Settings.ClientID     = globals.ClientID;
                gateway.Settings.ClientSecret = globals.ClientSecret;

                if (gateway.ReadAll().IsGood)
                {
                    if (string.IsNullOrEmpty(options.Name))
                    {
                        if (options.Data)
                        {
                            console.Out.WriteLine("Reading all data from the Netatmo web service.");
                            console.Out.WriteLine("Netatmo Data:");
                            console.Out.WriteLine(JsonSerializer.Serialize<NetatmoData>(gateway.Data, _serializerOptions));
                        }

                        if (options.Main)
                        {
                            console.Out.WriteLine("Reading all data from the Netatmo web service.");
                            console.Out.WriteLine("Main Data:");
                            console.Out.WriteLine(JsonSerializer.Serialize<MainData>(gateway.Main, _serializerOptions));
                        }

                        if (options.Outdoor)
                        {
                            console.Out.WriteLine("Reading all data from the Netatmo web service.");
                            console.Out.WriteLine("Outdoor Data:");
                            console.Out.WriteLine(JsonSerializer.Serialize<OutdoorData>(gateway.Outdoor, _serializerOptions));
                        }

                        if (options.Indoor1)
                        {
                            console.Out.WriteLine("Reading all data from the Netatmo web service.");
                            console.Out.WriteLine("Indoor 1 Data:");
                            console.Out.WriteLine(JsonSerializer.Serialize<IndoorData>(gateway.Indoor1, _serializerOptions));
                        }

                        if (options.Indoor2)
                        {
                            console.Out.WriteLine("Reading all data from the Netatmo web service.");
                            console.Out.WriteLine("Indoor 2 Data:");
                            console.Out.WriteLine(JsonSerializer.Serialize<IndoorData>(gateway.Indoor2, _serializerOptions));
                        }

                        if (options.Indoor3)
                        {
                            console.Out.WriteLine("Reading all data from the Netatmo web service.");
                            console.Out.WriteLine("Indoor 3 Data:");
                            console.Out.WriteLine(JsonSerializer.Serialize<IndoorData>(gateway.Indoor3, _serializerOptions));
                        }

                        if (options.Rain)
                        {
                            console.Out.WriteLine("Reading all data from the Netatmo web service.");
                            console.Out.WriteLine("Rain Data:");
                            console.Out.WriteLine(JsonSerializer.Serialize<RainData>(gateway.Rain, _serializerOptions));
                        }

                        if (options.Wind)
                        {
                            console.Out.WriteLine("Reading all data from the Netatmo web service.");
                            console.Out.WriteLine("Wind Data:");
                            console.Out.WriteLine(JsonSerializer.Serialize<WindData>(gateway.Wind, _serializerOptions));
                        }
                    }
                    else
                    {
                        if (options.Data)
                        {
                            console.Out.WriteLine($"Value of Netatmo data property '{options.Name}' = {gateway.Data.GetPropertyValue(options.Name)}");
                        }

                        if (options.Main)
                        {
                            console.Out.WriteLine($"Value of Netatmo main data property '{options.Name}' = {gateway.Main.GetPropertyValue(options.Name)}");
                        }

                        if (options.Outdoor)
                        {
                            console.Out.WriteLine($"Value of Netatmo outdoor data property '{options.Name}' = {gateway.Outdoor.GetPropertyValue(options.Name)}");
                        }

                        if (options.Indoor1)
                        {
                            console.Out.WriteLine($"Value of Netatmo indoor 1 data property '{options.Name}' = {gateway.Indoor1.GetPropertyValue(options.Name)}");
                        }

                        if (options.Indoor2)
                        {
                            console.Out.WriteLine($"Value of Netatmo indoor 2 data property '{options.Name}' = {gateway.Indoor2.GetPropertyValue(options.Name)}");
                        }

                        if (options.Indoor3)
                        {
                            console.Out.WriteLine($"Value of Netatmo indoor 3 data property '{options.Name}' = {gateway.Indoor3.GetPropertyValue(options.Name)}");
                        }

                        if (options.Rain)
                        {
                            console.Out.WriteLine($"Value of Netatmo rain data property '{options.Name}' = {gateway.Rain.GetPropertyValue(options.Name)}");
                        }

                        if (options.Wind)
                        {
                            console.Out.WriteLine($"Value of Netatmo wind data property '{options.Name}' = {gateway.Wind.GetPropertyValue(options.Name)}");
                        }
                    }
                }
                else
                {
                    console.Out.WriteLine($"Error reading all data from the Netatmo web service.");
                    return (int)ExitCodes.NotSuccessfullyCompleted;
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