// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ETAPU11Client.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>17-12-2020 12:52</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace ETAPU11Lib
{
    #region Using Directives

    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using UtilityLib;

    using ModbusLib;
    using ModbusLib.Models;

    using ETAPU11Lib.Models;

    #endregion

    public class ETAPU11Client : BaseClass
    {
        #region Private Data Members

        /// <summary>
        /// The Modbus client used internally.
        /// </summary>
        private readonly TcpModbusClient _client;

        #endregion Private Data Members

        #region Public Properties

        /// <summary>
        /// Get the connected state from the Modbus TCP client.
        /// </summary>
        public bool Connected { get => _client.Connected; }

        #endregion Public Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ETAPU11Client"/> class.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="logger"></param>
        public ETAPU11Client(TcpModbusClient client, ILogger<ETAPU11Client> logger)
            : base(logger)
        {
            _logger?.LogDebug($"ETAPU11Client()");

            _client = client;
        }

        #endregion Constructors

        #region Public Methods

        // Redirect all methods to embedded client.

        public bool Connect() => _client.Connect();
        public void Disconnect() => _client.Disconnect();

        public async Task<uint[]> ReadUInt32ArrayAsync(ushort startAddress, ushort length)
            => await _client.ReadUInt32ArrayAsync(startAddress, length);

        public async Task<uint> ReadUInt32Async(ushort startAddress)
            => await _client.ReadUInt32Async(startAddress);

        public async Task WriteUInt32Async(ushort startAddress, uint value)
            => await _client.WriteUInt32Async(startAddress, value);

        #endregion
    }
}
