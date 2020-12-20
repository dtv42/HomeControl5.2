namespace UtilityLib.Console
{
    #region Using Directives

    using System;
    using System.CommandLine;
    using System.CommandLine.IO;

    #endregion Using Directives

    public static class ConsoleExtensions
    {
        public static void RedWrite(this IConsole console, string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;

            console.Out.Write(message);

            Console.ResetColor();
        }

        public static void RedWriteLine(this IConsole console, string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;

            console.Out.WriteLine(message);

            Console.ResetColor();
        }

        public static void YellowWrite(this IConsole console, string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;

            console.Out.Write(message);

            Console.ResetColor();
        }

        public static void YellowWriteLine(this IConsole console, string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;

            console.Out.WriteLine(message);

            Console.ResetColor();
        }

        public static void GreenWrite(this IConsole console, string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;

            console.Out.Write(message);

            Console.ResetColor();
        }

        public static void GreenWriteLine(this IConsole console, string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;

            console.Out.WriteLine(message);

            Console.ResetColor();
        }

        public static void BlueWrite(this IConsole console, string message)
        {
            Console.ForegroundColor = ConsoleColor.Blue;

            console.Out.Write(message);

            Console.ResetColor();
        }

        public static void BlueWriteLine(this IConsole console, string message)
        {
            Console.ForegroundColor = ConsoleColor.Blue;

            console.Out.WriteLine(message);

            Console.ResetColor();
        }
    }
}
