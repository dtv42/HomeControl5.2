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

    using System;
    using System.CommandLine.Parsing;
    using System.Linq;

    #endregion

    /// <summary>
    ///  Extension methods for command line parser.
    /// </summary>
    public static class ParserExtensions
    {
        // 


        /// <summary>
        /// Checks if the commandline contains a named argument.
        /// </summary>
        /// <param name="parseResult">The commandline parser result.</param>
        /// <returns>True if an argument found.</returns>      
        public static bool HasArgument(this ParseResult parseResult)
        {
            if (parseResult is null)
            {
                throw new ArgumentNullException(nameof(parseResult));
            }

            return parseResult.Tokens.Any(t => t.Type == TokenType.Argument);
        }

        /// <summary>
        /// Returns the number of arguments found.
        /// </summary>
        /// <param name="parseResult">The commandline parser result.</param>
        /// <returns>The number of arguments.</returns>
        public static int ArgumentCount(this ParseResult parseResult)
        {
            if (parseResult is null)
            {
                throw new ArgumentNullException(nameof(parseResult));
            }

            return parseResult.Tokens.Count(t => t.Type == TokenType.Argument);
        }
    }
}
