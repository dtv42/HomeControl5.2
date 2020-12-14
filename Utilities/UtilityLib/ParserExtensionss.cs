// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ParserExtensionss.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>13-12-2020 09:11</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace UtilityLib
{
    #region Using Directives

    using System.CommandLine;
    using System.CommandLine.Parsing;
    using System.Linq;

    #endregion

    /// <summary>
    ///  Extension methods for command line parser.
    /// </summary>
    public static class ParserExtensionss
    {
        public static bool Has(this ParseResult result, Option option)
        {
            return result.Tokens.Any(t => option.Aliases.Any(a => a == t.Value));
        }
    }
}
