// --------------------------------------------------------------------------------------------------------------------
// <copyright file="SettingsController.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>14-12-2020 15:59</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace UtilityWeb.Controllers
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Swashbuckle.AspNetCore.Annotations;
    using UtilityLib.Webapp;
    using UtilityWeb.Models;
    using UtilityWeb.Services;

    #endregion Using Directives

    /// <summary>
    ///  AppSettings controller providing various GET operations.
    /// </summary>
    [ApiController]
    [Route("[controller]/[action]")]
    public class GatewayController : BaseController
    {
        #region Private Fields

        private readonly WebGateway _gateway;

        #endregion Private Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsController"/> class.
        /// The parameters provided by dependency injection are used to set private fields.
        /// </summary>
        /// <param name="gateway">The web gateway instance.</param>
        /// <param name="configuration">The application configuration instance.</param>
        /// <param name="logger">The logger instance.</param>
        public GatewayController(WebGateway gateway, IConfiguration configuration, ILogger<GatewayController> logger)
             : base(configuration, logger)
        {
            _logger.LogDebug("DataController()");

            _gateway = gateway;
        }

        #endregion

        #region REST Methods

        #region GET Methods

        [HttpGet]
        [ApiExplorerSettings(IgnoreApi = true)]
        public ActionResult YourAction()
        {
            return Redirect("swagger");
        }

        /// <summary>
        /// Returns all web gateway data.
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
        [SwaggerOperation(Tags = new[] { "Web Gateway API" })]
        [ProducesResponseType(typeof(WebGateway), 200)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetWebGateway(bool update = false)
        {
            _logger?.LogDebug("GetWebGateway()...");

            if (_gateway.IsLocked) return Locked("GetFroniusGateway gateway is locked");

            if (update)
            {
                var status = await _gateway.ReadAllAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(new WebGatewayInfo()
            {
                Settings = _gateway.Settings,
                IsStartupOk = _gateway.IsStartupOk,
                IsLocked = _gateway.IsLocked,
                Status = _gateway.Status
            });
        }

        /// <summary>
        /// Returns all web service related data.
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
        [SwaggerOperation(Tags = new[] { "Web Gateway API" })]
        [ProducesResponseType(typeof(WebGatewayData), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetWebGatewayData(bool update = false)
        {
            _logger?.LogDebug("GetWebGatewayData()...");

            if (_gateway.IsLocked) return Locked("Web gateway is locked");

            if (update)
            {
                var status = await _gateway.ReadAllAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(_gateway.Data);
        }

        /// <summary>
        /// Returns a subset of the web gateway data.
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
        [HttpGet("posts")]
        [SwaggerOperation(Tags = new[] { "Web Gateway API" })]
        [ProducesResponseType(typeof(List<PostData>), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetPostData(bool update = false)
        {
            _logger?.LogDebug("GetPostData()...");

            if (_gateway.IsLocked) return Locked("Web gateway is locked");

            if (update)
            {
                var status = await _gateway.ReadAllPostsAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(_gateway.Data.Posts);
        }

        /// <summary>
        /// Returns a subset of the web gateway data.
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
        [HttpGet("comments")]
        [SwaggerOperation(Tags = new[] { "Web Gateway API" })]
        [ProducesResponseType(typeof(List<CommentData>), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetCommentData(bool update = false)
        {
            _logger?.LogDebug("GetCommentData()...");

            if (_gateway.IsLocked) return Locked("Web gateway is locked");

            if (update)
            {
                var status = await _gateway.ReadAllCommentsAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(_gateway.Data.Comments);
        }

        /// <summary>
        /// Returns a subset of the web gateway data.
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
        [HttpGet("albums")]
        [SwaggerOperation(Tags = new[] { "Web Gateway API" })]
        [ProducesResponseType(typeof(List<AlbumData>), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetAlbumData(bool update = false)
        {
            _logger?.LogDebug("GetAlbumData()...");

            if (_gateway.IsLocked) return Locked("Web gateway is locked");

            if (update)
            {
                var status = await _gateway.ReadAllAlbumsAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(_gateway.Data.Albums);
        }

        /// <summary>
        /// Returns a subset of the web gateway data.
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
        [HttpGet("photos")]
        [SwaggerOperation(Tags = new[] { "Web Gateway API" })]
        [ProducesResponseType(typeof(List<PhotoData>), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetPhotoData(bool update = false)
        {
            _logger?.LogDebug("GetPhotoData()...");

            if (_gateway.IsLocked) return Locked("Web gateway is locked");

            if (update)
            {
                var status = await _gateway.ReadAllPostsAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(_gateway.Data.Posts);
        }

        /// <summary>
        /// Returns a subset of the web gateway data.
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
        [HttpGet("todos")]
        [SwaggerOperation(Tags = new[] { "Web Gateway API" })]
        [ProducesResponseType(typeof(List<PostData>), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetTodoData(bool update = false)
        {
            _logger?.LogDebug("GetTodoData()...");

            if (_gateway.IsLocked) return Locked("Web gateway is locked");

            if (update)
            {
                var status = await _gateway.ReadAllTodosAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(_gateway.Data.Todos);
        }

        /// <summary>
        /// Returns a subset of the web gateway data.
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
        [HttpGet("users")]
        [SwaggerOperation(Tags = new[] { "Web Gateway API" })]
        [ProducesResponseType(typeof(List<UserData>), 200)]
        [ProducesResponseType(typeof(string), 423)]
        [ProducesResponseType(typeof(string), 400)]
        [ProducesResponseType(typeof(string), 404)]
        [ProducesResponseType(typeof(string), 409)]
        [ProducesResponseType(typeof(string), 500)]
        [ProducesResponseType(typeof(string), 502)]
        [ProducesResponseType(typeof(string), 503)]
        public async Task<IActionResult> GetUserData(bool update = false)
        {
            _logger?.LogDebug("GetUserData()...");

            if (_gateway.IsLocked) return Locked("Web gateway is locked");

            if (update)
            {
                var status = await _gateway.ReadAllUsersAsync();

                if (!status.IsGood) return StatusCode(status);
            }

            return Ok(_gateway.Data.Users);
        }

        #endregion

        #endregion
    }
}