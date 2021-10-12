// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TokenData.cs" company="DTV-Online">
//   Copyright (c) 2021 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>15-1-2021 11:08</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace NetatmoLib.Models
{
    #region Using Directives

    using System.Text.Json.Serialization;

    #endregion

    /// <summary>
    /// Provides properties for the Netatmo access token.
    /// </summary>
    public class TokenData
    {
        #region Public Properties

        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = string.Empty;

        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; } = string.Empty;

        #endregion
    }
}