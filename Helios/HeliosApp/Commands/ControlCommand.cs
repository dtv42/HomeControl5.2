// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ControlCommand.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>26-4-2020 10:05</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace HeliosApp.Commands
{
    #region Using Directives

    using System.CommandLine;
    using System.CommandLine.Invocation;
    using System.CommandLine.IO;
    using System.CommandLine.Parsing;
    using System.Text.Json;

    using Microsoft.Extensions.Logging;

    using UtilityLib;
    using UtilityLib.Console;

    using HeliosLib;
    using HeliosLib.Models;

    using HeliosApp.Options;

    #endregion

    public class ControlCommand : BaseCommand
    {
        #region Private Data Members

        private readonly JsonSerializerOptions _serializerOptions = JsonExtensions.DefaultSerializerOptions;

        #endregion Private Data Members

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlCommand"/> class.
        /// </summary>
        /// <param name="gateway"></param>
        /// <param name="logger"></param>
        public ControlCommand(HeliosGateway gateway, ILogger<ControlCommand> logger)
            : base(logger, "control", "Control the Helios KWL EC 200 ventilation system.")
        {
            _logger?.LogDebug("ControlCommand()");

            AddOption(new Option<string>(new string[] { "-o", "--operation" }, "Sets the operation mode.").Name("string").FromAmongIgnoreCase("auto", "manual"));
            AddOption(new Option<bool>  (new string[] { "-b", "--booster"   }, "Sets the booster operation (-m,-l,-d)."));
            AddOption(new Option<bool>  (new string[] { "-s", "--standby"   }, "Sets the standby operation (-m,-l,-d)."));
            AddOption(new Option<int>   (new string[] { "-f", "--fan"       }, "Sets the fan ventilation level (0..4).").Name("number").FromAmong(0, 1, 2, 3, 4));
            AddOption(new Option<string>(new string[] { "-m", "--mode"      }, "Sets the mode.").Name("string").FromAmongIgnoreCase("on", "off"));
            AddOption(new Option<int>   (new string[] { "-l", "--level"     }, "Sets the ventilation level (0..4).").Name("number").FromAmong(0, 1, 2, 3, 4));
            AddOption(new Option<int>   (new string[] { "-d", "--duration"  }, "Sets the duration (5..180).").Name("number").Range(5, 180).Default(120));
            AddOption(new Option<bool>("--status", "Shows the data status"));

            // Setup execution handler.
            Handler = CommandHandler.Create<IConsole, ParseResult, GlobalOptions, ControlOptions>
                ((console, result, globals, options) =>
                {
                    logger.LogDebug("Handler()");

                    if (!options.CheckOptions(console)) return (int)ExitCodes.IncorrectFunction;

                    if (globals.Verbose)
                    {
                        console.Out.WriteLine($"Commandline Application: {RootCommand.ExecutableName}");
                        console.Out.WriteLine($"Password:      {globals.Password}");
                        console.Out.WriteLine($"Address:       {globals.Address}");
                        console.Out.WriteLine($"Timeout:       {globals.Timeout}");
                        console.Out.WriteLine();
                    }

                    var hasOptionO = result.HasOption("-o");
                    var hasOptionF = result.HasOption("-f");
                    var hasOptionM = result.HasOption("-m");
                    var hasOptionL = result.HasOption("-l");
                    var hasOptionD = result.HasOption("-d");

                    if (hasOptionO)
                    {
                        if (options.Operation.ToLower() == "auto")
                        {
                            gateway.SetOperationMode(OperationModes.Automatic);
                        }
                        else if (options.Operation.ToLower() == "manual")
                        {
                            gateway.SetOperationMode(OperationModes.Manual);
                        }
                    }
                    else if (hasOptionF)
                    {
                        gateway.SetFanLevel((FanLevels)options.Fan);
                    }
                    else if (options.Booster)
                    {
                        if (hasOptionM && !hasOptionL && !hasOptionD)
                        {
                            gateway.SetBoosterMode(options.Mode.ToLower() == "on");
                        }

                        if (hasOptionM && (hasOptionL || hasOptionD))
                        {
                            var data = new VentilationData()
                            {
                                Mode = (options.Mode.ToLower() == "on"),
                                Level = hasOptionL ? (FanLevels)options.Level : FanLevels.Level4,
                                Duration = hasOptionD ? options.Duration : 120
                            };

                            gateway.SetBooster(data);
                        }

                        if (!hasOptionM && hasOptionL && !hasOptionD)
                        {
                            gateway.SetBoosterLevel((FanLevels)options.Level);
                        }

                        if (!hasOptionM && !hasOptionL && hasOptionD)
                        {
                            gateway.SetBoosterDuration(options.Duration);
                        }
                    }
                    else if (options.Standby)
                    {
                        if (hasOptionM && !hasOptionL && !hasOptionD)
                        {
                            gateway.SetStandbyMode(options.Mode.ToLower() == "on");
                        }

                        if (hasOptionM && (hasOptionL || hasOptionD))
                        {
                            var data = new VentilationData()
                            {
                                Mode = (options.Mode.ToLower() == "on"),
                                Level = hasOptionL ? (FanLevels)options.Level : FanLevels.Level4,
                                Duration = hasOptionD ? options.Duration : 120
                            };

                            gateway.SetStandby(data);
                        }

                        if (!hasOptionM && hasOptionL && !hasOptionD)
                        {
                            gateway.SetStandbyLevel((FanLevels)options.Level);
                        }

                        if (!hasOptionM && !hasOptionL && hasOptionD)
                        {
                            gateway.SetStandbyDuration(options.Duration);
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
