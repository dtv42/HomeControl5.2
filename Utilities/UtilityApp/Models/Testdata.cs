﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Testdata.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>10-12-2020 16:36</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace UtilityApp.Models
{
    #region Using Directives

    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Text.Json.Serialization;

    using UtilityLib;

    #endregion Using Directives

    public class Testdata
    {
        [Range(0, 60)]
        public int Value { get; set; } = 42;

        [StringLength(10)]
        public string Name { get; set; } = "Data";

        public Guid Uuid { get; set; } = new Guid();

        [IPAddress]
        public string Address { get; set; } = "0.0.0.0";

        [IPEndPoint]
        public string Endpoint { get; set; } = "0.0.0.0:80";

        [Uri]
        public string Uri { get; set; } = "http://127.0.0.1:80";

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public System.Net.HttpStatusCode Code { get; set; } = System.Net.HttpStatusCode.OK;
    }
}