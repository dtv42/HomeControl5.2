// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingsCommand.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>10-12-2020 16:49</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace UtilityApp.Commands
{
    #region Using Directives

    using System.CommandLine;
    using System.CommandLine.IO;
    using System.CommandLine.Invocation;
    using System.Linq;
    using System.Text.Json;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using UtilityLib;
    using UtilityLib.Console;
    using UtilityApp.Models;

    #endregion Using Directives

    /// <summary>
    ///  Sample of a command using the application settings.
    /// </summary>
    public class SettingsCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        ///  Initializes a new instance of the <see cref="SettingsCommand"/> class.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        public SettingsCommand(IConfiguration configuration, ILogger<SettingsCommand> logger)
            : base(logger, "settings", "A dotnet console application sub command - settings command")
        {
            logger.LogDebug("SettingsCommand()");

            // Setup command options.
            AddOption(new Option<bool>(
                aliases: new string[] { "-o", "--options" },
                description: "Show option settings")
            );

            // Setup command options.
            AddOption(new Option<bool>(
                aliases: new string[] { "-j", "--json" },
                description: "Show application settings (JSON)")
            );

            // Get settings from configuration.
            AppSettings settings = new AppSettings();
            configuration.GetSection("AppSettings").Bind(settings);

            // Setup execution handler.
            Handler = CommandHandler.Create<IConsole, bool, bool, bool>((console, verbose, options, json) =>
            {
                logger.LogDebug("Handler()");

                if (verbose)
                {
                    console.Out.WriteLine($"Commandline Application: {RootCommand.ExecutableName}");
                    console.Out.WriteLine();
                }

                if (json)
                {
                    console.Out.WriteLine($"AppSettings: {JsonSerializer.Serialize(settings, _jsonoptions)}");
                    console.Out.WriteLine();
                }

                if (options)
                {
                    console.Out.WriteLine($"GlobalOptions:");
                    console.Out.WriteLine($"    Verbose:       {settings.GlobalOptions.Verbose}");
                    console.Out.WriteLine($"    Settings:      {settings.GlobalOptions.Settings}");
                    console.Out.WriteLine($"    Configuration: {settings.GlobalOptions.Configuration}");
                    console.Out.WriteLine($"    Password:      {settings.GlobalOptions.Password}");
                    console.Out.WriteLine($"    Host:          {settings.GlobalOptions.Host}");
                    console.Out.WriteLine();

                    console.Out.WriteLine($"GreetOptions:");
                    console.Out.WriteLine($"    Greeting:      {settings.GreetOptions.Greeting}");
                    console.Out.WriteLine($"    Name:          {settings.GreetOptions.Name}");
                    console.Out.WriteLine();
                }

                console.Out.WriteLine($"Settings:");
                console.Out.WriteLine($"    StringValue:         {settings.Data.StringValue}");
                console.Out.WriteLine($"    BooleanValue:        {settings.Data.BooleanValue}");
                console.Out.WriteLine($"    IntegerValue:        {settings.Data.IntegerValue}");
                console.Out.WriteLine($"    LongValue:           {settings.Data.LongValue}");
                console.Out.WriteLine($"    FloatValue:          {settings.Data.FloatValue}");
                console.Out.WriteLine($"    DoubleValue:         {settings.Data.DoubleValue}");
                console.Out.WriteLine($"    DecimalValue:        {settings.Data.DecimalValue}");
                console.Out.WriteLine($"    DateTimeValue:       {settings.Data.DateTimeValue}");
                console.Out.WriteLine($"    DateTimeOffsetValue: {settings.Data.DateTimeOffsetValue}");
                console.Out.WriteLine();
                console.Out.WriteLine($"    Settings:");
                console.Out.WriteLine($"        StringValue:     {settings.Data.Settings.StringValue}");
                console.Out.WriteLine($"        BooleanValue:    {settings.Data.Settings.BooleanValue}");
                console.Out.WriteLine($"        IntegerValue:    {settings.Data.Settings.IntegerValue}");
                console.Out.WriteLine($"        LongValue:       {settings.Data.Settings.LongValue}");
                console.Out.WriteLine($"        FloatValue:      {settings.Data.Settings.FloatValue}");
                console.Out.WriteLine($"        DoubleValue:     {settings.Data.Settings.DoubleValue}");
                console.Out.WriteLine($"        DecimalValue:    {settings.Data.Settings.DecimalValue}");
                console.Out.WriteLine();

                if (verbose)
                {
                    for (var i = 0; i < settings.Data.StringArray.Length; ++i)         console.Out.WriteLine($"    StringArray         [{i}]: {settings.Data.StringArray[i]}");
                    for (var i = 0; i < settings.Data.BooleanArray.Length; ++i)        console.Out.WriteLine($"    BooleanArray        [{i}]: {settings.Data.BooleanArray[i]}");
                    for (var i = 0; i < settings.Data.IntegerArray.Length; ++i)        console.Out.WriteLine($"    IntegerArray        [{i}]: {settings.Data.IntegerArray[i]}");
                    for (var i = 0; i < settings.Data.LongArray.Length; ++i)           console.Out.WriteLine($"    LongArray           [{i}]: {settings.Data.LongArray[i]}");
                    for (var i = 0; i < settings.Data.FloatArray.Length; ++i)          console.Out.WriteLine($"    FloatArray          [{i}]: {settings.Data.FloatArray[i]}");
                    for (var i = 0; i < settings.Data.DoubleArray.Length; ++i)         console.Out.WriteLine($"    DoubleArray         [{i}]: {settings.Data.DoubleArray[i]}");
                    for (var i = 0; i < settings.Data.DecimalArray.Length; ++i)        console.Out.WriteLine($"    DecimalArray        [{i}]: {settings.Data.DecimalArray[i]}");
                    for (var i = 0; i < settings.Data.DateTimeArray.Length; ++i)       console.Out.WriteLine($"    DateTimeArray       [{i}]: {settings.Data.DateTimeArray[i]}");
                    for (var i = 0; i < settings.Data.DateTimeOffsetArray.Length; ++i) console.Out.WriteLine($"    DateTimeOffsetArray [{i}]: {settings.Data.DateTimeOffsetArray[i]}");

                    console.Out.WriteLine();

                    for (var i = 0; i < settings.Data.StringList.Count; ++i)         console.Out.WriteLine($"    StringList         [{i}]: {settings.Data.StringList[i]}");
                    for (var i = 0; i < settings.Data.BooleanList.Count; ++i)        console.Out.WriteLine($"    BooleanList        [{i}]: {settings.Data.BooleanList[i]}");
                    for (var i = 0; i < settings.Data.IntegerList.Count; ++i)        console.Out.WriteLine($"    IntegerList        [{i}]: {settings.Data.IntegerList[i]}");
                    for (var i = 0; i < settings.Data.LongList.Count; ++i)           console.Out.WriteLine($"    LongList           [{i}]: {settings.Data.LongList[i]}");
                    for (var i = 0; i < settings.Data.FloatList.Count; ++i)          console.Out.WriteLine($"    FloatList          [{i}]: {settings.Data.FloatList[i]}");
                    for (var i = 0; i < settings.Data.DoubleList.Count; ++i)         console.Out.WriteLine($"    DoubleList         [{i}]: {settings.Data.DoubleList[i]}");
                    for (var i = 0; i < settings.Data.DecimalList.Count; ++i)        console.Out.WriteLine($"    DecimalList        [{i}]: {settings.Data.DecimalList[i]}");
                    for (var i = 0; i < settings.Data.DateTimeList.Count; ++i)       console.Out.WriteLine($"    DateTimeList       [{i}]: {settings.Data.DateTimeList[i]}");
                    for (var i = 0; i < settings.Data.DateTimeOffsetList.Count; ++i) console.Out.WriteLine($"    DateTimeOffsetList [{i}]: {settings.Data.DateTimeOffsetList[i]}");
                    for (var i = 0; i < settings.Data.Dictionary.Count; ++i)         console.Out.WriteLine($"    Dictionary         [{i}]: {settings.Data.Dictionary.Keys.ToArray()[i]}, {settings.Data.Dictionary.Values.ToArray()[i]}");

                    console.Out.WriteLine();
                }
                return (int)ExitCodes.SuccessfullyCompleted;
            });
        }

        #endregion Constructors
    }
}