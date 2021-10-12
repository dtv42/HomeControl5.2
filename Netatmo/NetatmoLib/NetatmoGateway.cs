// --------------------------------------------------------------------------------------------------------------------
// <copyright file="NetatmoGateway.cs" company="DTV-Online">
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

    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using UtilityLib;
    using UtilityLib.Webapp;

    using NetatmoLib.Models;
    using System.Linq;

    #endregion

    /// <summary>
    /// Class holding data from the Netatmo Web Service (KWL EC200).
    /// The data properties are based on the specification
    /// "Helios Ventilatoren Funktions- und Schnittstellenbeschreibung"
    /// NR. 82 269 - Modbus Gateway TCP/IP mit EasyControls. Druckschrift-Nr. 82269/07.14
    /// and the XML data returned from the easyControl Web Service.
    /// </summary>
    public class NetatmoGateway : BaseGateway
    {
        #region Private Data Members

        /// <summary>
        /// The HTTP client instantiated using the HTTP client factory.
        /// </summary>
        private readonly NetatmoClient _client;

        #endregion

        #region Public Properties

        /// <summary>
        /// The the Netatmo settings.
        /// </summary>
        public NetatmoSettings Settings { get; set; } = new NetatmoSettings();

        /// <summary>
        /// Indicates startup completed.
        /// </summary>
        public new bool IsStartupOk { get; private set; }

        /// <summary>
        /// The Data property holds all Helios data properties.
        /// </summary>
        [JsonIgnore]
        public NetatmoData Data { get; private set; } = new NetatmoData();

        public MainData Main { get; private set; } = new MainData();
        public OutdoorData Outdoor { get; private set; } = new OutdoorData();
        public IndoorData Indoor1 { get; private set; } = new IndoorData();
        public IndoorData Indoor2 { get; private set; } = new IndoorData();
        public IndoorData Indoor3 { get; private set; } = new IndoorData();
        public RainData Rain { get; private set; } = new RainData();
        public WindData Wind { get; private set; } = new WindData();

        /// <summary>
        /// Gets or sets the Netatmo token data.
        /// </summary>
        [JsonIgnore]
        public TokenData Token { get; private set; } = new TokenData();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="NetatmoGateway"/> class.
        /// </summary>
        /// <param name="client">The custom HTTP client.</param>
        /// <param name="settings">The Netatmo settings.</param>
        /// <param name="logger">The application logger instance.</param>
        public NetatmoGateway(NetatmoClient client,
                              INetatmoSettings settings,
                              ILogger<NetatmoGateway> logger)
            : base(logger)
        {
            _logger?.LogDebug($"NetatmoGateway()");

            Settings = new NetatmoSettings()
            {
                Address      = settings.Address,
                Timeout      = settings.Timeout,
                User         = settings.User,
                Password     = settings.Password,
                ClientID     = settings.ClientID,
                ClientSecret = settings.ClientSecret
            };

            _client = client;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Synchronous methods.
        /// </summary>
        public DataStatus ReadAll() => ReadAllAsync().Result;

        /// <summary>
        /// Updates all Helios properties reading the data from the Helios web service.
        /// If successful the data values will be updated (timestamp).
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> ReadAllAsync()
        {
            _logger?.LogDebug("ReadAllAsync() starting.");
            await LockAsync();
            Status = DataStatus.Good;

            try
            {
                var status = await GetAccessTokenAsync();

                if (status.IsGood)
                {
                    string json = await _client.GetStringAsync($"api/getstationsdata?access_token={Token.AccessToken}");

                    if (!string.IsNullOrEmpty(json))
                    {
                        var data = JsonSerializer.Deserialize<NetatmoData>(json);

                        if (data is not null)
                        {
                            Data = data;

                            var main = data.Body.Devices.FirstOrDefault(d => d.Type == "NAMain");

                            if (main is not null)
                            {
                                Main.Update(main);

                                var outdoor = main.Modules.FirstOrDefault(d => d.Type == "NAModule1");

                                if (outdoor is not null)
                                {
                                    Outdoor.Update(outdoor);
                                }

                                var wind = main.Modules.FirstOrDefault(d => d.Type == "NAModule2");

                                if (wind is not null)
                                {
                                    Wind.Update(wind);
                                }

                                var rain = main.Modules.FirstOrDefault(d => d.Type == "NAModule3");

                                if (rain is not null)
                                {
                                    Rain.Update(rain);
                                }

                                var modules = main.Modules.Where(d => d.Type == "NAModule4");
                                int index = 0;

                                foreach (var module in modules)
                                {
                                    if (index == 0) Indoor1.Update(module);
                                    if (index == 1) Indoor2.Update(module);
                                    if (index == 2) Indoor3.Update(module);
                                    ++index;
                                }

                                _logger?.LogDebug($"ReadAllAsync OK: {json}");
                            }
                        }
                        else
                        {
                            _logger?.LogError("ReadAllAsync: deserialization error.");
                            Status = DataStatus.BadDecodingError;
                            Status.Explanation = "Netatmo invalid data returned.";
                        }
                    }
                    else
                    {
                        _logger?.LogError("ReadAllAsync: no data returned.");
                        Status = DataStatus.BadUnknownResponse;
                        Status.Explanation = "Netatmo no data returned.";
                    }
                }
                else
                {
                    _logger?.LogError("ReadAllAsync: no valid access token.");
                    Status = DataStatus.BadNoCommunication;
                    Status.Explanation = "Netatmo not authenticated.";
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


        /// <summary>
        /// Tries to get all data from the Netatmo web service.
        /// </summary>
        /// <returns>Flag indicating success or failure.</returns>
        public override bool Startup()
        {
            _logger.LogInformation($"Netatmo Gateway Starting...");
            IsStartupOk = ReadAll().IsGood;

            if (IsStartupOk)
            {
                _logger.LogInformation("Netatmo Gateway:");
                _logger.LogInformation($"    Address:      {Settings.Address}");
                _logger.LogInformation($"    User:         {Settings.User}");
                _logger.LogInformation($"    Password:     {Settings.Password}");
                _logger.LogInformation($"    ClientID:     {Settings.ClientID}");
                _logger.LogInformation($"    ClientSecret: {Settings.ClientSecret}");
                _logger.LogInformation($"Startup OK");
            }
            else
            {
                _logger.LogWarning("Netatmo Gateway: Startup not OK");
            }

            return IsStartupOk;
        }

        /// <summary>
        /// Tries to connect to the Helios web service.
        /// </summary>
        /// <returns>Flag indicating success or failure.</returns>
        public override bool CheckAccess()
             => GetAccessTokenAsync().Result == DataStatus.Good;

        #endregion

        #region Private Methods

        /// <summary>
        /// Async method to retrieve a Netatmo access token.
        /// </summary>
        private async Task<DataStatus> GetAccessTokenAsync()
        {
            Status = DataStatus.Good;

            try
            {
                var postData = new List<KeyValuePair<string?, string?>>
                {
                    new KeyValuePair<string?, string?>("grant_type", "password"),
                    new KeyValuePair<string?, string?>("client_id", Settings.ClientID),
                    new KeyValuePair<string?, string?>("client_secret", Settings.ClientSecret),
                    new KeyValuePair<string?, string?>("username", Settings.User),
                    new KeyValuePair<string?, string?>("password", Settings.Password),
                    new KeyValuePair<string?, string?>("scope", "read_station")
                };
                HttpContent content = new FormUrlEncodedContent(postData);
                string json = await _client.PostAsync("oauth2/token", content);
                var data = JsonSerializer.Deserialize<TokenData>(json);

                if (data is not null)
                {
                    if (data.ExpiresIn <= 0)
                    {
                        postData = new List<KeyValuePair<string?, string?>>
                        {
                            new KeyValuePair<string?, string?>("grant_type", "refresh_token"),
                            new KeyValuePair<string?, string?>("refresh_token", Token.RefreshToken),
                            new KeyValuePair<string?, string?>("client_id", Settings.ClientID),
                            new KeyValuePair<string?, string?>("client_secret", Settings.ClientSecret)
                        };

                        content = new FormUrlEncodedContent(postData);
                        json = await _client.PostAsync("oauth2/token", content);
                        data = JsonSerializer.Deserialize<TokenData>(json);

                        if (data is not null)
                        {
                            Token = data;
                            _logger?.LogDebug($"RefreshAccessTokenAsync OK: {json}");
                            return Status;
                        }
                        else
                        {
                            _logger?.LogDebug("RefreshAccessTokenAsync: deserialization error.");
                            Status = DataStatus.BadDecodingError;
                            Status.Explanation = "Invalid data response.";
                            return Status;
                        }
                    }

                    Token = data;
                    _logger?.LogDebug($"GetAccessTokenAsync OK: {json}");
                    return Status;
                }
                else
                {
                    _logger?.LogDebug("GetAccessTokenAsync: deserialization error.");
                    Status = DataStatus.BadDecodingError;
                    Status.Explanation = "Invalid data response.";
                    return Status;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"GetAccessTokenAsync exception: {ex.Message}.");
                Status = DataStatus.BadInternalError;
                Status.Explanation = ex.Message;
                return Status;
            }
        }

        #endregion Private Methods
    }
}
