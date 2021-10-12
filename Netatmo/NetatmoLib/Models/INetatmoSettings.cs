// --------------------------------------------------------------------------------------------------------------------
// <copyright file="INetatmoSettings.cs" company="DTV-Online">
//   Copyright (c) 2021 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>15-1-2021 10:46</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace NetatmoLib.Models
{
    #region Using Directives

    using UtilityLib.Webapp;

    #endregion Using Directives

    /// <summary>
    /// Interface for Netatmo settings (User, Password, ID, Secret).
    /// </summary>
    public interface INetatmoSettings : IHttpClientSettings
    {
        /// <summary>
        /// The Neatmo settings;
        /// </summary>
        string User { get; set; }
        string Password { get; set; }
        string ClientID { get; set; }
        string ClientSecret { get; set; }
    }
}