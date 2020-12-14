// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseCommand.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>10-12-2020 09:53</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace UtilityLib
{
    #region Using Directives

    using System.CommandLine;
    using System.Text.Json;
    using Microsoft.Extensions.Logging;

    #endregion Using Directives

    /// <summary>
    /// Base Command class providing a logger data member.
    /// </summary>
    public class BaseCommand : Command
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
        /// Initializes a new instance of the <see cref="BaseCommand"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        /// <param name="name">The command name.</param>
        /// <param name="description">The optional command description.</param>
        public BaseCommand(ILogger logger, string name, string? description = null) : base(name, description)
        {
            _logger = logger;
            _logger?.LogDebug($"BaseCommand(name: {name}, description: {description})");
        }

        #endregion Constructors
    }
}
