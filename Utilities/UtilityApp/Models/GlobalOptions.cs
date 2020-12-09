// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GlobalOptions.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>9-12-2020 10:19</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace UtilityApp.Models
{
    #region Using Directives

    using System;

    using Serilog.Events;

    #endregion

    public class GlobalOptions
    {
        public LogEventLevel LogLevel { get; set; } = LogEventLevel.Fatal;
        public bool Verbose { get; set; } = false;
        public string Password { get; set; } = string.Empty;
        public Uri Uri { get; set; } = new Uri("http://localhost");
    }
}
