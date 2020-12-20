// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EM300LRGateway.cs" company="DTV-Online">
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
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using UtilityLib;
    using UtilityLib.Webapp;

    using EM300LRLib.Models;

    #endregion Using Directives

    /// <summary>
    /// Class holding data from the EM300LR EM300 LR energy monitor.
    /// The data properties are based on the specification EM300LR
    /// Technische Dokumentation TQ Energy Manager JSON-API.0104 (27.10.2017)
    /// </summary>
    public class EM300LRGateway : BaseGateway
    {
        #region Private Data Members

        /// <summary>
        /// The custom HTTP client instance.
        /// </summary>
        private readonly EM300LRClient _client;

        /// <summary>
        /// The EM300LR client settings.
        /// </summary>
        private readonly IEM300LRSettings _settings;

        /// <summary>
        /// The custom JSON serializer options.
        /// </summary>
        private readonly JsonSerializerOptions _serializerOptions = JsonExtensions.DefaultSerializerOptions;

        #endregion Private Data Members

        #region Public Properties

        /// <summary>
        /// Gets the EM300LR settings.
        /// </summary>
        public IEM300LRSettings Settings { get => _settings; }

        /// <summary>
        /// Indicates startup completed.
        /// </summary>
        public new bool IsStartupOk { get; private set; }

        /// <summary>
        /// The Data property holds all EM300LR data properties.
        /// </summary>
        [JsonIgnore]
        public EM300LRData Data { get; set; } = new EM300LRData();

        /// <summary>
        /// The InfoData property holds a subset of the EM300LR data values.
        /// </summary>
        [JsonIgnore]
        public TotalData TotalData { get; } = new TotalData();

        /// <summary>
        /// The InfoData property holds a subset of the EM300LR data values.
        /// </summary>
        [JsonIgnore]
        public Phase1Data Phase1Data { get; } = new Phase1Data();

        /// <summary>
        /// The InfoData property holds a subset of the EM300LR data values.
        /// </summary>
        [JsonIgnore]
        public Phase2Data Phase2Data { get; } = new Phase2Data();

        /// <summary>
        /// The InfoData property holds a subset of the EM300LR data values.
        /// </summary>
        [JsonIgnore]
        public Phase3Data Phase3Data { get; } = new Phase3Data();

        #endregion Public Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="EM300LRGateway"/> class.
        /// Note that the serializer options include a custom converter. 
        /// </summary>
        /// <param name="client">The custom HTTP client.</param>
        /// <param name="settings">The EM300LR settings.</param>
        /// <param name="logger">The application logger instance.</param>
        public EM300LRGateway(EM300LRClient client,
                              IEM300LRSettings settings,
                              ILogger<EM300LRGateway> logger)
            : base(logger)
        {
            _logger?.LogDebug($"EM300LRGateway()");

            _serializerOptions.Converters.Add(new NumberConverter());
            _settings = settings;
            _client = client;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Updates all EM300LR properties reading the data from the b-Control EM300LR web service.
        /// If successful the data values will be updated (timestamp).
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public DataStatus ReadAll() => ReadAllAsync().Result;

        /// <summary>
        /// Updates all EM300LR properties reading the data from the b-Control EM300LR web service.
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
                var status = await GetStartAsync();

                if (status.IsGood)
                {
                    status = await PostStartAsync();

                    if (status.IsGood)
                    {
                        string json = await _client.GetStringAsync($"mum-webservice/data.php");

                        if (!string.IsNullOrEmpty(json))
                        {
                            var data = JsonSerializer.Deserialize<EM300LRTcpData>(json, _serializerOptions);

                            if (!(data is null))
                            {
                                if (data.StatusCode == 0)
                                {
                                    Data.Refresh(data);
                                    TotalData.Refresh(data);
                                    Phase1Data.Refresh(data);
                                    Phase2Data.Refresh(data);
                                    Phase3Data.Refresh(data);

                                    _logger?.LogDebug($"ReadAllAsync OK.");
                                }
                                else
                                {
                                    _logger?.LogError($"EM300LR status code: {data.StatusCode}.");
                                    Status = DataStatus.BadDeviceFailure;
                                    Status.Explanation = $"EM300LR status code: {data.StatusCode}.";
                                }
                            }
                            else
                            {
                                _logger?.LogError("ReadAllAsync: deserialization error.");
                                Status = DataStatus.BadDecodingError;
                                Status.Explanation = "EM300LR invalid data returned.";
                            }
                        }
                        else
                        {
                            _logger?.LogError("ReadAllAsync: no data returned.");
                            Status = DataStatus.BadUnknownResponse;
                            Status.Explanation = "EM300LR no data returned.";
                        }
                    }
                    else
                    {
                        _logger?.LogError("ReadAllAsync: not authenticated.");
                        Status = DataStatus.BadNoCommunication;
                        Status.Explanation = "EM300LR not authenticated.";
                    }
                }
                else
                {
                    _logger?.LogError("ReadAllAsync: invalid response.");
                    Status = DataStatus.BadCommunicationError;
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
        /// Tries to get all data from the b-Control EM300LR web service.
        /// </summary>
        /// <returns>Flag indicating success or failure.</returns>
        public override bool Startup()
        {
            _logger.LogInformation($"b-control EM300LR Gateway Starting...");
            IsStartupOk = ReadAll().IsGood;

            if (IsStartupOk)
            {
                _logger.LogInformation("b-control EM300LR Gateway:");
                _logger.LogInformation($"    Base Address:  {_settings.Address}");
                _logger.LogInformation($"    Password:      {_settings.Password}");
                _logger.LogInformation($"    Serial Number: {_settings.SerialNumber}");
                _logger.LogInformation($"Startup OK");
            }
            else
            {
                _logger.LogWarning("b-control EM300LR Gateway: Startup not OK");
            }

            return IsStartupOk;
        }

        /// <summary>
        /// Tries to connect to the b-Control EM300LR web service.
        /// </summary>
        /// <returns>Flag indicating success or failure.</returns>
        public override bool CheckAccess()
            => GetStartAsync().Result == DataStatus.Good;

        /// <summary>
        /// Tries to connect to the b-Control EM300LR web service asnychronously.
        /// </summary>
        /// <returns>Flag indicating success or failure.</returns>
        public override async Task<bool> CheckAccessAsync()
            => (await GetStartAsync()) == DataStatus.Good;

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Async method to retrieve authentication data from the b-Control EM300LR web service.
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        private async Task<DataStatus> GetStartAsync()
        {
            Status = DataStatus.Good;

            try
            {
                string json = await _client.GetStringAsync($"start.php");

                if (!string.IsNullOrEmpty(json))
                {
                    var data = JsonSerializer.Deserialize<AuthentificationData>(json, _serializerOptions);

                    if (!(data is null))
                    {
                        if (_settings.SerialNumber == data.SerialNumber)
                        {
                            _logger?.LogDebug("GetStartAsync OK.");
                            return Status;
                        }
                        else
                        {
                            _logger?.LogDebug($"GetStartAsync: SerialNumber '{_settings.SerialNumber}' expected, but '{data.SerialNumber}' received.");
                            Status = DataStatus.BadNotFound;
                            Status.Explanation = $"SerialNumber '{_settings.SerialNumber}' expected, but '{data.SerialNumber}' received.";
                            return Status;
                        }
                    }
                    else
                    {
                        _logger?.LogDebug("GetStartAsync: deserialization error.");
                        Status = DataStatus.BadDecodingError;
                        Status.Explanation = "Invalid data response.";
                        return Status;
                    }
                }
                else
                {
                    _logger?.LogDebug("GetStartAsync: invalid response.");
                    Status = DataStatus.BadUnknownResponse;
                    Status.Explanation = "Invalid response.";
                    return Status;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"GetStartAsync exception: {ex.Message}.");
                Status = DataStatus.BadInternalError;
                Status.Explanation = ex.Message;
                return Status;
            }
        }

        /// <summary>
        /// Async method to retrieve authentication data from the b-Control EM300LR web service.
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        private async Task<DataStatus> PostStartAsync()
        {
            Status = DataStatus.Good;

            try
            {
                string json = await _client.PostStringAsync($"start.php", $"login={_settings.SerialNumber}&password={_settings.Password}");

                if (!string.IsNullOrEmpty(json))
                {
                    var data = JsonSerializer.Deserialize<AuthentificationData>(json, _serializerOptions);

                    if (!(data is null))
                    {
                        if (data.Authentication)
                        {
                            _logger?.LogDebug("PostStartAsync OK.");
                            return Status;
                        }
                        else
                        {
                            _logger?.LogDebug("PostStartAsync: not authenticated.");
                            Status = DataStatus.Bad;
                            Status.Explanation = "Not authenticated.";
                            return Status;
                        }
                    }
                    else
                    {
                        _logger?.LogDebug("PostStartAsync: deserialization error.");
                        Status = DataStatus.BadDecodingError;
                        Status.Explanation = "Invalid data response.";
                        return Status;
                    }
                }
                else
                {
                    _logger?.LogDebug("PostStartAsync: invalid response.");
                    Status = DataStatus.BadUnknownResponse;
                    Status.Explanation = "Invalid response.";
                    return Status;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"PostStartAsync exception: {ex.Message}.");
                Status = DataStatus.BadInternalError;
                Status.Explanation = ex.Message;
                return Status;
            }
        }

        #endregion Private Methods
    }
}
