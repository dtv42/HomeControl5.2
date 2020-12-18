﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BodyInverterInfo.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
// Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace FroniusLib.Models
{
    #region Using Directives

    using System.Collections.Generic;

    #endregion

    public class BodyInverterInfo
    {
        public Dictionary<string, RawInverterData> Data { get; set; } = new Dictionary<string, RawInverterData> { };
    }
}
