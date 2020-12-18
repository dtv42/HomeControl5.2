// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EM300LRTcpData.cs" company="DTV-Online">
//   Copyright(c) 2019 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace EM300LRLib.Models
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text.Json.Serialization;

    using UtilityLib;

    #endregion Using Directives

    /// <summary>
    /// Class holding all data from the b-Control EM300LR energy manager.
    /// </summary>
    public class EM300LRTcpData
    {
        #region Public Properties

        [JsonPropertyName("serial")]
        public string Serial { get; set; } = string.Empty;

        [JsonPropertyName("1-0:1.4.0*255")] public double ActivePowerPlus { get; set; }
        [JsonPropertyName("1-0:1.8.0*255")] public double ActiveEnergyPlus { get; set; }
        [JsonPropertyName("1-0:2.4.0*255")] public double ActivePowerMinus { get; set; }
        [JsonPropertyName("1-0:2.8.0*255")] public double ActiveEnergyMinus { get; set; }
        [JsonPropertyName("1-0:3.4.0*255")] public double ReactivePowerPlus { get; set; }
        [JsonPropertyName("1-0:3.8.0*255")] public double ReactiveEnergyPlus { get; set; }
        [JsonPropertyName("1-0:4.4.0*255")] public double ReactivePowerMinus { get; set; }
        [JsonPropertyName("1-0:4.8.0*255")] public double ReactiveEnergyMinus { get; set; }
        [JsonPropertyName("1-0:9.4.0*255")] public double ApparentPowerPlus { get; set; }
        [JsonPropertyName("1-0:9.8.0*255")] public double ApparentEnergyPlus { get; set; }
        [JsonPropertyName("1-0:10.4.0*255")] public double ApparentPowerMinus { get; set; }
        [JsonPropertyName("1-0:10.8.0*255")] public double ApparentEnergyMinus { get; set; }
        [JsonPropertyName("1-0:13.4.0*255")] public double PowerFactor { get; set; }
        [JsonPropertyName("1-0:14.4.0*255")] public double SupplyFrequency { get; set; }
        [JsonPropertyName("1-0:21.4.0*255")] public double ActivePowerPlusL1 { get; set; }
        [JsonPropertyName("1-0:21.8.0*255")] public double ActiveEnergyPlusL1 { get; set; }
        [JsonPropertyName("1-0:22.4.0*255")] public double ActivePowerMinusL1 { get; set; }
        [JsonPropertyName("1-0:22.8.0*255")] public double ActiveEnergyMinusL1 { get; set; }
        [JsonPropertyName("1-0:23.4.0*255")] public double ReactivePowerPlusL1 { get; set; }
        [JsonPropertyName("1-0:23.8.0*255")] public double ReactiveEnergyPlusL1 { get; set; }
        [JsonPropertyName("1-0:24.4.0*255")] public double ReactivePowerMinusL1 { get; set; }
        [JsonPropertyName("1-0:24.8.0*255")] public double ReactiveEnergyMinusL1 { get; set; }
        [JsonPropertyName("1-0:29.4.0*255")] public double ApparentPowerPlusL1 { get; set; }
        [JsonPropertyName("1-0:29.8.0*255")] public double ApparentEnergyPlusL1 { get; set; }
        [JsonPropertyName("1-0:30.4.0*255")] public double ApparentPowerMinusL1 { get; set; }
        [JsonPropertyName("1-0:30.8.0*255")] public double ApparentEnergyMinusL1 { get; set; }
        [JsonPropertyName("1-0:31.4.0*255")] public double CurrentL1 { get; set; }
        [JsonPropertyName("1-0:32.4.0*255")] public double VoltageL1 { get; set; }
        [JsonPropertyName("1-0:33.4.0*255")] public double PowerFactorL1 { get; set; }
        [JsonPropertyName("1-0:41.4.0*255")] public double ActivePowerPlusL2 { get; set; }
        [JsonPropertyName("1-0:41.8.0*255")] public double ActiveEnergyPlusL2 { get; set; }
        [JsonPropertyName("1-0:42.4.0*255")] public double ActivePowerMinusL2 { get; set; }
        [JsonPropertyName("1-0:42.8.0*255")] public double ActiveEnergyMinusL2 { get; set; }
        [JsonPropertyName("1-0:43.4.0*255")] public double ReactivePowerPlusL2 { get; set; }
        [JsonPropertyName("1-0:43.8.0*255")] public double ReactiveEnergyPlusL2 { get; set; }
        [JsonPropertyName("1-0:44.4.0*255")] public double ReactivePowerMinusL2 { get; set; }
        [JsonPropertyName("1-0:44.8.0*255")] public double ReactiveEnergyMinusL2 { get; set; }
        [JsonPropertyName("1-0:49.4.0*255")] public double ApparentPowerPlusL2 { get; set; }
        [JsonPropertyName("1-0:49.8.0*255")] public double ApparentEnergyPlusL2 { get; set; }
        [JsonPropertyName("1-0:50.4.0*255")] public double ApparentPowerMinusL2 { get; set; }
        [JsonPropertyName("1-0:50.8.0*255")] public double ApparentEnergyMinusL2 { get; set; }
        [JsonPropertyName("1-0:51.4.0*255")] public double CurrentL2 { get; set; }
        [JsonPropertyName("1-0:52.4.0*255")] public double VoltageL2 { get; set; }
        [JsonPropertyName("1-0:53.4.0*255")] public double PowerFactorL2 { get; set; }
        [JsonPropertyName("1-0:61.4.0*255")] public double ActivePowerPlusL3 { get; set; }
        [JsonPropertyName("1-0:61.8.0*255")] public double ActiveEnergyPlusL3 { get; set; }
        [JsonPropertyName("1-0:62.4.0*255")] public double ActivePowerMinusL3 { get; set; }
        [JsonPropertyName("1-0:62.8.0*255")] public double ActiveEnergyMinusL3 { get; set; }
        [JsonPropertyName("1-0:63.4.0*255")] public double ReactivePowerPlusL3 { get; set; }
        [JsonPropertyName("1-0:63.8.0*255")] public double ReactiveEnergyPlusL3 { get; set; }
        [JsonPropertyName("1-0:64.4.0*255")] public double ReactivePowerMinusL3 { get; set; }
        [JsonPropertyName("1-0:64.8.0*255")] public double ReactiveEnergyMinusL3 { get; set; }
        [JsonPropertyName("1-0:69.4.0*255")] public double ApparentPowerPlusL3 { get; set; }
        [JsonPropertyName("1-0:69.8.0*255")] public double ApparentEnergyPlusL3 { get; set; }
        [JsonPropertyName("1-0:70.4.0*255")] public double ApparentPowerMinusL3 { get; set; }
        [JsonPropertyName("1-0:70.8.0*255")] public double ApparentEnergyMinusL3 { get; set; }
        [JsonPropertyName("1-0:71.4.0*255")] public double CurrentL3 { get; set; }
        [JsonPropertyName("1-0:72.4.0*255")] public double VoltageL3 { get; set; }
        [JsonPropertyName("1-0:73.4.0*255")] public double PowerFactorL3 { get; set; }

        [JsonPropertyName("status")]
        public int StatusCode { get; set; }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Updates the Properties used in EM300LR data.
        /// </summary>
        /// <param name="data">The EM300LR data.</param>
        public void Refresh(EM300LRTcpData data)
        {
            ActivePowerPlus = data.ActivePowerPlus;
            ActiveEnergyPlus = data.ActiveEnergyPlus;
            ActivePowerMinus = data.ActivePowerMinus;
            ActiveEnergyMinus = data.ActiveEnergyMinus;
            ReactivePowerPlus = data.ReactivePowerPlus;
            ReactiveEnergyPlus = data.ReactiveEnergyPlus;
            ReactivePowerMinus = data.ReactivePowerMinus;
            ReactiveEnergyMinus = data.ReactiveEnergyMinus;
            ApparentPowerPlus = data.ApparentPowerPlus;
            ApparentEnergyPlus = data.ApparentEnergyPlus;
            ApparentPowerMinus = data.ApparentPowerMinus;
            ApparentEnergyMinus = data.ApparentEnergyMinus;
            PowerFactor = data.PowerFactor;
            SupplyFrequency = data.SupplyFrequency;
            ActivePowerPlusL1 = data.ActivePowerPlusL1;
            ActiveEnergyPlusL1 = data.ActiveEnergyPlusL1;
            ActivePowerMinusL1 = data.ActivePowerMinusL1;
            ActiveEnergyMinusL1 = data.ActiveEnergyMinusL1;
            ReactivePowerPlusL1 = data.ReactivePowerPlusL1;
            ReactiveEnergyPlusL1 = data.ReactiveEnergyPlusL1;
            ReactivePowerMinusL1 = data.ReactivePowerMinusL1;
            ReactiveEnergyMinusL1 = data.ReactiveEnergyMinusL1;
            ApparentPowerPlusL1 = data.ApparentPowerPlusL1;
            ApparentEnergyPlusL1 = data.ApparentEnergyPlusL1;
            ApparentPowerMinusL1 = data.ApparentPowerMinusL1;
            ApparentEnergyMinusL1 = data.ApparentEnergyMinusL1;
            CurrentL1 = data.CurrentL1;
            VoltageL1 = data.VoltageL1;
            PowerFactorL1 = data.PowerFactorL1;
            ActivePowerPlusL2 = data.ActivePowerPlusL2;
            ActiveEnergyPlusL2 = data.ActiveEnergyPlusL2;
            ActivePowerMinusL2 = data.ActivePowerMinusL2;
            ActiveEnergyMinusL2 = data.ActiveEnergyMinusL2;
            ReactivePowerPlusL2 = data.ReactivePowerPlusL2;
            ReactiveEnergyPlusL2 = data.ReactiveEnergyPlusL2;
            ReactivePowerMinusL2 = data.ReactivePowerMinusL2;
            ReactiveEnergyMinusL2 = data.ReactiveEnergyMinusL2;
            ApparentPowerPlusL2 = data.ApparentPowerPlusL2;
            ApparentEnergyPlusL2 = data.ApparentEnergyPlusL2;
            ApparentPowerMinusL2 = data.ApparentPowerMinusL2;
            ApparentEnergyMinusL2 = data.ApparentEnergyMinusL2;
            CurrentL2 = data.CurrentL2;
            VoltageL2 = data.VoltageL2;
            PowerFactorL2 = data.PowerFactorL2;
            ActivePowerPlusL3 = data.ActivePowerPlusL3;
            ActiveEnergyPlusL3 = data.ActiveEnergyPlusL3;
            ActivePowerMinusL3 = data.ActivePowerMinusL3;
            ActiveEnergyMinusL3 = data.ActiveEnergyMinusL3;
            ReactivePowerPlusL3 = data.ReactivePowerPlusL3;
            ReactiveEnergyPlusL3 = data.ReactiveEnergyPlusL3;
            ReactivePowerMinusL3 = data.ReactivePowerMinusL3;
            ReactiveEnergyMinusL3 = data.ReactiveEnergyMinusL3;
            ApparentPowerPlusL3 = data.ApparentPowerPlusL3;
            ApparentEnergyPlusL3 = data.ApparentEnergyPlusL3;
            ApparentPowerMinusL3 = data.ApparentPowerMinusL3;
            ApparentEnergyMinusL3 = data.ApparentEnergyMinusL3;
            CurrentL3 = data.CurrentL3;
            VoltageL3 = data.VoltageL3;
            PowerFactorL3 = data.PowerFactorL3;
            Serial = data.Serial;
            StatusCode = data.StatusCode;
        }

        #endregion Public Methods

        #region Public Property Helper

        /// <summary>
        /// Gets the property list for the Phase1Data class.
        /// </summary>
        /// <returns>The property list.</returns>
        public static List<string> GetProperties()
            => typeof(EM300LRTcpData).GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
            .Select(p => p.Name).ToList();

        /// <summary>
        /// Returns true if property with the specified name is found in the ETAPU11Data class.
        /// </summary>
        /// <param name="property">The property name.</param>
        /// <returns>Returns true if property is found.</returns>
        public static bool IsProperty(string property)
            => !(typeof(EM300LRTcpData).GetProperty(property) is null);

        #endregion Public Property Helper
    }
}