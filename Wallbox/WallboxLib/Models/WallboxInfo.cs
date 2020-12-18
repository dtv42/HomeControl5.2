// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeliosInfo.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>26-4-2020 10:16</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace WallboxLib.Models
{
    #region Using Directives

    using UtilityLib;

    #endregion

    public class WallboxInfo
    {
        public WallboxSettings Settings    { get; set; } = new WallboxSettings();
        public bool            IsStartupOk { get; set; }
        public bool            IsLocked    { get; set; }
        public DataStatus      Status      { get; set; } = new DataStatus();
    }
}
