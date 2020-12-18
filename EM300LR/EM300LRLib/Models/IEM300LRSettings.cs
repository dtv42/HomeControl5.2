// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EM300LRSettings.cs" company="DTV-Online">
//   Copyright(c) 2019 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace EM300LRLib.Models
{
    #region Using Directives

    using UtilityLib;

    #endregion Using Directives

    /// <summary>
    /// Class holding all EM300LR settings (password, and serial number).
    /// </summary>
    public interface IEM300LRSettings : IHttpClientSettings
    {
        string Password { get; set; }

        string SerialNumber { get; set; }
    }
}