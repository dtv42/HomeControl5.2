﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RWSingleController.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>13-5-2020 13:52</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace ModbusRTU.Controllers
{
    #region Using Directives

    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    using Swashbuckle.AspNetCore.Annotations;

    using ModbusLib;
    using ModbusRTU.Models;

    #endregion

    /// <summary>
    /// The Modbus Gateway MVC Controller for reading and writing various data values.
    /// </summary>
    /// <para>
    ///     ReadString          Reads an ASCII string (multiple holding registers)
    ///     ReadHexString       Reads an HEX string (multiple holding registers)
    ///     ReadBool            Reads a boolean value (single coil)
    ///     ReadBits            Reads a 16-bit bit array value (single holding register)
    ///     ReadShort           Reads a 16 bit integer (single holding register)
    ///     ReadUShort          Reads an unsigned 16 bit integer (single holding register)
    ///     ReadInt32           Reads a 32 bit integer (two holding registers)
    ///     ReadUInt32          Reads an unsigned 32 bit integer (two holding registers)
    ///     ReadFloat           Reads a 32 bit IEEE floating point number (two holding registers)
    ///     ReadDouble          Reads a 64 bit IEEE floating point number (four holding registers)
    ///     ReadLong            Reads a 64 bit integer (four holding registers)
    ///     ReadULong           Reads an unsigned 64 bit integer (four holding registers)
    /// </para>
    /// <para>
    ///     WriteString         Writes an ASCII string (multiple holding registers)
    ///     WriteHexString      Writes an HEX string (multiple holding registers)
    ///     WriteBool           Writes a boolean value (single coil)
    ///     WriteBits           Writes a 16-bit bit array value (single holding register)
    ///     WriteShort          Writes a 16 bit integer (single holding register)
    ///     WriteUShort         Writes an unsigned 16 bit integer (single holding register)
    ///     WriteInt32          Writes a 32 bit integer (two holding registers)
    ///     WriteUInt32         Writes an unsigned 32 bit integer (two holding registers)
    ///     WriteFloat          Writes a 32 bit IEEE floating point number (two holding registers)
    ///     WriteDouble         Writes a 64 bit IEEE floating point number (four holding registers)
    ///     WriteLong           Writes a 64 bit integer (four holding registers)
    ///     WriteULong          Writes an unsigned 64 bit integer (four holding registers)
    /// </para>
    [Route("[controller]")]
    [ApiController]
    public class RWSingleController : ModbusController
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RWSingleController"/> class.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="settings"></param>
        /// <param name="config"></param>
        /// <param name="environment"></param>
        /// <param name="lifetime"></param>
        /// <param name="logger"></param>
        public RWSingleController(IRtuModbusClient client,
                                  AppSettings settings,
                                  IConfiguration config,
                                  IHostEnvironment environment,
                                  IHostApplicationLifetime lifetime,
                                  ILogger<RWSingleController> logger)
            : base(client, settings, config, environment, lifetime, logger)
        { }

        #endregion

        /// <summary>
        /// Reads an ASCII string from a Modbus slave.
        /// </summary>
        /// <remarks>Note that this reads multiple holding registers.</remarks>
        /// <param name="offset">The Modbus address (offset) of the data value.</param>
        /// <param name="size">The size of the string.</param>
        /// <param name="slave">The slave ID of the Modbus RTU slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the string data value.</response>
        /// <response code="400">If the Modbus gateway cannot open the COM port.</response>
        /// <response code="403">If the Modbus gateway has no access to the COM port.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("String/{offset}")]
        [SwaggerOperation(Tags = new[] { "String Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseStringData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadStringAsync(ushort offset = 0, ushort size = 1, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = size
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusReadRequest(request, ReadRequestFunctions.ReadStringAsync);
        }

        /// <summary>
        /// Reads an HEX string from a Modbus slave.
        /// </summary>
        /// <remarks>Note that the resulting HEX string length is twice the number parameter.</remarks>
        /// <param name="offset">The Modbus address (offset) of the data value.</param>
        /// <param name="number">The number of 2-bytes HEX substrings in the string.</param>
        /// <param name="slave">The slave ID of the Modbus RTU slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the HEX string data value.</response>
        /// <response code="400">If the Modbus gateway cannot open the COM port.</response>
        /// <response code="403">If the Modbus gateway has no access to the COM port.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("HexString/{offset}")]
        [SwaggerOperation(Tags = new[] { "HEX String Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseStringData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadHexStringAsync(ushort offset = 0, ushort number = 1, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = number
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusReadRequest(request, ReadRequestFunctions.ReadHexStringAsync);
        }

        /// <summary>
        /// Reads a boolean value from a Modbus slave.
        /// </summary>
        /// <remarks>Note that this is equivalent to read a single coil.</remarks>
        /// <param name="offset">The Modbus address (offset) of the data value.</param>
        /// <param name="slave">The slave ID of the Modbus RTU slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the boolean data value.</response>
        /// <response code="400">If the Modbus gateway cannot open the COM port.</response>
        /// <response code="403">If the Modbus gateway has no access to the COM port.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("Bool/{offset}")]
        [SwaggerOperation(Tags = new[] { "Boolean Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseData<bool>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadBoolAsync(ushort offset = 0, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = 1
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusReadRequest(request, ReadRequestFunctions.ReadBoolAsync);
        }

        /// <summary>
        /// Reads a 16-bit bit array value from a Modbus slave.
        /// </summary>
        /// <remarks>Note that this reads a single holding register.</remarks>
        /// <param name="offset">The Modbus address (offset) of the data value.</param>
        /// <param name="slave">The slave ID of the Modbus RTU slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the boolean data value.</response>
        /// <response code="400">If the Modbus gateway cannot open the COM port.</response>
        /// <response code="403">If the Modbus gateway has no access to the COM port.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("Bits/{offset}")]
        [SwaggerOperation(Tags = new[] { "Bits Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseStringData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadBitsAsync(ushort offset = 0, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = 1
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusReadRequest(request, ReadRequestFunctions.ReadBitsAsync);
        }

        /// <summary>
        /// Reads a 16 bit integer from a Modbus slave.
        /// </summary>
        /// <remarks>Note that this reads a single holding register.</remarks>
        /// <param name="offset">The Modbus address (offset) of the data value.</param>
        /// <param name="slave">The slave ID of the Modbus RTU slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the short data value.</response>
        /// <response code="400">If the Modbus gateway cannot open the COM port.</response>
        /// <response code="403">If the Modbus gateway has no access to the COM port.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("Short/{offset}")]
        [SwaggerOperation(Tags = new[] { "Short Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseData<short>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadShortAsync(ushort offset = 0, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = 1
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusReadRequest(request, ReadRequestFunctions.ReadShortAsync);
        }

        /// <summary>
        /// Reads an unsigned 16 bit integer from a Modbus slave.
        /// </summary>
        /// <remarks>Note that this reads a single holding register.</remarks>
        /// <param name="offset">The Modbus address (offset) of the data value.</param>
        /// <param name="slave">The slave ID of the Modbus RTU slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the ushort data value.</response>
        /// <response code="400">If the Modbus gateway cannot open the COM port.</response>
        /// <response code="403">If the Modbus gateway has no access to the COM port.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("UShort/{offset}")]
        [SwaggerOperation(Tags = new[] { "UShort Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseData<ushort>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadUShortAsync(ushort offset = 0, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = 1
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusReadRequest(request, ReadRequestFunctions.ReadUShortAsync);
        }

        /// <summary>
        /// Reads a 32 bit integer from a Modbus slave.
        /// </summary>
        /// <remarks>Note that this reads two holding registers.</remarks>
        /// <param name="offset">The Modbus address (offset) of the data value.</param>
        /// <param name="slave">The slave ID of the Modbus RTU slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the int data value.</response>
        /// <response code="400">If the Modbus gateway cannot open the COM port.</response>
        /// <response code="403">If the Modbus gateway has no access to the COM port.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("Int32/{offset}")]
        [SwaggerOperation(Tags = new[] { "Int32 Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseData<int>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadInt32Async(ushort offset = 0, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = 1
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusReadRequest(request, ReadRequestFunctions.ReadInt32Async);
        }

        /// <summary>
        /// Reads an unsigned 32 bit integer from a Modbus slave.
        /// </summary>
        /// <remarks>Note that this reads two holding registers.</remarks>
        /// <param name="offset">The Modbus address (offset) of the data value.</param>
        /// <param name="slave">The slave ID of the Modbus RTU slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the uint data value.</response>
        /// <response code="400">If the Modbus gateway cannot open the COM port.</response>
        /// <response code="403">If the Modbus gateway has no access to the COM port.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("UInt32/{offset}")]
        [SwaggerOperation(Tags = new[] { "UInt32 Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseData<uint>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadUInt32Async(ushort offset = 0, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = 1
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusReadRequest(request, ReadRequestFunctions.ReadUInt32Async);
        }

        /// <summary>
        /// Reads a 32 bit IEEE floating point number from a Modbus slave.
        /// </summary>
        /// <remarks>Note that this reads two holding registers.</remarks>
        /// <param name="offset">The Modbus address (offset) of the data value.</param>
        /// <param name="slave">The slave ID of the Modbus RTU slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the float data value.</response>
        /// <response code="400">If the Modbus gateway cannot open the COM port.</response>
        /// <response code="403">If the Modbus gateway has no access to the COM port.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("Float/{offset}")]
        [SwaggerOperation(Tags = new[] { "Float Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseData<float>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadFloatAsync(ushort offset = 0, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = 1
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusReadRequest(request, ReadRequestFunctions.ReadFloatAsync);
        }

        /// <summary>
        /// Reads a 64 bit IEEE floating point number from a Modbus slave.
        /// </summary>
        /// <remarks>Note that this reads four holding registers.</remarks>
        /// <param name="offset">The Modbus address (offset) of the data value.</param>
        /// <param name="slave">The slave ID of the Modbus RTU slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the double data value.</response>
        /// <response code="400">If the Modbus gateway cannot open the COM port.</response>
        /// <response code="403">If the Modbus gateway has no access to the COM port.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("Double/{offset}")]
        [SwaggerOperation(Tags = new[] { "Double Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseData<double>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadDoubleAsync(ushort offset = 0, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = 1
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusReadRequest(request, ReadRequestFunctions.ReadDoubleAsync);
        }

        /// <summary>
        /// Reads a 64 bit integer from a Modbus slave.
        /// </summary>
        /// <remarks>Note that this reads four holding registers.</remarks>
        /// <param name="offset">The Modbus address (offset) of the data value.</param>
        /// <param name="slave">The slave ID of the Modbus RTU slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the long data value.</response>
        /// <response code="400">If the Modbus gateway cannot open the COM port.</response>
        /// <response code="403">If the Modbus gateway has no access to the COM port.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("Long/{offset}")]
        [SwaggerOperation(Tags = new[] { "Long Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseData<long>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadLongAsync(ushort offset = 0, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = 1
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusReadRequest(request, ReadRequestFunctions.ReadLongAsync);
        }

        /// <summary>
        /// Reads an unsigned 64 bit integer from a Modbus slave.
        /// </summary>
        /// <remarks>Note that this reads four holding registers.</remarks>
        /// <param name="offset">The Modbus address (offset) of the data value.</param>
        /// <param name="slave">The slave ID of the Modbus RTU slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the ulong data value.</response>
        /// <response code="400">If the Modbus gateway cannot open the COM port.</response>
        /// <response code="403">If the Modbus gateway has no access to the COM port.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("ULong/{offset}")]
        [SwaggerOperation(Tags = new[] { "ULong Data Values" })]
        [ProducesResponseType(typeof(ModbusResponseData<ulong>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadULongAsync(ushort offset = 0, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = 1
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusReadRequest(request, ReadRequestFunctions.ReadULongAsync);
        }

        /// <summary>
        /// Writes an ASCII string to a Modbus slave.
        /// </summary>
        /// <remarks>Note that this writes multiple holding registers.</remarks>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
        /// <param name="data">The Modbus string data value.</param>
        /// <param name="slave">The slave ID of the Modbus RTU slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data if OK.</response>
        /// <response code="400">If the Modbus gateway cannot open the COM port.</response>
        /// <response code="403">If the Modbus gateway has no access to the COM port.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpPut("String/{offset}")]
        [SwaggerOperation(Tags = new[] { "String Data Values" })]
        [ProducesResponseType(typeof(ModbusRequestData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> WriteStringAsync([FromBody, Required] string data, ushort offset = 0, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = (ushort)data.Length
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusWriteSingleRequest(request, data, WriteRequestFunctions.WriteStringAsync);
        }

        /// <summary>
        /// Writes an HEX string to a Modbus slave.
        /// </summary>
        /// <remarks>The HEX string is limited to pairs of HEX digits (0..9A..F).</remarks>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
        /// <param name="data">The Modbus HEX string data value.</param>
        /// <param name="slave">The slave ID of the Modbus RTU slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data if OK.</response>
        /// <response code="400">If the Modbus gateway cannot open the COM port.</response>
        /// <response code="403">If the Modbus gateway has no access to the COM port.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpPut("HexString/{offset}")]
        [SwaggerOperation(Tags = new[] { "HEX String Data Values" })]
        [ProducesResponseType(typeof(ModbusRequestData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> WriteHexStringAsync([FromBody, Required] string data, ushort offset = 0, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = (ushort)data.Length
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusWriteSingleRequest(request, data, WriteRequestFunctions.WriteHexStringAsync);
        }

        /// <summary>
        /// Writes a boolean value to a Modbus slave.
        /// </summary>
        /// <remarks>Note that this is equivalent to write a single coil.</remarks>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
        /// <param name="data">The Modbus data value.</param>
        /// <param name="slave">The slave ID of the Modbus RTU slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data if OK.</response>
        /// <response code="400">If the Modbus gateway cannot open the COM port.</response>
        /// <response code="403">If the Modbus gateway has no access to the COM port.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpPut("Bool/{offset}")]
        [SwaggerOperation(Tags = new[] { "Boolean Data Values" })]
        [ProducesResponseType(typeof(ModbusRequestData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> WriteBoolAsync([FromBody, Required] bool data, ushort offset = 0, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = 1
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusWriteSingleRequest(request, data, WriteRequestFunctions.WriteBoolAsync);
        }

        /// <summary>
        /// Writes a 16-bit bit array value to a Modbus slave.
        /// </summary>
        /// <remarks>Note that this writes a single holding register.</remarks>
        /// <param name="offset">The Modbus address (offset) of the data value.</param>
        /// <param name="data">The Modbus data value.</param>
        /// <param name="slave">The slave ID of the Modbus RTU slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the boolean data value.</response>
        /// <response code="400">If the Modbus gateway cannot open the COM port.</response>
        /// <response code="403">If the Modbus gateway has no access to the COM port.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpPut("Bits/{offset}")]
        [SwaggerOperation(Tags = new[] { "Bits Data Values" })]
        [ProducesResponseType(typeof(ModbusRequestData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> WriteBitsAsync([FromBody, Required] string data, ushort offset = 0, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = 1
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusWriteSingleRequest(request, data, WriteRequestFunctions.WriteBitsAsync);
        }

        /// <summary>
        /// Writes a 16 bit integer to a Modbus slave.
        /// <remarks>Note that this writes a single holding register.</remarks>
        /// </summary>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
        /// <param name="data">The Modbus data value.</param>
        /// <param name="slave">The slave ID of the Modbus RTU slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data if OK.</response>
        /// <response code="400">If the Modbus gateway cannot open the COM port.</response>
        /// <response code="403">If the Modbus gateway has no access to the COM port.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpPut("Short/{offset}")]
        [SwaggerOperation(Tags = new[] { "Short Data Values" })]
        [ProducesResponseType(typeof(ModbusRequestData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> WriteShortAsync([FromBody, Required] short data, ushort offset = 0, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = 1
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusWriteSingleRequest(request, data, WriteRequestFunctions.WriteShortAsync);
        }

        /// <summary>
        /// Writes an unsigned 16 bit integer (single holding register) to a Modbus slave.
        /// </summary>
        /// <remarks>Note that this writes a single holding register.</remarks>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
        /// <param name="data">The Modbus data value.</param>
        /// <param name="slave">The slave ID of the Modbus RTU slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data if OK.</response>
        /// <response code="400">If the Modbus gateway cannot open the COM port.</response>
        /// <response code="403">If the Modbus gateway has no access to the COM port.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpPut("UShort/{offset}")]
        [SwaggerOperation(Tags = new[] { "UShort Data Values" })]
        [ProducesResponseType(typeof(ModbusRequestData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> WriteUShortAsync([FromBody, Required] ushort data, ushort offset = 0, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = 1
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusWriteSingleRequest(request, data, WriteRequestFunctions.WriteUShortAsync);
        }

        /// <summary>
        /// Writes a 32 bit integer to a Modbus slave.
        /// </summary>
        /// <remarks>Note that this writes two holding registers.</remarks>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
        /// <param name="data">The Modbus data value.</param>
        /// <param name="slave">The slave ID of the Modbus RTU slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data if OK.</response>
        /// <response code="400">If the Modbus gateway cannot open the COM port.</response>
        /// <response code="403">If the Modbus gateway has no access to the COM port.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpPut("Int32/{offset}")]
        [SwaggerOperation(Tags = new[] { "Int32 Data Values" })]
        [ProducesResponseType(typeof(ModbusRequestData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> WriteInt32Async([FromBody, Required] int data, ushort offset = 0, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = 1
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusWriteSingleRequest(request, data, WriteRequestFunctions.WriteInt32Async);
        }

        /// <summary>
        /// Writes an unsigned 32 bit integer to a Modbus slave.
        /// </summary>
        /// <remarks>Note that this writes two holding registers.</remarks>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
        /// <param name="data">The Modbus data value.</param>
        /// <param name="slave">The slave ID of the Modbus RTU slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data if OK.</response>
        /// <response code="400">If the Modbus gateway cannot open the COM port.</response>
        /// <response code="403">If the Modbus gateway has no access to the COM port.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpPut("UInt32/{offset}")]
        [SwaggerOperation(Tags = new[] { "UInt32 Data Values" })]
        [ProducesResponseType(typeof(ModbusRequestData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> WriteUInt32Async([FromBody, Required] uint data, ushort offset = 0, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = 1
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusWriteSingleRequest(request, data, WriteRequestFunctions.WriteUInt32Async);
        }

        /// <summary>
        /// Writes a 32 bit IEEE floating point number to a Modbus slave.
        /// </summary>
        /// <remarks>Note that this writes two holding registers.</remarks>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
        /// <param name="data">The Modbus data value.</param>
        /// <param name="slave">The slave ID of the Modbus RTU slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data if OK.</response>
        /// <response code="400">If the Modbus gateway cannot open the COM port.</response>
        /// <response code="403">If the Modbus gateway has no access to the COM port.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpPut("Float/{offset}")]
        [SwaggerOperation(Tags = new[] { "Float Data Values" })]
        [ProducesResponseType(typeof(ModbusRequestData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> WriteFloatAsync([FromBody, Required] float data, ushort offset = 0, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = 1
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusWriteSingleRequest(request, data, WriteRequestFunctions.WriteFloatAsync);
        }

        /// <summary>
        /// Writes a 64 bit IEEE floating point number to a Modbus slave.
        /// </summary>
        /// <remarks>Note that this writes four holding registers.</remarks>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
        /// <param name="data">The Modbus data value.</param>
        /// <param name="slave">The slave ID of the Modbus RTU slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data if OK.</response>
        /// <response code="400">If the Modbus gateway cannot open the COM port.</response>
        /// <response code="403">If the Modbus gateway has no access to the COM port.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpPut("Double/{offset}")]
        [SwaggerOperation(Tags = new[] { "Double Data Values" })]
        [ProducesResponseType(typeof(ModbusRequestData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> WriteDoubleAsync([FromBody, Required] double data, ushort offset = 0, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = 1
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusWriteSingleRequest(request, data, WriteRequestFunctions.WriteDoubleAsync);
        }

        /// <summary>
        /// Writes a 64 bit integer to a Modbus slave.
        /// </summary>
        /// <remarks>Note that this writes four holding registers.</remarks>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
        /// <param name="data">The Modbus data value.</param>
        /// <param name="slave">The slave ID of the Modbus RTU slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data if OK.</response>
        /// <response code="400">If the Modbus gateway cannot open the COM port.</response>
        /// <response code="403">If the Modbus gateway has no access to the COM port.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpPut("Long/{offset}")]
        [SwaggerOperation(Tags = new[] { "Long Data Values" })]
        [ProducesResponseType(typeof(ModbusRequestData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> WriteLongAsync([FromBody, Required] long data, ushort offset = 0, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = 1
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusWriteSingleRequest(request, data, WriteRequestFunctions.WriteLongAsync);
        }

        /// <summary>
        /// Writes an unsigned 64 bit integer to a Modbus slave.
        /// </summary>
        /// <remarks>Note that this writes four holding registers.</remarks>
        /// <param name="offset">The Modbus address (offset) of the data item.</param>
        /// <param name="data">The Modbus data value.</param>
        /// <param name="slave">The slave ID of the Modbus RTU slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data if OK.</response>
        /// <response code="400">If the Modbus gateway cannot open the COM port.</response>
        /// <response code="403">If the Modbus gateway has no access to the COM port.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpPut("ULong/{offset}")]
        [SwaggerOperation(Tags = new[] { "ULong Data Values" })]
        [ProducesResponseType(typeof(ModbusRequestData), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> WriteULongAsync([FromBody, Required] ulong data, ushort offset = 0, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = 1
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusWriteSingleRequest(request, data, WriteRequestFunctions.WriteULongAsync);
        }
    }
}
