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
    /// The options for the test sub command.
    /// </summary>
    public class TestOptions
    {
        public string Name { get; set; } = "First";
        public string Value { get; set; } = "Zero";
        public bool AOption { get; set; }
        public bool BOption { get; set; }
        public bool COption { get; set; }
        public bool HOption { get; set; }
    }
}
