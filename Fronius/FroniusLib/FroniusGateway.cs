// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Fronius.cs" company="DTV-Online">
//   Copyright(c) 2017 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
// Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// --------------------------------------------------------------------------------------------------------------------
namespace FroniusLib
{
    #region Using Directives

    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    using UtilityLib;
    using FroniusLib.Models;

    #endregion

    /// <summary>
    /// Class holding data from the Fronius Symo 8.2-3-M inverter.
    /// The data properties are based on the specification Fronius
    /// Solar API V1 (42,0410,2012,EN 008-15092016).
    /// </summary>
    public class FroniusGateway : BaseGateway<FroniusSettings>
    {
        #region Private Data Members

        /// <summary>
        /// The HTTP client instantiated using the HTTP client factory.
        /// </summary>
        private readonly FroniusClient _client;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the Fronius settings.
        /// </summary>
        public FroniusSettings Settings { get => _settings; }

        /// <summary>
        /// Indicates startup completed.
        /// </summary>
        public new bool IsStartupOk { get; private set; }


        /// <summary>
        /// The Data property holds all Fronius data properties.
        /// </summary>
        [JsonIgnore]
        public FroniusData Data { get; } = new FroniusData();

        /// <summary>
        /// The InverterInfo holds all Fronius inverter data.
        /// </summary>
        [JsonIgnore]
        public InverterInfo InverterInfo { get; } = new InverterInfo();

        /// <summary>
        /// The CommonData property holds all Fronius common data.
        /// </summary>
        [JsonIgnore]
        public CommonData CommonData { get; } = new CommonData();

        /// <summary>
        /// The PhaseData property holds all Fronius phase data.
        /// </summary>
        [JsonIgnore]
        public PhaseData PhaseData { get; } = new PhaseData();

        /// <summary>
        /// The MinMaxData property holds all Fronius minmax data.
        /// </summary>
        [JsonIgnore]
        public MinMaxData MinMaxData { get; } = new MinMaxData();

        /// <summary>
        /// The LoggerInfo property holds all Fronius logger info data.
        /// </summary>
        [JsonIgnore]
        public LoggerInfo LoggerInfo { get; } = new LoggerInfo();

        /// <summary>
        /// Gets or sets the Fronius API version.
        /// </summary>
        [JsonIgnore]
        public APIVersionData VersionInfo { get; private set; } = new APIVersionData();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="FroniusGateway"/> class.
        /// </summary>
        /// <param name="client">The custom HTTP client.</param>
        /// <param name="logger">The application logger instance.</param>
        /// <param name="options">The Fronius settings.</param>
        public FroniusGateway(FroniusClient client,
                              FroniusSettings settings,
                              ILogger<FroniusGateway> logger)
            : base(settings, logger)
        {
            _logger?.LogDebug($"FroniusGateway()");

            _client = client;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Synchronous methods.
        /// </summary>
        public DataStatus ReadAll() => ReadAllAsync().Result;
        public DataStatus ReadInverterInfo() => ReadInverterInfoAsync().Result;
        public DataStatus ReadCommonData() => ReadCommonDataAsync().Result;
        public DataStatus ReadPhaseData() => ReadPhaseDataAsync().Result;
        public DataStatus ReadMinMaxData() => ReadMinMaxDataAsync().Result;
        public DataStatus ReadLoggerInfo() => ReadLoggerInfoAsync().Result;
        public DataStatus GetAPIVersion() => GetAPIVersionAsync().Result;

        /// <summary>
        /// Updates all Fronius properties reading the data from the Fronius web service.
        /// If successful the data values will be updated (timestamp).
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> ReadAllAsync()
        {
            _logger?.LogDebug("ReadAllAsync() starting.");
            Status = DataStatus.Uncertain;
            bool Ok = true;

            try
            {
                Ok &= (await ReadCommonDataAsync()).IsGood;
                Ok &= (await ReadInverterInfoAsync()).IsGood;
                Ok &= (await ReadLoggerInfoAsync()).IsGood;
                Ok &= (await ReadMinMaxDataAsync()).IsGood;
                Ok &= (await ReadPhaseDataAsync()).IsGood;

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
        /// Updates all Fronius inverter info properties reading the data from the Fronius web service.
        /// If successful the data value will be updated (timestamp).
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> ReadInverterInfoAsync()
        {
            _logger?.LogDebug("ReadInverterInfoAsync() starting.");
            await LockAsync();
            Status = DataStatus.Good;

            try
            {
                string json = await _client.GetStringAsync($"solar_api/v1/GetInverterInfo.cgi");

                if (!string.IsNullOrEmpty(json))
                {
                    var raw = JsonSerializer.Deserialize<FroniusInverterInfo>(json);

                    if (!(raw is null))
                    {
                        Data.InverterInfo = new InverterDeviceData(raw);

                        if (Data.InverterInfo.Response.Status.Code == 0)
                        {
                            InverterInfo.Refresh(Data.InverterInfo);
                            _logger?.LogDebug("ReadInverterInfoAsync() OK.");
                        }
                        else
                        {
                            _logger?.LogError($"Fronius status code: {Data.InverterInfo.Response.Status.Code}.");
                            _logger?.LogDebug($"ReadInverterInfoAsync() not OK.");
                            Status = DataStatus.BadDeviceFailure;
                            Status.Explanation = $"Fronius status message: {Data.InverterInfo.Response.Status.UserMessage}.";
                        }
                    }
                    else
                    {
                        _logger?.LogError("ReadInverterInfoAsync() deserialize error.");
                        Status = DataStatus.BadDecodingError;
                    }
                }
                else
                {
                    _logger?.LogError("ReadInverterInfoAsync() invalid response.");
                    Status = DataStatus.BadCommunicationError;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Exception in ReadInverterInfoAsync().");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug("ReadInverterInfoAsync() finished.");
            }

            return Status;
        }

        /// <summary>
        /// Updates all Fronius common data properties reading the data from the Fronius web service.
        /// If successful the data value will be updated (timestamp).
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> ReadCommonDataAsync()
        {
            _logger?.LogDebug("ReadCommonDataAsync() starting.");
            await LockAsync();
            Status = DataStatus.Good;

            try
            {
                string json = await _client.GetStringAsync($"solar_api/v1/GetInverterRealtimeData.cgi?Scope=Device&DeviceId={_settings.DeviceID}&DataCollection=CommonInverterData");

                if (!string.IsNullOrEmpty(json))
                {
                    var raw = JsonSerializer.Deserialize<FroniusCommonData>(json);

                    if (!(raw is null))
                    {
                        Data.CommonData = new CommonDeviceData(raw);

                        if (Data.CommonData.Response.Status.Code == 0)
                        {
                            CommonData.Refresh(Data.CommonData);
                            _logger?.LogDebug("ReadCommonDataAsync() OK.");
                        }
                        else
                        {
                            _logger?.LogError($"Fronius status code: {Data.CommonData.Response.Status.Code}.");
                            _logger?.LogDebug($"ReadCommonDataAsync() not OK.");
                            Status = DataStatus.BadDeviceFailure;
                            Status.Explanation = $"Fronius status message: {Data.CommonData.Response.Status.UserMessage}.";
                        }
                    }
                    else
                    {
                        _logger?.LogError("ReadCommonDataAsync() deserialize error.");
                        Status = DataStatus.BadDecodingError;
                    }
                }
                else
                {
                    _logger?.LogError("ReadCommonDataAsync() invalid response.");
                    Status = DataStatus.BadCommunicationError;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"ReadCommonDataAsync() exception: {ex.Message}.");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug("ReadCommonDataAsync() finished.");
            }

            return Status;
        }

        /// <summary>
        /// Updates all Fronius phase data properties reading the data from the Fronius web service.
        /// If successful the data value will be updated (timestamp).
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> ReadPhaseDataAsync()
        {
            _logger?.LogDebug("ReadPhaseDataAsync() starting.");
            await LockAsync();
            Status = DataStatus.Good;

            try
            {
                string json = await _client.GetStringAsync($"solar_api/v1/GetInverterRealtimeData.cgi?Scope=Device&DeviceId={_settings.DeviceID}&DataCollection=3PInverterData");

                if (!string.IsNullOrEmpty(json))
                {
                    var raw = JsonSerializer.Deserialize<FroniusPhaseData>(json);

                    if (!(raw is null))
                    {
                        Data.PhaseData = new PhaseDeviceData(raw);

                        if (Data.PhaseData.Response.Status.Code == 0)
                        {
                            PhaseData.Refresh(Data.PhaseData);
                            _logger?.LogDebug("ReadPhaseDataAsync() OK.");
                        }
                        else
                        {
                            _logger?.LogError($"Fronius status code: {Data.PhaseData.Response.Status.Code}.");
                            _logger?.LogDebug($"ReadPhaseDataAsync() not OK.");
                            Status = DataStatus.BadDeviceFailure;
                            Status.Explanation = $"Fronius status message: {Data.PhaseData.Response.Status.UserMessage}.";
                        }
                    }
                    else
                    {
                        _logger?.LogError("ReadPhaseDataAsync() deserialize error.");
                        Status = DataStatus.BadDecodingError;
                    }
                }
                else
                {
                    _logger?.LogError("ReadPhaseDataAsync() invalid response.");
                    Status = DataStatus.BadCommunicationError;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"ReadPhaseDataAsync() exception: {ex.Message}.");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug("ReadPhaseDataAsync() finished.");
            }

            return Status;
        }

        /// <summary>
        /// Updates all Fronius minmax data properties reading the data from the Fronius web service.
        /// If successful the data value will be updated (timestamp).
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> ReadMinMaxDataAsync()
        {
            _logger?.LogDebug("ReadMinMaxDataAsync() starting.");
            await LockAsync();
            Status = DataStatus.Good;

            try
            {
                string json = await _client.GetStringAsync($"solar_api/v1/GetInverterRealtimeData.cgi?Scope=Device&DeviceId={_settings.DeviceID}&DataCollection=MinMaxInverterData");

                if (!string.IsNullOrEmpty(json))
                {
                    var raw = JsonSerializer.Deserialize<FroniusMinMaxData>(json);

                    if (!(raw is null))
                    {
                        Data.MinMaxData = new MinMaxDeviceData(raw);

                        if (Data.MinMaxData.Response.Status.Code == 0)
                        {
                            MinMaxData.Refresh(Data.MinMaxData);
                            _logger?.LogDebug("ReadMinMaxDataAsync() OK.");
                        }
                        else
                        {
                            _logger?.LogError($"Fronius status code: {Data.MinMaxData.Response.Status.Code}.");
                            _logger?.LogDebug($"ReadMinMaxDataAsync() not OK.");
                            Status = DataStatus.BadDeviceFailure;
                            Status.Explanation = $"Fronius status message: {Data.MinMaxData.Response.Status.UserMessage}.";
                        }
                    }
                    else
                    {
                        _logger?.LogError("ReadMinMaxDataAsync() deserialize error.");
                        Status = DataStatus.BadDecodingError;
                    }
                }
                else
                {
                    _logger?.LogError("ReadMinMaxDataAsync() invalid response.");
                    Status = DataStatus.BadCommunicationError;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"ReadMinMaxDataAsync() exception: {ex.Message}.");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug("ReadMinMaxDataAsync() finished.");
            }

            return Status;
        }

        /// <summary>
        /// Updates all Fronius inverter info properties reading the data from the Fronius web service.
        /// If successful the data value will be updated (timestamp).
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> ReadLoggerInfoAsync()
        {
            _logger?.LogDebug("ReadLoggerInfoAsync() starting.");
            await LockAsync();
            Status = DataStatus.Good;

            try
            {
                string json = await _client.GetStringAsync($"solar_api/v1/GetInverterRealtimeData.cgi?Scope=Device&DeviceId={_settings.DeviceID}&DataCollection=3PInverterData");

                if (!string.IsNullOrEmpty(json))
                {
                    var raw = JsonSerializer.Deserialize<FroniusLoggerInfo>(json);

                    if (!(raw is null))
                    {
                        Data.LoggerInfo = new LoggerDeviceData(raw);

                        if (Data.LoggerInfo.Response.Status.Code == 0)
                        {
                            LoggerInfo.Refresh(Data.LoggerInfo);
                            _logger?.LogDebug("ReadLoggerInfoAsync() OK.");
                        }
                        else
                        {
                            _logger?.LogError($"Fronius status code: {Data.LoggerInfo.Response.Status.Code}.");
                            _logger?.LogDebug($"ReadLoggerInfoAsync() not OK.");
                            Status = DataStatus.BadDeviceFailure;
                            Status.Explanation = $"Fronius status message: {Data.LoggerInfo.Response.Status.UserMessage}.";
                        }
                    }
                    else
                    {
                        _logger?.LogError("ReadLoggerInfoAsync() deserialize error.");
                        Status = DataStatus.BadDecodingError;
                    }
                }
                else
                {
                    _logger?.LogError("ReadLoggerInfoAsync() invalid response.");
                    Status = DataStatus.BadCommunicationError;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"ReadLoggerInfoAsync() exception: {ex.Message}.");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug("ReadLoggerInfoAsync() finished.");
            }

            return Status;
        }

        /// <summary>
        /// Async method to retrieve the Fronius web API version.
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> GetAPIVersionAsync()
        {
            _logger?.LogDebug("GetAPIVersionAsync() starting.");
            await LockAsync();
            Status = DataStatus.Good;

            try
            {
                string json = await _client.GetStringAsync($"solar_api/GetAPIVersion.cgi");

                if (!string.IsNullOrEmpty(json))
                {
                    VersionInfo = JsonSerializer.Deserialize<APIVersionData>(json) ?? VersionInfo;
                    _logger?.LogDebug("GetAPIVersionAsync() OK.");
                }
                else
                {
                    _logger?.LogError("GetAPIVersionAsync() invalid response.");
                    Status = DataStatus.BadCommunicationError;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"GetAPIVersionAsync() exception: {ex.Message}.");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug("GetAPIVersionAsync() finished.");
            }

            return Status;
        }

        /// <summary>
        /// Tries to get all data from the Fronius web service.
        /// </summary>
        /// <returns>Flag indicating success or failure.</returns>
        public override bool Startup()
        {
            _logger.LogInformation($"Fronius Symo Gateway Starting...");
            IsStartupOk = ReadAll().IsGood;

            if (IsStartupOk)
            {
                _logger.LogInformation("Fronius Symo Gateway:");
                _logger.LogInformation($"    Base Address: {_settings.BaseAddress}");
                _logger.LogInformation($"    Device ID:    {_settings.DeviceID}");
                _logger.LogInformation($"Startup OK");
            }
            else
            {
                _logger.LogWarning("Fronius Symo Gateway: Startup not OK");
            }

            return IsStartupOk;
        }

        /// <summary>
        /// Tries to connect to the Fronius web service.
        /// </summary>
        /// <returns>Flag indicating success or failure.</returns>
        public override bool CheckAccess()
            => (GetAPIVersionAsync().Result == DataStatus.Good);

        /// <summary>
        /// Updates the client using the FroniusSettings instance.
        /// </summary>
        public void UpdateClient()
            => _client.Update();

        #endregion
    }
}
