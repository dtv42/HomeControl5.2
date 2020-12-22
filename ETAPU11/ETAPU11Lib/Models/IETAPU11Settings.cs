// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IETAPU11Settings.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>20-12-2020 20:27</created>
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
    public interface IETAPU11Settings : ITcpClientSettings
    {}
}
