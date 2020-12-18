// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DeviceData.cs" company="DTV-Online">
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
    public class DeviceData
    {
        #region Public Properties

        public StatusTypes KwlBeEnabled { get; set; } = new StatusTypes();
        public StatusTypes KwlBecEnabled { get; set; } = new StatusTypes();
        public ConfigOptions DeviceConfiguration { get; set; } = new ConfigOptions();
        public StatusTypes PreheaterStatus { get; set; } = new StatusTypes();
        public HeatExchangerTypes HeatExchangerType { get; set; } = new HeatExchangerTypes();
        public WeeklyProfiles WeeklyProfile { get; set; } = new WeeklyProfiles();
        public PreheaterTypes PreheaterType { get; set; } = new PreheaterTypes();
        public FunctionTypes KwlFunctionType { get; set; } = new FunctionTypes();
        public int HeaterAfterRunTime { get; set; }
        public ContactTypes ExternalContact { get; set; } = new ContactTypes();
        public FaultTypes FaultTypeOutput { get; set; } = new FaultTypes();
        public StatusTypes FilterChange { get; set; } = new StatusTypes();
        public int FilterChangeInterval { get; set; }
        public int FilterChangeRemaining { get; set; }
        public int BypassRoomTemperature { get; set; }
        public int BypassOutdoorTemperature { get; set; }
        public int BypassOutdoorTemperature2 { get; set; }
        public bool StartReset { get; set; }
        public bool FactoryReset { get; set; }
        public bool ModbusActivated { get; set; }
        public string StatusFlags { get; set; } = string.Empty;
        public int V02115 { get; set; }
        public int V02120 { get; set; }
        public int V02121 { get; set; }
        public int V02128 { get; set; }
        public int V02129 { get; set; }

        #endregion

        #region Public Methods

        public void Refresh(HeliosData data)
        {
            KwlBeEnabled = data.KwlBeEnabled;
            KwlBecEnabled = data.KwlBecEnabled;
            DeviceConfiguration = data.DeviceConfiguration;
            PreheaterStatus = data.PreheaterStatus;
            HeatExchangerType = data.HeatExchangerType;
            WeeklyProfile = data.WeeklyProfile;
            PreheaterType = data.PreheaterType;
            KwlFunctionType = data.KwlFunctionType;
            HeaterAfterRunTime = data.HeaterAfterRunTime;
            ExternalContact = data.ExternalContact;
            FaultTypeOutput = data.FaultTypeOutput;
            FilterChange = data.FilterChange;
            FilterChangeInterval = data.FilterChangeInterval;
            FilterChangeRemaining = data.FilterChangeRemaining;
            BypassRoomTemperature = data.BypassRoomTemperature;
            BypassOutdoorTemperature = data.BypassOutdoorTemperature;
            BypassOutdoorTemperature2 = data.BypassOutdoorTemperature2;
            StartReset = data.StartReset;
            FactoryReset = data.FactoryReset;
            ModbusActivated = data.ModbusActivated;
            StatusFlags = data.StatusFlags;
            V02115 = data.V02115;
            V02120 = data.V02120;
            V02121 = data.V02121;
            V02128 = data.V02128;
            V02129 = data.V02129;
        }

        #endregion
    }
}
