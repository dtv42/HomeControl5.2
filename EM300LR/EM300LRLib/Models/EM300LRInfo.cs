// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EM300LRInfo.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>17-12-2020 12:51</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace EM300LRLib.Models
{
    #region Using Directives

    using UtilityLib;

    #endregion

    public class EM300LRInfo
    {
        public EM300LRSettings Settings   { get; set; } = new EM300LRSettings();
        public bool            IsStartupOk { get; set; }
        public bool            IsLocked    { get; set; }
        public DataStatus      Status      { get; set; } = new DataStatus();
        }
}
