// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WebGateway.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>13-5-2020 13:54</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace UtilityWeb.Services
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using UtilityLib;
    using UtilityLib.Webapp;
    using UtilityWeb.Models;

    #endregion Using Directives

    /// <summary>
    ///  Sample gateway requesting data from the web.
    /// </summary>
    public class WebGateway : BaseGateway
    {
        #region Private Data Members

        /// <summary>
        /// The custom HTTP client instance.
        /// </summary>
        private readonly HttpClient _client;

        #endregion Private Data Members

        #region Public Properties

        /// <summary>
        /// The the web service settings.
        /// </summary>
        public WebGatewaySettings Settings { get; set; } = new WebGatewaySettings();

        /// <summary>
        /// Indicates startup completed.
        /// </summary>
        public new bool IsStartupOk { get; private set; }

        /// <summary>
        /// List of users (jsonplaceholder web service).
        /// </summary>
        public WebGatewayData Data { get; set; } = new WebGatewayData();

        #endregion Public Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WebGateway"/> class.
        /// </summary>
        /// <param name="factory">The HTTP client factory.</param>
        /// <param name="settings">The gateway settings.</param>
        /// <param name="logger">The application logger instance.</param>
        public WebGateway(IHttpClientFactory factory,
                          IWebGatewaySettings settings,
                          ILogger<WebGateway> logger)
            : base(logger)
        {
            _logger?.LogDebug($"WebGateway()");

            _client = factory.CreateClient("Gateway");
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client.DefaultRequestHeaders.Add("User-Agent", "GoogleClient");

            Settings = new WebGatewaySettings()
            {
                Address = settings.Address,
                Timeout = settings.Timeout,
                Retries = settings.Retries,
                Wait    = settings.Wait
            };
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Tries to get data from the remote web service.
        /// </summary>
        /// <returns>Flag indicating success or failure.</returns>
        public override bool Startup()
        {
            _logger.LogInformation("WebGateway Starting...");
            IsStartupOk = ReadAll().IsGood;

            if (IsStartupOk)
            {
                _logger.LogInformation("WebGateway:");
                _logger.LogInformation($"    Base Address:  {_client.BaseAddress}");
                _logger.LogInformation($"Startup OK");
            }
            else
            {
                _logger.LogWarning("WebGateway: Startup not OK");
            }

            return IsStartupOk;
        }

        /// <summary>
        /// Tries to connect to the remote web service.
        /// </summary>
        /// <returns>Flag indicating success or failure.</returns>
        public override bool CheckAccess()
            => CheckAccessAsync().Result;

        public override async Task<bool> CheckAccessAsync()
            => (await ReadAsync()) == DataStatus.Good;

        /// <summary>
        /// Reading the data from the remote web service.
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public DataStatus ReadAll() => ReadAllAsync().Result;

        /// <summary>
        /// Reading the data from the remote web service.
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> ReadAsync()
        {
            _logger?.LogDebug("ReadAsync() starting.");
            await LockAsync();
            Status = DataStatus.Good;

            try
            {
                string html = await _client.GetStringAsync("/");

                if (!string.IsNullOrEmpty(html))
                {
                    _logger?.LogDebug("ReadAsync() OK.");
                }
                else
                {
                    _logger?.LogError("ReadAsync: no data returned.");
                    Status = DataStatus.BadUnknownResponse;
                    Status.Explanation = "WebGateway no data returned.";
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Exception in ReadAsync().");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug("ReadAsync() finished.");
            }

            return Status;
        }

        /// <summary>
        /// Reading the data from the remote web service.
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> ReadAllAsync()
        {
            _logger?.LogDebug("ReadAllAsync() starting.");
            Status = DataStatus.Uncertain;
            bool Ok = true;

            try
            {
                Ok &= (await ReadAllPostsAsync()).IsGood;
                Ok &= (await ReadAllCommentsAsync()).IsGood;
                Ok &= (await ReadAllAlbumsAsync()).IsGood;
                Ok &= (await ReadAllPhotosAsync()).IsGood;
                Ok &= (await ReadAllTodosAsync()).IsGood;
                Ok &= (await ReadAllUsersAsync()).IsGood;

                if (!Ok)
                {
                    _logger?.LogError("ReadAllAsync() invalid response.");
                    Status = DataStatus.BadCommunicationError;
                }
                else
                {
                    Status = DataStatus.Good;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Exception in ReadAllAsync().");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                _logger?.LogDebug("ReadAllAsync() finished.");
            }

            return Status;
        }

        /// <summary>
        /// Reads all post data from the test web service.
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> ReadAllPostsAsync()
        {
            _logger?.LogDebug("ReadAllPostsAsync() starting.");
            await LockAsync();
            Status = DataStatus.Good;

            try
            {
                string json = await _client.GetStringAsync("posts");

                if (!string.IsNullOrEmpty(json))
                {
                    var data = JsonSerializer.Deserialize<List<PostData>>(json);

                    if (data is not null)
                    {
                        Data.Posts = data;
                        _logger?.LogDebug("ReadAllPostsAsync() OK.");
                    }
                    else
                    {
                        _logger?.LogError("ReadAllPostsAsync: invalid data returned.");
                        Status = DataStatus.BadDecodingError;
                        Status.Explanation = "WebGateway invalid data returned.";
                    }
                }
                else
                {
                    _logger?.LogError("ReadAllPostsAsync: no data returned.");
                    Status = DataStatus.BadUnknownResponse;
                    Status.Explanation = "WebGateway no data returned.";
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Exception in ReadAllPostsAsync().");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug("ReadAllUsers() finished.");
            }

            return Status;
        }

        /// <summary>
        /// Reads all comment data from the test web service.
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> ReadAllCommentsAsync()
        {
            _logger?.LogDebug("ReadAllCommentsAsync() starting.");
            await LockAsync();
            Status = DataStatus.Good;

            try
            {
                string json = await _client.GetStringAsync("comments");

                if (!string.IsNullOrEmpty(json))
                {
                    var data = JsonSerializer.Deserialize<List<CommentData>>(json);

                    if (data is not null)
                    {
                        Data.Comments = data;
                        _logger?.LogDebug("ReadAllCommentsAsync() OK.");
                    }
                    else
                    {
                        _logger?.LogError("ReadAllCommentsAsync: invalid data returned.");
                        Status = DataStatus.BadDecodingError;
                        Status.Explanation = "WebGateway invalid data returned.";
                    }
                }
                else
                {
                    _logger?.LogError("ReadAllCommentsAsync: no data returned.");
                    Status = DataStatus.BadUnknownResponse;
                    Status.Explanation = "WebGateway no data returned.";
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Exception in ReadAllCommentsAsync().");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug("ReadAllCommentsAsync() finished.");
            }

            return Status;
        }

        /// <summary>
        /// Reads all album data from the test web service.
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> ReadAllAlbumsAsync()
        {
            _logger?.LogDebug("ReadAllAlbumsAsync() starting.");
            await LockAsync();
            Status = DataStatus.Good;

            try
            {
                string json = await _client.GetStringAsync("albums");

                if (!string.IsNullOrEmpty(json))
                {
                    var data = JsonSerializer.Deserialize<List<AlbumData>>(json);

                    if (data is not null)
                    {
                        Data.Albums = data;
                        _logger?.LogDebug("ReadAllAlbumsAsync() OK.");
                    }
                    else
                    {
                        _logger?.LogError("ReadAllAlbumsAsync: invalid data returned.");
                        Status = DataStatus.BadDecodingError;
                        Status.Explanation = "WebGateway invalid data returned.";
                    }
                }
                else
                {
                    _logger?.LogError("ReadAllAlbumsAsync: no data returned.");
                    Status = DataStatus.BadUnknownResponse;
                    Status.Explanation = "WebGateway no data returned.";
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Exception in ReadAllAlbumsAsync().");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug("ReadAllAlbumsAsync() finished.");
            }

            return Status;
        }

        /// <summary>
        /// Reads all photo data from the test web service.
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> ReadAllPhotosAsync()
        {
            _logger?.LogDebug("ReadAllUsers() starting.");
            await LockAsync();
            Status = DataStatus.Good;

            try
            {
                string json = await _client.GetStringAsync("photos");

                if (!string.IsNullOrEmpty(json))
                {
                    var data = JsonSerializer.Deserialize<List<PhotoData>>(json);

                    if (data is not null)
                    {
                        Data.Photos = data;
                        _logger?.LogDebug("ReadAllPhotosAsync() OK.");
                    }
                    else
                    {
                        _logger?.LogError("ReadAllPhotosAsync: invalid data returned.");
                        Status = DataStatus.BadDecodingError;
                        Status.Explanation = "WebGateway invalid data returned.";
                    }
                }
                else
                {
                    _logger?.LogError("ReadAllPhotosAsync: no data returned.");
                    Status = DataStatus.BadUnknownResponse;
                    Status.Explanation = "WebGateway no data returned.";
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Exception in ReadAllPhotosAsync().");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug("ReadAllPhotosAsync() finished.");
            }

            return Status;
        }

        /// <summary>
        /// Reads all todo data from the test web service.
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> ReadAllTodosAsync()
        {
            _logger?.LogDebug("ReadAllTodosAsync() starting.");
            await LockAsync();
            Status = DataStatus.Good;

            try
            {
                string json = await _client.GetStringAsync("todos");

                if (!string.IsNullOrEmpty(json))
                {
                    var data = JsonSerializer.Deserialize<List<TodoData>>(json);

                    if (data is not null)
                    {
                        Data.Todos = data;
                        _logger?.LogDebug("ReadAllTodosAsync() OK.");
                    }
                    else
                    {
                        _logger?.LogError("ReadAllTodosAsync: invalid data returned.");
                        Status = DataStatus.BadDecodingError;
                        Status.Explanation = "WebGateway invalid data returned.";
                    }
                }
                else
                {
                    _logger?.LogError("ReadAllTodosAsync: no data returned.");
                    Status = DataStatus.BadUnknownResponse;
                    Status.Explanation = "WebGateway no data returned.";
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Exception in ReadAllTodosAsync().");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug("ReadAllTodosAsync() finished.");
            }

            return Status;
        }

        /// <summary>
        /// Reads all user data from the test web service.
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> ReadAllUsersAsync()
        {
            _logger?.LogDebug("ReadAllUsers() starting.");
            await LockAsync();
            Status = DataStatus.Good;

            try
            {
                string json = await _client.GetStringAsync("users");

                if (!string.IsNullOrEmpty(json))
                {
                    var data = JsonSerializer.Deserialize<List<UserData>>(json);

                    if (data is not null)
                    {
                        Data.Users = data;
                        _logger?.LogDebug("ReadAllUsers() OK.");
                    }
                    else
                    {
                        _logger?.LogError("ReadAllUsers: invalid data returned.");
                        Status = DataStatus.BadDecodingError;
                        Status.Explanation = "WebGateway invalid data returned.";
                    }
                }
                else
                {
                    _logger?.LogError("ReadAllUsers: no data returned.");
                    Status = DataStatus.BadUnknownResponse;
                    Status.Explanation = "WebGateway no data returned.";
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Exception in ReadAllUsers().");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug("ReadAllUsers() finished.");
            }

            return Status;
        }

        #endregion Public Methods
    }
}
