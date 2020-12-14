// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PropertyCommand.cs" company="DTV-Online">
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

    using System;
    using System.CommandLine;
    using System.CommandLine.IO;
    using System.CommandLine.Invocation;
    using System.CommandLine.Parsing;
    using System.Collections;
    using System.Reflection;
    using System.Text.Json;

    using Microsoft.Extensions.Logging;

    using UtilityLib;
    using UtilityApp.Models;
    using UtilityApp.Options;
    using Microsoft.Extensions.Configuration;

    #endregion Using Directives

    /// <summary>
    ///  Sample of a property command showing property infos using the AppSettings data instance.
    ///  Note that for named properties only simple types are supported (no arrays or lists).
    ///  A custom validation option for the command properties options.
    /// </summary>
    public sealed class PropertyCommand : BaseCommand
    {
        #region Constructors

        /// <summary>
        ///  Initializes a new instance of the <see cref="PropertyCommand"/> class.
        /// </summary>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        public PropertyCommand(IConfiguration configuration, ILogger<PropertyCommand> logger)
            : base(logger, "property", "A dotnet console application sub command - property command")
        {
            logger.LogDebug("PropertyCommand()");

            // Get settings data from configuration.
            SettingsData settings = new SettingsData();
            configuration.GetSection("AppSettings:Data").Bind(settings);

            // Setup command arguments and options.
            AddArgument(new Argument<string>("name", "The property name.").Arity(ArgumentArity.ZeroOrOne));
            AddArgument(new Argument<string>("value", "The property value.").Arity(ArgumentArity.ZeroOrOne));

            AddOption(new Option<bool>(new string[] { "-p", "--properties"   }, "Show all properties"));
            AddOption(new Option<bool>(new string[] { "-s", "--simple"       }, "Show simple properties"));
            AddOption(new Option<bool>(new string[] { "-a", "--arrays"       }, "Show arrays"));
            AddOption(new Option<bool>(new string[] { "-l", "--lists"        }, "Show lists"));
            AddOption(new Option<bool>(new string[] { "-d", "--dictionaries" }, "Show dictionaries"));
            AddOption(new Option<bool>(new string[] { "-v", "--value"        }, "Show value"));

            // Add custom validation.
            AddValidator(r =>
            {
                if (string.IsNullOrEmpty(r.GetArgumentValueOrDefault<string>("name")) &&
                    !r.Children.Contains("-p") && !r.Children.Contains("-s") &&
                    !r.Children.Contains("-a") && !r.Children.Contains("-l") && !r.Children.Contains("-d"))
                {
                        return "Please select at least a property type (-p|-s|-a|-l|-d) or specify a property name.";
                }

                return null;
            });

            // Setup execution handler.
            Handler = CommandHandler.Create<IConsole, bool, string, string, PropertyOptions>
                ((console, verbose, name, value, options) =>
            {
                logger.LogDebug("Handler()");

                if (verbose)
                {
                    console.Out.WriteLine($"Commandline Application: {RootCommand.ExecutableName}");
                    console.Out.WriteLine();
                    console.Out.WriteLine($"AppSettings: {JsonSerializer.Serialize(settings, _jsonoptions)}");
                    console.Out.WriteLine();
                }

                if (name is null)
                {
                    var properties = typeof(SettingsData).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                    foreach (var info in properties)
                    {
                        if ((options.All || options.Simple) && (info?.PropertyType.IsArray ?? false) && (info.GetValue(settings) is Array))
                        {
                            console.Out.WriteLine($"Property {info.Name}");
                            console.Out.WriteLine($"    CanRead:       {info.CanRead}");
                            console.Out.WriteLine($"    CanWrite:      {info.CanWrite}");
                            console.Out.WriteLine($"    DeclaringType: {info.DeclaringType}");
                            console.Out.WriteLine($"    PropertyType:  {info.PropertyType}");
                            console.Out.WriteLine($"    MemberType:    {info.MemberType}");

                            Array? array = (Array?)info.GetValue(settings);
                            console.Out.WriteLine($"    Array:         [{array?.Length}]");

                            if (options.Value)
                            {
                                for (int i = 0; i < array?.Length; ++i)
                                {
                                    console.Out.WriteLine($"    Value[{i}]:      {((Array?)info?.GetValue(settings))?.GetValue(i)}");
                                }
                            }
                        }
                        else if ((options.All || options.Simple) && (info?.PropertyType.IsGenericType ?? false) && (info.GetValue(settings) is IList))
                        {
                            console.Out.WriteLine($"Property {info.Name}");
                            console.Out.WriteLine($"    CanRead:       {info.CanRead}");
                            console.Out.WriteLine($"    CanWrite:      {info.CanWrite}");
                            console.Out.WriteLine($"    DeclaringType: {info.DeclaringType}");
                            console.Out.WriteLine($"    PropertyType:  {info.PropertyType}");
                            console.Out.WriteLine($"    MemberType:    {info.MemberType}");

                            IList? list = (IList?)info.GetValue(settings);
                            console.Out.WriteLine($"    List:          [{list?.Count}]");

                            if (options.Value)
                            {
                                for (int i = 0; i < list?.Count; ++i)
                                {
                                    console.Out.WriteLine($"    Value[{i}]:      {((IList?)info?.GetValue(settings))?[i]}");
                                }
                            }
                        }
                        else if ((options.All || options.Dictionaries) && (info?.PropertyType.IsGenericType ?? false) && (info.GetValue(settings) is IDictionary))
                        {
                            console.Out.WriteLine($"Property {info.Name}");
                            console.Out.WriteLine($"    CanRead:       {info.CanRead}");
                            console.Out.WriteLine($"    CanWrite:      {info.CanWrite}");
                            console.Out.WriteLine($"    DeclaringType: {info.DeclaringType}");
                            console.Out.WriteLine($"    PropertyType:  {info.PropertyType}");
                            console.Out.WriteLine($"    MemberType:    {info.MemberType}");

                            IDictionary? dictionary = (IDictionary?)info.GetValue(settings);
                            console.Out.WriteLine($"    Dictionary:    [{dictionary?.Count}]");

                            if (options.Value)
                            {
                                int i = 0;

                                if (dictionary is not null)
                                {
                                    foreach (DictionaryEntry? item in dictionary)
                                    {
                                        console.Out.WriteLine($"    Value[{i}]:      {item?.Key}, {item?.Value}");
                                    }
                                }
                            }
                        }
                        else if ((options.All || options.Simple) && !(info?.PropertyType.IsArray ?? false) && !(info?.PropertyType.IsGenericType ?? false))
                        {
                            console.Out.WriteLine($"Property {info?.Name}");
                            console.Out.WriteLine($"    CanRead:       {info?.CanRead}");
                            console.Out.WriteLine($"    CanWrite:      {info?.CanWrite}");
                            console.Out.WriteLine($"    DeclaringType: {info?.DeclaringType}");
                            console.Out.WriteLine($"    PropertyType:  {info?.PropertyType}");
                            console.Out.WriteLine($"    MemberType:    {info?.MemberType}");

                            if (options.Value)
                            {
                                console.Out.WriteLine($"    Value:         {info?.GetValue(settings)}");
                            }
                        }
                    }

                    console.Out.WriteLine();
                }
                else
                {
                    var info = typeof(SettingsData).GetProperty(name, BindingFlags.Public | BindingFlags.Instance);

                    if (info is null)
                    {
                        console.Out.WriteLine($"Property '{name}' not found.");
                        return (int)ExitCodes.InvalidData;
                    }
                    else if (value is null)
                    {
                        console.Out.WriteLine($"Property {info?.Name}");
                        console.Out.WriteLine($"    CanRead:       {info?.CanRead}");
                        console.Out.WriteLine($"    CanWrite:      {info?.CanWrite}");
                        console.Out.WriteLine($"    DeclaringType: {info?.DeclaringType}");
                        console.Out.WriteLine($"    PropertyType:  {info?.PropertyType}");
                        console.Out.WriteLine($"    MemberType:    {info?.MemberType}");

                        if ((info?.PropertyType.IsArray ?? false) && (info.GetValue(settings) is Array))
                        {
                            Array? array = (Array?)info.GetValue(settings);
                            console.Out.WriteLine($"    Array:         [{array?.Length}]");

                            if (options.Value)
                            {
                                for (int i = 0; i < array?.Length; ++i)
                                {
                                    console.Out.WriteLine($"    Value[{i}]:      {((Array?)info?.GetValue(settings))?.GetValue(i)}");
                                }
                            }
                        }
                        else if ((info?.PropertyType.IsGenericType ?? false) && (info.GetValue(settings) is IList))
                        {
                            IList? list = (IList?)info.GetValue(settings);
                            console.Out.WriteLine($"    List:          [{list?.Count}]");

                            for (int i = 0; i < list?.Count; ++i)
                            {
                                console.Out.WriteLine($"    Value[{i}]:      {((IList?)info?.GetValue(settings))?[i]}");
                            }
                        }
                        else if ((info?.PropertyType.IsGenericType ?? false) && (info.GetValue(settings) is IDictionary))
                        {
                            IDictionary? dictionary = (IDictionary?)info.GetValue(settings);
                            console.Out.WriteLine($"    Dictionary:    [{dictionary?.Count}]");

                            if (options.Value)
                            {
                                int i = 0;

                                if (!(dictionary is null))
                                {
                                    foreach (DictionaryEntry? item in dictionary)
                                    {
                                        console.Out.WriteLine($"    Value[{i}]:      {item?.Key}, {item?.Value}");
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (options.Value)
                            {
                                console.Out.WriteLine($"    Value:         {info?.GetValue(settings)}");
                            }
                        }

                        console.Out.WriteLine();
                    }
                    else
                    {
                        if (!(info?.PropertyType.IsArray ?? false) && !(info?.PropertyType.IsGenericType ?? false))
                        {
                            TypeCode typeCode = Type.GetTypeCode(info?.PropertyType);

                            switch (typeCode)
                            {
                                case TypeCode.String:
                                    info?.SetValue(settings, value);
                                    break;

                                case TypeCode.Boolean:
                                    if (bool.TryParse(value, out bool boolValue)) info?.SetValue(settings, boolValue);
                                    else console.Out.WriteLine($"Property value '{value}' invalid.");
                                    break;

                                case TypeCode.Int32:
                                    if (int.TryParse(value, out int intValue)) info?.SetValue(settings, intValue);
                                    else console.Out.WriteLine($"Property value '{value}' invalid.");
                                    break;

                                case TypeCode.Int64:
                                    if (long.TryParse(value, out long longValue)) info?.SetValue(settings, longValue);
                                    else console.Out.WriteLine($"Property value '{value}' invalid.");
                                    break;

                                case TypeCode.Single:
                                    if (float.TryParse(value, out float floatValue)) info?.SetValue(settings, floatValue);
                                    else console.Out.WriteLine($"Property value '{value}' invalid.");
                                    break;

                                case TypeCode.Double:
                                    if (double.TryParse(value, out double doubleValue)) info?.SetValue(settings, doubleValue);
                                    else console.Out.WriteLine($"Property value '{value}' invalid.");
                                    break;

                                case TypeCode.Decimal:
                                    if (decimal.TryParse(value, out decimal decimalValue)) info?.SetValue(settings, decimalValue);
                                    else console.Out.WriteLine($"Property value '{value}' invalid.");
                                    break;

                                case TypeCode.DateTime:
                                    if (DateTime.TryParse(value, out DateTime datetimeValue)) info?.SetValue(settings, datetimeValue);
                                    else console.Out.WriteLine($"Property value '{value}' invalid.");
                                    break;

                                default:
                                    console.Out.WriteLine($"Property type '{info?.PropertyType}' not supported.");
                                    break;
                            }

                            console.Out.WriteLine($"Property {info?.Name}");
                            console.Out.WriteLine($"    CanRead:       {info?.CanRead}");
                            console.Out.WriteLine($"    CanWrite:      {info?.CanWrite}");
                            console.Out.WriteLine($"    DeclaringType: {info?.DeclaringType}");
                            console.Out.WriteLine($"    PropertyType:  {info?.PropertyType}");
                            console.Out.WriteLine($"    MemberType:    {info?.MemberType}");
                            console.Out.WriteLine($"    Value:         {info?.GetValue(settings)}");
                        }
                        else
                        {
                            console.Out.WriteLine($"Only simple property types supported.");
                        }
                    }
                }

                return (int)ExitCodes.SuccessfullyCompleted;
            });
        }

        #endregion Constructors
    }
}