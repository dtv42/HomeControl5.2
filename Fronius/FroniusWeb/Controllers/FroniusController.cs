// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FroniusController.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
// Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace FroniusWeb.Controllers
{
    #region Using Directives

    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using Swashbuckle.AspNetCore.Annotations;

    using UtilityLib;
    using UtilityLib.Webapp;

    using FroniusLib;
    using FroniusLib.Models;

    #endregion

    /// <summary>
    /// The Fronius controller for reading Fronius data items.
    /// </summary>
    [ApiController]
    [Route("/")]
    [Produces("application/json")]
    public class FroniusController : BaseController
    {
        #region Private Fields

        private readonly FroniusGateway _gateway;

        #endregion Private Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FroniusController"/> class.
        /// The parameters provided by dependency injection are used to set private fields.
        /// </summary>
        /// <param name="gateway">The EM300LR gateway instance.</param>
        /// <param name="configuration">The application configuration instance.</param>
        /// <param name="logger">The logger instance.</param>
        public FroniusController(FroniusGateway gateway, IConfiguration configuration, ILogger<FroniusController> logger)
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
        /// Returns all Fronius gateway data.
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
        [SwaggerOperation(Tags = new[] { "Fronius API" })]
        [ProducesResponseType(typeof(FroniusGateway), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetFroniusGateway(bool update = false)
        {
            _logger?.LogDebug("GetFroniusGateway()...");

            if (_gateway.IsLocked) return Locked("GetFroniusGateway gateway is locked");

            if (update)
            {
                var status = await _gateway.ReadAllAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(new FroniusInfo()
            {
                Settings = _gateway.Settings,
                IsStartupOk = _gateway.IsStartupOk,
                IsLocked = _gateway.IsLocked,
                Status = _gateway.Status
            });
        }

        /// <summary>
        /// Returns all Fronius related data.
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
        [HttpGet("data")]
        [SwaggerOperation(Tags = new[] { "Fronius API" })]
        [ProducesResponseType(typeof(FroniusData), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetFroniusData(bool update = false)
        {
            _logger?.LogDebug("GetFroniusData()...");

            if (_gateway.IsLocked) return Locked("Fronius is locked");

            if (update)
            {
                var status = await _gateway.ReadAllAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(_gateway.Data);
        }

        /// <summary>
        /// Returns a subset of Fronius data.
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
        [HttpGet("common")]
        [SwaggerOperation(Tags = new[] { "Fronius API" })]
        [ProducesResponseType(typeof(CommonData), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetCommonData(bool update = false)
        {
            _logger?.LogDebug("GetCommonData()...");

            if (_gateway.IsLocked) return Locked("Fronius is locked");

            if (update)
            {
                var status = await _gateway.ReadCommonDataAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(_gateway.CommonData);
        }

        /// <summary>
        /// Returns a subset of Fronius data.
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
        [HttpGet("inverter")]
        [SwaggerOperation(Tags = new[] { "Fronius API" })]
        [ProducesResponseType(typeof(InverterInfo), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetInverterInfo(bool update = false)
        {
            _logger?.LogDebug("GetInverterInfo()...");

            if (_gateway.IsLocked) return Locked("Fronius is locked");

            if (update)
            {
                var status = await _gateway.ReadInverterInfoAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(_gateway.InverterInfo);
        }

        /// <summary>
        /// Returns a subset of Fronius data.
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
        [HttpGet("logger")]
        [SwaggerOperation(Tags = new[] { "Fronius API" })]
        [ProducesResponseType(typeof(LoggerInfo), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetLoggerInfo(bool update = false)
        {
            _logger?.LogDebug("GetLoggerInfo()...");

            if (_gateway.IsLocked) return Locked("Fronius is locked");

            if (update)
            {
                var status = await _gateway.ReadLoggerInfoAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(_gateway.LoggerInfo);
        }

        /// <summary>
        /// Returns a subset of Fronius data.
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
        [HttpGet("minmax")]
        [SwaggerOperation(Tags = new[] { "Fronius API" })]
        [ProducesResponseType(typeof(MinMaxData), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetMinMaxData(bool update = false)
        {
            _logger?.LogDebug("GetMinMaxData()...");

            if (_gateway.IsLocked) return Locked("Fronius is locked");

            if (update)
            {
                var status = await _gateway.ReadMinMaxDataAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(_gateway.MinMaxData);
        }

        /// <summary>
        /// Returns a subset of Fronius data.
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
        [HttpGet("phase")]
        [SwaggerOperation(Tags = new[] { "Fronius API" })]
        [ProducesResponseType(typeof(PhaseData), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetPhaseData(bool update = false)
        {
            _logger?.LogDebug("GetPhaseData()...");

            if (_gateway.IsLocked) return Locked("Fronius is locked");

            if (update)
            {
                var status = await _gateway.ReadPhaseDataAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(_gateway.PhaseData);
        }

        /// <summary>
        /// Returns the Fronius API version data.
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
        [HttpGet("version")]
        [SwaggerOperation(Tags = new[] { "Fronius API" })]
        [ProducesResponseType(typeof(APIVersionData), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetAPIVersionData(bool update = false)
        {
            _logger?.LogDebug("GetAPIVersionData()...");

            if (_gateway.IsLocked) return Locked("Fronius is locked");

            if (update)
            {
                var status = await _gateway.GetAPIVersionAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(_gateway.VersionInfo);
        }

        /// <summary>
        /// Returns a single Fronius property.
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
        [SwaggerOperation(Tags = new[] { "Fronius API" })]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetFroniusData(string name, bool update = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger?.LogDebug($"GetFroniusData() invalid property.");
                return BadRequest($"Property name is invalid.");
            }

            _logger?.LogDebug($"GetFroniusData({name})...");

            if (typeof(FroniusData).IsProperty(name))
            {
                if (update)
                {
                    if (_gateway.IsLocked) return Locked("Fronius is locked");

                    var status = await _gateway.ReadAllAsync();

                    if (!status.IsGood) return StatusCode(status);
                }

                return Ok(_gateway.Data.GetPropertyValue(name));
            }
            else
            {
                _logger?.LogDebug($"GetFroniusData('{name}') property not found.");
                return NotFound($"Property '{name}' not found.");
            }
        }

        /// <summary>
        /// Returns a single CommonData property.
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
        [HttpGet("common/property/{name}")]
        [SwaggerOperation(Tags = new[] { "Fronius API" })]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetFroniusCommonProperty(string name, bool update = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger?.LogDebug($"GetFroniusCommonProperty() invalid property.");
                return BadRequest($"Property name is invalid.");
            }

            _logger?.LogDebug($"GetFroniusCommonProperty({name})...");

            if (typeof(CommonData).IsProperty(name))
            {
                if (update)
                {
                    if (_gateway.IsLocked) return Locked("Fronius gateway is locked");

                    var status = await _gateway.ReadCommonDataAsync();

                    if (!status.IsGood) return StatusCode(status);
                }

                return Ok(_gateway.CommonData.GetPropertyValue(name));
            }
            else
            {
                _logger?.LogDebug($"GetFroniusCommonProperty('{name}') property not found.");
                return NotFound($"Property '{name}' not found.");
            }
        }

        /// <summary>
        /// Returns a single InverterInfo property.
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
        [HttpGet("inverter/property/{name}")]
        [SwaggerOperation(Tags = new[] { "Fronius API" })]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetFroniusInverterProperty(string name, bool update = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger?.LogDebug($"GetFroniusInverterProperty() invalid property.");
                return BadRequest($"Property name is invalid.");
            }

            _logger?.LogDebug($"GetFroniusInverterProperty({name})...");

            if (typeof(InverterInfo).IsProperty(name))
            {
                if (update)
                {
                    if (_gateway.IsLocked) return Locked("Fronius gateway is locked");

                    var status = await _gateway.ReadInverterInfoAsync();

                    if (!status.IsGood) return StatusCode(status);
                }

                return Ok(_gateway.InverterInfo.GetPropertyValue(name));
            }
            else
            {
                _logger?.LogDebug($"GetFroniusInverterProperty('{name}') property not found.");
                return NotFound($"Property '{name}' not found.");
            }
        }

        /// <summary>
        /// Returns a single LoggerInfo property.
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
        [HttpGet("logger/property/{name}")]
        [SwaggerOperation(Tags = new[] { "Fronius API" })]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetFroniusLoggerProperty(string name, bool update = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger?.LogDebug($"GetFroniusLoggerProperty() invalid property.");
                return BadRequest($"Property name is invalid.");
            }

            _logger?.LogDebug($"GetFroniusLoggerProperty({name})...");

            if (typeof(LoggerInfo).IsProperty(name))
            {
                if (update)
                {
                    if (_gateway.IsLocked) return Locked("Fronius gateway is locked");

                    var status = await _gateway.ReadLoggerInfoAsync();

                    if (!status.IsGood) return StatusCode(status);
                }

                return Ok(_gateway.LoggerInfo.GetPropertyValue(name));
            }
            else
            {
                _logger?.LogDebug($"GetFroniusLoggerProperty('{name}') property not found.");
                return NotFound($"Property '{name}' not found.");
            }
        }

        /// <summary>
        /// Returns a single MinMaxData property.
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
        [HttpGet("minmax/property/{name}")]
        [SwaggerOperation(Tags = new[] { "Fronius API" })]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetFroniusMinMaxProperty(string name, bool update = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger?.LogDebug($"GetFroniusMinMaxProperty() invalid property.");
                return BadRequest($"Property name is invalid.");
            }

            _logger?.LogDebug($"GetFroniusMinMaxProperty({name})...");

            if (typeof(MinMaxData).IsProperty(name))
            {
                if (update)
                {
                    if (_gateway.IsLocked) return Locked("Fronius gateway is locked");

                    var status = await _gateway.ReadMinMaxDataAsync();

                    if (!status.IsGood) return StatusCode(status);
                }

                return Ok(_gateway.MinMaxData.GetPropertyValue(name));
            }
            else
            {
                _logger?.LogDebug($"GetFroniusMinMaxProperty('{name}') property not found.");
                return NotFound($"Property '{name}' not found.");
            }
        }

        /// <summary>
        /// Returns a single PhaseData property.
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
        [HttpGet("phase/property/{name}")]
        [SwaggerOperation(Tags = new[] { "Fronius API" })]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetFroniusPhaseProperty(string name, bool update = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger?.LogDebug($"GetFroniusPhaseProperty() invalid property.");
                return BadRequest($"Property name is invalid.");
            }

            _logger?.LogDebug($"GetFroniusPhaseProperty({name})...");

            if (typeof(PhaseData).IsProperty(name))
            {
                if (update)
                {
                    if (_gateway.IsLocked) return Locked("Fronius gateway is locked");

                    var status = await _gateway.ReadPhaseDataAsync();

                    if (!status.IsGood) return StatusCode(status);
                }

                return Ok(_gateway.PhaseData.GetPropertyValue(name));
            }
            else
            {
                _logger?.LogDebug($"GetFroniusPhaseProperty('{name}') property not found.");
                return NotFound($"Property '{name}' not found.");
            }
        }

        /// <summary>
        /// Returns a single VersionInfo property.
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
        [HttpGet("version/property/{name}")]
        [SwaggerOperation(Tags = new[] { "Fronius API" })]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetFroniusVersionProperty(string name, bool update = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger?.LogDebug($"GetFroniusVersionProperty() invalid property.");
                return BadRequest($"Property name is invalid.");
            }

            _logger?.LogDebug($"GetFroniusVersionProperty({name})...");

            if (typeof(APIVersionData).IsProperty(name))
            {
                if (update)
                {
                    if (_gateway.IsLocked) return Locked("Fronius gateway is locked");

                    var status = await _gateway.GetAPIVersionAsync();

                    if (!status.IsGood) return StatusCode(status);
                }

                return Ok(_gateway.VersionInfo.GetPropertyValue(name));
            }
            else
            {
                _logger?.LogDebug($"GetFroniusVersionProperty('{name}') property not found.");
                return NotFound($"Property '{name}' not found.");
            }
        }

        #endregion

        #region PUT Methods

        #endregion

        #region POST Methods

        #endregion

        #region DELETE Methods

        #endregion

        #endregion
    }
}
