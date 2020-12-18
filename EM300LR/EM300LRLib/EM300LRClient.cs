// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EM300LRClient.cs" company="DTV-Online">
//   Copyright(c) 2019 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace EM300LRLib
{
    #region Using Directives

    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using UtilityLib;
    using EM300LRLib.Models;

    #endregion Using Directives

    public class EM300LRClient : BaseClass<EM300LRSettings>
    {
        #region Private Data Members

        /// <summary>
        /// The HTTP client used internally.
        /// </summary>
        private readonly HttpClient _client;

        #endregion Private Data Members

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EM300LRClient"/> class.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="logger"></param>
        /// <param name="options"></param>
        public EM300LRClient(HttpClient client,
                             EM300LRSettings settings,
                             ILogger<EM300LRClient> logger)
            : base(settings, logger)
        {
            _logger?.LogDebug($"EM300LRClient()");

            _client = client;
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client.DefaultRequestHeaders.Add("User-Agent", "EM300LRClient");

            Update();
        }

        #endregion Constructors

        #region Public Methods

        public void Update()
        {
            _client.BaseAddress = new Uri(_settings.BaseAddress);
            _client.Timeout = TimeSpan.FromSeconds(_settings.Timeout);
        }

        /// <summary>
        /// Helper method to perform a GET request and return the response as a string.
        /// </summary>
        /// <param name="request">The HTTP request</param>
        /// <returns>The string result.</returns>
        public async Task<string> GetStringAsync(string request)
        {
            var response = await _client.GetAsync(request);
            return response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : string.Empty;
        }

        /// <summary>
        /// Helper method to perform a POST request and return the response as a string.
        /// </summary>
        /// <param name="request">The HTTP request</param>
        /// <param name="content">The POST content</param>
        /// <returns>The string result.</returns>
        public async Task<string> PostStringAsync(string request, string content)
        {
            var stringcontent = new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded");
            var response = await _client.PostAsync(request, stringcontent);
            return response.IsSuccessStatusCode ? await response.Content.ReadAsStringAsync() : string.Empty;
        }

        #endregion Public Methods
    }
}