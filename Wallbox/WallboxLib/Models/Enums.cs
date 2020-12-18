// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Enums.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>26-4-2020 20:19</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace WallboxLib.Models
{
    #region Using Directives

    using System;

    #endregion

    public enum ComModulePresent
    {
        NotPresent = 0,
        Present = 1
    }

    public enum BackendPresent
    {
        NotPresent = 0,
        Present = 1
    }

    [Flags]
    public enum DipSwitches
    {
        DWS1_1 = 0b10000000,
        DWS1_2 = 0b01000000,
        DWS1_3 = 0b00100000,
        DWS1_4 = 0b00010000,
        DWS1_5 = 0b00001000,
        DWS1_6 = 0b00000100,
        DWS1_7 = 0b00000010,
        DWS1_8 = 0b00000001,
    }

    public enum ChargingStates
    {
        Startup = 0,
        NotReady = 1,
        Ready = 2,
        Charging = 3,
        Error = 4,
        Interrupted = 5
    }

    public enum PlugStates
    {
        Unplugged = 0,
        PluggedStation = 1,
        LockedStation = 3,
        PluggedVehicle = 5,
        LockedVehicle = 7
    }

    public enum AuthorizationFunction
    {
        Deactivated = 0,
        Activated = 1
    }

    public enum AuthorizationRequired
    {
        NotRequired = 0,
        Required = 1
    }

    public enum ChargingEnabled
    {
        CannotEnable = 0,
        CanEnable = 1
    }

    public enum UserEnabled
    {
        Disabled = 0,
        Enabled = 1
    }

    public enum InputStates
    {
        Off = 0,
        On = 1
    }

    public enum Reasons
    {
        NotEnded = 0,
        Terminated = 1,
        Deauthorized = 10
    }
}
