namespace EM300LRApp.Commands
{
    #region Using Directives

    using System;
    using System.CommandLine;
    using System.CommandLine.Invocation;
    using System.CommandLine.IO;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using UtilityLib;
    using UtilityLib.Console;

    using EM300LRApp.Options;
    using EM300LRApp.Models;
    using EM300LRLib;

    #endregion Using Directives

    public sealed class AppCommand : BaseRootCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AppCommand"/> class.
        /// </summary>
        /// <param name="configuration">The configuration instance.</param>
        /// <param name="options">The root command options.</param>
        /// <param name="logger">The logger instance.</param>
        public AppCommand(IConfiguration configuration,
                          EM300LRGateway gateway,
                          GlobalOptions options,
                          InfoCommand infoCommand,
                          ReadCommand readCommand,
                          MonitorCommand monitorCommand,
                          ILogger<AppCommand> logger)
            : base(options, logger, "Allows to read data from a b-Control EM300LR energy manager.")
        {
            logger.LogDebug("AppCommand()");

            // Adding global options to the default global options.
            AddGlobalOption(new Option<string>(
                alias: "--address",
                description: "Global address option")
                .Default(options.Address)
                .Name("uri")
                .Uri()
            );

            AddGlobalOption(new Option<string>(
                alias: "--timeout",
                description: "Global timeout option")
                .Default(options.Timeout)
                .Name("number")
            );

            AddGlobalOption(new Option<string>(
                alias: "--password",
                description: "Global password option")
                .Default(options.Password)
                .Name("string")
            );

            AddGlobalOption(new Option<string>(
                alias: "--serialnumber",
                description: "Global serial number option")
                .Default(options.SerialNumber)
                .Name("string")
            );

            // Adding sub commands to root command.
            AddCommand(infoCommand);
            AddCommand(readCommand);
            AddCommand(monitorCommand);

            // Get settings from configuration and gateway instance.
            var settings = configuration.GetSection("AppSettings").Get<AppSettings>();

            // Setup execution handler.
            Handler = CommandHandler.Create<IConsole, GlobalOptions>((console, options) =>
            {
                logger.LogDebug("Handler()");

                if (options.Verbose)
                {
                    console.Out.WriteLine($"Commandline Application: {ExecutableName}");
                    console.Out.WriteLine();
                    console.Out.WriteLine($"Configuration: {options.Configuration}");
                    console.Out.WriteLine($"Settings:      {options.Settings}");
                    console.Out.WriteLine($"Verbose:       {options.Verbose}");
                    console.Out.WriteLine($"Password:      {options.Password}");
                    console.Out.WriteLine($"Serialnumber:  {options.SerialNumber}");
                    console.Out.WriteLine($"Address:       {options.Address}");
                    console.Out.WriteLine($"Timeout:       {options.Timeout}");
                    console.Out.WriteLine();
                }

                ShowSettings(console, options, settings);
                ShowConfiguration(console, options, configuration);

                if (gateway.CheckAccess())
                {
                    Console.WriteLine($"EM300LR web service with serial number '{options.SerialNumber}' found at {options.Address}.");
                }
                else
                {
                    Console.WriteLine($"EM300LR web service with serial number '{options.SerialNumber}' not found at {options.Address}.");
                }

                return (int)ExitCodes.SuccessfullyCompleted;
            });
        }

        #endregion Constructors
    }
}
