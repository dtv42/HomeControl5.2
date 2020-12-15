// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExitCodes.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>10-12-2020 15:24</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace UtilityLib
{
    #region Using Directives

    using System.Runtime.InteropServices;

    #endregion Using Directives

    /// <summary>
    /// Enumeration of standard program exit codes.
    /// </summary>
    public enum ExitCodes
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

    /// <summary>
    /// Helper class to define standard program exit codes.
    /// </summary>
    public static class OsExitCodes
    {
        
        /// <summary>
        /// Standard program exit codes.
        /// </summary>

        public static readonly ExitCodes SuccessfullyCompleted = ExitCodes.SuccessfullyCompleted;

        public static readonly ExitCodes IncorrectFunction        = ExitCodes.IncorrectFunction;
        public static readonly ExitCodes FileNotFound             = ExitCodes.FileNotFound;
        public static readonly ExitCodes PathNotFound             = ExitCodes.PathNotFound;
        public static readonly ExitCodes CantOpenFile             = ExitCodes.CantOpenFile;
        public static readonly ExitCodes AccessDenied             = ExitCodes.AccessDenied;
        public static readonly ExitCodes InvalidData              = ExitCodes.InvalidData;
        public static readonly ExitCodes ProgramNotRecognized     = ExitCodes.ProgramNotRecognized;
        public static readonly ExitCodes NotSuccessfullyCompleted = ExitCodes.NotSuccessfullyCompleted;

        public static readonly ExitCodes OperationCanceled =
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? ExitCodes.OperationCanceled : ExitCodes.SIGINT;

        public static readonly ExitCodes UnhandledException =
            RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? ExitCodes.UnhandledException : ExitCodes.SIGABRT;
    }
}