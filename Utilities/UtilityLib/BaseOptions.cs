// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseOptions.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>10-12-2020 11:07</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace UtilityLib
{
    #region Using Directives

    using Serilog.Events;

    #endregion Using Directives

    /// <summary>
    /// Standard global options.
    /// </summary>
    public class BaseOptions
    {
        public LogEventLevel LogLevel { get; set; } = LogEventLevel.Fatal;
        public bool Verbose { get; set; } = false;
        public bool Settings { get; set; } = false;
    }
}
