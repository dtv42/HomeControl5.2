// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ETAPU11Info.cs" company="DTV-Online">
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

    using UtilityLib;

    #endregion

    public class ETAPU11Info
    {
        public ETAPU11Settings Settings { get; set; } = new ETAPU11Settings();
        public bool IsStartupOk { get; set; }
        public bool IsLocked { get; set; }
        public DataStatus Status { get; set; } = new DataStatus();
    }
}
