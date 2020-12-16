﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RWArrayController.cs" company="DTV-Online">
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

    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
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
    /// The Modbus Gateway MVC Controller for reading and writing various data values.
    /// </summary>
    /// <para>
    ///     ReadBoolArray       Reads an array of boolean values (multiple coils)
    ///     ReadBytes           Reads 8 bit values (multiple holding register)
    ///     ReadShortArray      Reads an array of 16 bit integers (multiple holding register)
    ///     ReadUShortArray     Reads an array of unsigned 16 bit integer (multiple holding register)
    ///     ReadInt32Array      Reads an array of 32 bit integers (multiple holding registers)
    ///     ReadUInt32Array     Reads an array of unsigned 32 bit integers (multiple holding registers)
    ///     ReadFloatArray      Reads an array of 32 bit IEEE floating point numbers (multiple holding registers)
    ///     ReadDoubleArray     Reads an array of 64 bit IEEE floating point numbers (multiple holding registers)
    ///     ReadLongArray       Reads an array of 64 bit integers (multiple holding registers)
    ///     ReadULongArray      Reads an array of unsigned 64 bit integers (multiple holding registers)
    /// </para>
    /// <para>
    ///     WriteBoolArray      Writes an array of boolean values (multiple coils)
    ///     WriteBytes          Writes 8 bit values (multiple holding register)
    ///     WriteShortArray     Writes an array of 16 bit integers (multiple holding register)
    ///     WriteUShortArray    Writes an array of unsigned 16 bit integer (multiple holding register)
    ///     WriteInt32Array     Writes an array of 32 bit integers (multiple holding registers)
    ///     WriteUInt32Array    Writes an array of unsigned 32 bit integers (multiple holding registers)
    ///     WriteFloatArray     Writes an array of 32 bit IEEE floating point numbers (multiple holding registers)
    ///     WriteDoubleArray    Writes an array of 64 bit IEEE floating point numbers (multiple holding registers)
    ///     WriteLongArray      Writes an array of 64 bit integers (multiple holding registers)
    ///     WriteULongArray     Writes an array of unsigned 64 bit integers (multiple holding registers)
    /// </para>
    [Route("[controller]")]
    [ApiController]
    public class RWArrayController : ModbusController
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RWArrayController"/> class.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="settings"></param>
        /// <param name="config"></param>
        /// <param name="environment"></param>
        /// <param name="lifetime"></param>
        /// <param name="logger"></param>
        public RWArrayController(ITcpModbusClient client,
                                 AppSettings settings,
                                 IConfiguration config,
                                 IHostEnvironment environment,
                                 IHostApplicationLifetime lifetime,
                                 ILogger<RWArrayController> logger)
            : base(client, settings, config, environment, lifetime, logger)
        { }

        #endregion

        /// <summary>
        /// Reads an array of boolean values from a Modbus slave.
        /// </summary>
        /// <remarks>Note that this is equivalent to read multiple coils.</remarks>
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
        public async Task<IActionResult> ReadBoolArrayAsync(ushort offset = 0, ushort number = 1, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = number
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusReadRequest(request, ReadRequestFunctions.ReadBoolArrayAsync);
        }

        /// <summary>
        /// Reads 8-bit values from a Modbus slave.
        /// </summary>
        /// <remarks>Note that this reads multiple holding registers (two bytes are stored in a single Modbus register).</remarks>
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
        public async Task<IActionResult> ReadBytesAsync(ushort offset = 0, ushort number = 1, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = number
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusReadRequest(request, ReadRequestFunctions.ReadBytesAsync);
        }

        /// <summary>
        /// Reads an array of 16 bit integers from a Modbus slave.
        /// </summary>
        /// <remarks>Note that this reads multiple holding registers.</remarks>
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
        public async Task<IActionResult> ReadShortArrayAsync(ushort offset = 0, ushort number = 1, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = number
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusReadRequest(request, ReadRequestFunctions.ReadShortArrayAsync);
        }

        /// <summary>
        /// Reads an array of unsigned 16 bit integer from a Modbus slave.
        /// </summary>
        /// <remarks>Note that this reads multiple holding registers.</remarks>
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
        public async Task<IActionResult> ReadUShortArrayAsync(ushort offset = 0, ushort number = 1, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = number
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusReadRequest(request, ReadRequestFunctions.ReadUShortArrayAsync);
        }

        /// <summary>
        /// Reads an array of 32 bit integers from a Modbus slave.
        /// </summary>
        /// <remarks>Note that this reads multiple holding registers.</remarks>
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
        public async Task<IActionResult> ReadInt32ArrayAsync(ushort offset = 0, ushort number = 1, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = number
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusReadRequest(request, ReadRequestFunctions.ReadInt32ArrayAsync);
        }

        /// <summary>
        /// Reads an array of unsigned 32 bit integers from a Modbus slave.
        /// </summary>
        /// <remarks>Note that this reads multiple holding registers.</remarks>
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
        public async Task<IActionResult> ReadUInt32ArrayAsync(ushort offset = 0, ushort number = 1, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = number
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusReadRequest(request, ReadRequestFunctions.ReadUInt32ArrayAsync);
        }

        /// <summary>
        /// Reads an array of 32 bit IEEE floating point numbers from a Modbus slave.
        /// </summary>
        /// <remarks>Note that this reads multiple holding registers.</remarks>
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
        public async Task<IActionResult> ReadFloatArrayAsync(ushort offset = 0, ushort number = 1, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = number
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusReadRequest(request, ReadRequestFunctions.ReadFloatArrayAsync);
        }

        /// <summary>
        /// Reads an array of 64 bit IEEE floating point numbers from a Modbus slave.
        /// </summary>
        /// <remarks>Note that this reads multiple holding registers.</remarks>
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
        public async Task<IActionResult> ReadDoubleArrayAsync(ushort offset = 0, ushort number = 1, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = number
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusReadRequest(request, ReadRequestFunctions.ReadDoubleArrayAsync);
        }

        /// <summary>
        /// Reads an array of 64 bit integers from a Modbus slave.
        /// </summary>
        /// <remarks>Note that this reads multiple holding registers.</remarks>
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
        public async Task<IActionResult> ReadLongArrayAsync(ushort offset = 0, ushort number = 1, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = number
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusReadRequest(request, ReadRequestFunctions.ReadLongArrayAsync);
        }

        /// <summary>
        /// Reads an array of unsigned 64 bit integers from a Modbus slave.
        /// </summary>
        /// <remarks>Note that this reads multiple holding registers.</remarks>
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
        public async Task<IActionResult> ReadULongArrayAsync(ushort offset = 0, ushort number = 1, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = number
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusReadRequest(request, ReadRequestFunctions.ReadULongArrayAsync);
        }

        /// <summary>
        /// Writes an array of boolean values to a Modbus slave.
        /// </summary>
        /// <remarks>Note that this is equivalent to write multiple coils.</remarks>
        /// <param name="data">The Modbus data values.</param>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data if OK.</response>
        /// <response code="400">If the request data are invalid.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occured in the slave.</response>
        [HttpPut("Bool/{offset}")]
        [SwaggerOperation(Tags = new[] { "Boolean Data Values" })]
        [ProducesResponseType(typeof(ModbusRequestData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> WriteBoolArrayAsync([FromBody, Required] List<bool> data, ushort offset = 0, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = (ushort)data.Count
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusWriteArrayRequest(request, data.ToArray(), WriteArrayRequestFunctions.WriteBoolArrayAsync);
        }

        /// <summary>
        /// Writes 8-bit values to a Modbus slave.
        /// </summary>
        /// <remarks>Note that this writes multiple holding registers (two bytes are stored in a single Modbus register).</remarks>
        /// <param name="data">The Modbus data values.</param>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data if OK.</response>
        /// <response code="400">If the request data are invalid.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occured in the slave.</response>
        [HttpPut("Bytes/{offset}")]
        [SwaggerOperation(Tags = new[] { "8-Bit Data Values" })]
        [ProducesResponseType(typeof(ModbusRequestData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> WriteBytesAsync([FromBody, Required] List<byte> data, ushort offset = 0, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = (ushort)data.Count
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusWriteArrayRequest(request, data.ToArray(), WriteArrayRequestFunctions.WriteBytesAsync);
        }

        /// <summary>
        /// Writes an array of 16 bit integers to a Modbus slave.
        /// </summary>
        /// <remarks>Note that this writes multiple holding registers.</remarks>
        /// <param name="data">The Modbus data values.</param>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data if OK.</response>
        /// <response code="400">If the request data are invalid.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occured in the slave.</response>
        [HttpPut("Short/{offset}")]
        [SwaggerOperation(Tags = new[] { "Short Data Values" })]
        [ProducesResponseType(typeof(ModbusRequestData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> WriteShortArrayAsync([FromBody, Required] List<short> data, ushort offset = 0, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = (ushort)data.Count
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusWriteArrayRequest(request, data.ToArray(), WriteArrayRequestFunctions.WriteShortArrayAsync);
        }

        /// <summary>
        /// Writes an array of unsigned 16 bit integer to a Modbus slave.
        /// </summary>
        /// <remarks>Note that this writes multiple holding registers.</remarks>
        /// <param name="data">The Modbus data values.</param>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data if OK.</response>
        /// <response code="400">If the request data are invalid.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occured in the slave.</response>
        [HttpPut("UShort/{offset}")]
        [SwaggerOperation(Tags = new[] { "UShort Data Values" })]
        [ProducesResponseType(typeof(ModbusRequestData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> WriteUShortArrayAsync([FromBody, Required] List<ushort> data, ushort offset = 0, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = (ushort)data.Count
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusWriteArrayRequest(request, data.ToArray(), WriteArrayRequestFunctions.WriteUShortArrayAsync);
        }

        /// <summary>
        /// Writes an array of 32 bit integers to a Modbus slave.
        /// </summary>
        /// <remarks>Note that this writes multiple holding registers.</remarks>
        /// <param name="data">The Modbus data values.</param>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data if OK.</response>
        /// <response code="400">If the request data are invalid.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occured in the slave.</response>
        [HttpPut("Int32/{offset}")]
        [SwaggerOperation(Tags = new[] { "Int32 Data Values" })]
        [ProducesResponseType(typeof(ModbusRequestData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> WriteInt32ArrayAsync([FromBody, Required] List<int> data, ushort offset = 0, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = (ushort)data.Count
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusWriteArrayRequest(request, data.ToArray(), WriteArrayRequestFunctions.WriteInt32ArrayAsync);
        }

        /// <summary>
        /// Writes an array of unsigned 32 bit integers to a Modbus slave.
        /// </summary>
        /// <remarks>Note that this writes multiple holding registers.</remarks>
        /// <param name="data">The Modbus data values.</param>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data if OK.</response>
        /// <response code="400">If the request data are invalid.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occured in the slave.</response>
        [HttpPut("UInt32/{offset}")]
        [SwaggerOperation(Tags = new[] { "UInt32 Data Values" })]
        [ProducesResponseType(typeof(ModbusRequestData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> WriteUInt32ArrayAsync([FromBody, Required] List<uint> data, ushort offset = 0, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = (ushort)data.Count
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusWriteArrayRequest(request, data.ToArray(), WriteArrayRequestFunctions.WriteUInt32ArrayAsync);
        }

        /// <summary>
        /// Writes an array of 32 bit IEEE floating point numbers to a Modbus slave.
        /// </summary>
        /// <remarks>Note that this writes multiple holding registers.</remarks>
        /// <param name="data">The Modbus data values.</param>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data if OK.</response>
        /// <response code="400">If the request data are invalid.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occured in the slave.</response>
        [HttpPut("Float/{offset}")]
        [SwaggerOperation(Tags = new[] { "Float Data Values" })]
        [ProducesResponseType(typeof(ModbusRequestData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> WriteFloatArrayAsync([FromBody, Required] List<float> data, ushort offset = 0, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = (ushort)data.Count
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusWriteArrayRequest(request, data.ToArray(), WriteArrayRequestFunctions.WriteFloatArrayAsync);
        }

        /// <summary>
        /// Writes an array of 64 bit IEEE floating point numbers to a Modbus slave.
        /// </summary>
        /// <remarks>Note that this writes multiple holding registers.</remarks>
        /// <param name="data">The Modbus data values.</param>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data if OK.</response>
        /// <response code="400">If the request data are invalid.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occured in the slave.</response>
        [HttpPut("Double/{offset}")]
        [SwaggerOperation(Tags = new[] { "Double Data Values" })]
        [ProducesResponseType(typeof(ModbusRequestData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> WriteDoubleArrayAsync([FromBody, Required] List<double> data, ushort offset = 0, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = (ushort)data.Count
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusWriteArrayRequest(request, data.ToArray(), WriteArrayRequestFunctions.WriteDoubleArrayAsync);
        }

        /// <summary>
        /// Writes an array of 64 bit integers to a Modbus slave.
        /// </summary>
        /// <remarks>Note that this writes multiple holding registers.</remarks>
        /// <param name="data">The Modbus data values.</param>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data if OK.</response>
        /// <response code="400">If the request data are invalid.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occured in the slave.</response>
        [HttpPut("Long/{offset}")]
        [SwaggerOperation(Tags = new[] { "Long Data Values" })]
        [ProducesResponseType(typeof(ModbusRequestData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> WriteLongArrayAsync([FromBody, Required] List<long> data, ushort offset = 0, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = (ushort)data.Count
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusWriteArrayRequest(request, data.ToArray(), WriteArrayRequestFunctions.WriteLongArrayAsync);
        }

        /// <summary>
        /// Writes an array of unsigned 64 bit integers to a Modbus slave.
        /// </summary>
        /// <remarks>Note that this writes multiple holding registers.</remarks>
        /// <param name="data">The Modbus data values.</param>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data if OK.</response>
        /// <response code="400">If the request data are invalid.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occured in the slave.</response>
        [HttpPut("ULong/{offset}")]
        [SwaggerOperation(Tags = new[] { "ULong Data Values" })]
        [ProducesResponseType(typeof(ModbusRequestData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> WriteULongArrayAsync([FromBody, Required] List<ulong> data, ushort offset = 0, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = (ushort)data.Count
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusWriteArrayRequest(request, data.ToArray(), WriteArrayRequestFunctions.WriteULongArrayAsync);
        }
    }
}
