// --------------------------------------------------------------------------------------------------------------------
// <copyright file="BoosterData.cs" company="DTV-Online">
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
    public class BoosterData
    {
        #region Public Properties

        public int PartyOperationDuration { get; set; }
        public FanLevels PartyVentilationLevel { get; set; } = new FanLevels();
        public int PartyOperationRemaining { get; set; }
        public StatusTypes PartyOperationActivate { get; set; } = new StatusTypes();
        public int StandbyOperationDuration { get; set; }
        public FanLevels StandbyVentilationLevel { get; set; } = new FanLevels();
        public int StandbyOperationRemaining { get; set; }
        public StatusTypes StandbyOperationActivate { get; set; } = new StatusTypes();
        public string StatusFlags { get; set; } = string.Empty;

        #endregion

        #region Public Methods

        public void Refresh(HeliosData data)
        {
            PartyOperationDuration = data.PartyOperationDuration;
            PartyVentilationLevel = data.PartyVentilationLevel;
            PartyOperationRemaining = data.PartyOperationRemaining;
            PartyOperationActivate = data.PartyOperationActivate;
            StandbyOperationDuration = data.StandbyOperationDuration;
            StandbyVentilationLevel = data.StandbyVentilationLevel;
            StandbyOperationActivate = data.StandbyOperationActivate;
            StatusFlags = data.StatusFlags;
        }

        #endregion
    }
}
