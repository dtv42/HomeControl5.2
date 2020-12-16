// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AppSettings.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>20-4-2020 13:29</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace ModbusApp.Models
{
    using ModbusLib.Models;
    using ModbusApp.Options;

    /// <summary>
    /// Helper class to provide application specific settings.
    /// </summary>
    public class AppSettings
    {
        #region Public Properties

        public RtuCommandOptions RtuOptions { get; set; } = new RtuCommandOptions();
        public TcpCommandOptions TcpOptions { get; set; } = new TcpCommandOptions();

        #endregion
    }
}
