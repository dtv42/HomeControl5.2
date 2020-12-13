// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseRootCommand.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>10-12-2020 10:03</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace UtilityLib
{
    #region Using Directives

    using System.CommandLine;

    using Microsoft.Extensions.Logging;

    using Serilog.Events;

    #endregion Using Directives

    /// <summary>
    /// Base RootCommand class providing a logger data member and standard command options.
    /// </summary>
    public class BaseRootCommand : RootCommand
    {
        #region Protected Data Members

        /// <summary>
        /// The logger instance.
        /// </summary>
        protected readonly ILogger? _logger;

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
                description: "global verbose option")
                .Default(options.Verbose)
            );

            AddOption(new Option<bool>(
                alias: "--settings",
                description: "command show settings")
                .Default(options.Settings)
            );

            AddOption(new Option<bool>(
                alias: "--configuration",
                description: "command show configuration")
                .Default(options.Configuration)
            );

            _logger = logger;
            _logger?.LogDebug($"BaseRootCommand(description: {description})");
        }

        #endregion Constructors
    }
}
