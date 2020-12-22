// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GlobalOptions.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>20-12-2020 20:34</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace ETAPU11App.Options
{
    #region Using Directives

    using UtilityLib.Console;

    using ModbusLib.Models;

    using ETAPU11Lib.Models;

    #endregion Using Directives

    /// <summary>
    /// The application global options. The default global options are inherited from <see cref="BaseOptions"/>.
    /// Note that secret options like the Password option is typically set using the ASP.NET Core Secret Manager. 
    /// </summary>
    public class GlobalOptions : BaseOptions, IETAPU11Settings
    {
        /// <summary>
        /// The MODBUS TCP master configuration.
        /// </summary>
        public TcpMasterData TcpMaster { get; set; } = new TcpMasterData();

        /// <summary>
        /// The MODBUS TCP slave configuration.
        /// </summary>
        public TcpSlaveData TcpSlave { get; set; } = new TcpSlaveData();
    }
}
