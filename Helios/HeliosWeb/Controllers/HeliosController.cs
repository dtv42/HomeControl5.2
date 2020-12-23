// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeliosController.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
// Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace HeliosWeb.Controllers
{
    #region Using Directives

    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using Swashbuckle.AspNetCore.Annotations;

    using UtilityLib;
    using UtilityLib.Webapp;

    using HeliosLib;
    using HeliosLib.Models;

    #endregion

    /// <summary>
    /// The Helios controller for reading Helios data items.
    /// </summary>
    [ApiController]
    [Route("/")]
    [Produces("application/json")]
    public class HeliosController : BaseController
    {
        #region Private Fields

        private readonly HeliosGateway _gateway;

        #endregion Private Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HeliosController"/> class.
        /// The parameters provided by dependency injection are used to set private fields.
        /// </summary>
        /// <param name="gateway">The EM300LR gateway instance.</param>
        /// <param name="configuration">The application configuration instance.</param>
        /// <param name="logger">The logger instance.</param>
        public HeliosController(HeliosGateway gateway, IConfiguration configuration, ILogger<HeliosController> logger)
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
        /// Returns all Helios gateway data.
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
        [SwaggerOperation(Tags = new[] { "Helios API" })]
        [ProducesResponseType(typeof(HeliosGateway), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetHeliosGateway(bool update = false)
        {
            _logger?.LogDebug("GetHeliosGateway()...");

            if (_gateway.IsLocked) return Locked("GetHeliosGateway gateway is locked");

            if (update)
            {
                if (!(await _gateway.LoginAsync())) return StatusCode(_gateway.Status);
            }

            return Ok(new HeliosInfo()
            {
                Settings = _gateway.Settings,
                IsStartupOk = _gateway.IsStartupOk,
                IsLocked = _gateway.IsLocked,
                Status = _gateway.Status
            });
        }

        /// <summary>
        /// Returns all Helios related data.
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
        [SwaggerOperation(Tags = new[] { "Helios API" })]
        [ProducesResponseType(typeof(HeliosData), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetHeliosData(bool update = false)
        {
            _logger?.LogDebug("GetHeliosData()...");

            if (_gateway.IsLocked) return Locked("Helios is locked");

            if (update)
            {
                var status = await _gateway.ReadAllAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(_gateway.Data);
        }

        /// <summary>
        /// Returns a subset of Helios data.
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
        [HttpGet("booster")]
        [SwaggerOperation(Tags = new[] { "Helios API" })]
        [ProducesResponseType(typeof(BoosterData), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetBoosterData(bool update = false)
        {
            _logger?.LogDebug("GetBoosterData()...");

            if (_gateway.IsLocked) return Locked("Helios is locked");

            if (update)
            {
                var status = await _gateway.ReadBoosterDataAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(_gateway.BoosterData);
        }

        /// <summary>
        /// Returns a subset of Helios data.
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
        [HttpGet("device")]
        [SwaggerOperation(Tags = new[] { "Helios API" })]
        [ProducesResponseType(typeof(DeviceData), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetDeviceData(bool update = false)
        {
            _logger?.LogDebug("GetDeviceData()...");

            if (_gateway.IsLocked) return Locked("Helios is locked");

            if (update)
            {
                var status = await _gateway.ReadDeviceDataAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(_gateway.DeviceData);
        }

        /// <summary>
        /// Returns a subset of Helios data.
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
        [HttpGet("display")]
        [SwaggerOperation(Tags = new[] { "Helios API" })]
        [ProducesResponseType(typeof(DisplayData), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetDisplayData(bool update = false)
        {
            _logger?.LogDebug("GetDisplayData()...");

            if (_gateway.IsLocked) return Locked("Helios is locked");

            if (update)
            {
                var status = await _gateway.ReadDisplayDataAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(_gateway.DisplayData);
        }

        /// <summary>
        /// Returns a subset of Helios data.
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
        [HttpGet("error")]
        [SwaggerOperation(Tags = new[] { "Helios API" })]
        [ProducesResponseType(typeof(ErrorData), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetErrorData(bool update = false)
        {
            _logger?.LogDebug("GetErrorData()...");

            if (_gateway.IsLocked) return Locked("Helios is locked");

            if (update)
            {
                var status = await _gateway.ReadErrorDataAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(_gateway.ErrorData);
        }

        /// <summary>
        /// Returns a subset of Helios data.
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
        [HttpGet("fan")]
        [SwaggerOperation(Tags = new[] { "Helios API" })]
        [ProducesResponseType(typeof(FanData), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetFanData(bool update = false)
        {
            _logger?.LogDebug("GetFanData()...");

            if (_gateway.IsLocked) return Locked("Helios is locked");

            if (update)
            {
                var status = await _gateway.ReadFanDataAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(_gateway.FanData);
        }

        /// <summary>
        /// Returns a subset of Helios data.
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
        [HttpGet("heater")]
        [SwaggerOperation(Tags = new[] { "Helios API" })]
        [ProducesResponseType(typeof(HeaterData), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetHeaterData(bool update = false)
        {
            _logger?.LogDebug("GetHeaterData()...");

            if (_gateway.IsLocked) return Locked("Helios is locked");

            if (update)
            {
                var status = await _gateway.ReadHeaterDataAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(_gateway.HeaterData);
        }

        /// <summary>
        /// Returns a subset of Helios data.
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
        [HttpGet("info")]
        [SwaggerOperation(Tags = new[] { "Helios API" })]
        [ProducesResponseType(typeof(InfoData), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetInfoData(bool update = false)
        {
            _logger?.LogDebug("GetInfoData()...");

            if (_gateway.IsLocked) return Locked("Helios is locked");

            if (update)
            {
                var status = await _gateway.ReadInfoDataAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(_gateway.InfoData);
        }

        /// <summary>
        /// Returns a subset of Helios data.
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
        [HttpGet("network")]
        [SwaggerOperation(Tags = new[] { "Helios API" })]
        [ProducesResponseType(typeof(NetworkData), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetNetworkData(bool update = false)
        {
            _logger?.LogDebug("GetNetworkData()...");

            if (_gateway.IsLocked) return Locked("Helios is locked");

            if (update)
            {
                var status = await _gateway.ReadNetworkDataAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(_gateway.NetworkData);
        }

        /// <summary>
        /// Returns a subset of Helios data.
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
        [HttpGet("operation")]
        [SwaggerOperation(Tags = new[] { "Helios API" })]
        [ProducesResponseType(typeof(OperationData), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetOperationData(bool update = false)
        {
            _logger?.LogDebug("GetOperationData()...");

            if (_gateway.IsLocked) return Locked("Helios is locked");

            if (update)
            {
                var status = await _gateway.ReadOperationDataAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(_gateway.OperationData);
        }

        /// <summary>
        /// Returns a subset of Helios data.
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
        [HttpGet("sensor")]
        [SwaggerOperation(Tags = new[] { "Helios API" })]
        [ProducesResponseType(typeof(SensorData), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetSensorData(bool update = false)
        {
            _logger?.LogDebug("GetSensorData()...");

            if (_gateway.IsLocked) return Locked("Helios is locked");

            if (update)
            {
                var status = await _gateway.ReadSensorDataAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(_gateway.SensorData);
        }

        /// <summary>
        /// Returns a subset of Helios data.
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
        [SwaggerOperation(Tags = new[] { "Helios API" })]
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

            if (_gateway.IsLocked) return Locked("Helios is locked");

            if (update)
            {
                var status = await _gateway.ReadSystemDataAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(_gateway.SystemData);
        }

        /// <summary>
        /// Returns a subset of Helios data.
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
        [HttpGet("technical")]
        [SwaggerOperation(Tags = new[] { "Helios API" })]
        [ProducesResponseType(typeof(TechnicalData), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetTechnicalData(bool update = false)
        {
            _logger?.LogDebug("GetTechnicalData()...");

            if (_gateway.IsLocked) return Locked("Helios is locked");

            if (update)
            {
                var status = await _gateway.ReadTechnicalDataAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(_gateway.TechnicalData);
        }

        /// <summary>
        /// Returns a subset of Helios data.
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
        [HttpGet("vacation")]
        [SwaggerOperation(Tags = new[] { "Helios API" })]
        [ProducesResponseType(typeof(VacationData), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetVacationData(bool update = false)
        {
            _logger?.LogDebug("GetVacationData()...");

            if (_gateway.IsLocked) return Locked("Helios is locked");

            if (update)
            {
                var status = await _gateway.ReadVacationDataAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(_gateway.VacationData);
        }

        /// <summary>
        /// Returns a single Helios data property.
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
        [SwaggerOperation(Tags = new[] { "Helios API" })]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetHeliosData(string name, bool update = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger?.LogDebug($"GetHeliosData() invalid property.");
                return BadRequest($"Property name is invalid.");
            }

            _logger?.LogDebug($"GetHeliosData({name})...");

            if (HeliosData.IsProperty(name))
            {
                if (update)
                {
                    if (_gateway.IsLocked) return Locked("Helios is locked");

                    var status = await _gateway.ReadAllAsync();

                    if (!status.IsGood) return StatusCode(status);
                }

                return Ok(_gateway.Data.GetPropertyValue(name));
            }
            else
            {
                _logger?.LogDebug($"GetHeliosData('{name}') property not found.");
                return NotFound($"Property '{name}' not found.");
            }
        }

        /// <summary>
        /// Returns a single Helios property by label.
        /// </summary>
        /// <remarks>The property name is a CamelCase name.</remarks>
        /// <param name="label">The name of the property.</param>
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
        [HttpGet("data/label/{name}")]
        [SwaggerOperation(Tags = new[] { "Helios API" })]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetHeliosDataProperty(string name, bool update = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger?.LogDebug($"GetHeliosDataProperty() invalid label.");
                return BadRequest($"Label name is invalid.");
            }

            _logger?.LogDebug($"GetHeliosDataProperty({name})...");

            if (HeliosData.IsHelios(name))
            {
                if (update)
                {
                    if (_gateway.IsLocked) return Locked("Helios gateway is locked");

                    var status = await _gateway.ReadAllAsync();

                    if (!status.IsGood) return StatusCode(status);
                }

                return Ok(_gateway.Data.GetHeliosValue(name));
            }
            else
            {
                _logger?.LogDebug($"GetHeliosDataProperty('{name}') property not found.");
                return NotFound($"Property with label '{name}' not found.");
            }
        }

        /// <summary>
        /// Returns a single Helios booster data property.
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
        [HttpGet("booster/property/{name}")]
        [SwaggerOperation(Tags = new[] { "Helios API" })]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetBoosterData(string name, bool update = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger?.LogDebug($"GetBoosterData() invalid property.");
                return BadRequest($"Property name is invalid.");
            }

            _logger?.LogDebug($"GetHeliosData({name})...");

            if (HeliosData.IsProperty(name))
            {
                if (update)
                {
                    if (_gateway.IsLocked) return Locked("Helios is locked");

                    var status = await _gateway.ReadBoosterDataAsync();

                    if (!status.IsGood) return StatusCode(status);
                }

                return Ok(_gateway.BoosterData.GetPropertyValue(name));
            }
            else
            {
                _logger?.LogDebug($"GetBoosterData('{name}') property not found.");
                return NotFound($"Property '{name}' not found.");
            }
        }

        /// <summary>
        /// Returns a single Helios device data property.
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
        [HttpGet("device/property/{name}")]
        [SwaggerOperation(Tags = new[] { "Helios API" })]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetDeviceData(string name, bool update = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger?.LogDebug($"GetDeviceData() invalid property.");
                return BadRequest($"Property name is invalid.");
            }

            _logger?.LogDebug($"GetDeviceData({name})...");

            if (HeliosData.IsProperty(name))
            {
                if (update)
                {
                    if (_gateway.IsLocked) return Locked("Helios is locked");

                    var status = await _gateway.ReadDeviceDataAsync();

                    if (!status.IsGood) return StatusCode(status);
                }

                return Ok(_gateway.DeviceData.GetPropertyValue(name));
            }
            else
            {
                _logger?.LogDebug($"GetDeviceData('{name}') property not found.");
                return NotFound($"Property '{name}' not found.");
            }
        }

        /// <summary>
        /// Returns a single Helios display data property.
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
        [HttpGet("display/property/{name}")]
        [SwaggerOperation(Tags = new[] { "Helios API" })]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetDisplayData(string name, bool update = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger?.LogDebug($"GetDisplayData() invalid property.");
                return BadRequest($"Property name is invalid.");
            }

            _logger?.LogDebug($"GetDisplayData({name})...");

            if (HeliosData.IsProperty(name))
            {
                if (update)
                {
                    if (_gateway.IsLocked) return Locked("Helios is locked");

                    var status = await _gateway.ReadDisplayDataAsync();

                    if (!status.IsGood) return StatusCode(status);
                }

                return Ok(_gateway.DisplayData.GetPropertyValue(name));
            }
            else
            {
                _logger?.LogDebug($"GetDisplayData('{name}') property not found.");
                return NotFound($"Property '{name}' not found.");
            }
        }

        /// <summary>
        /// Returns a single Helios error data property.
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
        [HttpGet("error/property/{name}")]
        [SwaggerOperation(Tags = new[] { "Helios API" })]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetErrorData(string name, bool update = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger?.LogDebug($"GetErrorData() invalid property.");
                return BadRequest($"Property name is invalid.");
            }

            _logger?.LogDebug($"GetErrorData({name})...");

            if (HeliosData.IsProperty(name))
            {
                if (update)
                {
                    if (_gateway.IsLocked) return Locked("Helios is locked");

                    var status = await _gateway.ReadErrorDataAsync();

                    if (!status.IsGood) return StatusCode(status);
                }

                return Ok(_gateway.ErrorData.GetPropertyValue(name));
            }
            else
            {
                _logger?.LogDebug($"GetErrorData('{name}') property not found.");
                return NotFound($"Property '{name}' not found.");
            }
        }

        /// <summary>
        /// Returns a single Helios fan data property.
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
        [HttpGet("fan/property/{name}")]
        [SwaggerOperation(Tags = new[] { "Helios API" })]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetFanData(string name, bool update = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger?.LogDebug($"GetFanData() invalid property.");
                return BadRequest($"Property name is invalid.");
            }

            _logger?.LogDebug($"GetFanData({name})...");

            if (HeliosData.IsProperty(name))
            {
                if (update)
                {
                    if (_gateway.IsLocked) return Locked("Helios is locked");

                    var status = await _gateway.ReadFanDataAsync();

                    if (!status.IsGood) return StatusCode(status);
                }

                return Ok(_gateway.FanData.GetPropertyValue(name));
            }
            else
            {
                _logger?.LogDebug($"GetFanData('{name}') property not found.");
                return NotFound($"Property '{name}' not found.");
            }
        }

        /// <summary>
        /// Returns a single Helios heater data property.
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
        [HttpGet("heater/property/{name}")]
        [SwaggerOperation(Tags = new[] { "Helios API" })]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetHeaterData(string name, bool update = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger?.LogDebug($"GetHeaterData() invalid property.");
                return BadRequest($"Property name is invalid.");
            }

            _logger?.LogDebug($"GetHeaterData({name})...");

            if (HeliosData.IsProperty(name))
            {
                if (update)
                {
                    if (_gateway.IsLocked) return Locked("Helios is locked");

                    var status = await _gateway.ReadHeaterDataAsync();

                    if (!status.IsGood) return StatusCode(status);
                }

                return Ok(_gateway.HeaterData.GetPropertyValue(name));
            }
            else
            {
                _logger?.LogDebug($"GetHeaterData('{name}') property not found.");
                return NotFound($"Property '{name}' not found.");
            }
        }

        /// <summary>
        /// Returns a single Helios info data property.
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
        [HttpGet("info/property/{name}")]
        [SwaggerOperation(Tags = new[] { "Helios API" })]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetInfoData(string name, bool update = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger?.LogDebug($"GetInfoData() invalid property.");
                return BadRequest($"Property name is invalid.");
            }

            _logger?.LogDebug($"GetInfoData({name})...");

            if (HeliosData.IsProperty(name))
            {
                if (update)
                {
                    if (_gateway.IsLocked) return Locked("Helios is locked");

                    var status = await _gateway.ReadInfoDataAsync();

                    if (!status.IsGood) return StatusCode(status);
                }

                return Ok(_gateway.InfoData.GetPropertyValue(name));
            }
            else
            {
                _logger?.LogDebug($"GetInfoData('{name}') property not found.");
                return NotFound($"Property '{name}' not found.");
            }
        }

        /// <summary>
        /// Returns a single Helios network data property.
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
        [HttpGet("network/property/{name}")]
        [SwaggerOperation(Tags = new[] { "Helios API" })]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetNetworkData(string name, bool update = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger?.LogDebug($"GetNetworkData() invalid property.");
                return BadRequest($"Property name is invalid.");
            }

            _logger?.LogDebug($"GetNetworkData({name})...");

            if (HeliosData.IsProperty(name))
            {
                if (update)
                {
                    if (_gateway.IsLocked) return Locked("Helios is locked");

                    var status = await _gateway.ReadNetworkDataAsync();

                    if (!status.IsGood) return StatusCode(status);
                }

                return Ok(_gateway.NetworkData.GetPropertyValue(name));
            }
            else
            {
                _logger?.LogDebug($"GetNetworkData('{name}') property not found.");
                return NotFound($"Property '{name}' not found.");
            }
        }

        /// <summary>
        /// Returns a single Helios operation data property.
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
        [HttpGet("operation/property/{name}")]
        [SwaggerOperation(Tags = new[] { "Helios API" })]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetOperationData(string name, bool update = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger?.LogDebug($"GetOperationData() invalid property.");
                return BadRequest($"Property name is invalid.");
            }

            _logger?.LogDebug($"GetOperationData({name})...");

            if (HeliosData.IsProperty(name))
            {
                if (update)
                {
                    if (_gateway.IsLocked) return Locked("Helios is locked");

                    var status = await _gateway.ReadOperationDataAsync();

                    if (!status.IsGood) return StatusCode(status);
                }

                return Ok(_gateway.OperationData.GetPropertyValue(name));
            }
            else
            {
                _logger?.LogDebug($"GetOperationData('{name}') property not found.");
                return NotFound($"Property '{name}' not found.");
            }
        }

        /// <summary>
        /// Returns a single Helios sensor data property.
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
        [HttpGet("sensor/property/{name}")]
        [SwaggerOperation(Tags = new[] { "Helios API" })]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetSensorData(string name, bool update = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger?.LogDebug($"GetSensorData() invalid property.");
                return BadRequest($"Property name is invalid.");
            }

            _logger?.LogDebug($"GetSensorData({name})...");

            if (HeliosData.IsProperty(name))
            {
                if (update)
                {
                    if (_gateway.IsLocked) return Locked("Helios is locked");

                    var status = await _gateway.ReadSensorDataAsync();

                    if (!status.IsGood) return StatusCode(status);
                }

                return Ok(_gateway.SensorData.GetPropertyValue(name));
            }
            else
            {
                _logger?.LogDebug($"GetSensorData('{name}') property not found.");
                return NotFound($"Property '{name}' not found.");
            }
        }

        /// <summary>
        /// Returns a single Helios system data property.
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
        [SwaggerOperation(Tags = new[] { "Helios API" })]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetSystemData(string name, bool update = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger?.LogDebug($"GetSystemData() invalid property.");
                return BadRequest($"Property name is invalid.");
            }

            _logger?.LogDebug($"GetSystemData({name})...");

            if (HeliosData.IsProperty(name))
            {
                if (update)
                {
                    if (_gateway.IsLocked) return Locked("Helios is locked");

                    var status = await _gateway.ReadSystemDataAsync();

                    if (!status.IsGood) return StatusCode(status);
                }

                return Ok(_gateway.SystemData.GetPropertyValue(name));
            }
            else
            {
                _logger?.LogDebug($"GetSystemData('{name}') property not found.");
                return NotFound($"Property '{name}' not found.");
            }
        }

        /// <summary>
        /// Returns a single Helios technical data property.
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
        [HttpGet("technical/property/{name}")]
        [SwaggerOperation(Tags = new[] { "Helios API" })]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetTechnicalData(string name, bool update = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger?.LogDebug($"GetTechnicalData() invalid property.");
                return BadRequest($"Property name is invalid.");
            }

            _logger?.LogDebug($"GetTechnicalData({name})...");

            if (HeliosData.IsProperty(name))
            {
                if (update)
                {
                    if (_gateway.IsLocked) return Locked("Helios is locked");

                    var status = await _gateway.ReadTechnicalDataAsync();

                    if (!status.IsGood) return StatusCode(status);
                }

                return Ok(_gateway.TechnicalData.GetPropertyValue(name));
            }
            else
            {
                _logger?.LogDebug($"GetTechnicalData('{name}') property not found.");
                return NotFound($"Property '{name}' not found.");
            }
        }

        /// <summary>
        /// Returns a single Helios vacation data property.
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
        [HttpGet("vacation/property/{name}")]
        [SwaggerOperation(Tags = new[] { "Helios API" })]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetVacationData(string name, bool update = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger?.LogDebug($"GetVacationData() invalid property.");
                return BadRequest($"Property name is invalid.");
            }

            _logger?.LogDebug($"GetVacationData({name})...");

            if (HeliosData.IsProperty(name))
            {
                if (update)
                {
                    if (_gateway.IsLocked) return Locked("Helios is locked");

                    var status = await _gateway.ReadVacationDataAsync();

                    if (!status.IsGood) return StatusCode(status);
                }

                return Ok(_gateway.VacationData.GetPropertyValue(name));
            }
            else
            {
                _logger?.LogDebug($"GetVacationData('{name}') property not found.");
                return NotFound($"Property '{name}' not found.");
            }
        }

        #endregion

        #region PUT Methods

        /// <summary>
        /// Sets the fan level of the Helios ventilation unit.
        /// </summary>
        /// <param name="level">The ventilation value</param>
        /// <returns>The action method result</returns>
        /// <response code="202">Return indicates accepted.</response>
        /// <response code="400">Return indicates BadRequest.</response>
        /// <response code="404">Return indicates NotFound.</response>
        /// <response code="409">Return indicates Conflict.</response>
        /// <response code="500">Return indicates InternalError.</response>
        /// <response code="502">Return indicates BadGateway.</response>
        /// <response code="503">Return indicates Unavailable.</response>
        [HttpPut("fan/level")]
        [SwaggerOperation(Tags = new[] { "Helios API" })]
        [ProducesResponseType(202)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public IActionResult SetFanLevel([FromBody, Required, Range(0, 4)] FanLevels level)
        {
            _logger.LogInformation($"SetFanLevel({level})");

            if (!_gateway.IsStartupOk) return StatusCode(_gateway.Status);

            if (_gateway.SetFanLevel(level))
                return Accepted();
            else
                return StatusCode(_gateway.Status);
        }

        /// <summary>
        /// Sets the operation mode of the Helios ventilation unit.
        /// </summary>
        /// <param name="mode">The operation mode</param>
        /// <returns>The action method result</returns>
        /// <response code="202">Return indicates accepted.</response>
        /// <response code="400">Return indicates BadRequest.</response>
        /// <response code="404">Return indicates NotFound.</response>
        /// <response code="409">Return indicates Conflict.</response>
        /// <response code="500">Return indicates InternalError.</response>
        /// <response code="502">Return indicates BadGateway.</response>
        /// <response code="503">Return indicates Unavailable.</response>
        [HttpPut("operation")]
        [SwaggerOperation(Tags = new[] { "Helios API" })]
        [ProducesResponseType(202)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public IActionResult SetOperationMode([FromBody, Required, Range(0, 1)] OperationModes mode)
        {
            _logger.LogInformation($"SetOperationMode({mode})");

            if (!_gateway.IsStartupOk) return StatusCode(_gateway.Status);

            if (_gateway.SetOperationMode(mode))
                return Accepted();
            else
                return StatusCode(_gateway.Status);
        }

        /// <summary>
        /// Sets the operation mode of the Helios ventilation unit to Automatic.
        /// </summary>
        /// <returns>The action method result</returns>
        /// <response code="202">Return indicates accepted.</response>
        /// <response code="400">Return indicates BadRequest.</response>
        /// <response code="404">Return indicates NotFound.</response>
        /// <response code="409">Return indicates Conflict.</response>
        /// <response code="500">Return indicates InternalError.</response>
        /// <response code="502">Return indicates BadGateway.</response>
        /// <response code="503">Return indicates Unavailable.</response>
        [HttpPut("operation/auto")]
        [SwaggerOperation(Tags = new[] { "Helios API" })]
        [ProducesResponseType(202)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public IActionResult SetOperationAuto()
        {
            _logger.LogInformation($"SetOperationAuto()");

            if (!_gateway.IsStartupOk) return StatusCode(_gateway.Status);

            if (_gateway.SetOperationMode(OperationModes.Automatic))
                return Accepted();
            else
                return StatusCode(_gateway.Status);
        }

        /// <summary>
        /// Sets the operation mode of the Helios ventilation unit to Manual.
        /// </summary>
        /// <returns>The action method result</returns>
        /// <response code="202">Return indicates accepted.</response>
        /// <response code="400">Return indicates BadRequest.</response>
        /// <response code="404">Return indicates NotFound.</response>
        /// <response code="409">Return indicates Conflict.</response>
        /// <response code="500">Return indicates InternalError.</response>
        /// <response code="502">Return indicates BadGateway.</response>
        /// <response code="503">Return indicates Unavailable.</response>
        [HttpPut("operation/manual")]
        [SwaggerOperation(Tags = new[] { "Helios API" })]
        [ProducesResponseType(202)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public IActionResult SetOperationManual()
        {
            _logger.LogInformation($"SetOperationManual()");

            if (!_gateway.IsStartupOk) return StatusCode(_gateway.Status);

            if (_gateway.SetOperationMode(OperationModes.Manual))
                return Accepted();
            else
                return StatusCode(_gateway.Status);
        }

        /// <summary>
        /// Sets the booster mode of the Helios ventilation unit.
        /// </summary>
        /// <param name="ventilation">The ventilation mode parameters</param>
        /// <returns>The action method result</returns>
        /// <response code="202">Return indicates accepted.</response>
        /// <response code="400">Return indicates BadRequest.</response>
        /// <response code="404">Return indicates NotFound.</response>
        /// <response code="409">Return indicates Conflict.</response>
        /// <response code="500">Return indicates InternalError.</response>
        /// <response code="502">Return indicates BadGateway.</response>
        /// <response code="503">Return indicates Unavailable.</response>
        [HttpPut("booster")]
        [SwaggerOperation(Tags = new[] { "Helios API" })]
        [ProducesResponseType(202)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public IActionResult SetBooster([FromBody, Required] VentilationData ventilation)
        {
            _logger.LogInformation($"SetBoosterMode({ventilation.Mode}, {ventilation.Level}, {ventilation.Duration})");

            if (!_gateway.IsStartupOk) return StatusCode(_gateway.Status);

            if (_gateway.SetBooster(ventilation))
                return Accepted();
            else
                return StatusCode(_gateway.Status);
        }

        /// <summary>
        /// Sets the booster mode ventilation level of the Helios ventilation unit.
        /// </summary>
        /// <param name="level">The ventilation level</param>
        /// <returns>The action method result</returns>
        /// <response code="202">Return indicates accepted.</response>
        /// <response code="400">Return indicates BadRequest.</response>
        /// <response code="404">Return indicates NotFound.</response>
        /// <response code="409">Return indicates Conflict.</response>
        /// <response code="500">Return indicates InternalError.</response>
        /// <response code="502">Return indicates BadGateway.</response>
        /// <response code="503">Return indicates Unavailable.</response>
        [HttpPut("booster/level")]
        [SwaggerOperation(Tags = new[] { "Helios API" })]
        [ProducesResponseType(202)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public IActionResult SetBoosterLevel([FromBody, Required, Range(0, 4)] FanLevels level)
        {
            _logger.LogInformation($"SetBoosterLevel({level})");

            if (!_gateway.IsStartupOk) return StatusCode(_gateway.Status);

            if (_gateway.SetBoosterLevel(level))
                return Accepted();
            else
                return StatusCode(_gateway.Status);
        }

        /// <summary>
        /// Sets the booster mode duration of the Helios ventilation unit.
        /// </summary>
        /// <param name="duration">The booster mode duration</param>
        /// <returns>The action method result</returns>
        /// <response code="202">Return indicates accepted.</response>
        /// <response code="400">Return indicates BadRequest.</response>
        /// <response code="404">Return indicates NotFound.</response>
        /// <response code="409">Return indicates Conflict.</response>
        /// <response code="500">Return indicates InternalError.</response>
        /// <response code="502">Return indicates BadGateway.</response>
        /// <response code="503">Return indicates Unavailable.</response>
        [HttpPut("booster/duration")]
        [SwaggerOperation(Tags = new[] { "Helios API" })]
        [ProducesResponseType(202)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public IActionResult SetBoosterDuration([FromBody, Required, Range(5, 180)] int duration)
        {
            _logger.LogInformation($"SetBoosterDuration({duration})");

            if (!_gateway.IsStartupOk) return StatusCode(_gateway.Status);

            if (_gateway.SetBoosterDuration(duration))
                return Accepted();
            else
                return StatusCode(_gateway.Status);
        }

        /// <summary>
        /// Activate the booster mode of the Helios ventilation unit.
        /// </summary>
        /// <returns>The action method result</returns>
        /// <response code="202">Return indicates accepted.</response>
        /// <response code="400">Return indicates BadRequest.</response>
        /// <response code="404">Return indicates NotFound.</response>
        /// <response code="409">Return indicates Conflict.</response>
        /// <response code="500">Return indicates InternalError.</response>
        /// <response code="502">Return indicates BadGateway.</response>
        /// <response code="503">Return indicates Unavailable.</response>
        [HttpPut("booster/on")]
        [SwaggerOperation(Tags = new[] { "Helios API" })]
        [ProducesResponseType(202)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public IActionResult SetBoosterModeOn()
        {
            _logger.LogInformation($"SetBoosterModeOn()");

            if (!_gateway.IsStartupOk) return StatusCode(_gateway.Status);

            if (_gateway.SetBoosterMode(true))
                return Accepted();
            else
                return StatusCode(_gateway.Status);
        }

        /// <summary>
        /// Deactivate the booster mode of the Helios ventilation unit.
        /// </summary>
        /// <returns>The action method result</returns>
        /// <response code="202">Return indicates accepted.</response>
        /// <response code="400">Return indicates BadRequest.</response>
        /// <response code="404">Return indicates NotFound.</response>
        /// <response code="409">Return indicates Conflict.</response>
        /// <response code="500">Return indicates InternalError.</response>
        /// <response code="502">Return indicates BadGateway.</response>
        /// <response code="503">Return indicates Unavailable.</response>
        [HttpPut("booster/off")]
        [SwaggerOperation(Tags = new[] { "Helios API" })]
        [ProducesResponseType(202)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public IActionResult SetBoosterModeOff()
        {
            _logger.LogInformation($"SetBoosterModeOff()");

            if (!_gateway.IsStartupOk) return StatusCode(_gateway.Status);

            if (_gateway.SetBoosterMode(false))
                return Accepted();
            else
                return StatusCode(_gateway.Status);
        }

        /// <summary>
        /// Sets the standby mode of the Helios ventilation unit.
        /// </summary>
        /// <param name="ventilation">The ventilation mode parameters</param>
        /// <returns>The action method result</returns>
        /// <response code="202">Return indicates accepted.</response>
        /// <response code="400">Return indicates BadRequest.</response>
        /// <response code="404">Return indicates NotFound.</response>
        /// <response code="409">Return indicates Conflict.</response>
        /// <response code="500">Return indicates InternalError.</response>
        /// <response code="502">Return indicates BadGateway.</response>
        /// <response code="503">Return indicates Unavailable.</response>
        [HttpPut("standby")]
        [SwaggerOperation(Tags = new[] { "Helios API" })]
        [ProducesResponseType(202)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public IActionResult SetStandby([FromBody] VentilationData ventilation)
        {
            _logger.LogInformation($"SetStandbyMode({ventilation.Mode}, {ventilation.Level}, {ventilation.Duration})");

            if (!_gateway.IsStartupOk) return StatusCode(_gateway.Status);

            if (_gateway.SetStandby(ventilation))
                return Accepted();
            else
                return StatusCode(_gateway.Status);
        }

        /// <summary>
        /// Sets the standby mode ventilation level of the Helios ventilation unit.
        /// </summary>
        /// <param name="level">The ventilation level</param>
        /// <returns>The action method result</returns>
        /// <response code="202">Return indicates accepted.</response>
        /// <response code="400">Return indicates BadRequest.</response>
        /// <response code="404">Return indicates NotFound.</response>
        /// <response code="409">Return indicates Conflict.</response>
        /// <response code="500">Return indicates InternalError.</response>
        /// <response code="502">Return indicates BadGateway.</response>
        /// <response code="503">Return indicates Unavailable.</response>
        [HttpPut("standby/level")]
        [SwaggerOperation(Tags = new[] { "Helios API" })]
        [ProducesResponseType(202)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public IActionResult SetStandbyLevel([FromBody, Required, Range(0, 4)] FanLevels level)
        {
            _logger.LogInformation($"SetBoosterLevel({level})");

            if (!_gateway.IsStartupOk) return StatusCode(_gateway.Status);

            if (_gateway.SetStandbyLevel(level))
                return Accepted();
            else
                return StatusCode(_gateway.Status);
        }

        /// <summary>
        /// Sets the standby mode duration of the Helios ventilation unit.
        /// </summary>
        /// <param name="duration">The booster mode duration</param>
        /// <returns>The action method result</returns>
        /// <response code="202">Return indicates accepted.</response>
        /// <response code="400">Return indicates BadRequest.</response>
        /// <response code="404">Return indicates NotFound.</response>
        /// <response code="409">Return indicates Conflict.</response>
        /// <response code="500">Return indicates InternalError.</response>
        /// <response code="502">Return indicates BadGateway.</response>
        /// <response code="503">Return indicates Unavailable.</response>
        [HttpPut("standby/duration")]
        [SwaggerOperation(Tags = new[] { "Helios API" })]
        [ProducesResponseType(202)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public IActionResult SetStandbyDuration([FromBody, Required, Range(5, 180)] int duration)
        {
            _logger.LogInformation($"SetBoosterDuration({duration})");

            if (!_gateway.IsStartupOk) return StatusCode(_gateway.Status);

            if (_gateway.SetStandbyDuration(duration))
                return Accepted();
            else
                return StatusCode(_gateway.Status);
        }

        /// <summary>
        /// Activate the standby mode of the Helios ventilation unit.
        /// </summary>
        /// <returns>The action method result</returns>
        /// <response code="202">Return indicates accepted.</response>
        /// <response code="400">Return indicates BadRequest.</response>
        /// <response code="404">Return indicates NotFound.</response>
        /// <response code="409">Return indicates Conflict.</response>
        /// <response code="500">Return indicates InternalError.</response>
        /// <response code="502">Return indicates BadGateway.</response>
        /// <response code="503">Return indicates Unavailable.</response>
        [HttpPut("standby/on")]
        [SwaggerOperation(Tags = new[] { "Helios API" })]
        [ProducesResponseType(202)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public IActionResult SetStandbyModeOn()
        {
            _logger.LogInformation($"SetStandbyModeOn()");

            if (!_gateway.IsStartupOk) return StatusCode(_gateway.Status);

            if (_gateway.SetBoosterMode(true))
                return Accepted();
            else
                return StatusCode(_gateway.Status);
        }

        /// <summary>
        /// Deactivate the standby mode of the Helios ventilation unit.
        /// </summary>
        /// <returns>The action method result</returns>
        /// <response code="202">Return indicates accepted.</response>
        /// <response code="400">Return indicates BadRequest.</response>
        /// <response code="404">Return indicates NotFound.</response>
        /// <response code="409">Return indicates Conflict.</response>
        /// <response code="500">Return indicates InternalError.</response>
        /// <response code="502">Return indicates BadGateway.</response>
        /// <response code="503">Return indicates Unavailable.</response>
        [HttpPut("standby/off")]
        [SwaggerOperation(Tags = new[] { "Helios API" })]
        [ProducesResponseType(202)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public IActionResult SetStandbyModeOff()
        {
            _logger.LogInformation($"SetStandbyModeOff()");

            if (!_gateway.IsStartupOk) return StatusCode(_gateway.Status);

            if (_gateway.SetBoosterMode(false))
                return Accepted();
            else
                return StatusCode(_gateway.Status);
        }

        #endregion

        #region POST Methods

        #endregion

        #region DELETE Methods

        #endregion

        #endregion
    }
}
