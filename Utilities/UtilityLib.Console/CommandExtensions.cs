namespace UtilityLib.Console
{
    #region Using Directives

    using System;
    using System.CommandLine;
    using System.CommandLine.Help;

    #endregion Using Directives

    /// <summary>
    ///  Extension methods for command line execution.
    /// </summary>
    public static class CommandExtensions
    {
        /// <summary>
        /// Shows the help text for the command.
        /// </summary>
        /// <param name="command">The commandline command instance.</param>
        /// <param name="console">The console instance.</param>
        public static void ShowHelp(this Command command, IConsole console)
        {
            if (command is null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            HelpBuilder helpBuilder = new HelpBuilder(console);
            helpBuilder.Write(command);
        }
    }
}
