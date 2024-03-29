// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DiscreteInputsController.cs" company="DTV-Online">
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
    /// The Modbus Gateway MVC Controller for reading discrete inputs.
    /// </summary>
    /// <para>
    ///     Read Discrete Inputs            (fc 2)
    /// </para>
    [Route("[controller]")]
    [ApiController]
    public class DiscreteInputsController : ModbusController
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="DiscreteInputsController"/> class.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="configuration"></param>
        /// <param name="logger"></param>
        public DiscreteInputsController(IRtuModbusClient client, IConfiguration configuration, ILogger<CoilController> logger)
            : base(client, configuration, logger)
        {
            _logger.LogDebug("DiscreteInputsController()");
        }

        #endregion

        /// <summary>
        /// Reading multiple discrete inputs from a Modbus slave.
        /// </summary>
        /// <param name="offset">The Modbus address (offset) of the first data item.</param>
        /// <param name="number">The number of the Modbus data items.</param>
        /// <param name="slave">The slave ID of the Modbus TCP slave.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Returns the request data and the array of discrete inputs.</response>
        /// <response code="400">If the Modbus gateway cannot open the COM port.</response>
        /// <response code="403">If the Modbus gateway has no access to the COM port.</response>
        /// <response code="404">If the Modbus gateway cannot connect to the slave.</response>
        /// <response code="500">If an error or an unexpected exception occurs.</response>
        /// <response code="502">If an unexpected exception occurs in the slave.</response>
        [HttpGet("{offset}")]
        [SwaggerOperation(Tags = new[] { "Coils & Discrete Inputs" })]
        [ProducesResponseType(typeof(ModbusResponseArrayData<bool>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 403)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> ReadInputsAsync(ushort offset = 0, ushort number = 1, byte? slave = null)
        {
            ModbusRequestData request = new ModbusRequestData()
            {
                Offset = offset,
                Number = number
            };

            if (slave.HasValue) request.Slave.ID = slave.Value;

            return await ModbusReadRequest(request, ReadRequestFunctions.ReadInputsAsync);
        }
    }
}
