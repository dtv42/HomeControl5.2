// --------------------------------------------------------------------------------------------------------------------
// <copyright file="HeliosGateway.cs" company="DTV-Online">
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
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using UtilityLib;
    using HeliosLib.Models;

    #endregion

    /// <summary>
    /// Class holding data from the Heios easyControl Web Service (KWL EC200).
    /// The data properties are based on the specification
    /// "Helios Ventilatoren Funktions- und Schnittstellenbeschreibung"
    /// NR. 82 269 - Modbus Gateway TCP/IP mit EasyControls. Druckschrift-Nr. 82269/07.14
    /// and the XML data returned from the easyControl Web Service.
    /// </summary>
    public class HeliosGateway : BaseGateway<HeliosSettings>
    {
        #region Private Data Members

        /// <summary>
        /// The HTTP client instantiated using the HTTP client factory.
        /// </summary>
        private readonly HeliosClient _client;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the Helios settings.
        /// </summary>
        public HeliosSettings Settings { get => _settings; }

        /// <summary>
        /// Indicates startup completed.
        /// </summary>
        public new bool IsStartupOk { get; private set; }


        /// <summary>
        /// The Data property holds all Helios data properties.
        /// </summary>
        [JsonIgnore]
        public HeliosData Data { get; } = new HeliosData();

        /// <summary>
        /// The BoosterData holds all Helios booster data.
        /// </summary>
        [JsonIgnore]
        public BoosterData BoosterData { get; } = new BoosterData();

        /// <summary>
        /// The DeviceData holds all Helios device data.
        /// </summary>
        [JsonIgnore]
        public DeviceData DeviceData { get; } = new DeviceData();

        /// <summary>
        /// The DisplayData holds all Helios current system data.
        /// </summary>
        [JsonIgnore]
        public DisplayData DisplayData { get; } = new DisplayData();

        /// <summary>
        /// The ErrorData holds all Helios error data.
        /// </summary>
        [JsonIgnore]
        public ErrorData ErrorData { get; } = new ErrorData();

        /// <summary>
        /// The FanData holds all Helios fan data.
        /// </summary>
        [JsonIgnore]
        public FanData FanData { get; } = new FanData();

        /// <summary>
        /// The HeaterData holds all Helios heater data.
        /// </summary>
        [JsonIgnore]
        public HeaterData HeaterData { get; } = new HeaterData();

        /// <summary>
        /// The InfoData holds all Helios info data.
        /// </summary>
        [JsonIgnore]
        public InfoData InfoData { get; } = new InfoData();

        /// <summary>
        /// The NetworkData holds all Helios network data.
        /// </summary>
        [JsonIgnore]
        public NetworkData NetworkData { get; } = new NetworkData();

        /// <summary>
        /// The OperationData holds all Helios initial operation data.
        /// </summary>
        [JsonIgnore]
        public OperationData OperationData { get; } = new OperationData();

        /// <summary>
        /// The SensorData holds all Helios sensor data.
        /// </summary>
        [JsonIgnore]
        public SensorData SensorData { get; } = new SensorData();

        /// <summary>
        /// The SystemData holds all Helios system data.
        /// </summary>
        [JsonIgnore]
        public SystemData SystemData { get; } = new SystemData();

        /// <summary>
        /// The TechnicalData holds all Helios technical data.
        /// </summary>
        [JsonIgnore]
        public TechnicalData TechnicalData { get; } = new TechnicalData();

        /// <summary>
        /// The VacationData holds all Helios vacation data.
        /// </summary>
        [JsonIgnore]
        public VacationData VacationData { get; } = new VacationData();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="HeliosGateway"/> class.
        /// </summary>
        /// <param name="client">The custom HTTP client.</param>
        /// <param name="logger">The application logger instance.</param>
        /// <param name="options">The Helios settings.</param>
        public HeliosGateway(HeliosClient client,
                             HeliosSettings settings,
                             ILogger<HeliosGateway> logger)
            : base(settings, logger)
        {
            _logger?.LogDebug($"HeliosGateway()");

            _client = client;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Synchronous methods.
        /// </summary>
        public DataStatus ReadAll() => ReadAllAsync().Result;
        public DataStatus ReadBoosterData() => ReadBoosterDataAsync().Result;
        public DataStatus ReadDeviceData() => ReadDeviceDataAsync().Result;
        public DataStatus ReadDisplayData() => ReadDisplayDataAsync().Result;
        public DataStatus ReadErrorData() => ReadErrorDataAsync().Result;
        public DataStatus ReadFanData() => ReadFanDataAsync().Result;
        public DataStatus ReadHeaterData() => ReadHeaterDataAsync().Result;
        public DataStatus ReadInfoData() => ReadInfoDataAsync().Result;
        public DataStatus ReadNetworkData() => ReadNetworkDataAsync().Result;
        public DataStatus ReadOperationData() => ReadOperationDataAsync().Result;
        public DataStatus ReadSensorData() => ReadSensorDataAsync().Result;
        public DataStatus ReadSystemData() => ReadSystemDataAsync().Result;
        public DataStatus ReadTechnicalData() => ReadTechnicalDataAsync().Result;
        public DataStatus ReadVacationData() => ReadVacationDataAsync().Result;
        public bool Login() => LoginAsync().Result;
        public bool SetOperationMode(OperationModes mode) => SetOperationModeAsync(mode).Result;
        public bool SetFanLevel(FanLevels level) => SetFanLevelAsync(level).Result;
        public bool SetBooster(VentilationData data) => SetBoosterAsync(data).Result;
        public bool SetBoosterMode(bool mode) => SetBoosterModeAsync(mode).Result;
        public bool SetBoosterLevel(FanLevels level) => SetBoosterLevelAsync(level).Result;
        public bool SetBoosterDuration(int duration) => SetBoosterDurationAsync(duration).Result;
        public bool SetStandby(VentilationData data) => SetStandbyAsync(data).Result;
        public bool SetStandbyMode(bool mode) => SetStandbyModeAsync(mode).Result;
        public bool SetStandbyLevel(FanLevels level) => SetStandbyLevelAsync(level).Result;
        public bool SetStandbyDuration(int duration) => SetStandbyDurationAsync(duration).Result;

        /// <summary>
        /// Updates all Helios properties reading the data from the Helios web service.
        /// If successful the data values will be updated (timestamp).
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> ReadAllAsync()
        {
            _logger?.LogDebug("ReadAllAsync() starting.");
            Status = DataStatus.Good;
            bool Ok = true;

            try
            {
                Ok &= (await ReadBoosterDataAsync()).IsGood;
                Ok &= (await ReadDeviceDataAsync()).IsGood;
                Ok &= (await ReadDisplayDataAsync()).IsGood;
                Ok &= (await ReadErrorDataAsync()).IsGood;
                Ok &= (await ReadFanDataAsync()).IsGood;
                Ok &= (await ReadHeaterDataAsync()).IsGood;
                Ok &= (await ReadInfoDataAsync()).IsGood;
                Ok &= (await ReadNetworkDataAsync()).IsGood;
                Ok &= (await ReadOperationDataAsync()).IsGood;
                Ok &= (await ReadSensorDataAsync()).IsGood;
                Ok &= (await ReadTechnicalDataAsync()).IsGood;
                Ok &= (await ReadVacationDataAsync()).IsGood;

                if (!Ok)
                {
                    _logger?.LogError("ReadAllAsync() invalid response.");
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
                _logger?.LogDebug("ReadAllAsync() finished.");
            }

            return Status;
        }

        /// <summary>
        /// Updates all Helios booster data properties reading the data from the Helios web service.
        /// If successful the data value will be updated (timestamp).
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> ReadBoosterDataAsync()
        {
            _logger?.LogDebug("ReadBoosterDataAsync() starting.");
            await LockAsync();
            Status = DataStatus.Good;

            try
            {
                if (await LoginAsync())
                {
                    string xml = await _client.PostStringAsync("/data/werte3.xml", "xml=/data/werte3.xml");

                    if (!string.IsNullOrEmpty(xml))
                    {
                        Data.Update(new RawData(xml));
                        BoosterData.Refresh(Data);
                    }
                    else
                    {
                        _logger?.LogError("ReadBoosterDataAsync() invalid response.");
                        Status = DataStatus.BadCommunicationError;
                    }
                }
                else
                {
                    _logger?.LogError("ReadBoosterDataAsync() invalid response.");
                    Status = DataStatus.BadResourceUnavailable;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Exception in ReadBoosterDataAsync().");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug("ReadBoosterDataAsync() finished.");
            }

            return Status;
        }

        /// <summary>
        /// Updates all Helios device data properties reading the data from the Helios web service.
        /// If successful the data value will be updated (timestamp).
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> ReadDeviceDataAsync()
        {
            _logger?.LogDebug("ReadDeviceDataAsync() starting.");
            await LockAsync();
            Status = DataStatus.Good;

            try
            {
                if (await LoginAsync())
                {
                    string xml = await _client.PostStringAsync("/data/werte12.xml", "xml=/data/werte12.xml");

                    if (!string.IsNullOrEmpty(xml))
                    {
                        Data.Update(new RawData(xml));
                        DeviceData.Refresh(Data);
                    }
                    else
                    {
                        _logger?.LogError("ReadDeviceDataAsync() invalid response.");
                        Status = DataStatus.BadCommunicationError;
                    }
                }
                else
                {
                    _logger?.LogError("ReadDeviceDataAsync() invalid response.");
                    Status = DataStatus.BadResourceUnavailable;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Exception in ReadDeviceDataAsync().");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug("ReadDeviceDataAsync() finished.");
            }

            return Status;
        }

        /// <summary>
        /// Updates all Helios display data properties reading the data from the Helios web service.
        /// If successful the data value will be updated (timestamp).
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> ReadDisplayDataAsync()
        {
            _logger?.LogDebug("ReadDisplayDataAsync() starting.");
            await LockAsync();
            Status = DataStatus.Good;

            try
            {
                if (await LoginAsync())
                {
                    string xml = await _client.PostStringAsync("/data/werte8.xml", "xml=/data/werte8.xml");

                    if (!string.IsNullOrEmpty(xml))
                    {
                        Data.Update(new RawData(xml));
                        DisplayData.Refresh(Data);
                    }
                    else
                    {
                        _logger?.LogError("ReadDisplayDataAsync() invalid response.");
                        Status = DataStatus.BadCommunicationError;
                    }
                }
                else
                {
                    _logger?.LogError("ReadDisplayDataAsync() invalid response.");
                    Status = DataStatus.BadResourceUnavailable;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Exception in ReadDisplayDataAsync().");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug("ReadDisplayDataAsync() finished.");
            }

            return Status;
        }

        /// <summary>
        /// Updates all Helios current error data properties reading the data from the Helios web service.
        /// If successful the data value will be updated (timestamp).
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> ReadErrorDataAsync()
        {
            _logger?.LogDebug("ReadErrorDataAsync() starting.");
            await LockAsync();
            Status = DataStatus.Good;

            try
            {
                if (await LoginAsync())
                {
                    string xml = await _client.PostStringAsync("/data/werte16.xml", "xml=/data/werte16.xml");

                    if (!string.IsNullOrEmpty(xml))
                    {
                        Data.Update(new RawData(xml));
                        ErrorData.Refresh(Data);
                    }
                    else
                    {
                        _logger?.LogError("ReadErrorDataAsync() invalid response.");
                        Status = DataStatus.BadCommunicationError;
                    }
                }
                else
                {
                    _logger?.LogError("ReadErrorDataAsync() invalid response.");
                    Status = DataStatus.BadResourceUnavailable;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Exception in ReadErrorDataAsync().");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug("ReadErrorDataAsync() finished.");
            }

            return Status;
        }

        /// <summary>
        /// Updates all Helios fan data properties reading the data from the Helios web service.
        /// If successful the data value will be updated (timestamp).
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> ReadFanDataAsync()
        {
            _logger?.LogDebug("ReadFanDataAsync() starting.");
            await LockAsync();
            Status = DataStatus.Good;

            try
            {
                if (await LoginAsync())
                {
                    string xml = await _client.PostStringAsync("/data/werte13.xml", "xml=/data/werte13.xml");

                    if (!string.IsNullOrEmpty(xml))
                    {
                        Data.Update(new RawData(xml));
                        FanData.Refresh(Data);
                    }
                    else
                    {
                        _logger?.LogError("ReadFanDataAsync() invalid response.");
                        Status = DataStatus.BadCommunicationError;
                    }
                }
                else
                {
                    _logger?.LogError("ReadFanDataAsync() invalid response.");
                    Status = DataStatus.BadResourceUnavailable;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Exception in ReadFanDataAsync().");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug("ReadFanDataAsync() finished.");
            }

            return Status;
        }

        /// <summary>
        /// Updates all Helios heater data properties reading the data from the Helios web service.
        /// If successful the data value will be updated (timestamp).
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> ReadHeaterDataAsync()
        {
            _logger?.LogDebug("ReadHeaterDataAsync() starting.");
            await LockAsync();
            Status = DataStatus.Good;

            try
            {
                if (await LoginAsync())
                {
                    string xml = await _client.PostStringAsync("/data/werte5.xml", "xml=/data/werte5.xml");

                    if (!string.IsNullOrEmpty(xml))
                    {
                        Data.Update(new RawData(xml));
                        HeaterData.Refresh(Data);
                    }
                    else
                    {
                        _logger?.LogError("ReadHeaterDataAsync() invalid response.");
                        Status = DataStatus.BadCommunicationError;
                    }
                }
                else
                {
                    _logger?.LogError("ReadHeaterDataAsync() invalid response.");
                    Status = DataStatus.BadResourceUnavailable;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Exception in ReadHeaterDataAsync().");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug("ReadHeaterDataAsync() finished.");
            }

            return Status;
        }

        /// <summary>
        /// Updates all Helios info data properties reading the data from the Helios web service.
        /// If successful the data value will be updated (timestamp).
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> ReadInfoDataAsync()
        {
            _logger?.LogDebug("ReadInfoDataAsync() starting.");
            await LockAsync();
            Status = DataStatus.Good;

            try
            {
                if (await LoginAsync())
                {
                    string xml = await _client.PostStringAsync("/data/werte4.xml", "xml=/data/werte4.xml");

                    if (!string.IsNullOrEmpty(xml))
                    {
                        Data.Update(new RawData(xml));
                        InfoData.Refresh(Data);
                    }
                    else
                    {
                        _logger?.LogError("ReadInfoDataAsync() invalid response.");
                        Status = DataStatus.BadCommunicationError;
                    }
                }
                else
                {
                    _logger?.LogError("ReadInfoDataAsync() invalid response.");
                    Status = DataStatus.BadResourceUnavailable;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Exception in ReadInfoDataAsync().");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug("ReadInfoDataAsync() finished.");
            }

            return Status;
        }

        /// <summary>
        /// Updates all Helios network data properties reading the data from the Helios web service.
        /// If successful the data value will be updated (timestamp).
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> ReadNetworkDataAsync()
        {
            _logger?.LogDebug("ReadNetworkDataAsync() starting.");
            await LockAsync();
            Status = DataStatus.Good;

            try
            {
                if (await LoginAsync())
                {
                    string xml = await _client.PostStringAsync("/data/werte10.xml", "xml=/data/werte10.xml");

                    if (!string.IsNullOrEmpty(xml))
                    {
                        Data.Update(new RawData(xml));
                        NetworkData.Refresh(Data);
                    }
                    else
                    {
                        _logger?.LogError("ReadNetworkDataAsync() invalid response.");
                        Status = DataStatus.BadCommunicationError;
                    }
                }
                else
                {
                    _logger?.LogError("ReadNetworkDataAsync() invalid response.");
                    Status = DataStatus.BadResourceUnavailable;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Exception in ReadNetworkDataAsync().");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug("ReadNetworkDataAsync() finished.");
            }

            return Status;
        }

        /// <summary>
        /// Updates all Helios initial operation data properties reading the data from the Helios web service.
        /// If successful the data value will be updated (timestamp).
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> ReadOperationDataAsync()
        {
            _logger?.LogDebug("ReadOperationDataAsync() starting.");
            await LockAsync();
            Status = DataStatus.Good;

            try
            {
                if (await LoginAsync())
                {
                    string xml = await _client.PostStringAsync("/data/werte1.xml", "xml=/data/werte1.xml");

                    if (!string.IsNullOrEmpty(xml))
                    {
                        Data.Update(new RawData(xml));
                        OperationData.Refresh(Data);
                    }
                    else
                    {
                        _logger?.LogError("ReadOperationDataAsync() invalid response.");
                        Status = DataStatus.BadCommunicationError;
                    }
                }
                else
                {
                    _logger?.LogError("ReadOperationDataAsync() invalid response.");
                    Status = DataStatus.BadResourceUnavailable;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Exception in ReadOperationDataAsync().");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug("ReadOperationDataAsync() finished.");
            }

            return Status;
        }

        /// <summary>
        /// Updates all Helios sensor data properties reading the data from the Helios web service.
        /// If successful the data value will be updated (timestamp).
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> ReadSensorDataAsync()
        {
            _logger?.LogDebug("ReadSensorDataAsync() starting.");
            await LockAsync();
            Status = DataStatus.Good;

            try
            {
                if (await LoginAsync())
                {
                    string xml = await _client.PostStringAsync("/data/werte14.xml", "xml=/data/werte14.xml");

                    if (!string.IsNullOrEmpty(xml))
                    {
                        Data.Update(new RawData(xml));
                        SensorData.Refresh(Data);
                    }
                    else
                    {
                        _logger?.LogError("ReadSensorDataAsync() invalid response.");
                        Status = DataStatus.BadCommunicationError;
                    }
                }
                else
                {
                    _logger?.LogError("ReadSensorDataAsync() invalid response.");
                    Status = DataStatus.BadResourceUnavailable;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Exception in ReadSensorDataAsync().");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug("ReadSensorDataAsync() finished.");
            }

            return Status;
        }

        /// <summary>
        /// Updates all Helios system data properties reading the data from the Helios web service.
        /// If successful the data value will be updated (timestamp).
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> ReadSystemDataAsync()
        {
            _logger?.LogDebug("ReadSystemDataAsync() starting.");
            await LockAsync();
            Status = DataStatus.Good;

            try
            {
                if (await LoginAsync())
                {
                    string xml = await _client.PostStringAsync("/data/werte11.xml", "xml=/data/werte11.xml");

                    if (!string.IsNullOrEmpty(xml))
                    {
                        Data.Update(new RawData(xml));
                        SystemData.Refresh(Data);
                    }
                    else
                    {
                        _logger?.LogError("ReadSystemDataAsync() invalid response.");
                        Status = DataStatus.BadCommunicationError;
                    }
                }
                else
                {
                    _logger?.LogError("ReadSystemDataAsync() invalid response.");
                    Status = DataStatus.BadResourceUnavailable;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Exception in ReadSystemDataAsync().");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug("ReadSystemDataAsync() finished.");
            }

            return Status;
        }

        /// <summary>
        /// Updates all Helios technical data properties reading the data from the Helios web service.
        /// If successful the data value will be updated (timestamp).
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> ReadTechnicalDataAsync()
        {
            _logger?.LogDebug("ReadTechnicalDataAsync() starting.");
            await LockAsync();
            Status = DataStatus.Good;

            try
            {
                if (await LoginAsync())
                {
                    string xml = await _client.PostStringAsync("/data/werte7.xml", "xml=/data/werte7.xml");

                    if (!string.IsNullOrEmpty(xml))
                    {
                        Data.Update(new RawData(xml));
                        TechnicalData.Refresh(Data);
                    }
                    else
                    {
                        _logger?.LogError("ReadTechnicalDataAsync() invalid response.");
                        Status = DataStatus.BadCommunicationError;
                    }
                }
                else
                {
                    _logger?.LogError("ReadTechnicalDataAsync() invalid response.");
                    Status = DataStatus.BadResourceUnavailable;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Exception in ReadTechnicalDataAsync().");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug("ReadTechnicalDataAsync() finished.");
            }

            return Status;
        }

        /// <summary>
        /// Updates all Helios vacation data properties reading the data from the Helios web service.
        /// If successful the data value will be updated (timestamp).
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> ReadVacationDataAsync()
        {
            _logger?.LogDebug("ReadVacationDataAsync() starting.");
            await LockAsync();
            Status = DataStatus.Good;

            try
            {
                if (await LoginAsync())
                {
                    string xml = await _client.PostStringAsync("/data/werte6.xml", "xml=/data/werte6.xml");

                    if (!string.IsNullOrEmpty(xml))
                    {
                        Data.Update(new RawData(xml));
                        VacationData.Refresh(Data);
                    }
                    else
                    {
                        _logger?.LogError("ReadVacationDataAsync() invalid response.");
                        Status = DataStatus.BadCommunicationError;
                    }
                }
                else
                {
                    _logger?.LogError("ReadVacationDataAsync() invalid response.");
                    Status = DataStatus.BadResourceUnavailable;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Exception in ReadVacationDataAsync().");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug("ReadVacationDataAsync() finished.");
            }

            return Status;
        }

        /// <summary>
        /// Async method to try to login into the Helios web service.
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<bool> LoginAsync()
        {
            _logger?.LogDebug("LoginAsync() starting.");
            Status = DataStatus.Good;

            try
            {
                string html = await _client.PostStringAsync("/info.htm", $"v00402={_settings.Password}");

                if (!string.IsNullOrEmpty(html))
                {
                    _logger?.LogDebug("LoginAsync() OK.");
                }
                else
                {
                    _logger?.LogError("LoginAsync() invalid response.");
                    Status = DataStatus.BadCommunicationError;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"LoginAsync() exception: {ex.Message}.");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                _logger?.LogDebug("LoginAsync() finished.");
            }

            return Status.IsGood;
        }

        /// <summary>
        /// Async method to set the ventilation level using the Helios web service.
        /// </summary>
        /// <param name="mode">The operation mode</param>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<bool> SetOperationModeAsync(OperationModes mode)
        {
            _logger?.LogDebug("SetOperationModeAsync() starting.");
            await LockAsync();
            Status = DataStatus.Good;

            try
            {
                string html = await _client.PostStringAsync("/info.htm", $"v00101={(int)mode}");

                if (!string.IsNullOrEmpty(html))
                {
                    _logger?.LogDebug("SetOperationModeAsync() OK.");
                }
                else
                {
                    _logger?.LogError("SetOperationModeAsync() invalid response.");
                    Status = DataStatus.BadCommunicationError;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"SetOperationModeAsync() exception: {ex.Message}.");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug("SetOperationModeAsync() finished.");
            }

            return Status.IsGood;
        }

        /// <summary>
        /// Async method to set the ventilation level using the Helios web service.
        /// Note that the Helios ventilation unit is set to Manual operation mode.
        /// </summary>
        /// <param name="level">The ventilation level</param>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<bool> SetFanLevelAsync(FanLevels level)
        {
            _logger?.LogDebug("SetFanLevelAsync() starting.");
            await LockAsync();
            Status = DataStatus.Good;

            try
            {
                string html = await _client.PostStringAsync("/info.htm", $"&v00101=1&v00102={(int)level}");

                if (!string.IsNullOrEmpty(html))
                {
                    _logger?.LogDebug("SetFanLevelAsync() OK.");
                }
                else
                {
                    _logger?.LogError("SetFanLevelAsync() invalid response.");
                    Status = DataStatus.BadCommunicationError;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"SetFanLevelAsync() exception: {ex.Message}.");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug("SetFanLevelAsync() finished.");
            }

            return Status.IsGood;
        }

        /// <summary>
        /// Async method to set the booster (party) mode using the Helios web service.
        /// </summary>
        /// <param name="data">The ventilation data</param>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<bool> SetBoosterAsync(VentilationData data)
        {
            _logger?.LogDebug("SetBoosterAsync() starting.");
            await LockAsync();
            Status = DataStatus.Good;

            try
            {
                string post = $"v00091={data.Duration}&v00092={(int)data.Level}&v00094={(data.Mode ? 1 : 0)}";
                string html = await _client.PostStringAsync("/party.htm", post);

                if (!string.IsNullOrEmpty(html))
                {
                    _logger?.LogDebug("SetBoosterAsync() OK.");
                }
                else
                {
                    _logger?.LogError("SetBoosterAsync() invalid response.");
                    Status = DataStatus.BadCommunicationError;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"SetBoosterAsync() exception: {ex.Message}.");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug("SetBoosterAsync() finished.");
            }

            return Status.IsGood;
        }

        /// <summary>
        /// Async method to set the booster (party) mode using the Helios web service.
        /// </summary>
        /// <param name="mode">The booster mode</param>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<bool> SetBoosterModeAsync(bool mode)
        {
            _logger?.LogDebug("SetBoosterModeAsync() starting.");
            await LockAsync();
            Status = DataStatus.Good;

            try
            {
                string post = $"v00094={(mode ? 1 : 0)}";
                string html = await _client.PostStringAsync("/party.htm", post);

                if (!string.IsNullOrEmpty(html))
                {
                    _logger?.LogDebug("SetBoosterModeAsync() OK.");
                }
                else
                {
                    _logger?.LogError("SetBoosterModeAsync() invalid response.");
                    Status = DataStatus.BadCommunicationError;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"SetBoosterModeAsync() exception: {ex.Message}.");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug("SetBoosterModeAsync() finished.");
            }

            return Status.IsGood;
        }

        /// <summary>
        /// Async method to set the booster (party) mode ventilation level using the Helios web service.
        /// </summary>
        /// <param name="level">The ventilation level</param>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<bool> SetBoosterLevelAsync(FanLevels level)
        {
            _logger?.LogDebug("SetBoosterLevelAsync() starting.");
            await LockAsync();
            Status = DataStatus.Good;

            try
            {
                string post = $"v00092={(int)level}";
                string html = await _client.PostStringAsync("/party.htm", post);

                if (!string.IsNullOrEmpty(html))
                {
                    _logger?.LogDebug("SetBoosterLevelAsync() OK.");
                }
                else
                {
                    _logger?.LogError("SetBoosterLevelAsync() invalid response.");
                    Status = DataStatus.BadCommunicationError;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"SetBoosterLevelAsync() exception: {ex.Message}.");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug("SetBoosterLevelAsync() finished.");
            }

            return Status.IsGood;
        }

        /// <summary>
        /// Async method to set the booster (party) mode using the Helios web service.
        /// </summary>
        /// <param name="duration">The booster mode duration</param>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<bool> SetBoosterDurationAsync(int duration)
        {
            _logger?.LogDebug("SetBoosterDurationAsync() starting.");
            await LockAsync();
            Status = DataStatus.Good;

            try
            {
                string post = $"v00091={duration}";
                string html = await _client.PostStringAsync("/party.htm", post);

                if (!string.IsNullOrEmpty(html))
                {
                    _logger?.LogDebug("SetBoosterDurationAsync() OK.");
                }
                else
                {
                    _logger?.LogError("SetBoosterDurationAsync() invalid response.");
                    Status = DataStatus.BadCommunicationError;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"SetBoosterDurationAsync() exception: {ex.Message}.");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug("SetBoosterDurationAsync() finished.");
            }

            return Status.IsGood;
        }

        /// <summary>
        /// Async method to set the standby (whisper) mode using the Helios web service.
        /// </summary>
        /// <param name="data">The ventilation data</param>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<bool> SetStandbyAsync(VentilationData data)
        {
            _logger?.LogDebug("SetStandbyAsync() starting.");
            await LockAsync();
            Status = DataStatus.Good;

            try
            {
                string post = $"v00096={data.Duration}&v00097={(int)data.Level}&v00099={(data.Mode ? 1 : 0)}";
                string html = await _client.PostStringAsync("/ruhe.htm", post);

                if (!string.IsNullOrEmpty(html))
                {
                    _logger?.LogDebug("SetStandbyAsync() OK.");
                }
                else
                {
                    _logger?.LogError("SetStandbyAsync() invalid response.");
                    Status = DataStatus.BadCommunicationError;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"SetStandbyAsync() exception: {ex.Message}.");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug("SetStandbyAsync() finished.");
            }

            return Status.IsGood;
        }

        /// <summary>
        /// Async method to set the standby (whisper) mode using the Helios web service.
        /// </summary>
        /// <param name="mode">The standby mode</param>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<bool> SetStandbyModeAsync(bool mode)
        {
            _logger?.LogDebug("SetStandbyModeAsync() starting.");
            await LockAsync();
            Status = DataStatus.Good;

            try
            {
                string post = $"v00099={(mode ? 1 : 0)}";
                string html = await _client.PostStringAsync("/ruhe.htm", post);

                if (!string.IsNullOrEmpty(html))
                {
                    _logger?.LogDebug("SetStandbyModeAsync() OK.");
                }
                else
                {
                    _logger?.LogError("SetStandbyModeAsync() invalid response.");
                    Status = DataStatus.BadCommunicationError;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"SetStandbyModeAsync() exception: {ex.Message}.");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug("SetStandbyModeAsync() finished.");
            }

            return Status.IsGood;
        }

        /// <summary>
        /// Async method to set the standby (whisper) mode using the Helios web service.
        /// </summary>
        /// <param name="level">The ventilation level</param>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<bool> SetStandbyLevelAsync(FanLevels level)
        {
            _logger?.LogDebug("SetStandbyLevelAsync() starting.");
            await LockAsync();
            Status = DataStatus.Good;

            try
            {
                string post = $"v00097={(int)level}";
                string html = await _client.PostStringAsync("/ruhe.htm", post);

                if (!string.IsNullOrEmpty(html))
                {
                    _logger?.LogDebug("SetStandbyLevelAsync() OK.");
                }
                else
                {
                    _logger?.LogError("SetStandbyLevelAsync() invalid response.");
                    Status = DataStatus.BadCommunicationError;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"SetStandbyLevelAsync() exception: {ex.Message}.");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug("SetStandbyLevelAsync() finished.");
            }

            return Status.IsGood;
        }

        /// <summary>
        /// Async method to set the standby (whisper) mode using the Helios web service.
        /// </summary>
        /// <param name="duration">The standby mode duration</param>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<bool> SetStandbyDurationAsync(int duration)
        {
            _logger?.LogDebug("SetStandbyDurationAsync() starting.");
            await LockAsync();
            Status = DataStatus.Good;

            try
            {
                string post = $"v00096={duration}";
                string html = await _client.PostStringAsync("/ruhe.htm", post);

                if (!string.IsNullOrEmpty(html))
                {
                    _logger?.LogDebug("SetStandbyDurationAsync() OK.");
                }
                else
                {
                    _logger?.LogError("SetStandbyDurationAsync() invalid response.");
                    Status = DataStatus.BadCommunicationError;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"SetStandbyDurationAsync() exception: {ex.Message}.");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug("SetStandbyDurationAsync() finished.");
            }

            return Status.IsGood;
        }

        /// <summary>
        /// Tries to get all data from the Helios web service.
        /// </summary>
        /// <returns>Flag indicating success or failure.</returns>
        public override bool Startup()
        {
            _logger.LogInformation($"Helios Gateway Starting...");
            IsStartupOk = ReadAll().IsGood;

            if (IsStartupOk)
            {
                _logger.LogInformation("Helios Gateway:");
                _logger.LogInformation($"    Base Address: {_settings.BaseAddress}");
                _logger.LogInformation($"    Password:     {_settings.Password}");
                _logger.LogInformation($"Startup OK");
            }
            else
            {
                _logger.LogWarning("Helios Gateway: Startup not OK");
            }

            return IsStartupOk;
        }

        /// <summary>
        /// Tries to connect to the Helios web service.
        /// </summary>
        /// <returns>Flag indicating success or failure.</returns>
        public override bool CheckAccess()
            => (LoginAsync().Result);

        /// <summary>
        /// Updates the client using the HeliosSettings instance.
        /// </summary>
        public void UpdateClient()
            => _client.Update();

        #endregion
    }
}
