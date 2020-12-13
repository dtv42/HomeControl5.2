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
