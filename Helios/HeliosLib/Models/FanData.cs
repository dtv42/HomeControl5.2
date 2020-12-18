// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FanData.cs" company="DTV-Online">
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
    public class FanData
    {
        #region Public Properties

        public double ExhaustVentilatorVoltageLevel1 { get; set; }
        public double SupplyVentilatorVoltageLevel1 { get; set; }
        public double ExhaustVentilatorVoltageLevel2 { get; set; }
        public double SupplyVentilatorVoltageLevel2 { get; set; }
        public double ExhaustVentilatorVoltageLevel3 { get; set; }
        public double SupplyVentilatorVoltageLevel3 { get; set; }
        public double ExhaustVentilatorVoltageLevel4 { get; set; }
        public double SupplyVentilatorVoltageLevel4 { get; set; }
        public MinimumFanLevels MinimumVentilationLevel { get; set; } = new MinimumFanLevels();
        public FanLevels SupplyLevel { get; set; } = new FanLevels();
        public FanLevels ExhaustLevel { get; set; } = new FanLevels();
        public FanLevels FanLevelRegion02 { get; set; } = new FanLevels();
        public FanLevels FanLevelRegion24 { get; set; } = new FanLevels();
        public FanLevels FanLevelRegion46 { get; set; } = new FanLevels();
        public FanLevels FanLevelRegion68 { get; set; } = new FanLevels();
        public FanLevels FanLevelRegion80 { get; set; } = new FanLevels();
        public double OffsetExhaust { get; set; }
        public FanLevelConfig FanLevelConfiguration { get; set; } = new FanLevelConfig();
        public string StatusFlags { get; set; } = string.Empty;

        #endregion

        #region Public Methods

        public void Refresh(HeliosData data)
        {
            ExhaustVentilatorVoltageLevel1 = data.ExhaustVentilatorVoltageLevel1;
            SupplyVentilatorVoltageLevel1 = data.SupplyVentilatorVoltageLevel1;
            ExhaustVentilatorVoltageLevel2 = data.ExhaustVentilatorVoltageLevel2;
            SupplyVentilatorVoltageLevel2 = data.SupplyVentilatorVoltageLevel2;
            ExhaustVentilatorVoltageLevel3 = data.ExhaustVentilatorVoltageLevel3;
            SupplyVentilatorVoltageLevel3 = data.SupplyVentilatorVoltageLevel3;
            ExhaustVentilatorVoltageLevel4 = data.ExhaustVentilatorVoltageLevel4;
            SupplyVentilatorVoltageLevel4 = data.SupplyVentilatorVoltageLevel4;
            MinimumVentilationLevel = data.MinimumVentilationLevel;
            SupplyLevel = data.SupplyLevel;
            ExhaustLevel = data.ExhaustLevel;
            FanLevelRegion02 = data.FanLevelRegion02;
            FanLevelRegion24 = data.FanLevelRegion24;
            FanLevelRegion46 = data.FanLevelRegion46;
            FanLevelRegion68 = data.FanLevelRegion68;
            FanLevelRegion80 = data.FanLevelRegion80;
            OffsetExhaust = data.OffsetExhaust;
            FanLevelConfiguration = data.FanLevelConfiguration;
            StatusFlags = data.StatusFlags;
        }

        #endregion
    }
}
