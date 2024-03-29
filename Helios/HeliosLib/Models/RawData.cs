﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RawData.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>26-4-2020 10:05</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace HeliosLib.Models
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Xml;

    #endregion

    public class RawData
    {
        public string Language { get; set; } = string.Empty;

        public Dictionary<string, string> Parameters { get; set; } = new Dictionary<string, string>() { };

        public RawData(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            var parameter = doc.DocumentElement;

            if (parameter is not null)
            {
                Language = parameter.GetElementsByTagName("LANG")[0]?.InnerText ?? string.Empty;
                var labels = parameter.GetElementsByTagName("ID");
                var values = parameter.GetElementsByTagName("VA");

                for (int i = 0; i < labels.Count; ++i)
                {
                    var label = labels[i];
                    var value = values[i];

                    if ((label is not null) && (value is not null))
                    {
                        Parameters.Add(label.InnerText, value.InnerText);
                    }
                }
            }
        }
    }
}
