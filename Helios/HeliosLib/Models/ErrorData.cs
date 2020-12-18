// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ErrorData.cs" company="DTV-Online">
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
    public class ErrorData
    {
        #region Public Properties

        public int NumberOfErrors { get; set; }
        public int NumberOfWarnings { get; set; }
        public int NumberOfInfos { get; set; }
        public string Errors { get; set; } = string.Empty;
        public int Warnings { get; set; }
        public string Infos { get; set; } = string.Empty;
        public string StatusFlags { get; set; } = string.Empty;
        public int DataExchange { get; set; }

        #endregion

        #region Public Methods

        public void Refresh(HeliosData data)
        {
            NumberOfErrors = data.NumberOfErrors;
            NumberOfWarnings = data.NumberOfWarnings;
            NumberOfInfos = data.NumberOfInfos;
            Errors = data.Errors;
            Warnings = data.Warnings;
            Infos = data.Infos;
            StatusFlags = data.StatusFlags;
            DataExchange = data.DataExchange;
        }

        #endregion
    }
}
