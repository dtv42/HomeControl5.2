// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SensorData.cs" company="DTV-Online">
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

    public class SensorData
    {
        #region Public Properties

        public KwlFTFConfig KwlFTFConfig0 { get; set; } = new KwlFTFConfig();
        public KwlFTFConfig KwlFTFConfig1 { get; set; } = new KwlFTFConfig();
        public KwlFTFConfig KwlFTFConfig2 { get; set; } = new KwlFTFConfig();
        public KwlFTFConfig KwlFTFConfig3 { get; set; } = new KwlFTFConfig();
        public KwlFTFConfig KwlFTFConfig4 { get; set; } = new KwlFTFConfig();
        public KwlFTFConfig KwlFTFConfig5 { get; set; } = new KwlFTFConfig();
        public KwlFTFConfig KwlFTFConfig6 { get; set; } = new KwlFTFConfig();
        public KwlFTFConfig KwlFTFConfig7 { get; set; } = new KwlFTFConfig();
        public SensorStatus HumidityControlStatus { get; set; } = new SensorStatus();
        public int HumidityControlTarget { get; set; }
        public int HumidityControlStep { get; set; }
        public int HumidityControlStop { get; set; }
        public SensorStatus CO2ControlStatus { get; set; } = new SensorStatus();
        public int CO2ControlTarget { get; set; }
        public int CO2ControlStep { get; set; }
        public SensorStatus VOCControlStatus { get; set; } = new SensorStatus();
        public int VOCControlTarget { get; set; }
        public int VOCControlStep { get; set; }
        public string SensorName1 { get; set; } = string.Empty;
        public string SensorName2 { get; set; } = string.Empty;
        public string SensorName3 { get; set; } = string.Empty;
        public string SensorName4 { get; set; } = string.Empty;
        public string SensorName5 { get; set; } = string.Empty;
        public string SensorName6 { get; set; } = string.Empty;
        public string SensorName7 { get; set; } = string.Empty;
        public string SensorName8 { get; set; } = string.Empty;
        public string CO2SensorName1 { get; set; } = string.Empty;
        public string CO2SensorName2 { get; set; } = string.Empty;
        public string CO2SensorName3 { get; set; } = string.Empty;
        public string CO2SensorName4 { get; set; } = string.Empty;
        public string CO2SensorName5 { get; set; } = string.Empty;
        public string CO2SensorName6 { get; set; } = string.Empty;
        public string CO2SensorName7 { get; set; } = string.Empty;
        public string CO2SensorName8 { get; set; } = string.Empty;
        public string VOCSensorName1 { get; set; } = string.Empty;
        public string VOCSensorName2 { get; set; } = string.Empty;
        public string VOCSensorName3 { get; set; } = string.Empty;
        public string VOCSensorName4 { get; set; } = string.Empty;
        public string VOCSensorName5 { get; set; } = string.Empty;
        public string VOCSensorName6 { get; set; } = string.Empty;
        public string VOCSensorName7 { get; set; } = string.Empty;
        public string VOCSensorName8 { get; set; } = string.Empty;
        public string StatusFlags { get; set; } = string.Empty;
        public string V02137 { get; set; } = string.Empty;
        public int V02142 { get; set; }
        public int V02143 { get; set; }
        public int V02144 { get; set; }
        public int V02145 { get; set; }
        public int V02146 { get; set; }
        public int V02147 { get; set; }
        public int V02148 { get; set; }
        public TimeSpan V02149 { get; set; } = new TimeSpan();
        public TimeSpan V02150 { get; set; } = new TimeSpan();
        public int V02151 { get; set; }
        public int V02152 { get; set; }

        #endregion

        #region Public Methods

        public void Refresh(HeliosData data)
        {
            KwlFTFConfig0 = data.KwlFTFConfig0;
            KwlFTFConfig1 = data.KwlFTFConfig1;
            KwlFTFConfig2 = data.KwlFTFConfig2;
            KwlFTFConfig3 = data.KwlFTFConfig3;
            KwlFTFConfig4 = data.KwlFTFConfig4;
            KwlFTFConfig5 = data.KwlFTFConfig5;
            KwlFTFConfig6 = data.KwlFTFConfig6;
            KwlFTFConfig7 = data.KwlFTFConfig7;
            HumidityControlStatus = data.HumidityControlStatus;
            HumidityControlTarget = data.HumidityControlTarget;
            HumidityControlStep = data.HumidityControlStep;
            HumidityControlStop = data.HumidityControlStop;
            CO2ControlStatus = data.CO2ControlStatus;
            CO2ControlTarget = data.CO2ControlTarget;
            CO2ControlStep = data.CO2ControlStep;
            VOCControlStatus = data.VOCControlStatus;
            VOCControlTarget = data.VOCControlTarget;
            VOCControlStep = data.VOCControlStep;
            SensorName1 = data.SensorName1;
            SensorName2 = data.SensorName2;
            SensorName3 = data.SensorName3;
            SensorName4 = data.SensorName4;
            SensorName5 = data.SensorName5;
            SensorName6 = data.SensorName6;
            SensorName7 = data.SensorName7;
            SensorName8 = data.SensorName8;
            CO2SensorName1 = data.CO2SensorName1;
            CO2SensorName2 = data.CO2SensorName2;
            CO2SensorName3 = data.CO2SensorName3;
            CO2SensorName4 = data.CO2SensorName4;
            CO2SensorName5 = data.CO2SensorName5;
            CO2SensorName6 = data.CO2SensorName6;
            CO2SensorName7 = data.CO2SensorName7;
            CO2SensorName8 = data.CO2SensorName8;
            VOCSensorName1 = data.VOCSensorName1;
            VOCSensorName2 = data.VOCSensorName2;
            VOCSensorName3 = data.VOCSensorName3;
            VOCSensorName4 = data.VOCSensorName4;
            VOCSensorName5 = data.VOCSensorName5;
            VOCSensorName6 = data.VOCSensorName6;
            VOCSensorName7 = data.VOCSensorName7;
            VOCSensorName8 = data.VOCSensorName8;
            StatusFlags = data.StatusFlags;
            V02137 = data.V02137;
            V02142 = data.V02142;
            V02143 = data.V02143;
            V02144 = data.V02144;
            V02145 = data.V02145;
            V02146 = data.V02146;
            V02147 = data.V02147;
            V02148 = data.V02148;
            V02149 = data.V02149;
            V02150 = data.V02150;
            V02151 = data.V02151;
            V02152 = data.V02152;
        }

        #endregion
    }
}
