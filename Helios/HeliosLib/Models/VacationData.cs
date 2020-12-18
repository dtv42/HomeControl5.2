// --------------------------------------------------------------------------------------------------------------------
// <copyright file="VacationData.cs" company="DTV-Online">
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

    using System;

    #endregion

    public class VacationData
    {
        #region Public Properties

        public VacationOperations VacationOperation { get; set; } = new VacationOperations();
        public FanLevels VacationVentilationLevel { get; set; } = new FanLevels();
        public DateTime VacationStartDate { get; set; } = new DateTime();
        public DateTime VacationEndDate { get; set; } = new DateTime();
        public int VacationInterval { get; set; }
        public int VacationDuration { get; set; }
        public FanLevels SupplyLevel { get; set; } = new FanLevels();
        public FanLevels ExhaustLevel { get; set; } = new FanLevels();
        public string StatusFlags { get; set; } = string.Empty;

        #endregion

        #region Public Methods

        public void Refresh(HeliosData data)
        {
            VacationOperation = data.VacationOperation;
            VacationVentilationLevel = data.VacationVentilationLevel;
            VacationStartDate = data.VacationStartDate;
            VacationEndDate = data.VacationEndDate;
            VacationInterval = data.VacationInterval;
            VacationDuration = data.VacationDuration;
            SupplyLevel = data.SupplyLevel;
            ExhaustLevel = data.ExhaustLevel;
            StatusFlags = data.StatusFlags;
        }

        #endregion
    }
}
