// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ETAPU11Gateway.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>17-12-2020 12:52</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace ETAPU11Lib
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using NModbus;

    using UtilityLib;
    using UtilityLib.Webapp;

    using ETAPU11Lib.Models;
    using static ETAPU11Lib.Models.ETAPU11Data;

    #endregion

    /// <summary>
    /// Class holding data from the ETA PU 11 pellet boiler unit.
    /// The value properties are based on the specification ETAtouch Modbus/TCP interface
    /// Version 1.0 ETA Heiztechnik GmbH February 25, 2014
    /// </summary>
    public class ETAPU11Gateway : BaseGateway
    {
        #region Private Fields

        /// <summary>
        /// The Modbus TCP/IP client instance.
        /// </summary>
        private readonly ETAPU11Client _client;

        /// <summary>
        /// The EM300LR client settings.
        /// </summary>
        private readonly ETAPU11Settings _settings;

        #endregion Private Fields

        #region Public Properties

        /// <summary>
        /// Gets the ETAPU11 settings.
        /// </summary>
        public ETAPU11Settings Settings { get => _settings; }

        /// <summary>
        /// Indicates startup completed.
        /// </summary>
        public new bool IsStartupOk { get; private set; }

        /// <summary>
        /// The Data property holds all ETAPU11 data properties.
        /// </summary>
        [JsonIgnore]
        public ETAPU11Data Data { get; } = new ETAPU11Data();

        /// <summary>
        /// The BoilerData property holds a subset of the Modbus data values.
        /// </summary>
        [JsonIgnore]
        public BoilerData BoilerData { get; } = new BoilerData();

        /// <summary>
        /// The HotwaterData property holds a subset of the Modbus data values.
        /// </summary>
        [JsonIgnore]
        public HotwaterData HotwaterData { get; } = new HotwaterData();

        /// <summary>
        /// The HeatingData property holds a subset of the Modbus data values.
        /// </summary>
        [JsonIgnore]
        public HeatingData HeatingData { get; } = new HeatingData();

        /// <summary>
        /// The StorageData property holds a subset of the Modbus data values.
        /// </summary>
        [JsonIgnore]
        public StorageData StorageData { get; } = new StorageData();

        /// <summary>
        /// The SystemData property holds a subset of the Modbus data values.
        /// </summary>
        [JsonIgnore]
        public SystemData SystemData { get; } = new SystemData();

        #endregion Public Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ETAPU11Gateway"/> class.
        /// </summary>
        /// <param name="client">The custom Modbus TCP client.</param>
        /// <param name="settings">The ETAPU11 settings.</param>
        /// <param name="logger">The application logger instance.</param>
        public ETAPU11Gateway(ETAPU11Client client,
                              IETAPU11Settings settings,
                              ILogger<ETAPU11Gateway> logger)
            : base(logger)
        {
            _logger?.LogDebug($"ETAPU11Gateway()");

            _settings = new ETAPU11Settings
            {
                TcpMaster = settings.TcpMaster,
                TcpSlave = settings.TcpSlave
            };
            _client = client;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Synchronous methods.
        /// </summary>
        public DataStatus ReadAll() => ReadAllAsync().Result;
        public DataStatus ReadBlockAll() => ReadBlockAllAsync().Result;
        public DataStatus ReadProperty(string property) => ReadPropertyAsync(property).Result;
        public DataStatus ReadProperties(List<string> properties) => ReadPropertiesAsync(properties).Result;
        public DataStatus ReadBoilerData() => ReadBoilerDataAsync().Result;
        public DataStatus ReadHotwaterData() => ReadHotwaterDataAsync().Result;
        public DataStatus ReadHeatingData() => ReadHeatingDataAsync().Result;
        public DataStatus ReadStorageData() => ReadStorageDataAsync().Result;
        public DataStatus ReadSystemData() => ReadSystemDataAsync().Result;
        public DataStatus WriteProperty(string property, string data) => WritePropertyAsync(property, data).Result;

        /// <summary>
        /// Updates all data properties reading the data from ETA PU 11 pellet boiler unit.
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> ReadAllAsync()
        {
            _logger?.LogDebug("ReadAllAsync() starting.");
            await LockAsync();
            Status = DataStatus.Uncertain;
            DataStatus error = DataStatus.Bad;

            try
            {
                if (_client.Connect())
                {
                    ETAPU11Data data = new ETAPU11Data();
                    bool Ok = true;

                    foreach (var property in ETAPU11Data.GetProperties())
                    {
                        if (ETAPU11Data.IsReadable(property))
                        {
                            if ((property != "DataBlock1") && (property != "DataBlock2"))
                            {
                                var status = await ReadPropertyAsync(data, property);
                                Ok &= status.IsGood;
                                if (!Ok) error = status;
                            }
                        }
                    }

                    if (Ok)
                    {
                        _logger?.LogDebug($"ReadAllAsync OK.");
                        Status = DataStatus.Good;
                    }
                    else
                    {
                        _logger?.LogDebug($"ReadAllAsync not OK: {error.Name}.");
                        Status = error;
                    }

                    Data.Refresh(data);
                    BoilerData.Refresh(data);
                    HotwaterData.Refresh(data);
                    HeatingData.Refresh(data);
                    StorageData.Refresh(data);
                    SystemData.Refresh(data);
                }
                else
                {
                    _logger?.LogError("ReadAllAsync not connected.");
                    Status = DataStatus.BadNoCommunication;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"ReadAllAsync exception.");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                if (_client.Connected)
                {
                    _client.Disconnect();
                }
                Unlock();
                _logger?.LogDebug("ReadAllAsync() finished.");
            }

            return Status;
        }

        /// <summary>
        /// Updates all data properties reading the data in blocks from ETA PU 11 pellet boiler unit.
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> ReadBlockAllAsync()
        {
            _logger?.LogDebug("ReadBlockAllAsync() starting.");
            await LockAsync();
            Status = DataStatus.Uncertain;

            try
            {
                if (_client.Connect())
                {
                    ETAPU11Data data = new ETAPU11Data
                    {
                        DataBlock1 = await _client.ReadUInt32ArrayAsync(GetOffset("DataBlock1"), GetLength("DataBlock1")),
                        DataBlock2 = await _client.ReadUInt32ArrayAsync(GetOffset("DataBlock2"), GetLength("DataBlock2")),
                    };

                    Data.Refresh(data);
                    BoilerData.Refresh(data);
                    HotwaterData.Refresh(data);
                    HeatingData.Refresh(data);
                    StorageData.Refresh(data);
                    SystemData.Refresh(data);

                    if (Status.IsGood)
                    {
                        _logger?.LogDebug("BlockReadAsync OK.");
                        Status = DataStatus.Good;
                    }
                    else
                    {
                        _logger?.LogDebug($"BlockReadAsync not OK: {Status}.");
                        Status = DataStatus.GoodResultsMayBeIncomplete;
                    }
                }
                else
                {
                    _logger?.LogError("BlockReadAsync not connected.");
                    Status = DataStatus.BadCommunicationError;
                }
            }
            catch (ArgumentNullException anx)
            {
                _logger?.LogError(anx, "ArgumentNullException in BlockReadAsync.");
                Status = DataStatus.BadOutOfRange;
                Status.Explanation = $"Exception: {anx.Message}";
            }
            catch (ArgumentOutOfRangeException aor)
            {
                _logger?.LogError(aor, "ArgumentOutOfRangeException in BlockReadAsync.");
                Status = DataStatus.BadOutOfRange;
                Status.Explanation = $"Exception: {aor.Message}";
            }
            catch (ArgumentException aex)
            {
                _logger?.LogError(aex, "ArgumentException in BlockReadAsync.");
                Status = DataStatus.BadOutOfRange;
                Status.Explanation = $"Exception: {aex.Message}";
            }
            catch (ObjectDisposedException odx)
            {
                _logger?.LogError(odx, "ObjectDisposedException in BlockReadAsync.");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {odx.Message}";
            }
            catch (FormatException fex)
            {
                _logger?.LogError(fex, "FormatException in BlockReadAsync.");
                Status = DataStatus.BadEncodingError;
                Status.Explanation = $"Exception: {fex.Message}";
            }
            catch (IOException iox)
            {
                _logger?.LogError(iox, "IOException in BlockReadAsync.");
                Status = DataStatus.BadCommunicationError;
                Status.Explanation = $"Exception: {iox.Message}";
            }
            catch (InvalidModbusRequestException imr)
            {
                _logger?.LogError(imr, "InvalidModbusRequestException in BlockReadAsync.");
                Status = DataStatus.BadCommunicationError;
                Status.Explanation = $"Exception: {imr.Message}";
            }
            catch (InvalidOperationException iox)
            {
                _logger?.LogError(iox, "InvalidOperationException in BlockReadAsync.");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {iox.Message}";
            }
            catch (SlaveException slx)
            {
                _logger?.LogError(slx, "SlaveException in BlockReadAsync.");
                Status = DataStatus.BadDeviceFailure;
                Status.Explanation = $"Exception: {slx.Message}";
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Exception in BlockReadAsync.");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                if (_client.Connected)
                {
                    _client.Disconnect();
                }
                Unlock();
                _logger?.LogDebug("ReadBlockAsync() finished.");
            }

            return Status;
        }

        /// <summary>
        /// Updates a single data property reading the data from ETA PU 11 pellet boiler unit.
        /// </summary>
        /// <param name="property">The name of the property.</param>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> ReadPropertyAsync(string property)
        {
            _logger?.LogDebug($"ReadPropertyAsync('{property}') starting.");
            Status = DataStatus.Uncertain;

            if (typeof(ETAPU11Data).IsProperty(property))
            {
                if (ETAPU11Data.IsReadable(property))
                {
                    try
                    {
                        await LockAsync();

                        if (_client.Connect())
                        {
                            Status = await ReadPropertyAsync(Data, property);

                            if (Status.IsGood)
                            {
                                _logger?.LogDebug($"ReadPropertyAsync('{property}') OK.");
                            }
                            else
                            {
                                _logger?.LogDebug($"ReadPropertyAsync('{property}') not OK.");
                            }
                        }
                        else
                        {
                            _logger?.LogError($"ReadPropertyAsync('{property}') not connected.");
                            Status = DataStatus.BadNoCommunication;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, $"ReadPropertyAsync('{property}') exception: {ex.Message}.");
                        Status = DataStatus.BadInternalError;
                        Status.Explanation = $"Exception: {ex.Message}";
                    }
                    finally
                    {
                        if (_client.Connected)
                        {
                            _client.Disconnect();
                        }

                        Unlock();
                        _logger?.LogDebug($"ReadPropertyAsync('{property}') finished.");
                    }
                }
                else
                {
                    _logger?.LogDebug($"ReadPropertyAsync('{property}') property not readable.");
                    Status = DataStatus.BadNotReadable;
                    Status.Explanation = $"Property '{property}' not readable.";
                }
            }
            else
            {
                _logger?.LogDebug($"ReadPropertyAsync('{property}') property not found.");
                Status = DataStatus.BadNotFound;
                Status.Explanation = $"Property '{property}' not found.";
            }

            return Status;
        }

        /// <summary>
        /// Updates a list of properties reading the data from ETA PU 11 pellet boiler unit.
        /// </summary>
        /// <param name="properties">The list of the property names.</param>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> ReadPropertiesAsync(List<string> properties)
        {
            _logger?.LogDebug($"ReadPropertyAsync(List<property>) starting.");
            await LockAsync();
            Status = DataStatus.Uncertain;
            DataStatus error = DataStatus.Bad;

            try
            {
                if (_client.Connect())
                {
                    ETAPU11Data data = Data;
                    bool Ok = true;

                    foreach (var property in properties)
                    {
                        if (ETAPU11Data.IsProperty(property))
                        {
                            if (ETAPU11Data.IsReadable(property))
                            {
                                var status = await ReadPropertyAsync(data, property);
                                Ok &= status.IsGood;
                                if (!Ok) error = status;
                            }
                            else
                            {
                                _logger?.LogDebug($"ReadPropertiesAsync(List<property>) property '{property}' not readable.");
                                Status = DataStatus.BadNotReadable;
                                Status.Explanation = $"Property '{property}' not readable.";
                            }
                        }
                        else
                        {
                            _logger?.LogDebug($"ReadPropertiesAsync(List<property>) property '{property}' not found.");
                            Status = DataStatus.BadNotFound;
                            Status.Explanation = $"Property '{property}' not found.";
                        }
                    }

                    if (Ok)
                    {
                        _logger?.LogDebug("ReadPropertiesAsync(List<property>) OK.");

                        //Data = data;
                        foreach (var property in properties)
                        {
                            var value = data.GetPropertyValue(property);

                            if (value != null)
                            {
                                Data.SetPropertyValue(property, value);
                            }
                        }

                        Status = DataStatus.Good;
                    }
                    else
                    {
                        _logger?.LogDebug($"ReadPropertiesAsync(List<property>) not OK: {error.Name}.");
                        Status = error;
                    }
                }
                else
                {
                    _logger?.LogError("ReadPropertiesAsync(List<property>) not connected.");
                    Status = DataStatus.BadNoCommunication;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"ReadPropertiesAsync(List<property>) exception: {ex.Message}.");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                if (_client.Connected)
                {
                    _client.Disconnect();
                }

                Unlock();
                _logger?.LogDebug($"ReadPropertyAsync(List<property>) finished.");
            }

            return Status;
        }

        /// <summary>
        /// Updates the ETA PU 11 pellet boiler unit Boiler Data.
        /// </summary>
        /// <returns></returns>
        public async Task<DataStatus> ReadBoilerDataAsync()
        {
            var properties = typeof(BoilerData)
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                    .Select(p => p.Name).ToList();
            var status = await ReadPropertiesAsync(properties);

            if (status.IsGood)
            {
                BoilerData.Refresh(Data);
            }

            return status;
        }

        /// <summary>
        /// Updates the ETA PU 11 pellet boiler unit Hot Water Data.
        /// </summary>
        /// <returns></returns>
        public async Task<DataStatus> ReadHotwaterDataAsync()
        {
            var properties = typeof(HotwaterData)
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                    .Select(p => p.Name).ToList();
            var status = await ReadPropertiesAsync(properties);

            if (status.IsGood)
            {
                HotwaterData.Refresh(Data);
            }

            return status;
        }

        /// <summary>
        /// Updates the ETA PU 11 pellet boiler unit Heating Data.
        /// </summary>
        /// <returns></returns>
        public async Task<DataStatus> ReadHeatingDataAsync()
        {
            var properties = typeof(HeatingData)
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                    .Select(p => p.Name).ToList();
            var status = await ReadPropertiesAsync(properties);

            if (status.IsGood)
            {
                HeatingData.Refresh(Data);
            }

            return status;
        }

        /// <summary>
        /// Updates the ETA PU 11 pellet boiler unit Storage Data.
        /// </summary>
        /// <returns></returns>
        public async Task<DataStatus> ReadStorageDataAsync()
        {
            var properties = typeof(StorageData)
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                    .Select(p => p.Name).ToList();
            var status = await ReadPropertiesAsync(properties);

            if (status.IsGood)
            {
                StorageData.Refresh(Data);
            }

            return status;
        }

        /// <summary>
        /// Updates the ETA PU 11 pellet boiler unit System Data.
        /// </summary>
        /// <returns></returns>
        public async Task<DataStatus> ReadSystemDataAsync()
        {
            var properties = typeof(SystemData)
                    .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                    .Select(p => p.Name).ToList();
            var status = await ReadPropertiesAsync(properties);

            if (status.IsGood)
            {
                SystemData.Refresh(Data);
            }

            return status;
        }

        /// <summary>
        /// Updates a single data item at ETA PU 11 pellet boiler unit
        /// writing the data value. Note that the property is updated.
        /// </summary>
        /// <param name="property">The name of the property.</param>
        /// <param name="data">The data value of the property.</param>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> WritePropertyAsync(string property, string data)
        {
            _logger?.LogDebug($"WriteAllAsync({property}) starting.");
            await LockAsync();
            Status = DataStatus.Good;

            if (ETAPU11Data.IsWritable(property))
            {
                try
                {
                    if (_client.Connect())
                    {
                        dynamic? value = Data.GetPropertyValue(property);

                        if (!(value is null))
                        {
                            switch (value)
                            {
                                case double d when double.TryParse(data, out double doubleData):
                                    Data.SetPropertyValue(property, doubleData);
                                    Status = await WritePropertyAsync(Data, property);
                                    break;
                                case uint when uint.TryParse(data, out uint uint32Data):
                                    Data.SetPropertyValue(property, uint32Data);
                                    Status = await WritePropertyAsync(Data, property);
                                    break;
                                case TimeSpan t when TimeSpan.TryParse(data, out TimeSpan timeData):
                                    Data.SetPropertyValue(property, timeData);
                                    Status = await WritePropertyAsync(Data, property);
                                    break;
                                case DateTimeOffset o when DateTimeOffset.TryParse(data, out DateTimeOffset dateData):
                                    Data.SetPropertyValue(property, dateData);
                                    Status = await WritePropertyAsync(Data, property);
                                    break;
                                case BoilerStates boilerstates when Enum.TryParse<BoilerStates>(data, true, out BoilerStates enumData):
                                    Data.SetPropertyValue(property, enumData);
                                    Status = await WritePropertyAsync(Data, property);
                                    break;
                                case FlowControlStates flowcontrolstates when Enum.TryParse<FlowControlStates>(data, true, out FlowControlStates enumData):
                                    Data.SetPropertyValue(property, enumData);
                                    Status = await WritePropertyAsync(Data, property);
                                    break;
                                case DiverterValveStates divertervalvestates when Enum.TryParse<DiverterValveStates>(data, true, out DiverterValveStates enumData):
                                    Data.SetPropertyValue(property, enumData);
                                    Status = await WritePropertyAsync(Data, property);
                                    break;
                                case DemandValues demandvalues when Enum.TryParse<DemandValues>(data, true, out DemandValues enumData):
                                    Data.SetPropertyValue(property, enumData);
                                    Status = await WritePropertyAsync(Data, property);
                                    break;
                                case DemandValuesEx demandvaluesxx when Enum.TryParse<DemandValuesEx>(data, true, out DemandValuesEx enumData):
                                    Data.SetPropertyValue(property, enumData);
                                    Status = await WritePropertyAsync(Data, property);
                                    break;
                                case FlowMixValveStates flowmixvalvestates when Enum.TryParse<FlowMixValveStates>(data, true, out FlowMixValveStates enumData):
                                    Data.SetPropertyValue(property, enumData);
                                    Status = await WritePropertyAsync(Data, property);
                                    break;
                                case ScrewStates screwstates when Enum.TryParse<ScrewStates>(data, true, out ScrewStates enumData):
                                    Data.SetPropertyValue(property, enumData);
                                    Status = await WritePropertyAsync(Data, property);
                                    break;
                                case AshRemovalStates ashremovalstates when Enum.TryParse<AshRemovalStates>(data, true, out AshRemovalStates enumData):
                                    Data.SetPropertyValue(property, enumData);
                                    Status = await WritePropertyAsync(Data, property);
                                    break;
                                case HopperStates hopperstates when Enum.TryParse<HopperStates>(data, true, out HopperStates enumData):
                                    Data.SetPropertyValue(property, enumData);
                                    Status = await WritePropertyAsync(Data, property);
                                    break;
                                case StartValues startvalues when Enum.TryParse<StartValues>(data, true, out StartValues enumData):
                                    Data.SetPropertyValue(property, enumData);
                                    Status = await WritePropertyAsync(Data, property);
                                    break;
                                case VacuumStates vacuumstates when Enum.TryParse<VacuumStates>(data, true, out VacuumStates enumData):
                                    Data.SetPropertyValue(property, enumData);
                                    Status = await WritePropertyAsync(Data, property);
                                    break;
                                case OnOffStates onoffstates when Enum.TryParse<OnOffStates>(data, true, out OnOffStates enumData):
                                    Data.SetPropertyValue(property, enumData);
                                    Status = await WritePropertyAsync(Data, property);
                                    break;
                                case HWTankStates hwtankstates when Enum.TryParse<HWTankStates>(data, true, out HWTankStates enumData):
                                    Data.SetPropertyValue(property, enumData);
                                    Status = await WritePropertyAsync(Data, property);
                                    break;
                                case HeatingCircuitStates heatingcircuitstates when Enum.TryParse<HeatingCircuitStates>(data, true, out HeatingCircuitStates enumData):
                                    Data.SetPropertyValue(property, enumData);
                                    Status = await WritePropertyAsync(Data, property);
                                    break;
                                case HWRunningStates hwrunningstates when Enum.TryParse<HWRunningStates>(data, true, out HWRunningStates enumData):
                                    Data.SetPropertyValue(property, enumData);
                                    Status = await WritePropertyAsync(Data, property);
                                    break;
                                case ConveyingSystemStates conveyingsystemstates when Enum.TryParse<ConveyingSystemStates>(data, true, out ConveyingSystemStates enumData):
                                    Data.SetPropertyValue(property, enumData);
                                    Status = await WritePropertyAsync(Data, property);
                                    break;
                                case FirebedStates firebedstates when Enum.TryParse<FirebedStates>(data, true, out FirebedStates enumData):
                                    Data.SetPropertyValue(property, enumData);
                                    Status = await WritePropertyAsync(Data, property);
                                    break;
                            }

                            if (Status.IsGood)
                            {
                                _logger?.LogDebug($"WriteDataAsync {data} to '{property}' OK.");
                            }
                            else
                            {
                                _logger?.LogDebug($"WriteDataAsync {data} to '{property}' not OK.");
                            }
                        }
                        else
                        {
                            _logger?.LogError($"WriteDataAsync get property value of '{property}' not successful.");
                            Status = DataStatus.BadDecodingError;
                        }
                    }
                    else
                    {
                        _logger?.LogError("WriteDataAsync not connected.");
                        Status = DataStatus.BadNoCommunication;
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, $"WriteDataAsync exception: {ex.Message}.");
                    Status = DataStatus.BadInternalError;
                    Status.Explanation = $"Exception: {ex.Message}";
                }
                finally
                {
                    if (_client.Connected)
                    {
                        _client.Disconnect();
                    }
                    Unlock();
                    _logger?.LogDebug($"WriteDataAsync({property}) finished.");
                }
            }
            else
            {
                _logger?.LogDebug($"WriteDataAsync invalid property '{property}'.");
                Status = DataStatus.BadNotWritable;
                Status.Explanation = $"Property '{property}' not writable.";
            }

            return Status;
        }

        /// <summary>
        /// Tries to get all data from Modbus TCP/IP slave.
        /// </summary>
        /// <returns>Flag indicating success or failure.</returns>
        public override bool Startup()
        {
            _logger.LogInformation($"ETA PU11 Gateway Starting...");
            IsStartupOk = ReadAll().IsGood;

            if (IsStartupOk)
            {
                _logger.LogInformation("ETA PU11 Gateway:");
                _logger.LogInformation($"    Modbus TCP Address: {_settings.TcpSlave.Address}");
                _logger.LogInformation($"    Modbus TCP Port:    {_settings.TcpSlave.Port}");
                _logger.LogInformation($"    Modbus Slave ID:    {_settings.TcpSlave.ID}");
                _logger.LogInformation($"Startup OK");
            }
            else
            {
                _logger.LogWarning("ETA PU11 Gateway: Startup not OK");
            }

            return IsStartupOk;
        }

        /// <summary>
        /// Tries to connect to the Modbus TCP/IP slave.
        /// </summary>
        /// <returns>Flag indicating success or failure.</returns>
        public override bool CheckAccess()
        {
            _logger?.LogDebug($"CheckAccess() starting.");

            try
            {
                if (_client.Connect())
                {
                    _logger?.LogDebug($"Connect OK.");
                    return true;
                }
                else
                {
                    _logger?.LogDebug($"Connect not OK.");
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Connect exception.");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                if (_client.Connected)
                {
                    _client.Disconnect();
                    _logger?.LogDebug($"CheckAccess() finished.");
                }
            }

            return false;
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Helper method to read the property from the ETAPU11 pellet boiler.
        /// </summary>
        /// <param name="data">The ETAPU11 data.</param>
        /// <param name="property">The property name.</param>
        /// <returns>The status indicating success or failure.</returns>
        private async Task<DataStatus> ReadPropertyAsync(ETAPU11Data data, string property)
        {
            DataStatus status = DataStatus.Good;

            try
            {
                if (ETAPU11Data.IsReadable(property))
                {
                    object? value = data.GetPropertyValue(property);
                    ushort offset = GetOffset(property);
                    ushort length = GetLength(property);
                    ushort scale = GetScale(property);

                    if (!(value is null))
                    {
                        _logger?.LogDebug($"ReadAsync property '{property}' => Type: {value.GetType()}, Offset: {offset}, Length: {length}.");

                        if (value is double)
                        {
                            var intvalue = await _client.ReadUInt32Async(offset);
                            data.SetPropertyValue(property, data.GetDoubleValue(property, intvalue));
                            _logger?.LogDebug($"ReadAsync property '{property}' => Value: {intvalue}.");
                        }
                        else if (value is uint)
                        {
                            var intvalue = await _client.ReadUInt32Async(offset);
                            data.SetPropertyValue(property, intvalue);
                            _logger?.LogDebug($"ReadAsync property '{property}' => Value: {intvalue}.");
                        }
                        else if (value is TimeSpan)
                        {
                            var intvalue = await _client.ReadUInt32Async(offset);
                            data.SetPropertyValue(property, data.GetTimeSpanValue(property, intvalue));
                            _logger?.LogDebug($"ReadAsync property '{property}' => Value: {intvalue}.");
                        }
                        else if (value is DateTimeOffset)
                        {
                            var intvalue = await _client.ReadUInt32Async(offset);
                            data.SetPropertyValue(property, data.GetDateTimeOffsetValue(intvalue));
                            _logger?.LogDebug($"ReadAsync property '{property}' => Value: {intvalue}.");
                        }
                        else if (value.GetType().IsEnum)
                        {
                            var intvalue = await _client.ReadUInt32Async(offset);
                            data.SetPropertyValue(property, Enum.ToObject(value.GetType(), intvalue));
                            _logger?.LogDebug($"ReadAsync property '{property}' => Value: {intvalue}.");
                        }
                    }
                    else
                    {
                        _logger?.LogDebug($"ReadAsync property '{property}' get value not successful.");
                        status = DataStatus.BadUnexpectedError;
                    }
                }
                else
                {
                    _logger?.LogDebug($"ReadAsync property '{property}' not readable.");
                    status = DataStatus.BadNotReadable;
                }
            }
            catch (ArgumentNullException anx)
            {
                _logger?.LogError(anx, $"ArgumentNullException in ReadAsync property '{property}'.");
                status = DataStatus.BadOutOfRange;
            }
            catch (ArgumentOutOfRangeException aor)
            {
                _logger?.LogError(aor, $"ArgumentOutOfRangeException in ReadAsync property '{property}'.");
                status = DataStatus.BadOutOfRange;
            }
            catch (ArgumentException aex)
            {
                _logger?.LogError(aex, $"ArgumentException in ReadAsync property '{property}'.");
                status = DataStatus.BadOutOfRange;
            }
            catch (ObjectDisposedException odx)
            {
                _logger?.LogError(odx, $"ObjectDisposedException in ReadAsync property '{property}'.");
                status = DataStatus.BadInternalError;
            }
            catch (FormatException fex)
            {
                _logger?.LogError(fex, $"FormatException in ReadAsync property '{property}'.");
                status = DataStatus.BadEncodingError;
            }
            catch (IOException iox)
            {
                _logger?.LogError(iox, $"IOException in ReadAsync property '{property}'.");
                status = DataStatus.BadCommunicationError;
            }
            catch (InvalidModbusRequestException imr)
            {
                _logger?.LogError(imr, $"InvalidModbusRequestException in ReadAsync property '{property}'.");
                status = DataStatus.BadCommunicationError;
            }
            catch (InvalidOperationException iox)
            {
                _logger?.LogError(iox, $"InvalidOperationException in ReadAsync property '{property}'.");
                status = DataStatus.BadInternalError;
            }
            catch (SlaveException slx)
            {
                _logger?.LogError(slx, $"SlaveException in ReadAsync property '{property}'.");
                status = DataStatus.BadDeviceFailure;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Exception in ReadAsync property '{property}'.");
                status = DataStatus.BadInternalError;
            }

            return status;
        }

        /// <summary>
        /// Helper method to write the property to the ETAPU11 pellet boiler.
        /// </summary>
        /// <param name="data">The ETAPU11 data.</param>
        /// <param name="property">The property name.</param>
        /// <returns>The status indicating success or failure.</returns>
        private async Task<DataStatus> WritePropertyAsync(ETAPU11Data data, string property)
        {
            DataStatus status = DataStatus.Good;

            try
            {
                if (ETAPU11Data.IsWritable(property))
                {
                    dynamic? value = data.GetPropertyValue(property);
                    ushort offset = GetOffset(property);
                    ushort length = GetLength(property);
                    ushort scale = GetScale(property);

                    if (!(value is null))
                    {
                        switch (value)
                        {
                            case double:
                                {
                                    _logger?.LogDebug($"WriteAsync property '{property}' => Type: {value.GetType()}, Value: {value}, Offset: {offset}, Length: {length}.");
                                    uint intvalue = data.GetUInt32Value(property, (double)value);
                                    await _client.WriteUInt32Async(offset, intvalue);
                                    break;
                                }

                            case uint:
                                _logger?.LogDebug($"WriteAsync property '{property}' => Type: {value.GetType()}, Value: {value}, Offset: {offset}, Length: {length}.");
                                await _client.WriteUInt32Async(offset, (uint)value);
                                break;
                            case TimeSpan:
                                {
                                    _logger?.LogDebug($"WriteAsync property '{property}' => Type: {value.GetType()}, Value: {value}, Offset: {offset}, Length: {length}.");
                                    uint intvalue = data.GetUInt32Value(property, (TimeSpan)value);
                                    await _client.WriteUInt32Async(offset, intvalue);
                                    break;
                                }

                            case DateTimeOffset:
                                {
                                    _logger?.LogDebug($"WriteAsync property '{property}' => Type: {value.GetType()}, Value: {value}, Offset: {offset}, Length: {length}.");
                                    uint intvalue = data.GetUInt32Value(property, (int)((DateTimeOffset)value).ToUnixTimeSeconds());
                                    await _client.WriteUInt32Async(offset, intvalue);
                                    break;
                                }

                            default:
                                if (value.GetType().IsEnum)
                                {
                                    _logger?.LogDebug($"WriteAsync property '{property}' => Type: {value.GetType()}, Value: {value}, Offset: {offset}, Length: {length}.");
                                    await _client.WriteUInt32Async(offset, (uint)value);
                                }

                                break;
                        }
                    }
                    else
                    {
                        _logger?.LogDebug($"WriteAsync property '{property}' get value not successful.");
                        status = DataStatus.BadUnexpectedError;
                    }
                }
                else
                {
                    _logger?.LogDebug($"WriteAsync property '{property}' not writable.");
                    status = DataStatus.BadNotWritable;
                }
            }
            catch (ArgumentNullException anx)
            {
                _logger?.LogError(anx, $"ArgumentNullException in WriteAsync property '{property}'.");
                status = DataStatus.BadOutOfRange;
            }
            catch (ArgumentOutOfRangeException aor)
            {
                _logger?.LogError(aor, $"ArgumentOutOfRangeException in WriteAsync property '{property}'.");
                status = DataStatus.BadOutOfRange;
            }
            catch (ArgumentException aex)
            {
                _logger?.LogError(aex, $"ArgumentException in WriteAsync property '{property}'.");
                status = DataStatus.BadOutOfRange;
            }
            catch (ObjectDisposedException odx)
            {
                _logger?.LogError(odx, $"ObjectDisposedException in WriteAsync property '{property}'.");
                status = DataStatus.BadInternalError;
            }
            catch (FormatException fex)
            {
                _logger?.LogError(fex, $"FormatException in WriteAsync property '{property}'.");
                status = DataStatus.BadEncodingError;
            }
            catch (IOException iox)
            {
                _logger?.LogError(iox, $"IOException in WriteAsync property '{property}'.");
                status = DataStatus.BadCommunicationError;
            }
            catch (InvalidModbusRequestException imr)
            {
                _logger?.LogError(imr, $"InvalidModbusRequestException in WriteAsync property '{property}'.");
                status = DataStatus.BadCommunicationError;
            }
            catch (InvalidOperationException iox)
            {
                _logger?.LogError(iox, $"InvalidOperationException in WriteAsync property '{property}'.");
                status = DataStatus.BadInternalError;
            }
            catch (SlaveException slx)
            {
                _logger?.LogError(slx, $"SlaveException in WriteAsync property '{property}'.");
                status = DataStatus.BadDeviceFailure;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"Exception in WriteAsync property '{property}'.");
                status = DataStatus.BadInternalError;
            }

            return status;
        }

        #endregion Private Methods
    }
}
