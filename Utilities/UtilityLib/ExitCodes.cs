// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExitCodes.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>13-5-2020 13:53</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace UtilityLib
{
    #region Using Directives

    using System.Runtime.InteropServices;

    #endregion Using Directives

    /// <summary>
    /// Helper class to define standard program exit codes.
    /// </summary>
    public static class ExitCodes
    {
        /// <summary>
        /// Standard program exit codes.
        /// </summary>
        public enum Codes
        {
            // values from http://www.febooti.com/products/automation-workshop/online-help/events/run-dos-cmd-command/exit-codes/
            SuccessfullyCompleted = 0,

            IncorrectFunction = 1,
            FileNotFound = 2,
            PathNotFound = 3,
            CantOpenFile = 4,
            AccessDenied = 5,
            InvalidData = 13,
            ProgramNotRecognized = 9009,
            NotSuccessfullyCompleted = -1,

            // OSPlatform Windows
            OperationCanceled = unchecked((int)0xC000013A),
            UnhandledException = unchecked((int)0xE0434F4D),

            // ISPlatform Unix
            SIGINT = 130,
            SIGABRT = 134
        }

        public static readonly Codes SuccessfullyCompleted = Codes.SuccessfullyCompleted;

        public static readonly Codes IncorrectFunction        = Codes.IncorrectFunction;
        public static readonly Codes FileNotFound             = Codes.FileNotFound;
        public static readonly Codes PathNotFound             = Codes.PathNotFound;
        public static readonly Codes CantOpenFile             = Codes.CantOpenFile;
        public static readonly Codes AccessDenied             = Codes.AccessDenied;
        public static readonly Codes InvalidData              = Codes.InvalidData;
        public static readonly Codes ProgramNotRecognized     = Codes.ProgramNotRecognized;
        public static readonly Codes NotSuccessfullyCompleted = Codes.NotSuccessfullyCompleted;

        public static readonly Codes OperationCanceled =
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? Codes.OperationCanceled : Codes.SIGINT;

        public static readonly Codes UnhandledException =
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? Codes.UnhandledException : Codes.SIGABRT;
    }
}