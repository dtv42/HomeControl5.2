// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WallboxController.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>26-4-2020 20:23</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace WallboxWeb.Controllers
{
    using System.Collections.Generic;
    #region Using Directives

    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;

    using Swashbuckle.AspNetCore.Annotations;

    using UtilityLib;
    using UtilityLib.Webapp;

    using WallboxLib;
    using WallboxLib.Models;
    using WallboxWeb.Models;

    #endregion Using Directives

    /// <summary>
    /// The Wallbox controller for reading Wallbox data items.
    /// </summary>
    [ApiController]
    [Route("/")]
    [Produces("application/json")]
    public class WallboxController : BaseController
    {
        #region Private Fields

        private readonly WallboxGateway _gateway;

        #endregion Private Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WallboxController"/> class.
        /// The parameters provided by dependency injection are used to set private fields.
        /// </summary>
        /// <param name="gateway">The EM300LR gateway instance.</param>
        /// <param name="configuration">The application configuration instance.</param>
        /// <param name="logger">The logger instance.</param>
        public WallboxController(WallboxGateway gateway, IConfiguration configuration, ILogger<WallboxController> logger)
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
        /// Returns all Wallbox gateway data.
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
        [SwaggerOperation(Tags = new[] { "Wallbox API" })]
        [ProducesResponseType(typeof(WallboxGateway), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetWallboxGateway(bool update = false)
        {
            _logger?.LogDebug("GetWallboxGateway()...");

            if (_gateway.IsLocked) return Locked("GetWallbox gateway is locked");

            if (update)
            {
                var status = await _gateway.ReadAllAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(new WallboxInfo()
            {
                Settings = _gateway.Settings,
                IsStartupOk = _gateway.IsStartupOk,
                IsLocked = _gateway.IsLocked,
                Status = _gateway.Status
            });
        }

        /// <summary>
        /// Returns all Wallbox related data.
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
        [SwaggerOperation(Tags = new[] { "Wallbox API" })]
        [ProducesResponseType(typeof(WallboxData), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetWallboxData(bool update = false)
        {
            _logger?.LogDebug("GetWallboxData()...");

            if (_gateway.IsLocked) return Locked("Wallbox gateway is locked");

            if (update)
            {
                var status = await _gateway.ReadAllAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(_gateway.Data);
        }

        /// <summary>
        /// Returns a subset of Wallbox data.
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
        [HttpGet("report1")]
        [SwaggerOperation(Tags = new[] { "Wallbox API" })]
        [ProducesResponseType(typeof(Report1Data), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetReport1Data(bool update = false)
        {
            _logger?.LogDebug("GetReport1Data()...");

            if (_gateway.IsLocked) return Locked("Wallbox gateway is locked");

            if (update)
            {
                var status = await _gateway.ReadReport1Async();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(_gateway.Report1);
        }

        /// <summary>
        /// Returns a subset of Wallbox data.
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
        [HttpGet("report2")]
        [SwaggerOperation(Tags = new[] { "Wallbox API" })]
        [ProducesResponseType(typeof(Report2Data), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetReport2Data(bool update = false)
        {
            _logger?.LogDebug("GetReport2Data()...");

            if (_gateway.IsLocked) return Locked("Wallbox gateway is locked");

            if (update)
            {
                var status = await _gateway.ReadReport2Async();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(_gateway.Report2);
        }

        /// <summary>
        /// Returns a subset of Wallbox data.
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
        [HttpGet("report3")]
        [SwaggerOperation(Tags = new[] { "Wallbox API" })]
        [ProducesResponseType(typeof(Report3Data), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetReport3Data(bool update = false)
        {
            _logger?.LogDebug("GetReport3Data()...");

            if (_gateway.IsLocked) return Locked("Wallbox gateway is locked");

            if (update)
            {
                var status = await _gateway.ReadReport3Async();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(_gateway.Report3);
        }

        /// <summary>
        /// Returns a subset of Wallbox data.
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
        [HttpGet("report100")]
        [SwaggerOperation(Tags = new[] { "Wallbox API" })]
        [ProducesResponseType(typeof(ReportsData), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetReport100Data(bool update = false)
        {
            _logger?.LogDebug("GetReport100Data()...");

            if (_gateway.IsLocked) return Locked("Wallbox gateway is locked");

            if (update)
            {
                var status = await _gateway.ReadReport100Async();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(_gateway.Report100);
        }

        /// <summary>
        /// Returns a list of Wallbox charging reports.
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
        [HttpGet("reports")]
        [SwaggerOperation(Tags = new[] { "Wallbox API" })]
        [ProducesResponseType(typeof(List<ReportsData>), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetReportsData(bool update = false)
        {
            _logger?.LogDebug("GetReportsData()...");

            if (_gateway.IsLocked) return Locked("Wallbox gateway is locked");

            if (update)
            {
                var status = await _gateway.ReadReportsAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(_gateway.Reports);
        }

        /// <summary>
        /// Returns a single Wallbox charging report.
        /// </summary>
        /// <param name="id">The report ID (101 - 130).</param>
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
        [HttpGet("reports/{id}")]
        [SwaggerOperation(Tags = new[] { "Wallbox API" })]
        [ProducesResponseType(typeof(ReportsData), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetReportData(int id, bool update = false)
        {
            _logger?.LogDebug($"GetReportData({id})...");

            if (!WallboxGateway.IsReportIDOk(id))
            {
                _logger?.LogDebug($"GetReportData() invalid report ID.");
                return BadRequest($"Report ID is invalid.");
            }

            if (_gateway.IsLocked) return Locked("Wallbox gateway is locked");

            if (update)
            {
                var status = await _gateway.ReadReportsAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            var index = id - WallboxGateway.REPORTS_ID - 1;
            return Ok(_gateway.Reports[index]);
        }

        /// <summary>
        /// Returns Wallbox info related data.
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
        [SwaggerOperation(Tags = new[] { "Wallbox API" })]
        [ProducesResponseType(typeof(WallboxData), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetWallboxInfo(bool update = false)
        {
            _logger?.LogDebug("GetWallboxInfo()...");

            if (_gateway.IsLocked) return Locked("Wallbox gateway is locked");

            if (update)
            {
                var status = await _gateway.ReadInfoAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(_gateway.Info);
        }

        /// <summary>
        /// Returns a single Wallbox report 1 property.
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
        [HttpGet("report1/property/{name}")]
        [SwaggerOperation(Tags = new[] { "Wallbox API" })]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetReport1Property(string name, bool update = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger?.LogDebug($"GetReport1Property() invalid property.");
                return BadRequest($"Property name is invalid.");
            }

            _logger?.LogDebug($"GetReport1Property({name})...");

            if (typeof(Report1Data).IsProperty(name))
            {
                if (update)
                {
                    if (_gateway.IsLocked) return Locked("Wallbox gateway is locked");

                    var status = await _gateway.ReadReport1Async();

                    if (!status.IsGood) return StatusCode(status);
                }

                return Ok(_gateway.Report1.GetPropertyValue(name));
            }
            else
            {
                _logger?.LogDebug($"GetReport1Property('{name}') property not found.");
                return NotFound($"Property '{name}' not found.");
            }
        }

        /// <summary>
        /// Returns a single Wallbox report 2 property.
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
        [HttpGet("report2/property/{name}")]
        [SwaggerOperation(Tags = new[] { "Wallbox API" })]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetReport2Property(string name, bool update = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger?.LogDebug($"GetReport2Property() invalid property.");
                return BadRequest($"Property name is invalid.");
            }

            _logger?.LogDebug($"GetReport2Property({name})...");

            if (typeof(Report2Data).IsProperty(name))
            {
                if (update)
                {
                    if (_gateway.IsLocked) return Locked("Wallbox gateway is locked");

                    var status = await _gateway.ReadReport2Async();

                    if (!status.IsGood) return StatusCode(status);
                }

                return Ok(_gateway.Report2.GetPropertyValue(name));
            }
            else
            {
                _logger?.LogDebug($"GetReport2Property('{name}') property not found.");
                return NotFound($"Property '{name}' not found.");
            }
        }

        /// <summary>
        /// Returns a single Wallbox report 3 property.
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
        [HttpGet("report3/property/{name}")]
        [SwaggerOperation(Tags = new[] { "Wallbox API" })]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetReport3Property(string name, bool update = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger?.LogDebug($"GetReport3Property() invalid property.");
                return BadRequest($"Property name is invalid.");
            }

            _logger?.LogDebug($"GetReport3Property({name})...");

            if (typeof(Report3Data).IsProperty(name))
            {
                if (update)
                {
                    if (_gateway.IsLocked) return Locked("Wallbox gateway is locked");

                    var status = await _gateway.ReadReport3Async();

                    if (!status.IsGood) return StatusCode(status);
                }

                return Ok(_gateway.Report3.GetPropertyValue(name));
            }
            else
            {
                _logger?.LogDebug($"GetReport3Property('{name}') property not found.");
                return NotFound($"Property '{name}' not found.");
            }
        }

        /// <summary>
        /// Returns a single Wallbox report 100 property.
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
        [HttpGet("report100/property/{name}")]
        [SwaggerOperation(Tags = new[] { "Wallbox API" })]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetReport100Property(string name, bool update = false)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger?.LogDebug($"GetReport100Property() invalid property.");
                return BadRequest($"Property name is invalid.");
            }

            _logger?.LogDebug($"Property100Property({name})...");

            if (typeof(ReportsData).IsProperty(name))
            {
                if (update)
                {
                    if (_gateway.IsLocked) return Locked("Wallbox gateway is locked");

                    var status = await _gateway.ReadReport100Async();

                    if (!status.IsGood) return StatusCode(status);
                }

                return Ok(_gateway.Report100.GetPropertyValue(name));
            }
            else
            {
                _logger?.LogDebug($"GetReport100Property('{name}') property not found.");
                return NotFound($"Property '{name}' not found.");
            }
        }

        /// <summary>
        /// Returns a single Wallbox charging report property.
        /// </summary>
        /// <remarks>The property name is a CamelCase name.</remarks>
        /// <param name="id">The report ID.</param>
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
        [HttpGet("reports/{id}/property/{name}")]
        [SwaggerOperation(Tags = new[] { "Wallbox API" })]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetReportsProperty(int id, string name, bool update = false)
        {
            if (!WallboxGateway.IsReportIDOk(id))
            {
                _logger?.LogDebug($"GetReportData() invalid report ID.");
                return BadRequest($"Report ID is invalid.");
            }

            if (string.IsNullOrEmpty(name))
            {
                _logger?.LogDebug($"GetReportsProperty() invalid property.");
                return BadRequest($"Property name is invalid.");
            }

            _logger?.LogDebug($"GetReportsProperty({id}, {name})...");

            if (typeof(ReportsData).IsProperty(name))
            {
                if (update)
                {
                    if (_gateway.IsLocked) return Locked("Wallbox gateway is locked");

                    var status = await _gateway.ReadReportsAsync();

                    if (!status.IsGood) return StatusCode(status);
                }

            var index = id - WallboxGateway.REPORTS_ID - 1;
                return Ok(_gateway.Reports[index].GetPropertyValue(name));
            }
            else
            {
                _logger?.LogDebug($"GetReportsProperty('{id}, {name}') property not found.");
                return NotFound($"Property '{name}' not found.");
            }
        }

        #endregion

        #region PUT Methods

        /// <summary>
        /// Sets the current with delay on the BMW Wallbox charging station.
        /// </summary>
        /// <param name="value">Current and (optional) Delay value.</param>
        /// <returns>The action method result</returns>
        /// <response code="202">Return indicates accepted.</response>
        /// <response code="400">Return indicates BadRequest.</response>
        /// <response code="404">Return indicates NotFound.</response>
        /// <response code="409">Return indicates Conflict.</response>
        /// <response code="500">Return indicates InternalError.</response>
        /// <response code="502">Return indicates BadGateway.</response>
        /// <response code="503">Return indicates Unavailable.</response>
        [HttpPut("current")]
        [SwaggerOperation(Tags = new[] { "Wallbox API" })]
        [ProducesResponseType(202)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public IActionResult SetCurrent([FromBody] CurrentData value)
        {
            _logger.LogInformation($"SetCurrent({value.Current}, {value.Delay})");
            if (!_gateway.IsStartupOk) return StatusCode(_gateway.Status);

            if (!WallboxGateway.IsCurrentValueOk(value.Current)) return BadRequest($"Current value '{value.Current}' invalid (0; 6000 - 63000).");

            if (value.Delay.HasValue)
            {
                if (!WallboxGateway.IsDelayValueOk(value.Delay.Value)) return BadRequest($"Delay value '{value.Delay}' invalid (0; 1 - 860400).");

                if (_gateway.SetCurrent(value.Current, value.Delay.Value).IsGood)
                    return Accepted();
                else
                    return StatusCode(_gateway.Status);
            }
            else
            {
                if (_gateway.SetCurrent(value.Current).IsGood)
                    return Accepted();
                else
                    return StatusCode(_gateway.Status);

            }
        }

        /// <summary>
        /// Sets the energy limit on the BMW Wallbox charging station.
        /// </summary>
        /// <param name="value">Energy value in 0.1 kWh (0; 1..999999999)</param>
        /// <returns>The action method result</returns>
        /// <response code="202">Return indicates accepted.</response>
        /// <response code="400">Return indicates BadRequest.</response>
        /// <response code="404">Return indicates NotFound.</response>
        /// <response code="409">Return indicates Conflict.</response>
        /// <response code="500">Return indicates InternalError.</response>
        /// <response code="502">Return indicates BadGateway.</response>
        /// <response code="503">Return indicates Unavailable.</response>
        [HttpPut("energy")]
        [SwaggerOperation(Tags = new[] { "Wallbox API" })]
        [ProducesResponseType(202)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public IActionResult SetEnergy([FromBody] uint value)
        {
            _logger.LogInformation($"SetEnergy({value})");
            if (!_gateway.IsStartupOk) return StatusCode(_gateway.Status);

            if (!WallboxGateway.IsEnergyValueOk(value)) return BadRequest($"Energy value '{value}' invalid (0; 1..999999999).");

            if (_gateway.SetEnergy(value).IsGood)
                return Accepted();
            else
                return StatusCode(_gateway.Status);
        }

        /// <summary>
        /// Authorize a charging session on the BMW Wallbox charging station.
        /// </summary>
        /// <param name="tag">The RFID tag (8 byte HEX string)</param>
        /// <param name="classifier">The RFID classifier (10 byte HEX string)</param>
        /// <returns>The action method result</returns>
        /// <response code="202">Return indicates accepted.</response>
        /// <response code="400">Return indicates BadRequest.</response>
        /// <response code="404">Return indicates NotFound.</response>
        /// <response code="409">Return indicates Conflict.</response>
        /// <response code="500">Return indicates InternalError.</response>
        /// <response code="502">Return indicates BadGateway.</response>
        /// <response code="503">Return indicates Unavailable.</response>
        [HttpPut("start")]
        [SwaggerOperation(Tags = new[] { "Wallbox API" })]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public IActionResult StartCommand(string tag, string classifier)
        {
            _logger.LogInformation($"StartCommand({tag}, {classifier})");

            if (!_gateway.IsStartupOk) return StatusCode(_gateway.Status);
            if (!WallboxGateway.IsRFIDTagStringOk(tag)) return BadRequest($"RFID tag invalid");
            if (!WallboxGateway.IsRFIDClassifierStringOk(classifier)) return BadRequest($"RFID classifier invalid");

            if (_gateway.StartRFID(tag, classifier).IsGood)
                return Accepted();
            else
                return StatusCode(_gateway.Status);
        }

        /// <summary>
        /// Deauthorize a charging session on the BMW Wallbox charging station.
        /// </summary>
        /// <param name="tag">The RFID tag (8 byte HEX string)</param>
        /// <returns>The action method result</returns>
        /// <response code="202">Return indicates accepted.</response>
        /// <response code="400">Return indicates BadRequest.</response>
        /// <response code="404">Return indicates NotFound.</response>
        /// <response code="409">Return indicates Conflict.</response>
        /// <response code="500">Return indicates InternalError.</response>
        /// <response code="502">Return indicates BadGateway.</response>
        /// <response code="503">Return indicates Unavailable.</response>
        [HttpPut("stop")]
        [SwaggerOperation(Tags = new[] { "Wallbox API" })]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public IActionResult StopCommand(string tag)
        {
            _logger.LogInformation($"StopCommand({tag})");

            if (!_gateway.IsStartupOk) return StatusCode(_gateway.Status);
            if (!WallboxGateway.IsRFIDTagStringOk(tag)) return BadRequest($"RFID tag invalid");

            if (_gateway.StopRFID(tag).IsGood)
                return Accepted();
            else
                return StatusCode(_gateway.Status);
        }

        /// <summary>
        /// Disable the BMW Wallbox charging station.
        /// </summary>
        /// <returns>The action method result</returns>
        /// <response code="202">Return indicates accepted.</response>
        /// <response code="400">Return indicates BadRequest.</response>
        /// <response code="404">Return indicates NotFound.</response>
        /// <response code="409">Return indicates Conflict.</response>
        /// <response code="500">Return indicates InternalError.</response>
        /// <response code="502">Return indicates BadGateway.</response>
        /// <response code="503">Return indicates Unavailable.</response>
        [HttpPut("disable")]
        [SwaggerOperation(Tags = new[] { "Wallbox API" })]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public IActionResult DisableCommand()
        {
            _logger.LogInformation($"DisableCommand()");

            if (!_gateway.IsStartupOk) return StatusCode(_gateway.Status);

            if (_gateway.EnableCommand(0).IsGood)
                return Accepted();
            else
                return StatusCode(_gateway.Status);
        }

        /// <summary>
        /// Unlocking the socket on the BMW Wallbox charging station.
        /// </summary>
        /// <returns>The action method result</returns>
        /// <response code="202">Return indicates accepted.</response>
        /// <response code="400">Return indicates BadRequest.</response>
        /// <response code="404">Return indicates NotFound.</response>
        /// <response code="409">Return indicates Conflict.</response>
        /// <response code="500">Return indicates InternalError.</response>
        /// <response code="502">Return indicates BadGateway.</response>
        /// <response code="503">Return indicates Unavailable.</response>
        [HttpPut("unlock")]
        [SwaggerOperation(Tags = new[] { "Wallbox API" })]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public IActionResult UnlockCommand()
        {
            _logger.LogInformation($"UnlockCommand()");

            if (!_gateway.IsStartupOk) return StatusCode(_gateway.Status);

            if (_gateway.UnlockSocket().IsGood)
                return Accepted();
            else
                return StatusCode(_gateway.Status);
        }

        #endregion

        #region POST Methods

        #endregion

        #region DELETE Methods

        // No DELETE Method supported.

        #endregion

        #endregion REST Methods
    }
}
