// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IPEndPointConverter.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>12-12-2020 13:24</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace UtilityLib
{
    #region Using Directives

    using System;
    using System.Net;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    #endregion Using Directives

    public class IPEndPointConverter : JsonConverter<IPEndPoint>
    {
        public override IPEndPoint Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return IPEndPoint.Parse(reader.GetString() ?? "");
        }

        public override void Write(Utf8JsonWriter writer, IPEndPoint value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
