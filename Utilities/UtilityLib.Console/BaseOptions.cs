// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BaseOptions.cs" company="DTV-Online">
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
    /// <summary>
    /// Global and standard options (<see cref="BaseRootCommand"/>).
    /// </summary>
    public class BaseOptions
    {
        /// <summary>
        /// Displays verbose application output on console.
        /// </summary>
        public bool Verbose { get; set; } = false;

        /// <summary>
        /// Shows application settings (JSON) on console.
        /// </summary>
        public bool Settings { get; set; } = false;

        /// <summary>
        /// Shows application configuration (JSON) on console.
        /// </summary>
        public bool Configuration { get; set; } = false;
    }
}
