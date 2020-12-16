﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ITcpClientSettings.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>13-5-2020 13:52</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace ModbusLib.Models
{
    public interface ITcpClientSettings
    {
        TcpMasterData TcpMaster { get; set; }
        TcpSlaveData TcpSlave { get; set; }
    }
}