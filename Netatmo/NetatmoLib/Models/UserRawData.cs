// --------------------------------------------------------------------------------------------------------------------
// <copyright file="UserData.cs" company="DTV-Online">
//   Copyright (c) 2021 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>15-1-2021 15:57</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace NetatmoLib.Models
{
    #region Using Directives

    using System.Text.Json.Serialization;

    #endregion

    public class UserRawData
    {
        [JsonPropertyName("mail")]
        public string Mail { get; set; } = string.Empty;

        [JsonPropertyName("administrative")]
        public AdministrativeRawData Administrative { get; set; } = new AdministrativeRawData();
    }
}
