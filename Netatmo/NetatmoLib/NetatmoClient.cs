// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NetatmoClient.cs" company="DTV-Online">
//   Copyright (c) 2021 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>15-1-2021 10:44</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace NetatmoLib
{
    #region Using Directives

    using System.Net.Http;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using UtilityLib;

    #endregion

    public class NetatmoClient : BaseClass
    {
        #region Private Data Members

        /// <summary>
        /// The HTTP client used internally.
        /// </summary>
        private readonly HttpClient _client;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NetatmoClient"/> class.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="logger"></param>
        public NetatmoClient(HttpClient client, ILogger<NetatmoClient> logger)
            : base(logger)
        {
            _logger?.LogDebug($"NetatmoClient()");

            _client = client;
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client.DefaultRequestHeaders.Add("User-Agent", "NetatmoClient");
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Helper method to perform a GET request and return the response as a string.
        /// </summary>
        /// <param name="request">The HTTP request</param>
        /// <returns>The string result.</returns>
        public async Task<string> GetStringAsync(string request)
            => await _client.GetStringAsync(request);

        /// <summary>
        /// Helper method to perform a POST request and return the response as a string.
        /// </summary>
        /// <param name="request">The HTTP request</param>
        /// <returns>The string result.</returns>
        public async Task<string> PostAsync(string request, HttpContent content)
        {
            HttpResponseMessage response = await _client.PostAsync(request, content);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }


        #endregion

    }
}
