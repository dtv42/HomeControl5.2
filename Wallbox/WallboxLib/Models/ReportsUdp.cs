// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ReportsUdp.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>26-4-2020 20:19</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace WallboxLib.Models
{
    #region Using Directives

    using System.Text.Json.Serialization;

    #endregion

    /// <summary>
    ///  {
    ///    "ID": "100",
    ///    "Session ID": 43,
    ///    "Curr HW": 20000,
    ///    "E start": 5062978,
    ///    "E pres": 239738,
    ///    "started[s]": 1547187752,
    ///    "ended[s]": 1547205057,
    ///    "started": "2019-01-11 06:22:32.000",
    ///    "ended": "2019-01-11 11:10:57.000",
    ///    "reason": 1,
    ///    "timeQ": 3,
    ///    "RFID class": "01010200000000000000",
    ///    "Serial": "18711747",
    ///    "Sec": 274727
    ///  }
    /// </summary>
    public class ReportsUdp
    {
        #region Public Properties

        /// <summary>
        /// ID of the report (100 - 130)
        /// </summary>
        public string ID { get; set; } = string.Empty;

        /// <summary>
        /// uint32 - ID of the current charging session. This value will be assigned automatically and is not resettable.
        ///          This value is incremented session by session.
        /// </summary>
        [JsonPropertyName("Session ID")]
        public uint SessionID { get; set; }

        /// <summary>
        /// uint16 - Maximum current value in mA that can be supported by the hardware of the device.
        ///          This value represents the minimum of the DIP switch settings, cable coding and
        ///          temperature monitoring function.
        ///          Possible values: 0; 6000 - 32000
        /// </summary>
        [JsonPropertyName("Curr HW")]
        public ushort CurrHW { get; set; }

        /// <summary>
        /// uint32 - Total energy consumption (persistent, device related) without the current charging session
        ///          in 0.1 Wh at the beginning of the charging session.
        ///          Possible values: 0 - 999999999
        /// </summary>
        [JsonPropertyName("E start")]
        public uint Estart { get; set; }

        /// <summary>
        /// uint32 - Energy transferred in the current charging session in 0.1 Wh.This value is reset at the
        ///          beginning of a new charging session.
        ///          Possible values: 0 - 999999999
        /// </summary>
        [JsonPropertyName("E pres")]
        public uint Epres { get; set; }

        /// <summary>
        /// uint32 - State of the system clock in seconds from the last startup of the device at the start of
        ///          the charging session.
        /// </summary>
        [JsonPropertyName("started[s]")]
        public uint StartedSec { get; set; }

        /// <summary>
        /// uint32 - State of the system clock in seconds from the last startup of the device at the end of
        ///          the charging session.
        /// </summary>
        [JsonPropertyName("ended[s]")]
        public uint EndedSec { get; set; }

        /// <summary>
        /// String - This date stamp will represent the current time in UTC at the start of the charging session.
        ///          YYYY-MM-DD hh:mm:ss.000 (23 chars)
        /// </summary>
        [JsonPropertyName("started")]
        public string Started { get; set; } = string.Empty;

        /// <summary>
        /// String - This date stamp will represent the current time in UTC at the end of the charging session.
        ///          YYYY-MM-DD hh:mm:ss.000 (23 chars)
        /// </summary>
        [JsonPropertyName("ended")]
        public string Ended { get; set; } = string.Empty;

        /// <summary>
        /// Enum indicating the reason for ending the charging session.
        /// </summary>
        [JsonPropertyName("reason")]
        public Reasons Reason { get; set; }

        /// <summary>
        /// 0 - Not synced time.
        /// X - Strong synced time.
        /// 2 - Weak synced time.
        /// </summary>
        [JsonPropertyName("timeQ")]
        public ushort TimeQ { get; set; }

        /// <summary>
        /// string - RFID Token ID if session started with RFID. First character is the lowest nibble.
        ///          00000000000000000000 indicates no RFIS card was used. (20 chars)
        /// </summary>
        [JsonPropertyName("RFID class")]
        public string RFIDclass { get; set; } = "00000000000000000000";

        /// <summary>
        /// String (8 chars) - Serial number of the device
        /// </summary>
        public string Serial { get; set; } = string.Empty;

        /// <summary>
        /// uint32 - Current state of the system clock in seconds
        ///          from the last startup of the device.
        /// </summary>
        public uint Sec { get; set; }

        #endregion
    }
}
