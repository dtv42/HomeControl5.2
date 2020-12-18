// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeliosEnums.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>26-4-2020 10:05</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace HeliosLib.Models
{
    #region Enums

    /// <summary>
    /// 
    /// </summary>
    public enum AutoSoftwareUpdates
    {
        Disabled = 0,
        Enabled = 1
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ConfigOptions
    {
        DiBt = 1,
        Passive = 2
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ContactTypes
    {
        Function1 = 1,
        Function2 = 2,
        Function3 = 3,
        Function4 = 4,
        Function5 = 5,
        Function6 = 6
    }

    /// <summary>
    /// 
    /// </summary>
    public enum DateFormats
    {
        DDMMYY = 0,
        MMDDYYYY = 1,
        YYYYMMDD = 2
    }

    /// <summary>
    /// 
    /// </summary>
    public enum DaylightSaving
    {
        Winter = 0,
        Summer = 1
    }

    /// <summary>
    /// 
    /// </summary>
    public enum FanLevelConfig
    {
        Continuous = 0,
        Discrete = 1
    }

    /// <summary>
    /// 
    /// </summary>
    public enum FanLevels
    {
        Level0 = 0,
        Level1 = 1,
        Level2 = 2,
        Level3 = 3,
        Level4 = 4
    }

    /// <summary>
    /// 
    /// </summary>
    public enum FaultTypes
    {
        MultipleFaults = 1,
        SingleFault = 2
    }

    /// <summary>
    /// 
    /// </summary>
    public enum FunctionTypes
    {
        Function1 = 1,
        Function2 = 2
    }

    /// <summary>
    /// 
    /// </summary>
    public enum GlobalUpdates
    {
        NotUpdated = 0,
        Manual = 1
    }

    /// <summary>
    /// 
    /// </summary>
    public enum HeatExchangerTypes
    {
        Plastic = 1,
        Aluminum = 2,
        Enthalpie = 3
    }

    /// <summary>
    /// 
    /// </summary>
    public enum HeliosPortalAccess
    {
        Disabled = 0,
        Enabled = 1
    }

    /// <summary>
    /// 
    /// </summary>
    public enum KwlFTFConfig
    {
        RF = 1,
        Temp = 2,
        Combined = 3
    }

    /// <summary>
    /// 
    /// </summary>
    public enum KwlSensorConfig
    {
        None = 0,
        Imstalled = 1
    }

    /// <summary>
    /// 
    /// </summary>
    public enum MinimumFanLevels
    {
        Level0 = 0,
        Level1 = 1
    }

    /// <summary>
    /// 
    /// </summary>
    public enum OperationModes
    {
        Automatic = 0,
        Manual = 1
    }

    /// <summary>
    /// 
    /// </summary>
    public enum PreheaterTypes
    {
        Basis = 1,
        ERW = 2,
        SEWT = 3,
        Other = 4
    }

    /// <summary>
    /// 
    /// </summary>
    public enum SensorStatus
    {
        Off = 0,
        Steps = 2,
        Smooth = 3
    }

    /// <summary>
    /// 
    /// </summary>
    public enum StatusTypes
    {
        Off = 0,
        On = 1
    }

    /// <summary>
    /// 
    /// </summary>
    public enum VacationOperations
    {
        Off = 0,
        Interval = 1,
        Constant = 2
    }

    /// <summary>
    /// 
    /// </summary>
    public enum WeeklyProfiles
    {
        Standard1 = 0,
        Standard2 = 1,
        Fixed = 2,
        Individual1 = 3,
        Individual2 = 4,
        NA = 5,
        Off = 6
    }

    #endregion
}
