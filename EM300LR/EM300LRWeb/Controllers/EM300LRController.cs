// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FroniusController.cs" company="DTV-Online">
//   Copyright(c) 2018 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
// Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace EM300LRWeb.Controllers
{
    #region Using Directives

    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    using Swashbuckle.AspNetCore.Annotations;

    using UtilityLib;
    using EM300LRLib;
    using EM300LRLib.Models;
    using EM300LRWeb.Models;

    #endregion Using Directives

    /// <summary>
    /// The EM300LR controller for reading EM300LR data items.
    /// </summary>
    [ApiController]
    [Route("/")]
    [Produces("application/json")]
    public class EM300LRController : BaseController<AppSettings>
    {
        #region Private Fields

        private readonly EM300LRGateway _gateway;

        #endregion Private Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EM300LRController"/> class.
        /// The parameters provided by dependency injection are used to set private fields.
        /// </summary>
        /// <param name="gateway"></param>
        /// <param name="settings"></param>
        /// <param name="config"></param>
        /// <param name="environment"></param>
        /// <param name="lifetime"></param>
        /// <param name="logger"></param>
        public EM300LRController(EM300LRGateway gateway,
                                 AppSettings settings,
                                 IConfiguration config,
                                 IHostEnvironment environment,
                                 IHostApplicationLifetime lifetime,
                                 ILogger<EM300LRController> logger)
            : base(settings, config, environment, lifetime, logger)
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
        /// Returns all EM300LR gateway data.
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
        [SwaggerOperation(Tags = new[] { "EM300LR API" })]
        [ProducesResponseType(typeof(EM300LRGateway), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetEM300LRGateway(bool update = false)
        {
            _logger?.LogDebug("GetEM300LRGateway()...");

            if (_gateway.IsLocked) return Locked("GetEM300LR gateway is locked");

            if (update)
            {
                var status = await _gateway.ReadAllAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(new EM300LRInfo()
            {
                Settings = _gateway.Settings,
                IsStartupOk = _gateway.IsStartupOk,
                IsLocked = _gateway.IsLocked,
                Status = _gateway.Status
            });
        }

        /// <summary>
        /// Returns all EM300LR related data.
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
        [SwaggerOperation(Tags = new[] { "EM300LR API" })]
        [ProducesResponseType(typeof(EM300LRData), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetEM300LRData(bool update = false)
        {
            _logger?.LogDebug("GetEM300LRData()...");

            if (_gateway.IsLocked) return Locked("EM300LR gateway is locked");

            if (update)
            {
                var status = await _gateway.ReadAllAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(_gateway.Data);
        }

        /// <summary>
        /// Returns a subset of EM300LR data.
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
        [HttpGet("total")]
        [SwaggerOperation(Tags = new[] { "EM300LR API" })]
        [ProducesResponseType(typeof(TotalData), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetTotalData(bool update = false)
        {
            _logger?.LogDebug("GetTotalData()...");

            if (_gateway.IsLocked) return Locked("EM300LR gateway is locked");

            if (update)
            {
                var status = await _gateway.ReadAllAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(_gateway.TotalData);
        }

        /// <summary>
        /// Returns a subset of EM300LR data.
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
        [HttpGet("phase1")]
        [SwaggerOperation(Tags = new[] { "EM300LR API" })]
        [ProducesResponseType(typeof(Phase1Data), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetPhase1Data(bool update = false)
        {
            _logger?.LogDebug("GetPhase1Data()...");

            if (_gateway.IsLocked) return Locked("EM300LR gateway is locked");

            if (update)
            {
                var status = await _gateway.ReadAllAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(_gateway.Phase1Data);
        }

        /// <summary>
        /// Returns a subset of EM300LR data.
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
        [HttpGet("phase2")]
        [SwaggerOperation(Tags = new[] { "EM300LR API" })]
        [ProducesResponseType(typeof(Phase2Data), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetPhase2Data(bool update = false)
        {
            _logger?.LogDebug("GetPhase2Data()...");

            if (_gateway.IsLocked) return Locked("EM300LR gateway is locked");

            if (update)
            {
                var status = await _gateway.ReadAllAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(_gateway.Phase2Data);
        }

        /// <summary>
        /// Returns a subset of EM300LR data.
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
        [HttpGet("phase3")]
        [SwaggerOperation(Tags = new[] { "EM300LR API" })]
        [ProducesResponseType(typeof(Phase1Data), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetPhase3Data(bool update = false)
        {
            _logger?.LogDebug("GetPhase3Data()...");

            if (_gateway.IsLocked) return Locked("EM300LR gateway is locked");

            if (update)
            {
                var status = await _gateway.ReadAllAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(_gateway.Phase3Data);
        }

        /// <summary>
        /// Returns a single EM300LRData property.
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
        [SwaggerOperation(Tags = new[] { "EM300LR API" })]
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
        public async Task<IActionResult> GetEM300LRDataProperty(string name, bool update = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger?.LogDebug($"GetEM300LRDataProperty() invalid property.");
                return BadRequest($"Property name is invalid.");
            }

            _logger?.LogDebug($"GetEM300LRDataProperty({name})...");

            if (typeof(EM300LRData).IsProperty(name))
            {
                if (update)
                {
                    if (_gateway.IsLocked) return Locked("EM300LR gateway is locked");

                    var status = await _gateway.ReadAllAsync();

                    if (!status.IsGood) return StatusCode(status);
                }

                return Ok(_gateway.Data.GetPropertyValue(name));
            }
            else
            {
                _logger?.LogDebug($"GetEM300LRDataProperty('{name}') property not found.");
                return NotFound($"Property '{name}' not found.");
            }
        }

        /// <summary>
        /// Returns a single TotalData property.
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
        [HttpGet("total/property/{name}")]
        [SwaggerOperation(Tags = new[] { "EM300LR API" })]
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
        public async Task<IActionResult> GetEM300LRTotalProperty(string name, bool update = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger?.LogDebug($"GetEM300LRTotalProperty() invalid property.");
                return BadRequest($"Property name is invalid.");
            }

            _logger?.LogDebug($"GetEM300LRTotalProperty({name})...");

            if (typeof(TotalData).IsProperty(name))
            {
                if (update)
                {
                    if (_gateway.IsLocked) return Locked("EM300LR gateway is locked");

                    var status = await _gateway.ReadAllAsync();

                    if (!status.IsGood) return StatusCode(status);
                }

                return Ok(_gateway.TotalData.GetPropertyValue(name));
            }
            else
            {
                _logger?.LogDebug($"GetEM300LRTotalProperty('{name}') property not found.");
                return NotFound($"Property '{name}' not found.");
            }
        }

        /// <summary>
        /// Returns a single Phase1Data property.
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
        [HttpGet("phase1/property/{name}")]
        [SwaggerOperation(Tags = new[] { "EM300LR API" })]
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
        public async Task<IActionResult> GetEM300LRPhase1Property(string name, bool update = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger?.LogDebug($"GetEM300LRPhase1Property() invalid property.");
                return BadRequest($"Property name is invalid.");
            }

            _logger?.LogDebug($"GetEM300LRPhase1Property({name})...");

            if (typeof(Phase1Data).IsProperty(name))
            {
                if (update)
                {
                    if (_gateway.IsLocked) return Locked("EM300LR gateway is locked");

                    var status = await _gateway.ReadAllAsync();

                    if (!status.IsGood) return StatusCode(status);
                }

                return Ok(_gateway.Phase1Data.GetPropertyValue(name));
            }
            else
            {
                _logger?.LogDebug($"GetEM300LRPhase1Property('{name}') property not found.");
                return NotFound($"Property '{name}' not found.");
            }
        }

        /// <summary>
        /// Returns a single Phase2Data property.
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
        [HttpGet("phase2/property/{name}")]
        [SwaggerOperation(Tags = new[] { "EM300LR API" })]
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
        public async Task<IActionResult> GetEM300LRPhase2Property(string name, bool update = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger?.LogDebug($"GetEM300LRPhase2Property() invalid property.");
                return BadRequest($"Property name is invalid.");
            }

            _logger?.LogDebug($"GetEM300LRPhase2Property({name})...");

            if (typeof(Phase2Data).IsProperty(name))
            {
                if (update)
                {
                    if (_gateway.IsLocked) return Locked("EM300LR gateway is locked");

                    var status = await _gateway.ReadAllAsync();

                    if (!status.IsGood) return StatusCode(status);
                }

                return Ok(_gateway.Phase2Data.GetPropertyValue(name));
            }
            else
            {
                _logger?.LogDebug($"GetEM300LRPhase2Property('{name}') property not found.");
                return NotFound($"Property '{name}' not found.");
            }
        }

        /// <summary>
        /// Returns a single Phase3Data property.
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
        [HttpGet("phase3/property/{name}")]
        [SwaggerOperation(Tags = new[] { "EM300LR API" })]
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
        public async Task<IActionResult> GetEM300LRPhase3Property(string name, bool update = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger?.LogDebug($"GetEM300LRPhase3Property() invalid property.");
                return BadRequest($"Property name is invalid.");
            }

            _logger?.LogDebug($"GetEM300LRPhase3Property({name})...");

            if (typeof(Phase3Data).IsProperty(name))
            {
                if (update)
                {
                    if (_gateway.IsLocked) return Locked("EM300LR gateway is locked");

                    var status = await _gateway.ReadAllAsync();

                    if (!status.IsGood) return StatusCode(status);
                }

                return Ok(_gateway.Phase3Data.GetPropertyValue(name));
            }
            else
            {
                _logger?.LogDebug($"GetEM300LRPhase3Property('{name}') property not found.");
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
        
        #endregion REST Methods
    }
}
