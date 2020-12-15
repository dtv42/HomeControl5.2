// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParserExtensions.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>15-12-2020 08:14</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace UtilityLib.Console
{
    #region Using Directives

    using System.CommandLine;
    using System.CommandLine.Parsing;
    using System.Linq;

    #endregion

    /// <summary>
    ///  Extension methods for command line parser.
    /// </summary>
    public static class ParserExtensions
    {
        /// <summary>
        /// Checks if a token matches one of the option aliases.
        /// </summary>
        /// <param name="result">The parse result instance.</param>
        /// <param name="option">The command line option.</param>
        /// <returns></returns>
        public static bool Has(this ParseResult result, Option option)
        {
            return result.Tokens.Any(t => option.Aliases.Any(a => a == t.Value));
        }
    }
}
