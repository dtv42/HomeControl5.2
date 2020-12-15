// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseRootCommand.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>15-12-2020 07:47</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace UtilityLib.Console
{
    #region Using Directives

    using System.CommandLine;
    using System.CommandLine.IO;
    using System.Text.Json;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    #endregion Using Directives

    /// <summary>
    /// Base RootCommand class providing a logger data member, global and standard command options.
    /// The verbose option is global (can be used in all sub commands). 
    /// The settings and configuration option is available in the derived root command only.
    /// </summary>
    public class BaseRootCommand : RootCommand
    {
        #region Protected Data Members

        /// <summary>
        /// The logger instance.
        /// </summary>
        protected readonly ILogger? _logger;

        /// <summary>
        /// The default JSON serializer options.
        /// </summary>
        protected readonly JsonSerializerOptions _jsonoptions = JsonExtensions.DefaultSerializerOptions;

        #endregion Protected Data Members

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseRootCommand"/> class.
        /// </summary>
        /// <param name="options">The global options instance.</param>
        /// <param name="logger">The logger instance.</param>
        /// <param name="description">The optional command description.</param>
        public BaseRootCommand(BaseOptions options, ILogger logger, string description = "") : base(description)
        {
            AddGlobalOption(new Option<bool>(
                alias: "--verbose",
                description: "Global verbose option")
                .Default(options.Verbose)
            );

            AddOption(new Option<bool>(
                alias: "--settings",
                description: "Show settings option")
                .Default(options.Settings)
            );

            AddOption(new Option<bool>(
                alias: "--configuration",
                description: "Show configuration option")
                .Default(options.Configuration)
            );

            _logger = logger;
            _logger?.LogDebug($"BaseRootCommand(description: {description})");
        }

        public void ShowSettings<TSettings>(IConsole console, BaseOptions options, TSettings settings) where TSettings : class, new()
        {
            if (options.Settings)
            {
                console.Out.WriteLine($"AppSettings: {JsonSerializer.Serialize(settings, _jsonoptions)}");
                console.Out.WriteLine();
            }
        }

        public void ShowConfiguration(IConsole console, BaseOptions options, IConfiguration configuration)
        {
            if (options.Configuration)
            {
                console.Out.WriteLine($"Configuration: {JsonSerializer.Serialize(configuration.AsEnumerable(), _jsonoptions)}");
                console.Out.WriteLine();
            }
        }

        #endregion Constructors
    }
}
