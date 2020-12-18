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
    using System.Net.Http;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;

    using UtilityLib;
    using UtilityLib.Webapp;

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
        /// Indicates startup completed.
        /// </summary>
        public new bool IsStartupOk { get; private set; }

        #endregion Public Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WebGateway"/> class.
        /// </summary>
        /// <param name="factory">The HTTP client factory.</param>
        /// <param name="logger">The application logger instance.</param>
        public WebGateway(IHttpClientFactory factory, ILogger<WebGateway> logger)
            : base(logger)
        {
            _logger?.LogDebug($"WebGateway()");

            _client = factory.CreateClient("Gateway");
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client.DefaultRequestHeaders.Add("User-Agent", "GoogleClient");
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
            => ReadAll() == DataStatus.Good;

        public override async Task<bool> CheckAccessAsync()
            => (await ReadAllAsync()) == DataStatus.Good;

        /// <summary>
        /// Reading the data from the remote web service.
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public DataStatus ReadAll() => ReadAllAsync().Result;

        /// <summary>
        /// Reading the data from the remote web service.
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> ReadAllAsync()
        {
            _logger?.LogDebug("ReadAllAsync() starting.");
            await LockAsync();
            Status = DataStatus.Uncertain;

            try
            {
                string html = await _client.GetStringAsync("humans.txt");

                if (!string.IsNullOrEmpty(html))
                {
                    if (html.Contains("careers.google.com"))
                    {
                        _logger?.LogDebug($"ReadAllAsync OK.");
                        Status = DataStatus.Good;
                    }
                    else
                    {
                        _logger?.LogError("ReadAllAsync: invalid data returned.");
                        Status = DataStatus.BadDecodingError;
                        Status.Explanation = "WebGateway invalid data returned.";
                    }
                }
                else
                {
                    _logger?.LogError("ReadAllAsync: no data returned.");
                    Status = DataStatus.BadUnknownResponse;
                    Status.Explanation = "WebGateway no data returned.";
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
                Unlock();
                _logger?.LogDebug("ReadAllAsync() finished.");
            }

            return Status;
        }

        #endregion Public Methods
    }
}
