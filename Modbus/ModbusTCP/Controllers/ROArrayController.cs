﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ROArrayController.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>13-5-2020 13:53</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace ModbusTCP.Controllers
{
    #region Using Directives

    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    using Swashbuckle.AspNetCore.Annotations;

    using ModbusLib;
    using ModbusTCP.Models;

    #endregion

    /// <summary>
    /// The Modbus Gateway MVC Controller for reading various data values.
    /// </summary>
    /// <para>
    ///     ReadOnlyBoolArray   Reads an array of boolean values (multiple discrete inputs)
    ///     ReadOnlyBytes       Reads 8 bit values (multiple input register)
    ///     ReadOnlyShortArray  Reads an array of 16 bit integers (multiple input register)
    ///     ReadOnlyUShortArray Reads an array of unsigned 16 bit integer (multiple input register)
    ///     ReadOnlyInt32Array  Reads an array of 32 bit integers (multiple input registers)
    ///     ReadOnlyUInt32Array Reads an array of unsigned 32 bit integers (multiple input registers)
    ///     ReadOnlyFloatArray  Reads an array of 32 bit IEEE floating point numbers (multiple input registers)
    ///     ReadOnlyDoubleArray Reads an array of 64 bit IEEE floating point numbers (multiple input registers)
    ///     ReadOnlyLongArray   Reads an array of 64 bit integers (multiple input registers)
    ///     ReadOnlyULongArray  Reads an array of unsigned 64 bit integers (multiple input registers)
    /// </para>
    [Route("[controller]")]
    [ApiController]
    public class ROArrayController : ModbusController
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ROArrayController"/> class.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="settings"></param>
        /// <param name="config"></param>
        /// <param name="environment"></param>
        /// <param name="lifetime"></param>
        /// <param name="logger"></param>
        public ROArrayController(ITcpModbusClient client,
                                 AppSettings settings,
                                 IConfiguration config,
                                 IHostEnvironment environment,
                                 IHostApplicationLifetime lifetime,
                                 ILogger<ROArrayController> logger)
            : base(client, settings, config, environment, lifetime, logger)
        { }

        #endregion

        /// <summary>
        /// Reads an array of boolean values from a Modbus slave.
        /// </summary>
        /// <remarks>Note that this is equivalent to read multiple discrete inputs.</remarks>
        /// <param name="offset">The Modbus address (offset) of the first data value.</param>
        /// <param name="number">The number of Modbus data values.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the array of boolean data values.</response>
        /// <response code="400">If the request data are invalid.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occured in the slave.</response>
        [HttpGet("Bool/{offset}")]
        [SwaggerOperation(Tags = new[] { "Boolean Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseArrayData<bool>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadOnlyBoolArrayAsync(ushort offset = 0, ushort number = 1, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = number
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusReadRequest(request, ReadRequestFunctions.ReadOnlyBoolArrayAsync);
        }

        /// <summary>
        /// Reads 8-bit values from a Modbus slave.
        /// </summary>
        /// <remarks>Note that this reads multiple input registers (two bytes are stored in a single Modbus register).</remarks>
        /// <param name="offset">The Modbus address (offset) of the first data value.</param>
        /// <param name="number">The number of Modbus data values.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the array of 8-bit data values.</response>
        /// <response code="400">If the request data are invalid.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occured in the slave.</response>
        [HttpGet("Bytes/{offset}")]
        [SwaggerOperation(Tags = new[] { "8-Bit Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseArrayData<byte>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadOnlyBytesAsync(ushort offset = 0, ushort number = 1, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = number
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusReadRequest(request, ReadRequestFunctions.ReadOnlyBytesAsync);
        }

        /// <summary>
        /// Reads an array of 16 bit integers from a Modbus slave.
        /// </summary>
        /// <remarks>Note that this reads multiple input registers.</remarks>
        /// <param name="offset">The Modbus address (offset) of the first data value.</param>
        /// <param name="number">The number of the Modbus data values.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the array of short data values.</response>
        /// <response code="400">If the request data are invalid.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occured in the slave.</response>
        [HttpGet("Short/{offset}")]
        [SwaggerOperation(Tags = new[] { "Short Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseArrayData<short>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadOnlyShortArrayAsync(ushort offset = 0, ushort number = 1, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = number
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusReadRequest(request, ReadRequestFunctions.ReadOnlyShortArrayAsync);
        }

        /// <summary>
        /// Reads an array of unsigned 16 bit integers from a Modbus slave.
        /// </summary>
        /// <remarks>Note that this reads multiple input registers.</remarks>
        /// <param name="offset">The Modbus address (offset) of the first data value.</param>
        /// <param name="number">The number of the Modbus data values.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the array of ushort data values.</response>
        /// <response code="400">If the request data are invalid.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occured in the slave.</response>
        [HttpGet("UShort/{offset}")]
        [SwaggerOperation(Tags = new[] { "UShort Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseArrayData<ushort>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadOnlyUShortArrayAsync(ushort offset = 0, ushort number = 1, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = number
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusReadRequest(request, ReadRequestFunctions.ReadOnlyUShortArrayAsync);
        }

        /// <summary>
        /// Reads an array of 32 bit integers from a Modbus slave.
        /// </summary>
        /// <remarks>Note that this reads multiple input registers.</remarks>
        /// <param name="offset">The Modbus address (offset) of the first data value.</param>
        /// <param name="number">The number of the Modbus data values.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the array of int data values.</response>
        /// <response code="400">If the request data are invalid.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occured in the slave.</response>
        [HttpGet("Int32/{offset}")]
        [SwaggerOperation(Tags = new[] { "Int32 Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseArrayData<int>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadOnlyInt32ArrayAsync(ushort offset = 0, ushort number = 1, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = number
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusReadRequest(request, ReadRequestFunctions.ReadOnlyInt32ArrayAsync);
        }

        /// <summary>
        /// Reads an array of unsigned 32 bit integers from a Modbus slave.
        /// </summary>
        /// <remarks>Note that this reads multiple input registers.</remarks>
        /// <param name="offset">The Modbus address (offset) of the first data value.</param>
        /// <param name="number">The number of the Modbus data values.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the array of uint data values.</response>
        /// <response code="400">If the request data are invalid.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occured in the slave.</response>
        [HttpGet("UInt32/{offset}")]
        [SwaggerOperation(Tags = new[] { "UInt32 Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseArrayData<uint>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadOnlyUInt32ArrayAsync(ushort offset = 0, ushort number = 1, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = number
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusReadRequest(request, ReadRequestFunctions.ReadOnlyUInt32ArrayAsync);
        }

        /// <summary>
        /// Reads an array of 32 bit IEEE floating point numbers from a Modbus slave.
        /// </summary>
        /// <remarks>Note that this reads multiple input registers.</remarks>
        /// <param name="offset">The Modbus address (offset) of the first data value.</param>
        /// <param name="number">The number of the Modbus data values.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the array of float data values.</response>
        /// <response code="400">If the request data are invalid.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occured in the slave.</response>
        [HttpGet("Float/{offset}")]
        [SwaggerOperation(Tags = new[] { "Float Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseArrayData<float>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadOnlyFloatArrayAsync(ushort offset = 0, ushort number = 1, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = number
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusReadRequest(request, ReadRequestFunctions.ReadOnlyFloatArrayAsync);
        }

        /// <summary>
        /// Reads an array of 64 bit IEEE floating point numbers from a Modbus slave.
        /// </summary>
        /// <remarks>Note that this reads multiple input registers.</remarks>
        /// <param name="offset">The Modbus address (offset) of the first data value.</param>
        /// <param name="number">The number of the Modbus data values.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the array of double data values.</response>
        /// <response code="400">If the request data are invalid.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occured in the slave.</response>
        [HttpGet("Double/{offset}")]
        [SwaggerOperation(Tags = new[] { "Double Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseArrayData<double>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadOnlyDoubleArrayAsync(ushort offset = 0, ushort number = 1, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = number
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusReadRequest(request, ReadRequestFunctions.ReadOnlyDoubleArrayAsync);
        }

        /// <summary>
        /// Reads an array of 64 bit integers from a Modbus slave.
        /// </summary>
        /// <remarks>Note that this reads multiple input registers.</remarks>
        /// <param name="offset">The Modbus address (offset) of the first data value.</param>
        /// <param name="number">The number of the Modbus data values.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the array of long data values.</response>
        /// <response code="400">If the request data are invalid.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occured in the slave.</response>
        [HttpGet("Long/{offset}")]
        [SwaggerOperation(Tags = new[] { "Long Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseArrayData<long>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadOnlyLongArrayAsync(ushort offset = 0, ushort number = 1, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = number
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusReadRequest(request, ReadRequestFunctions.ReadOnlyLongArrayAsync);
        }

        /// <summary>
        /// Reads an array of unsigned 64 bit integers from a Modbus slave.
        /// </summary>
        /// <remarks>Note that this reads multiple input registers.</remarks>
        /// <param name="offset">The Modbus address (offset) of the first data value.</param>
        /// <param name="number">The number of the Modbus data values.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the array of ulong data values.</response>
        /// <response code="400">If the request data are invalid.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occured in the slave.</response>
        [HttpGet("ULong/{offset}")]
        [SwaggerOperation(Tags = new[] { "ULong Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseArrayData<ulong>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadOnlyULongArrayAsync(ushort offset = 0, ushort number = 1, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = number
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusReadRequest(request, ReadRequestFunctions.ReadOnlyULongArrayAsync);
        }
    }
}
