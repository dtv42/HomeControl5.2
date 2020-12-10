// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GreetOptions.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>10-12-2020 16:19</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace UtilityApp.Options
{
    /// <summary>
    /// The options for the greet sub command.
    /// </summary>
    public class GreetOptions
    {
        public string Greeting { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}
