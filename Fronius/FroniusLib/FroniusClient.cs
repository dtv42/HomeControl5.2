namespace FroniusLib
{
    #region Using Directives

    using System;
    using System.Net.Http;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using UtilityLib;
    using FroniusLib.Models;

    #endregion

    public class FroniusClient : BaseClass<FroniusSettings>
    {
        #region Private Data Members

        /// <summary>
        /// The HTTP client used internally.
        /// </summary>
        private readonly HttpClient _client;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FroniusClient"/> class.
        /// </summary>
        /// <param name="client"></param>
        /// <param name="logger"></param>
        public FroniusClient(HttpClient client,
                             FroniusSettings settings,
                             ILogger<FroniusClient> logger)
            : base(settings, logger)
        {
            _logger?.LogDebug($"FroniusClient()");

            _client = client;
            _client.DefaultRequestHeaders.Add("Accept", "application/json");
            _client.DefaultRequestHeaders.Add("User-Agent", "FroniusClient");

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

        #endregion
    }
}
