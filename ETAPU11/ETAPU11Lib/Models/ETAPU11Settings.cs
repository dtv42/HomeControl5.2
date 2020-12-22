// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ETAPU11Settings.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>17-12-2020 12:52</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace ETAPU11Lib.Models
{
    #region Using Directives

    using ModbusLib.Models;

    #endregion

    /// <summary>
    /// The ETAPU11 specific settings are provided here.
    /// </summary>
    public class ETAPU11Settings : TcpClientSettings, IETAPU11Settings
    {
    }
}
