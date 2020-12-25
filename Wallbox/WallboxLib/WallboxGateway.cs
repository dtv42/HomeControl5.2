// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WallboxGateway.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>26-4-2020 20:20</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace WallboxLib
{
    #region Using Directives

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;

    using UtilityLib;
    using UtilityLib.Webapp;

    using WallboxLib.Models;

    #endregion

    /// <summary>
    /// Class holding data from the BMW wallbox.
    /// The data properties are based on the KEBA specification "KeContact P30 Charging station UDP Programmers Guide V 2.01".
    /// Document: V 2.01 / Document No.: 92651, from 10.09.2018
    /// </summary>
    public class WallboxGateway : BaseGateway
    {
        #region Constants

        public static readonly int MAX_REPORTS = 30;
        public static readonly int REPORTS_ID = 100;

        #endregion

        #region Private Data Members

        /// <summary>
        /// The instantiated UDP client.
        /// </summary>
        private readonly WallboxClient _client;

        /// <summary>
        /// The Wallbox client settings.
        /// </summary>
        private readonly WallboxSettings _settings;

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the Wallbox settings.
        /// </summary>
        public WallboxSettings Settings { get => _settings; }

        /// <summary>
        /// The Data property holds all Wallbox data values.
        /// </summary>
        [JsonIgnore]
        public WallboxData Data { get; } = new WallboxData();

        /// <summary>
        /// The Report1 property holds all Wallbox report 1 data.
        /// </summary>
        [JsonIgnore]
        public Report1Data Report1 { get; } = new Report1Data();

        /// <summary>
        /// The Report2 property holds all Wallbox report 2 data.
        /// </summary>
        [JsonIgnore]
        public Report2Data Report2 { get; } = new Report2Data();

        /// <summary>
        /// The Report3 property holds all Wallbox report 3 data.
        /// </summary>
        [JsonIgnore]
        public Report3Data Report3 { get; } = new Report3Data();

        /// <summary>
        /// The Report100 property holds all Wallbox report 100 data.
        /// </summary>
        [JsonIgnore]
        public ReportsData Report100 { get; } = new ReportsData();

        /// <summary>
        /// The Reports property holds all Wallbox charging report data.
        /// </summary>
        [JsonIgnore]
        public List<ReportsData> Reports { get; } = new List<ReportsData> { };

        /// <summary>
        /// Gets or sets the Wallbox info data.
        /// </summary>
        [JsonIgnore]
        public InfoData Info { get; private set; } = new InfoData();

        /// <summary>
        /// Gets a flag indicating the correct startup.
        /// </summary>
        public new bool IsStartupOk { get; private set; }

        #endregion Public Properties

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="WallboxGateway"/> class.
        /// </summary>
        /// <param name="client">The UDP Wallbox client.</param>
        /// <param name="settings">The Wallbox settings.</param>
        /// <param name="logger">The application logger instance.</param>
        public WallboxGateway(WallboxClient client,
                              IWallboxSettings settings,
                              ILogger<WallboxGateway> logger)
            : base(logger)
        {
            _logger?.LogDebug($"Wallbox()");

            _settings = new WallboxSettings()
            {
                EndPoint = settings.EndPoint,
                Port = settings.Port,
                Timeout = settings.Timeout
            };

            _client = client;

            for (int i = 0; i < MAX_REPORTS; ++i)
            {
                Reports.Add(new ReportsData());
            }
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Synchronous methods.
        /// </summary>
        public DataStatus ReadAll() => ReadAllAsync().Result;
        public DataStatus ReadReport1() => ReadReport1Async().Result;
        public DataStatus ReadReport2() => ReadReport2Async().Result;
        public DataStatus ReadReport3() => ReadReport3Async().Result;
        public DataStatus ReadReport100() => ReadReport100Async().Result;
        public DataStatus ReadReports() => ReadReportsAsync().Result;
        public DataStatus ReadReport(int id) => ReadReportAsync(id).Result;
        public DataStatus ReadInfo() => ReadInfoAsync().Result;

        public DataStatus SetCurrent(uint current) => SetCurrentAsync(current).Result;
        public DataStatus SetCurrent(uint current, uint delay) => SetCurrentAsync(current, delay).Result;
        public DataStatus SetEnergy(uint energy) => SetEnergyAsync(energy).Result;
        public DataStatus SetOutput(ushort command) => SetOutputAsync(command).Result;
        public DataStatus StartRFID(string tag, string classifier) => StartRFIDAsync(tag, classifier).Result;
        public DataStatus StopRFID(string tag) => StopRFIDAsync(tag).Result;
        public DataStatus EnableCommand(ushort modifier) => EnableCommandAsync(modifier).Result;
        public DataStatus UnlockSocket() => UnlockSocketAsync().Result;

        /// <summary>
        /// Updates all Wallbox properties reading the data from the Wallbox UDP service.
        /// If successful the data value will be updated (timestamp).
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> ReadAllAsync()
        {
            _logger?.LogDebug("ReadAllAsync() starting.");
            Status = DataStatus.Good;
            bool Ok = true;

            try
            {
                Ok &= (await ReadReport1Async()).IsGood;
                Ok &= (await ReadReport2Async()).IsGood;
                Ok &= (await ReadReport3Async()).IsGood;
                Ok &= (await ReadReport100Async()).IsGood;
                Ok &= (await ReadReportsAsync()).IsGood;

                if (!Ok)
                {
                    _logger?.LogError("ReadAllAsync not OK.");
                }
            }
            catch (InvalidDataException idx)
            {
                _logger?.LogError(idx, "Invalid Data Exception in ReadAllAsync().");
                Status = DataStatus.BadDataUnavailable;
                Status.Explanation = idx.Message;
            }
            catch (TimeoutException tox)
            {
                _logger?.LogError(tox, "TimeoutException in ReadAllAsync().");
                Status = DataStatus.BadTimeout;
                Status.Explanation = tox.Message;
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
        /// Updates the Wallbox properties reading the report 1 data from the Wallbox UDP service.
        /// If successful the data value will be updated (timestamp).
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> ReadReport1Async()
        {
            _logger?.LogDebug("ReadReport1Async() starting.");
            await LockAsync();
            Status = DataStatus.Good;

            try
            {
                string json = await _client.SendReceiveAsync("report 1");
                _logger?.LogDebug($"JSON: {json}");

                if (!string.IsNullOrEmpty(json))
                {
                    var report = JsonSerializer.Deserialize<Report1Udp>(json);

                    if (report is null)
                    {
                        _logger?.LogError("ReadReport1Async not OK.");
                        Status = DataStatus.BadDecodingError;
                        Status.Explanation = "ReadReport1Async not OK.";
                    }
                    else
                    {
                        Data.Report1 = report;
                        Report1.Refresh(Data.Report1);
                        _logger?.LogDebug("ReadReport1Async OK.");
                    }
                }
                else
                {
                    _logger?.LogError("ReadReport1Async not OK.");
                    Status = DataStatus.BadUnknownResponse;
                    Status.Explanation = "ReadReport1Async not OK.";
                }
            }
            catch (InvalidDataException idx)
            {
                _logger?.LogError(idx, "Invalid Data Exception in ReadReport1Async().");
                Status = DataStatus.BadDataUnavailable;
                Status.Explanation = idx.Message;
            }
            catch (TimeoutException tox)
            {
                _logger?.LogError(tox, "TimeoutException in ReadReport1Async().");
                Status = DataStatus.BadTimeout;
                Status.Explanation = tox.Message;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Exception in ReadReport1Async().");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug("ReadReport1Async() finished.");
            }

            return Status;
        }

        /// <summary>
        /// Updates the Wallbox properties reading the report 2 data from the Wallbox UDP service.
        /// If successful the data value will be updated (timestamp).
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> ReadReport2Async()
        {
            _logger?.LogDebug("ReadReport2Async() starting.");
            await LockAsync();
            Status = DataStatus.Good;

            try
            {
                string json = await _client.SendReceiveAsync("report 2");
                _logger?.LogDebug($"JSON: {json}");

                if (!string.IsNullOrEmpty(json))
                {
                    var report = JsonSerializer.Deserialize<Report2Udp>(json);

                    if (report is null)
                    {
                        _logger?.LogError("ReadReport2Async not OK.");
                        Status = DataStatus.BadDecodingError;
                        Status.Explanation = "ReadReport2Async not OK.";
                    }
                    else
                    {
                        Data.Report2 = report;
                        Report2.Refresh(Data.Report2);
                        _logger?.LogDebug("ReadReport2Async OK.");
                    }
                }
                else
                {
                    _logger?.LogError("ReadReport2Async not OK.");
                    Status = DataStatus.BadUnknownResponse;
                    Status.Explanation = "ReadReport2Async not OK.";
                }
            }
            catch (InvalidDataException idx)
            {
                _logger?.LogError(idx, "Invalid Data Exception in ReadReport2Async().");
                Status = DataStatus.BadDataUnavailable;
                Status.Explanation = idx.Message;
            }
            catch (TimeoutException tox)
            {
                _logger?.LogError(tox, "TimeoutException in ReadReport2Async().");
                Status = DataStatus.BadTimeout;
                Status.Explanation = tox.Message;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Exception in ReadReport2Async().");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug("ReadReport2Async() finished.");
            }

            return Status;
        }

        /// <summary>
        /// Updates the Wallbox properties reading the report 3 data from the Wallbox UDP service.
        /// If successful the data value will be updated (timestamp).
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> ReadReport3Async()
        {
            _logger?.LogDebug("ReadReport3Async() starting.");
            await LockAsync();
            Status = DataStatus.Good;

            try
            {
                string json = await _client.SendReceiveAsync("report 3");
                _logger?.LogDebug($"JSON: {json}");

                if (!string.IsNullOrEmpty(json))
                {
                    var report = JsonSerializer.Deserialize<Report3Udp>(json);

                    if (report is null)
                    {
                        _logger?.LogError("ReadReport3Async not OK.");
                        Status = DataStatus.BadDecodingError;
                        Status.Explanation = "ReadReport2Async not OK.";
                    }
                    else
                    {
                        Data.Report3 = report;
                        Report3.Refresh(Data.Report3);
                        _logger?.LogDebug("ReadReport3Async OK.");
                    }
                }
                else
                {
                    _logger?.LogError("ReadReport3Async not OK.");
                    Status = DataStatus.BadUnknownResponse;
                    Status.Explanation = "ReadReport2Async not OK.";
                }
            }
            catch (InvalidDataException idx)
            {
                _logger?.LogError(idx, "Invalid Data Exception in ReadReport3Async().");
                Status = DataStatus.BadDataUnavailable;
                Status.Explanation = idx.Message;
            }
            catch (TimeoutException tox)
            {
                _logger?.LogError(tox, "TimeoutException in ReadReport3Async().");
                Status = DataStatus.BadTimeout;
                Status.Explanation = tox.Message;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Exception in ReadReport3Async().");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug("ReadReport3Async() finished.");
            }

            return Status;
        }

        /// <summary>
        /// Updates the Wallbox properties reading the report 100 data from the Wallbox UDP service.
        /// If successful the data value will be updated (timestamp).
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> ReadReport100Async()
        {
            _logger?.LogDebug("ReadReport100Async() starting.");
            await LockAsync();
            Status = DataStatus.Good;

            try
            {
                string json = await _client.SendReceiveAsync("report 100");
                _logger?.LogDebug($"JSON: {json}");

                if (!string.IsNullOrEmpty(json))
                {
                    var report = JsonSerializer.Deserialize<ReportsUdp>(json);

                    if (report is null)
                    {
                        _logger?.LogError("ReadReport100Async not OK.");
                        Status = DataStatus.BadDecodingError;
                        Status.Explanation = "ReadReport100Async not OK.";
                    }
                    else
                    {
                        Data.Report100 = report;
                        Report100.Refresh(Data.Report100);
                        _logger?.LogDebug("ReadReport100Async OK.");
                    }
                }
                else
                {
                    _logger?.LogError("ReadReport100Async not OK.");
                    Status = DataStatus.BadUnknownResponse;
                    Status.Explanation = "ReadReport100Async not OK.";
                }
            }
            catch (InvalidDataException idx)
            {
                _logger?.LogError(idx, "Invalid Data Exception in ReadReport100Async().");
                Status = DataStatus.BadDataUnavailable;
                Status.Explanation = idx.Message;
            }
            catch (TimeoutException tox)
            {
                _logger?.LogError(tox, "TimeoutException in ReadReport100Async().");
                Status = DataStatus.BadTimeout;
                Status.Explanation = tox.Message;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Exception in ReadReport100Async().");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug("ReadReport100Async() finished.");
            }

            return Status;
        }

        /// <summary>
        /// Updates the Wallbox properties reading charging report data from the Wallbox UDP service.
        /// If successful the data value will be updated (timestamp).
        /// </summary>
        /// <param name="id">The reports ID (101 - 130).</param>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> ReadReportAsync(int id)
        {
            if (IsReportIDOk(id))
            {
                _logger?.LogDebug($"ReadReportAsync({id}) starting.");
                await LockAsync();
                Status = DataStatus.Good;
                int index = id - REPORTS_ID - 1;

                try
                {
                    string json = await _client.SendReceiveAsync($"report {id}");
                    _logger?.LogDebug($"JSON: {json}");

                    if (!string.IsNullOrEmpty(json))
                    {
                        var report = JsonSerializer.Deserialize<ReportsUdp>(json);

                        if (report is null)
                        {
                            _logger?.LogDebug($"ReadReportAsync({id}) not OK.");
                            Status = DataStatus.BadDecodingError;
                            Status.Explanation = $"ReadReportAsync({id}) not OK.";
                        }
                        else
                        {
                            Data.Reports[index] = report;
                            Reports[index].Refresh(Data.Reports[index]);
                            _logger?.LogDebug($"ReadReportAsync({id}) OK.");
                        }
                    }
                    else
                    {
                        _logger?.LogDebug($"ReadReportAsync({id}) not OK.");
                        Status = DataStatus.BadUnknownResponse;
                        Status.Explanation = $"ReadReportAsync({id}) not OK.";
                    }
                }
                catch (InvalidDataException idx)
                {
                    _logger?.LogError(idx, $"Invalid Data Exception in ReadReport({id})Async().");
                    Status = DataStatus.BadDataUnavailable;
                    Status.Explanation = idx.Message;
                }
                catch (TimeoutException tox)
                {
                    _logger?.LogError(tox, $"TimeoutException in ReadReport({id})Async().");
                    Status = DataStatus.BadTimeout;
                    Status.Explanation = tox.Message;
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, $"ReadReportAsync({id}) exception: {ex.Message}.");
                    Status = DataStatus.BadInternalError;
                }
                finally
                {
                    Unlock();
                    _logger?.LogDebug($"ReadReportAsync({id}) finished.");
                }
            }
            else
            {
                _logger?.LogDebug($"ReadReportAsync({id}) report ID out of bounds.");
                Status = DataStatus.BadOutOfRange;
                Status.Explanation = $"Report ID out of bounds.";
            }

            return Status;
        }

        /// <summary>
        /// Updates the Wallbox properties reading all the charging report data from the Wallbox UDP service.
        /// If successful the data value will be updated (timestamp).
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> ReadReportsAsync()
        {
            _logger?.LogDebug("ReadReportsAsync() starting.");
            Status = DataStatus.Good;
            bool Ok = true;

            try
            {
                for (int id = REPORTS_ID + 1; id <= (REPORTS_ID + MAX_REPORTS); ++id)
                {
                    Ok &= (await ReadReportAsync(id)).IsGood;
                }

                if (!Ok)
                {
                    _logger?.LogError("ReadReportsAsync not OK.");
                    Status = DataStatus.BadUnknownResponse;
                    Status.Explanation = "ReadReportsAsync not OK.";
                }
            }
            catch (InvalidDataException idx)
            {
                _logger?.LogError(idx, "Invalid Data Exception in ReadReportsAsync().");
                Status = DataStatus.BadDataUnavailable;
                Status.Explanation = idx.Message;
            }
            catch (TimeoutException tox)
            {
                _logger?.LogError(tox, "TimeoutException in ReadReportsAsync().");
                Status = DataStatus.BadTimeout;
                Status.Explanation = tox.Message;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Exception in ReadReportsAsync().");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                _logger?.LogDebug("ReadReportsAsync() finished.");
            }

            return Status;
        }

        /// <summary>
        /// Async method to retrieve the Wallbox info data.
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> ReadInfoAsync()
        {
            _logger?.LogDebug("ReadInfoAsync() starting.");
            await LockAsync();
            Status = DataStatus.Good;

            try
            {
                string json = "{ " + await _client.SendReceiveAsync("i") + " }";
                _logger?.LogDebug($"JSON: {json}");

                if (!string.IsNullOrEmpty(json))
                {
                    var info = JsonSerializer.Deserialize<InfoData>(json);

                    if (info is null)
                    {
                        _logger?.LogError("ReadInfoAsync not OK.");
                        Status = DataStatus.BadDecodingError;
                        Status.Explanation = "ReadInfoAsync not OK.";
                    }
                    else
                    {
                        Info = info;
                        _logger?.LogDebug("ReadInfoAsync() OK.");
                    }
                }
                else
                {
                    _logger?.LogError("ReadInfoAsync not OK.");
                    Status = DataStatus.BadUnknownResponse;
                    Status.Explanation = "ReadInfoAsync not OK.";
                }
            }
            catch (InvalidDataException idx)
            {
                _logger?.LogError(idx, "Invalid Data Exception in ReadInfoAsync().");
                Status = DataStatus.BadDataUnavailable;
                Status.Explanation = idx.Message;
            }
            catch (TimeoutException tox)
            {
                _logger?.LogError(tox, "TimeoutException in ReadInfoAsync().");
                Status = DataStatus.BadTimeout;
                Status.Explanation = tox.Message;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"ReadInfoAsync() exception: {ex.Message}.");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug("ReadInfoAsync() finished.");
            }

            return Status;
        }

        /// <summary>
        /// Async method to control the charging current at any time.
        /// Note that the current value will change immediatley
        /// </summary>
        /// <param name="current">Current value in mA (0; 6000 - 63000).</param>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> SetCurrentAsync(uint current)
        {
            _logger?.LogDebug($"SetCurrentAsync({current}) starting.");
            await LockAsync();
            Status = DataStatus.Good;

            if (!IsCurrentValueOk(current))
            {
                _logger?.LogDebug($"SetCurrentAsync({current}) current value out of bounds.");
                Status = DataStatus.BadNotFound;
                Status.Explanation = $"Current value {current} out of bounds.";
                return Status;
            }

            try
            {
                string reply = await _client.SendReceiveAsync($"curr {current}");
                _logger?.LogDebug($"Reply: {reply}");

                if (!string.IsNullOrEmpty(reply) && reply.StartsWith("TCH-OK"))
                {
                    _logger?.LogDebug($"SetCurrentAsync({current}) OK.");
                }
                else
                {
                    _logger?.LogError($"SetCurrentAsync({current}) not OK.");
                    Status = DataStatus.BadUnknownResponse;
                    Status.Explanation = "SetCurrentAsync not OK.";
                }
            }
            catch (InvalidDataException idx)
            {
                _logger?.LogError(idx, "Invalid Data Exception in SetCurrentAsync().");
                Status = DataStatus.BadDataUnavailable;
                Status.Explanation = idx.Message;
            }
            catch (TimeoutException tox)
            {
                _logger?.LogError(tox, "TimeoutException in SetCurrentAsync().");
                Status = DataStatus.BadTimeout;
                Status.Explanation = tox.Message;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"SetCurrentAsync() exception: {ex.Message}.");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug("SetCurrentAsync() finished.");
            }

            return Status;
        }

        /// <summary>
        /// Async method to control the charging current at any time.
        /// </summary>
        /// <param name="current">Current value in mA (0; 6000 - 63000).</param>
        /// <param name="delay">Delay in seconds(0; 1 - 860400).</param>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> SetCurrentAsync(uint current, uint delay)
        {
            _logger?.LogDebug($"SetCurrentAsync({current}, {delay}) starting.");
            await LockAsync();
            Status = DataStatus.Good;

            if (!IsCurrentValueOk(current))
            {
                _logger?.LogDebug($"SetCurrentAsync({current}, {delay}) current value out of bounds.");
                Status = DataStatus.BadNotFound;
                Status.Explanation = $"Current value {current} out of bounds.";
                return Status;
            }

            if (!IsDelayValueOk(current))
            {
                _logger?.LogDebug($"SetCurrentAsync({current}, {delay}) delay value out of bounds.");
                Status = DataStatus.BadNotFound;
                Status.Explanation = $"Delay value {delay} out of bounds.";
                return Status;
            }

            try
            {
                string reply = await _client.SendReceiveAsync($"currtime {current} {delay}");
                _logger?.LogDebug($"Reply: {reply}");

                if (!string.IsNullOrEmpty(reply) && reply.StartsWith("TCH-OK"))
                {
                    _logger?.LogDebug($"SetCurrentAsync({current}, {delay}) OK.");
                }
                else
                {
                    _logger?.LogError($"SetCurrentAsync({current}, {delay}) not OK.");
                    Status = DataStatus.BadUnknownResponse;
                    Status.Explanation = "SetCurrentAsync not OK.";
                }
            }
            catch (InvalidDataException idx)
            {
                _logger?.LogError(idx, "Invalid Data Exception in SetCurrentAsync().");
                Status = DataStatus.BadDataUnavailable;
                Status.Explanation = idx.Message;
            }
            catch (TimeoutException tox)
            {
                _logger?.LogError(tox, "TimeoutException in SetCurrentAsync().");
                Status = DataStatus.BadTimeout;
                Status.Explanation = tox.Message;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"SetCurrentAsync() exception: {ex.Message}.");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug("SetCurrentAsync() finished.");
            }

            return Status;
        }

        /// <summary>
        /// Async method to set an energy limit for a running or the next charging session.
        /// </summary>
        /// <param name="energy">Energy value in 0.1 Wh (0; 1 - 999999999).</param>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> SetEnergyAsync(uint energy)
        {
            _logger?.LogDebug($"SetEnergyAsync({energy}) starting.");
            await LockAsync();
            Status = DataStatus.Good;

            if ((energy != 0) && ((energy < 1) || (energy > 999999999)))
            {
                _logger?.LogDebug($"SetEnergyAsync({energy}) energy value out of bounds.");
                Status = DataStatus.BadNotFound;
                Status.Explanation = $"Energy value {energy} out of bounds.";
                return Status;
            }

            try
            {
                string reply = await _client.SendReceiveAsync($"setenergy {energy}");
                _logger?.LogDebug($"Reply: {reply}");

                if (!string.IsNullOrEmpty(reply) && reply.StartsWith("TCH-OK"))
                {
                    _logger?.LogDebug($"SetEnergyAsync({energy}) OK.");
                }
                else
                {
                    _logger?.LogError($"SetEnergyAsync({energy}) not OK.");
                    Status = DataStatus.BadUnknownResponse;
                    Status.Explanation = "SetEnergyAsync not OK.";
                }
            }
            catch (InvalidDataException idx)
            {
                _logger?.LogError(idx, "Invalid Data Exception in SetEnergyAsync().");
                Status = DataStatus.BadDataUnavailable;
                Status.Explanation = idx.Message;
            }
            catch (TimeoutException tox)
            {
                _logger?.LogError(tox, "TimeoutException in SetEnergyAsync().");
                Status = DataStatus.BadTimeout;
                Status.Explanation = tox.Message;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"SetEnergyAsync({energy}) exception: {ex.Message}.");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug($"SetEnergyAsync({energy}) finished.");
            }

            return Status;
        }

        /// <summary>
        /// Async method to authorize a charging session.
        /// </summary>
        /// <param name="tag">The RFID tag (8 byte HEX string).</param>
        /// <param name="classifier">The RFID classifier (10 byte HEX string).</param>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> StartRFIDAsync(string tag, string classifier)
        {
            _logger?.LogDebug($"StartRFIDAsync({tag}, {classifier}) starting.");
            await LockAsync();
            Status = DataStatus.Good;

            if (!IsRFIDTagStringOk(tag))
            {
                _logger?.LogDebug($"StartRFIDAsync({tag}, {classifier}) invalid tag.");
                Status = DataStatus.BadNotFound;
                Status.Explanation = $"Invalid RFID tag.";
                return Status;
            }

            if (!IsRFIDClassifierStringOk(tag))
            {
                _logger?.LogDebug($"StartRFIDAsync({tag}, {classifier}) invalid classifier.");
                Status = DataStatus.BadNotFound;
                Status.Explanation = $"Invalid RFID classifier.";
                return Status;
            }

            try
            {
                string reply = await _client.SendReceiveAsync($"start {tag} {classifier}");
                _logger?.LogDebug($"Reply: {reply}");

                if (!string.IsNullOrEmpty(reply) && reply.StartsWith("TCH-OK"))
                {
                    _logger?.LogDebug($"StartRFIDAsync({tag}, {classifier}) OK.");
                }
                else
                {
                    _logger?.LogError($"StartRFIDAsync({tag}, {classifier}) not OK.");
                    Status = DataStatus.BadUnknownResponse;
                    Status.Explanation = "StartRFIDAsync not OK.";
                }
            }
            catch (InvalidDataException idx)
            {
                _logger?.LogError(idx, "Invalid Data Exception in StartRFIDAsync().");
                Status = DataStatus.BadDataUnavailable;
                Status.Explanation = idx.Message;
            }
            catch (TimeoutException tox)
            {
                _logger?.LogError(tox, "TimeoutException in StartRFIDAsync().");
                Status = DataStatus.BadTimeout;
                Status.Explanation = tox.Message;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"StartRFIDAsync({tag}, {classifier}) exception: {ex.Message}.");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug($"StartRFIDAsync({tag}, {classifier}) finished.");
            }

            return Status;
        }

        /// <summary>
        /// Async method to set an energy limit for a running or the next charging session.
        /// </summary>
        /// <param name="tag">The RFID tag (8 byte HEX string).</param>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> StopRFIDAsync(string tag)
        {
            _logger?.LogDebug($"StopRFIDAsync({tag}) starting.");
            await LockAsync();
            Status = DataStatus.Good;

            if (!IsRFIDTagStringOk(tag))
            {
                _logger?.LogDebug($"StopRFIDAsync({tag}) invalid tag.");
                Status = DataStatus.BadNotFound;
                Status.Explanation = $"Invalid RFID tag.";
                return Status;
            }

            try
            {
                string reply = await _client.SendReceiveAsync($"stop {tag}");
                _logger?.LogDebug($"Reply: {reply}");

                if (!string.IsNullOrEmpty(reply) && reply.StartsWith("TCH-OK"))
                {
                    _logger?.LogDebug($"StopRFIDAsync({tag}) OK.");
                }
                else
                {
                    _logger?.LogError($"StopRFIDAsync({tag}) not OK.");
                    Status = DataStatus.BadUnknownResponse;
                    Status.Explanation = "StopRFIDAsync not OK.";
                }
            }
            catch (InvalidDataException idx)
            {
                _logger?.LogError(idx, "Invalid Data Exception in StopRFIDAsync().");
                Status = DataStatus.BadDataUnavailable;
                Status.Explanation = idx.Message;
            }
            catch (TimeoutException tox)
            {
                _logger?.LogError(tox, "TimeoutException in StopRFIDAsync().");
                Status = DataStatus.BadTimeout;
                Status.Explanation = tox.Message;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"StopRFIDAsync({tag}) exception: {ex.Message}.");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug($"StopRFIDAsync({tag}) finished.");
            }

            return Status;
        }

        /// <summary>
        /// Async method to set the output relay.
        /// </summary>
        /// <param name="command">Output value (0: Close; 1: Open).</param>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> SetOutputAsync(ushort command)
        {
            _logger?.LogDebug($"SetOutputAsync({command}) starting.");
            await LockAsync();
            Status = DataStatus.Good;

            if (!IsOutputValueOk(command))
            {
                _logger?.LogDebug($"SetOutputAsync({command}) command value out of bounds.");
                Status = DataStatus.BadNotFound;
                Status.Explanation = $"Output value {command} out of bounds.";
                return Status;
            }

            try
            {
                string reply = await _client.SendReceiveAsync($"output {command}");
                _logger?.LogDebug($"Reply: {reply}");

                if (!string.IsNullOrEmpty(reply) && reply.StartsWith("TCH-OK"))
                {
                    _logger?.LogDebug($"SetOutputAsync({command}) OK.");
                }
                else
                {
                    _logger?.LogError($"SetOutputAsync({command}) not OK.");
                    Status = DataStatus.BadUnknownResponse;
                    Status.Explanation = "SetOutputAsync not OK.";
                }
            }
            catch (InvalidDataException idx)
            {
                _logger?.LogError(idx, "Invalid Data Exception in SetOutputAsync().");
                Status = DataStatus.BadDataUnavailable;
                Status.Explanation = idx.Message;
            }
            catch (TimeoutException tox)
            {
                _logger?.LogError(tox, "TimeoutException in SetOutputAsync().");
                Status = DataStatus.BadTimeout;
                Status.Explanation = tox.Message;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"SetOutputAsync({command}) exception: {ex.Message}.");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug($"SetOutputAsync({command}) finished.");
            }

            return Status;
        }

        /// <summary>
        /// Async method to disable the system.
        /// </summary>
        /// <param name="modifier">Enable value (0, 1).</param>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> EnableCommandAsync(ushort modifier)
        {
            _logger?.LogDebug($"EnableCommandAsync({modifier}) starting.");
            await LockAsync();
            Status = DataStatus.Good;

            if (!IsEnableValueOk(modifier))
            {
                _logger?.LogDebug($"EnableCommandAsync({modifier}) modifier value out of bounds.");
                Status = DataStatus.BadNotFound;
                Status.Explanation = $"Enable modifier value {modifier} out of bounds.";
                return Status;
            }

            try
            {
                string reply = await _client.SendReceiveAsync($"ena {modifier}");
                _logger?.LogDebug($"Reply: {reply}");

                if (!string.IsNullOrEmpty(reply) && reply.StartsWith("TCH-OK"))
                {
                    _logger?.LogDebug($"EnableCommandAsync({ modifier}) OK.");
                }
                else
                {
                    _logger?.LogError($"EnableCommandAsync({modifier}) not OK.");
                    Status = DataStatus.BadUnknownResponse;
                    Status.Explanation = "EnableCommandAsync not OK.";
                }
            }
            catch (InvalidDataException idx)
            {
                _logger?.LogError(idx, "Invalid Data Exception in EnableCommandAsync().");
                Status = DataStatus.BadDataUnavailable;
                Status.Explanation = idx.Message;
            }
            catch (TimeoutException tox)
            {
                _logger?.LogError(tox, "TimeoutException in EnableCommandAsync().");
                Status = DataStatus.BadTimeout;
                Status.Explanation = tox.Message;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"EnableCommandAsync({modifier}) exception: {ex.Message}.");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug($"EnableCommandAsync({modifier}) finished.");
            }

            return Status;
        }

        /// <summary>
        /// Async method to set the output relay.
        /// </summary>
        /// <returns>The status indicating success or failure.</returns>
        public async Task<DataStatus> UnlockSocketAsync()
        {
            _logger?.LogDebug($"UnlockSocketAsync() starting.");
            await LockAsync();
            Status = DataStatus.Good;

            try
            {
                string reply = await _client.SendReceiveAsync($"unlock");
                _logger?.LogDebug($"Reply: {reply}");

                if (!string.IsNullOrEmpty(reply) && reply.StartsWith("TCH-OK"))
                {
                    _logger?.LogDebug($"UnlockSocketAsync() OK.");
                }
                else
                {
                    _logger?.LogError($"UnlockSocketAsync() not OK.");
                    Status = DataStatus.BadUnknownResponse;
                    Status.Explanation = "UnlockSocketAsync not OK.";
                }
            }
            catch (InvalidDataException idx)
            {
                _logger?.LogError(idx, "Invalid Data Exception in UnlockSocketAsync().");
                Status = DataStatus.BadDataUnavailable;
                Status.Explanation = idx.Message;
            }
            catch (TimeoutException tox)
            {
                _logger?.LogError(tox, "TimeoutException in UnlockSocketAsync().");
                Status = DataStatus.BadTimeout;
                Status.Explanation = tox.Message;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, $"UnlockSocketAsync() exception: {ex.Message}.");
                Status = DataStatus.BadInternalError;
                Status.Explanation = $"Exception: {ex.Message}";
            }
            finally
            {
                Unlock();
                _logger?.LogDebug($"UnlockSocketAsync() finished.");
            }

            return Status;
        }

        /// <summary>
        /// Tries to get all data from the Wallbox charging station.
        /// </summary>
        /// <returns>Flag indicating success or failure.</returns>
        public override bool Startup()
        {
            _logger.LogInformation($"BMW Wallbox Gateway Starting...");
            IsStartupOk = ReadAllAsync().Result.IsGood;

            if (IsStartupOk)
            {
                _logger.LogInformation("BMW Wallbox Gateway:");
                _logger.LogInformation($"    Endpoint: {_settings.EndPoint}");
                _logger.LogInformation($"    UDP Port:  {_settings.Port}");
                _logger.LogInformation($"Startup OK");
            }
            else
            {
                _logger.LogWarning("BMW Wallbox Gateway: Startup not OK");
            }

            return IsStartupOk;
        }

        /// <summary>
        /// Check if valid Info data can be retrieved.
        /// </summary>
        /// <returns>True if valid Info data is available.</returns>
        public override bool CheckAccess()
            => (ReadInfoAsync().Result == DataStatus.Good);

        /// <summary>
        /// Returns true if the specified current value is OK.
        /// </summary>
        /// <param name="current">The current value</param>
        /// <returns>True if current value OK</returns>
        public static bool IsReportIDOk(int id)
            => ((id > REPORTS_ID) && (id <= (REPORTS_ID + MAX_REPORTS)));

        /// <summary>
        /// Returns true if the specified current value is OK.
        /// </summary>
        /// <param name="current">The current value</param>
        /// <returns>True if current value OK</returns>
        public static bool IsCurrentValueOk(uint current)
            => ((current == 0) || ((current >= 6000) && (current <= 63000)));

        /// <summary>
        /// Returns true if the specified delay value is OK.
        /// </summary>
        /// <param name="delay">The delay value</param>
        /// <returns>True if delay value OK</returns>
        public static bool IsDelayValueOk(uint delay)
            => ((delay == 0) || ((delay >= 1) && (delay <= 860400)));

        /// <summary>
        /// Returns true if the specified energy value is OK.
        /// </summary>
        /// <param name="energy">The energy value</param>
        /// <returns>True if energy value OK</returns>
        public static bool IsEnergyValueOk(uint energy)
            => ((energy == 0) || (energy == 1) || (energy <= 999999999));


        /// <summary>
        /// Returns true if the RFID tag string is OK.
        /// </summary>
        /// <param name="tag">The tag string</param>
        /// <returns>True if tag string OK</returns>
        public static bool IsRFIDTagStringOk(string tag)
            => (!string.IsNullOrEmpty(tag) &&
                (tag.Length == 16) &&
                Int32.TryParse(tag, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int _));

        /// <summary>
        /// Returns true if the RFID classifier string is OK.
        /// </summary>
        /// <param name="classifier">The classifier string</param>
        /// <returns>True if classifier string OK</returns>
        public static bool IsRFIDClassifierStringOk(string classifier)
            => (string.IsNullOrEmpty(classifier) &&
                (classifier.Length == 20) &&
                Int32.TryParse(classifier, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int _));

        /// <summary>
        /// Returns true if the specified output command value is OK.
        /// </summary>
        /// <param name="energy">The output command value</param>
        /// <returns>True if output command value OK</returns>
        public static bool IsOutputValueOk(uint command)
            => ((command == 0) || (command == 1));

        /// <summary>
        /// Returns true if the specified enable modifier value is OK.
        /// </summary>
        /// <param name="energy">The enable modifier value</param>
        /// <returns>True if enable modifier value OK</returns>
        public static bool IsEnableValueOk(uint modifier)
            => ((modifier == 0) || (modifier == 1));

        #endregion
    }
}
