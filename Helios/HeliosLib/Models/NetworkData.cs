// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NetworkData.cs" company="DTV-Online">
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
    public class NetworkData
    {
        #region Public Properties

        public bool UseDHCP { get; set; }
        public string IPAddress { get; set; } = string.Empty;
        public string SubnetMask { get; set; } = string.Empty;
        public string Gateway { get; set; } = string.Empty;
        public string StandardDNS { get; set; } = string.Empty;
        public string FallbackDNS { get; set; } = string.Empty;
        public string HostName { get; set; } = string.Empty;
        public string StatusFlags { get; set; } = string.Empty;

        #endregion

        #region Public Methods

        public void Refresh(HeliosData data)
        {
            UseDHCP = data.UseDHCP;
            IPAddress = data.IPAddress;
            SubnetMask = data.SubnetMask;
            Gateway = data.Gateway;
            StandardDNS = data.StandardDNS;
            FallbackDNS = data.FallbackDNS;
            HostName = data.HostName;
            StatusFlags = data.StatusFlags;
        }

        #endregion
    }
}
