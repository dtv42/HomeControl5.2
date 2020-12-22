// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ETAPU11Controller.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>17-12-2020 12:52</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace ETAPU11Web.Controllers
{
    #region Using Directives

    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using Swashbuckle.AspNetCore.Annotations;

    using UtilityLib;
    using UtilityLib.Webapp;

    using ETAPU11Lib;
    using ETAPU11Lib.Models;

    using ETAPU11Web.Models;

    #endregion Using Directives

    /// <summary>
    /// The ETAPU11 controller for reading ETAPU11 data items.
    /// </summary>
    [ApiController]
    [Route("/")]
    [Produces("application/json")]
    public class ETAPU11Controller : BaseController
    {
        #region Private Fields

        private readonly ETAPU11Gateway _gateway;

        #endregion Private Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ETAPU11Controller"/> class.
        /// The parameters provided by dependency injection are used to set private fields.
        /// </summary>
        /// <param name="gateway">The ETAPU11 gateway instance.</param>
        /// <param name="configuration">The application configuration instance.</param>
        /// <param name="logger">The logger instance.</param>
        public ETAPU11Controller(ETAPU11Gateway gateway, IConfiguration configuration, ILogger<ETAPU11Controller> logger)
            : base(configuration, logger)
        {
            _gateway = gateway;
        }

        #endregion Constructors

        #region REST Methods

        #region GET Methods

        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult YourAction()
        {
            return Redirect("swagger");
        }

        /// <summary>
        /// Returns all ETAPU11 gateway data.
        /// </summary>
        /// <param name="update">Indicates if an update is requested.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Return indicates Ok.</response>
        /// <response code="400">Return indicates BadRequest.</response>
        /// <response code="404">Return indicates NotFound.</response>
        /// <response code="409">Return indicates Conflict.</response>
        /// <response code="423">Return indicates Locked source.</response>
        /// <response code="500">Return indicates InternalError.</response>
        /// <response code="502">Return indicates BadGateway.</response>
        /// <response code="503">Return indicates Unavailable.</response>
        [HttpGet("gateway")]
        [SwaggerOperation(Tags = new[] { "ETAPU11 API" })]
        [ProducesResponseType(typeof(ETAPU11Gateway), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetETAPU11Gateway(bool update = false)
        {
            _logger?.LogDebug("GetETAPU11Gateway()...");

            if (_gateway.IsLocked) return Locked("ETAPU11 gateway is locked");

            if (update)
            {
                var status = await _gateway.ReadAllAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(new ETAPU11Info()
            {
                Settings = _gateway.Settings,
                IsStartupOk = _gateway.IsStartupOk,
                IsLocked = _gateway.IsLocked,
                Status = _gateway.Status
            });
        }

        /// <summary>
        /// Returns all ETAPU11 related data.
        /// </summary>
        /// <param name="block">Indicates thet a block read is requested.</param>
        /// <param name="update">Indicates if an update is requested.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Return indicates Ok.</response>
        /// <response code="400">Return indicates BadRequest.</response>
        /// <response code="404">Return indicates NotFound.</response>
        /// <response code="409">Return indicates Conflict.</response>
        /// <response code="423">Return indicates Locked source.</response>
        /// <response code="500">Return indicates InternalError.</response>
        /// <response code="502">Return indicates BadGateway.</response>
        /// <response code="503">Return indicates Unavailable.</response>
        [HttpGet("data")]
        [SwaggerOperation(Tags = new[] { "ETAPU11 API" })]
        [ProducesResponseType(typeof(ETAPU11Data), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetETAPU11Data(bool block = true, bool update = false)
        {
            _logger?.LogDebug("GetETAPU11Data()...");

            if (_gateway.IsLocked) return Locked("ETAPU11 gateway is locked");

            if (update)
            {
                var status = block ? await _gateway.ReadBlockAllAsync() : await _gateway.ReadAllAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            // Fix: Json Serializer cannot serialize NaN
            if (double.IsNaN(_gateway.Data.Flow)) _gateway.Data.Flow = 0;
            if (double.IsNaN(_gateway.Data.ResidualO2)) _gateway.Data.ResidualO2 = 0;

            return Ok(_gateway.Data);
        }

        /// <summary>
        /// Returns a subset of ETAPU11 data.
        /// </summary>
        /// <param name="update">Indicates if an update is requested.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Return indicates Ok.</response>
        /// <response code="400">Return indicates BadRequest.</response>
        /// <response code="404">Return indicates NotFound.</response>
        /// <response code="409">Return indicates Conflict.</response>
        /// <response code="423">Return indicates Locked source.</response>
        /// <response code="500">Return indicates InternalError.</response>
        /// <response code="502">Return indicates BadGateway.</response>
        /// <response code="503">Return indicates Unavailable.</response>
        [HttpGet("boiler")]
        [SwaggerOperation(Tags = new[] { "ETAPU11 API" })]
        [ProducesResponseType(typeof(BoilerData), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetBoilerData(bool update = false)
        {
            _logger?.LogDebug("GetBoilerData()...");

            if (_gateway.IsLocked) return Locked("ETAPU11 gateway is locked");

            if (update)
            {
                var status = await _gateway.ReadBlockAllAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(_gateway.BoilerData);
        }

        /// <summary>
        /// Returns a subset of ETAPU11 data.
        /// </summary>
        /// <param name="update">Indicates if an update is requested.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Return indicates Ok.</response>
        /// <response code="400">Return indicates BadRequest.</response>
        /// <response code="404">Return indicates NotFound.</response>
        /// <response code="409">Return indicates Conflict.</response>
        /// <response code="423">Return indicates Locked source.</response>
        /// <response code="500">Return indicates InternalError.</response>
        /// <response code="502">Return indicates BadGateway.</response>
        /// <response code="503">Return indicates Unavailable.</response>
        [HttpGet("hotwater")]
        [SwaggerOperation(Tags = new[] { "ETAPU11 API" })]
        [ProducesResponseType(typeof(HotwaterData), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetHotwaterData(bool update = false)
        {
            _logger?.LogDebug("GetHotwaterData()...");

            if (_gateway.IsLocked) return Locked("ETAPU11 gateway is locked");

            if (update)
            {
                var status = await _gateway.ReadBlockAllAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(_gateway.HotwaterData);
        }

        /// <summary>
        /// Returns a subset of ETAPU11 data.
        /// </summary>
        /// <param name="update">Indicates if an update is requested.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Return indicates Ok.</response>
        /// <response code="400">Return indicates BadRequest.</response>
        /// <response code="404">Return indicates NotFound.</response>
        /// <response code="409">Return indicates Conflict.</response>
        /// <response code="423">Return indicates Locked source.</response>
        /// <response code="500">Return indicates InternalError.</response>
        /// <response code="502">Return indicates BadGateway.</response>
        /// <response code="503">Return indicates Unavailable.</response>
        [HttpGet("heating")]
        [SwaggerOperation(Tags = new[] { "ETAPU11 API" })]
        [ProducesResponseType(typeof(HeatingData), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        [ProducesResponseType(typeof(string), 502)]
        public async Task<IActionResult> GetHeatingData(bool update = false)
        {
            _logger?.LogDebug("GetHeatingData()...");

            if (_gateway.IsLocked) return Locked("ETAPU11 gateway is locked");

            if (update)
            {
                var status = await _gateway.ReadBlockAllAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            // Fix: Json Serializer cannot serialize NaN
            if (double.IsNaN(_gateway.Data.Flow)) _gateway.Data.Flow = 0;

            return Ok(_gateway.HeatingData);
        }

        /// <summary>
        /// Returns a subset of ETAPU11 data.
        /// </summary>
        /// <param name="update">Indicates if an update is requested.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Return indicates Ok.</response>
        /// <response code="400">Return indicates BadRequest.</response>
        /// <response code="404">Return indicates NotFound.</response>
        /// <response code="409">Return indicates Conflict.</response>
        /// <response code="423">Return indicates Locked source.</response>
        /// <response code="500">Return indicates InternalError.</response>
        /// <response code="502">Return indicates BadGateway.</response>
        /// <response code="503">Return indicates Unavailable.</response>
        [HttpGet("storage")]
        [SwaggerOperation(Tags = new[] { "ETAPU11 API" })]
        [ProducesResponseType(typeof(StorageData), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetStorageData(bool update = false)
        {
            _logger?.LogDebug("GetStorageData()...");

            if (_gateway.IsLocked) return Locked("ETAPU11 gateway is locked");

            if (update)
            {
                var status = await _gateway.ReadBlockAllAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(_gateway.StorageData);
        }

        /// <summary>
        /// Returns a subset of ETAPU11 data.
        /// </summary>
        /// <param name="update">Indicates if an update is requested.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Return indicates Ok.</response>
        /// <response code="400">Return indicates BadRequest.</response>
        /// <response code="404">Return indicates NotFound.</response>
        /// <response code="409">Return indicates Conflict.</response>
        /// <response code="423">Return indicates Locked source.</response>
        /// <response code="500">Return indicates InternalError.</response>
        /// <response code="502">Return indicates BadGateway.</response>
        /// <response code="503">Return indicates Unavailable.</response>
        [HttpGet("system")]
        [SwaggerOperation(Tags = new[] { "ETAPU11 API" })]
        [ProducesResponseType(typeof(SystemData), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetSystemData(bool update = false)
        {
            _logger?.LogDebug("GetSystemData()...");

            if (_gateway.IsLocked) return Locked("ETAPU11 gateway is locked");

            if (update)
            {
                var status = await _gateway.ReadBlockAllAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(_gateway.SystemData);
        }

        /// <summary>
        /// Returns a single ETAPU11 property.
        /// </summary>
        /// <remarks>The property name is a CamelCase name.</remarks>
        /// <param name="name">The name of the property.</param>
        /// <param name="update">Indicates if an update is requested.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Return indicates Ok.</response>
        /// <response code="400">Return indicates BadRequest.</response>
        /// <response code="404">Return indicates NotFound.</response>
        /// <response code="409">Return indicates Conflict.</response>
        /// <response code="423">Return indicates Locked source.</response>
        /// <response code="500">Return indicates InternalError.</response>
        /// <response code="502">Return indicates BadGateway.</response>
        /// <response code="503">Return indicates Unavailable.</response>
        [HttpGet("data/property/{name}")]
        [SwaggerOperation(Tags = new[] { "ETAPU11 API" })]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetETAPU11DataProperty(string name, bool update = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger?.LogDebug($"GetETAPU11DataProperty() invalid property.");
                return BadRequest($"Property name is invalid.");
            }

            _logger?.LogDebug($"GetETAPU11Data({name})...");

            if (typeof(ETAPU11Data).IsProperty(name))
            {
                if (update)
                {
                    if (ETAPU11Data.IsReadable(name))
                    {
                        if (_gateway.IsLocked) return Locked("ETAPU11 gateway is locked");

                        var status = await _gateway.ReadPropertyAsync(name);

                        if (!status.IsGood) return StatusCode(status);
                    }
                    else
                    {
                        _logger?.LogDebug($"GetETAPU11DataProperty('{name}') property not readable.");
                        return BadRequest($"Property '{name}' not readable.");
                    }
                }

                return Ok(_gateway.Data.GetPropertyValue(name));
            }
            else
            {
                _logger?.LogDebug($"GetETAPU11DataProperty('{name}') property not found.");
                return NotFound($"Property '{name}' not found.");
            }
        }

        /// <summary>
        /// Returns a single BoilerData property.
        /// </summary>
        /// <remarks>The property name is a CamelCase name.</remarks>
        /// <param name="name">The name of the property.</param>
        /// <param name="update">Indicates if an update is requested.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Return indicates Ok.</response>
        /// <response code="400">Return indicates BadRequest.</response>
        /// <response code="404">Return indicates NotFound.</response>
        /// <response code="409">Return indicates Conflict.</response>
        /// <response code="423">Return indicates Locked source.</response>
        /// <response code="500">Return indicates InternalError.</response>
        /// <response code="502">Return indicates BadGateway.</response>
        /// <response code="503">Return indicates Unavailable.</response>
        [HttpGet("boiler/property/{name}")]
        [SwaggerOperation(Tags = new[] { "ETAPU11 API" })]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetBoilerProperty(string name, bool update = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger?.LogDebug($"GetBoilerProperty() invalid property.");
                return BadRequest($"Property name is invalid.");
            }

            _logger?.LogDebug($"GetBoilerProperty({name})...");

            if (typeof(BoilerData).IsProperty(name))
            {
                if (update)
                {
                    if (_gateway.IsLocked) return Locked("ETAPU11 gateway is locked");

                    var status = await _gateway.ReadPropertyAsync(name);

                    if (!status.IsGood) return StatusCode(status);
                }

                return Ok(_gateway.BoilerData.GetPropertyValue(name));
            }
            else
            {
                _logger?.LogDebug($"GetBoilerProperty('{name}') property not found.");
                return NotFound($"Property '{name}' not found.");
            }
        }

        /// <summary>
        /// Returns a single HotwaterData property.
        /// </summary>
        /// <remarks>The property name is a CamelCase name.</remarks>
        /// <param name="name">The name of the property.</param>
        /// <param name="update">Indicates if an update is requested.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Return indicates Ok.</response>
        /// <response code="400">Return indicates BadRequest.</response>
        /// <response code="404">Return indicates NotFound.</response>
        /// <response code="409">Return indicates Conflict.</response>
        /// <response code="423">Return indicates Locked source.</response>
        /// <response code="500">Return indicates InternalError.</response>
        /// <response code="502">Return indicates BadGateway.</response>
        /// <response code="503">Return indicates Unavailable.</response>
        [HttpGet("hotwater/property/{name}")]
        [SwaggerOperation(Tags = new[] { "ETAPU11 API" })]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetHotwaterProperty(string name, bool update = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger?.LogDebug($"GetHotwaterProperty() invalid property.");
                return BadRequest($"Property name is invalid.");
            }

            _logger?.LogDebug($"GetHotwaterProperty({name})...");

            if (typeof(HotwaterData).IsProperty(name))
            {
                if (update)
                {
                    if (_gateway.IsLocked) return Locked("ETAPU11 gateway is locked");

                    var status = await _gateway.ReadPropertyAsync(name);

                    if (!status.IsGood) return StatusCode(status);
                }

                return Ok(_gateway.HotwaterData.GetPropertyValue(name));
            }
            else
            {
                _logger?.LogDebug($"GetHotwaterProperty('{name}') property not found.");
                return NotFound($"Property '{name}' not found.");
            }
        }

        /// <summary>
        /// Returns a single HeatingData property.
        /// </summary>
        /// <remarks>The property name is a CamelCase name.</remarks>
        /// <param name="name">The name of the property.</param>
        /// <param name="update">Indicates if an update is requested.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Return indicates Ok.</response>
        /// <response code="400">Return indicates BadRequest.</response>
        /// <response code="404">Return indicates NotFound.</response>
        /// <response code="409">Return indicates Conflict.</response>
        /// <response code="423">Return indicates Locked source.</response>
        /// <response code="500">Return indicates InternalError.</response>
        /// <response code="502">Return indicates BadGateway.</response>
        /// <response code="503">Return indicates Unavailable.</response>
        [HttpGet("heating/property/{name}")]
        [SwaggerOperation(Tags = new[] { "ETAPU11 API" })]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetHeatingProperty(string name, bool update = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger?.LogDebug($"GetHeatingProperty() invalid property.");
                return BadRequest($"Property name is invalid.");
            }

            _logger?.LogDebug($"GetHeatingProperty({name})...");

            if (typeof(HeatingData).IsProperty(name))
            {
                if (update)
                {
                    if (_gateway.IsLocked) return Locked("ETAPU11 gateway is locked");

                    var status = await _gateway.ReadPropertyAsync(name);

                    if (!status.IsGood) return StatusCode(status);
                }

                return Ok(_gateway.HeatingData.GetPropertyValue(name));
            }
            else
            {
                _logger?.LogDebug($"GetHeatingProperty('{name}') property not found.");
                return NotFound($"Property '{name}' not found.");
            }
        }

        /// <summary>
        /// Returns a single StorageData property.
        /// </summary>
        /// <remarks>The property name is a CamelCase name.</remarks>
        /// <param name="name">The name of the property.</param>
        /// <param name="update">Indicates if an update is requested.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Return indicates Ok.</response>
        /// <response code="400">Return indicates BadRequest.</response>
        /// <response code="404">Return indicates NotFound.</response>
        /// <response code="409">Return indicates Conflict.</response>
        /// <response code="423">Return indicates Locked source.</response>
        /// <response code="500">Return indicates InternalError.</response>
        /// <response code="502">Return indicates BadGateway.</response>
        /// <response code="503">Return indicates Unavailable.</response>
        [HttpGet("storage/property/{name}")]
        [SwaggerOperation(Tags = new[] { "ETAPU11 API" })]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetStorageProperty(string name, bool update = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger?.LogDebug($"GetStorageProperty() invalid property.");
                return BadRequest($"Property name is invalid.");
            }

            _logger?.LogDebug($"GetStorageProperty({name})...");

            if (typeof(StorageData).IsProperty(name))
            {
                if (update)
                {
                    if (_gateway.IsLocked) return Locked("ETAPU11 gateway is locked");

                    var status = await _gateway.ReadPropertyAsync(name);

                    if (!status.IsGood) return StatusCode(status);
                }

                return Ok(_gateway.StorageData.GetPropertyValue(name));
            }
            else
            {
                _logger?.LogDebug($"GetStorageProperty('{name}') property not found.");
                return NotFound($"Property '{name}' not found.");
            }
        }

        /// <summary>
        /// Returns a single SystemData property.
        /// </summary>
        /// <remarks>The property name is a CamelCase name.</remarks>
        /// <param name="name">The name of the property.</param>
        /// <param name="update">Indicates if an update is requested.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Return indicates Ok.</response>
        /// <response code="400">Return indicates BadRequest.</response>
        /// <response code="404">Return indicates NotFound.</response>
        /// <response code="409">Return indicates Conflict.</response>
        /// <response code="423">Return indicates Locked source.</response>
        /// <response code="500">Return indicates InternalError.</response>
        /// <response code="502">Return indicates BadGateway.</response>
        /// <response code="503">Return indicates Unavailable.</response>
        [HttpGet("system/property/{name}")]
        [SwaggerOperation(Tags = new[] { "ETAPU11 API" })]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetSystemProperty(string name, bool update = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger?.LogDebug($"GetSystemProperty() invalid property.");
                return BadRequest($"Property name is invalid.");
            }

            _logger?.LogDebug($"GetSystemProperty({name})...");

            if (typeof(SystemData).IsProperty(name))
            {
                if (update)
                {
                    if (_gateway.IsLocked) return Locked("ETAPU11 gateway is locked");

                    var status = await _gateway.ReadPropertyAsync(name);

                    if (!status.IsGood) return StatusCode(status);
                }

                return Ok(_gateway.SystemData.GetPropertyValue(name));
            }
            else
            {
                _logger?.LogDebug($"GetSystemProperty('{name}') property not found.");
                return NotFound($"Property '{name}' not found.");
            }
        }

        #endregion GET Methods

        #region PUT Methods

        /// <summary>
        /// Writes a single ETAPU11 property.
        /// </summary>
        /// <remarks>The property name is a CamelCase name.</remarks>
        /// <param name="name">The name of the property.</param>
        /// <param name="value">The value of the property.</param>
        /// <returns>The action method result.</returns>
        /// <response code="200">Return indicates Ok.</response>
        /// <response code="400">Return indicates BadRequest.</response>
        /// <response code="404">Return indicates NotFound.</response>
        /// <response code="409">Return indicates Conflict.</response>
        /// <response code="423">Return indicates Locked source.</response>
        /// <response code="500">Return indicates InternalError.</response>
        /// <response code="502">Return indicates BadGateway.</response>
        /// <response code="503">Return indicates Unavailable.</response>
        [HttpPut("data/property/{name}")]
        [SwaggerOperation(Tags = new[] { "ETAPU11 API" })]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> PutETAPU11Data(string name, [FromQuery] string value)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger?.LogDebug($"PutETAPU11Data({name}, {value}) invalid property.");
                return BadRequest($"Property name is invalid.");
            }

            if (string.IsNullOrEmpty(value))
            {
                _logger?.LogDebug($"PutETAPU11Data({name}, {value}) invalid value.");
                return BadRequest($"Property value is invalid.");
            }

            _logger?.LogDebug($"PutETAPU11Data({name}, {value})...");

            if (ETAPU11Data.IsProperty(name))
            {
                if (ETAPU11Data.IsWritable(name))
                {
                    if (_gateway.IsLocked) return Locked("ETAPU11 gateway is locked");

                    var status = await _gateway.WritePropertyAsync(name, value);

                    if (!status.IsGood) return StatusCode(status);

                    return Ok();
                }
                else
                {
                    _logger?.LogDebug($"PutETAPU11Data('{name}, {value}') property not writable.");
                    return BadRequest($"Property '{name}' not writable.");
                }
            }
            else
            {
                _logger?.LogDebug($"PutETAPU11Data('{name}, {value}') property not found.");
                return NotFound($"Property '{name}' not found.");
            }
        }

        #endregion PUT Methods

        #endregion REST Methods
    }
}