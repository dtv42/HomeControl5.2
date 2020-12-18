// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TechnicalData.cs" company="DTV-Online">
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
    public class TechnicalData
    {
        #region Public Properties

        public string ItemDescription { get; set; } = string.Empty;
        public string OrderNumber { get; set; } = string.Empty;
        public string MacAddress { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string ProductionCode { get; set; } = string.Empty;
        public string SecurityCode { get; set; } = string.Empty;
        public string StatusFlags { get; set; } = string.Empty;

        #endregion

        #region Public Methods

        public void Refresh(HeliosData data)
        {
            ItemDescription = data.ItemDescription;
            OrderNumber = data.OrderNumber;
            MacAddress = data.MacAddress;
            SerialNumber = data.SerialNumber;
            ProductionCode = data.ProductionCode;
            SecurityCode = data.SecurityCode;
            StatusFlags = data.StatusFlags;
        }

        #endregion
    }
}
