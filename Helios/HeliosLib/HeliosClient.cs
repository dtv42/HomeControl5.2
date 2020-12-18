// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeliosClient.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>26-4-2020 10:06</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace HeliosLib
{
    #region Using Directives

    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using UtilityLib;
    using HeliosLib.Models;

    #endregion

    public class HeliosClient : BaseClass<HeliosSettings>
    {
        #region Private Data Members

        /// <summary>
        /// The HTTP client used internally.
        /// </summary>
        private readonly HttpClient _client;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HeliosClient"/> class.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="logger"></param>
        public HeliosClient(HttpClient client,
                            HeliosSettings settings,
                            ILogger<HeliosClient> logger)
            : base(settings, logger)
        {
            _logger?.LogDebug($"HeliosClient()");

            _client = client;
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client.DefaultRequestHeaders.Add("User-Agent", "HeliosClient");

            Update();
        }

        #endregion

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
            => await _client.GetStringAsync(request);

        /// <summary>
        /// Helper method to perform a POST request and return the response as a string.
        /// </summary>
        /// <param name="request">The HTTP request</param>
        /// <returns>The string result.</returns>
        public async Task<string> PostStringAsync(string request, string content)
        {
            HttpResponseMessage response = await _client.PostAsync(request, new StringContent(content));
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            return string.Empty;
        }

        #endregion

    }
}
