// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WriteCommand.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>17-12-2020 12:52</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace ETAPU11App.Commands
{
    #region Using Directives

    using System.CommandLine;
    using System.CommandLine.Invocation;
    using System.CommandLine.IO;
    using System.Text.Json;

    using Microsoft.Extensions.Logging;

    using UtilityLib;
    using UtilityLib.Console;

    using ETAPU11Lib;

    using ETAPU11App.Options;

    #endregion Using Directives

    /// <summary>
    /// Application command "write".
    /// </summary>
    public class WriteCommand : BaseCommand
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
        public WriteCommand(ETAPU11Gateway gateway, ILogger<WriteCommand> logger)
            : base(logger, "write", "Writing data values to an ETA PU 11 pellet boiler.")
        {
            _logger?.LogDebug("WriteCommand()");

            // Setup command arguments and options.
            AddArgument(new Argument<string>("name", "The property name (REQUIRED).").Arity(ArgumentArity.ExactlyOne));
            AddArgument(new Argument<string>("value", "The property value (REQUIRED).").Arity(ArgumentArity.ExactlyOne));

            AddOption(new Option<bool>(new string[] { "-d", "--data"    }, "Writing an all data property"));
            AddOption(new Option<bool>(new string[] { "-b", "--boiler"  }, "Writing a boiler data property."));
            AddOption(new Option<bool>(new string[] { "-w", "--water"   }, "Writing a hot water data property."));
            AddOption(new Option<bool>(new string[] { "-c", "--circuit" }, "Writing a heating circuit data property."));
            AddOption(new Option<bool>(new string[] { "-s", "--storage" }, "Writing a pellets storage data property."));
            AddOption(new Option<bool>(new string[] { "-y", "--system"  }, "Writing a system info data property."));
            AddOption(new Option<bool>("--status", "Shows the data status"));

            // Setup execution handler.
            Handler = CommandHandler.Create<IConsole, GlobalOptions, WriteOptions>
                    ((console, globals, options) =>
            {
                logger.LogDebug("Handler()");

                if (!options.CheckOptions(console)) return (int)ExitCodes.IncorrectFunction;

                if (globals.Verbose)
                {
                    console.Out.WriteLine($"Commandline Application: {RootCommand.ExecutableName}");
                    console.Out.WriteLine($"Slave Address: {globals.TcpSlave.Address}");
                    console.Out.WriteLine($"Slave Port:    {globals.TcpSlave.Port}");
                    console.Out.WriteLine($"Slave ID:      {globals.TcpSlave.ID}");
                    console.Out.WriteLine();
                }

                console.Out.WriteLine($"Writing value '{options.Value}' to property '{options.Name}' at ETAPU11 pellet boiler");
                var status = gateway.WriteProperty(options.Name, options.Value);

                if (!status.IsGood)
                {
                    console.Out.WriteLine($"Error writing property '{options.Name}' to ETAPU11 pellet boiler.");
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

        #endregion Constructors
    }
}
