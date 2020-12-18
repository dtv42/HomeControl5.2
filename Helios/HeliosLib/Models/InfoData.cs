// --------------------------------------------------------------------------------------------------------------------
// <copyright file="InfoData.cs" company="DTV-Online">
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

    public class InfoData
    {
        #region Public Properties

        public string ItemDescription { get; set; } = string.Empty;
        public string OrderNumber { get; set; } = string.Empty;
        public string MacAddress { get; set; } = string.Empty;
        public int PartyOperationRemaining { get; set; }
        public StatusTypes PartyOperationActivate { get; set; } = new StatusTypes();
        public int StandbyOperationRemaining { get; set; }
        public StatusTypes StandbyOperationActivate { get; set; } = new StatusTypes();
        public OperationModes OperationMode { get; set; } = new OperationModes();
        public FanLevels VentilationLevel { get; set; } = new FanLevels();
        public int VentilationPercentage { get; set; }
        public VacationOperations VacationOperation { get; set; } = new VacationOperations();
        public DateTime VacationEndDate { get; set; } = new DateTime();
        public ContactTypes ExternalContact { get; set; } = new ContactTypes();
        public string StatusFlags { get; set; } = string.Empty;

        #endregion

        #region Public Methods

        public void Refresh(HeliosData data)
        {
            ItemDescription = data.ItemDescription;
            OrderNumber = data.OrderNumber;
            MacAddress = data.MacAddress;
            PartyOperationRemaining = data.PartyOperationRemaining;
            PartyOperationActivate = data.PartyOperationActivate;
            StandbyOperationRemaining = data.StandbyOperationRemaining;
            StandbyOperationActivate = data.StandbyOperationActivate;
            OperationMode = data.OperationMode;
            VentilationLevel = data.VentilationLevel;
            VentilationPercentage = data.VentilationPercentage;
            VacationOperation = data.VacationOperation;
            VacationEndDate = data.VacationEndDate;
            ExternalContact = data.ExternalContact;
            StatusFlags = data.StatusFlags;
        }

        #endregion
    }
}
