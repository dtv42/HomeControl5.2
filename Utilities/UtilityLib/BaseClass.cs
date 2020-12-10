// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseClass.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>10-12-2020 09:43</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace UtilityLib
{
    #region Using Directives

    using Microsoft.Extensions.Logging;

    #endregion Using Directives

    /// <summary>
    /// Base class providing a logger data member.
    /// </summary>
    public class BaseClass
    {
        #region Protected Data Members

        /// <summary>
        /// The logger instance.
        /// </summary>
        protected readonly ILogger? _logger;

        #endregion Protected Data Members

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseClass"/> class.
        /// </summary>
        /// <param name="logger">The logger instance.</param>
        public BaseClass(ILogger logger)
        {
            _logger = logger;
            _logger?.LogDebug("BaseClass()");
        }

        #endregion Constructors
    }
}
