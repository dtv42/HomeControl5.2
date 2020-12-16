// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RootCommand.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>20-4-2020 13:29</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace ModbusApp.Commands
{
    #region Using Directives

    using System.CommandLine;
    using System.CommandLine.IO;
    using System.CommandLine.Invocation;
    using System.Linq;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using UtilityLib;
    using UtilityLib.Console;

    using ModbusApp.Models;
    using ModbusApp.Options;

    #endregion

    /// <summary>
    /// Root command for the application providing an inherited option.
    /// Note that the default value is set from the application settings.
    /// </summary>
    internal sealed class AppCommand : BaseRootCommand
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="AppCommand"/> class.
        /// </summary>
        /// <param name="configuration">The configuration instance.</param>
        /// <param name="options">The root command options.</param>
        /// <param name="logger">The logger instance.</param>
        public AppCommand(IConfiguration configuration,
                          GlobalOptions options,
                          RtuCommand rtuCommand,
                          TcpCommand tcpCommand,
                          ILogger<AppCommand> logger)
            : base(options, logger, "Allows to read and write Modbus data using Modbus TCP or Modbus RTU.")
        {
            logger.LogDebug("AppCommand()");

            // Add custom validation.
            AddValidator(r => 
            {
                var noOptionV = r.OptionResult("--verbose")?.Token is null;
                var noOptionS = r.OptionResult("--settings")?.Token is null;
                var noOptionC = r.OptionResult("--configuration")?.Token is null;

                if (noOptionV && noOptionS && noOptionC)
                {
                    return "Please select at least an option or specify a sub command.";
                }

                return null;
            });

            // Adding sub commands to root command.
            AddCommand(rtuCommand);
            AddCommand(tcpCommand);

            // Setup execution handler.
            Handler = CommandHandler.Create<IConsole, GlobalOptions>((console, options) =>
            {
                logger.LogDebug("Handler()");

                // Get settings from configuration.
                AppSettings settings = new AppSettings();
                configuration.GetSection("AppSettings").Bind(settings);

                if (options.Verbose)
                {
                    console.Out.WriteLine($"Commandline Application: {ExecutableName}");
                    console.Out.WriteLine();
                    console.Out.WriteLine($"Configuration:  {options.Configuration}");
                    console.Out.WriteLine($"Settings:       {options.Settings}");
                    console.Out.WriteLine($"Verbose:        {options.Verbose}");
                    console.Out.WriteLine();                
                    console.Out.WriteLine("Rtu Master:");   
                    console.Out.WriteLine($"SerialPort:     {settings.RtuOptions.RtuMaster.SerialPort}");
                    console.Out.WriteLine($"Baudrate:       {settings.RtuOptions.RtuMaster.Baudrate}");
                    console.Out.WriteLine($"Parity:         {settings.RtuOptions.RtuMaster.Parity}");
                    console.Out.WriteLine($"DataBits:       {settings.RtuOptions.RtuMaster.DataBits}");
                    console.Out.WriteLine($"StopBits:       {settings.RtuOptions.RtuMaster.StopBits}");
                    console.Out.WriteLine($"ReadTimeout:    {settings.RtuOptions.RtuMaster.ReadTimeout}");
                    console.Out.WriteLine($"WriteTimeout:   {settings.RtuOptions.RtuMaster.WriteTimeout}");
                    console.Out.WriteLine($"Retries:        {settings.RtuOptions.RtuMaster.Retries}");
                    console.Out.WriteLine();
                    console.Out.WriteLine("Rtu Slave:");
                    console.Out.WriteLine($"ID:             {settings.RtuOptions.RtuSlave.ID}");
                    console.Out.WriteLine();
                    console.Out.WriteLine("Tcp Master:");
                    console.Out.WriteLine($"ReceiveTimeout: {settings.TcpOptions.TcpMaster.ReceiveTimeout}");
                    console.Out.WriteLine($"SendTimeout:    {settings.TcpOptions.TcpMaster.SendTimeout}");
                    console.Out.WriteLine();
                    console.Out.WriteLine("Tcp Slave:");
                    console.Out.WriteLine($"Address:        {settings.TcpOptions.TcpSlave.Address}");
                    console.Out.WriteLine($"Port:           {settings.TcpOptions.TcpSlave.Port}");
                    console.Out.WriteLine($"ID:             {settings.TcpOptions.TcpSlave.ID}");
                    console.Out.WriteLine();
                }

                ShowSettings(console, options, settings);
                ShowConfiguration(console, options, configuration);

                return (int)ExitCodes.SuccessfullyCompleted;
            });
        }
        
        #endregion
    }
}
