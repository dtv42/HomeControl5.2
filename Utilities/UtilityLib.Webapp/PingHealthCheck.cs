// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PingHealthCheck.cs" company="DTV-Online">
//   Copyright (c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>15-12-2020 10:51</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace UtilityLib.Webapp
{
    #region Using Directives

    using System;
    using System.Net.NetworkInformation;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Diagnostics.HealthChecks;

    #endregion Using Directives

    public class PingHealthCheck : IHealthCheck
    {
        #region Private Data Members

        private readonly string _host;
        private readonly int _timeout;
        private readonly int _pingInterval;
        private DateTime _lastPingTime = DateTime.MinValue;
        private HealthCheckResult _lastPingResult = HealthCheckResult.Healthy();

        #endregion Private Data Members

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PingHealthCheck"/> class.
        /// </summary>
        /// <param name="host">The ping host.</param>
        /// <param name="timeout">The ping timeout (msec).</param>
        /// <param name="pingInterval">The ping interval (sec).</param>
        public PingHealthCheck(string host, int timeout, int pingInterval = 0)
        {
            _host = host;
            _timeout = timeout;
            _pingInterval = pingInterval;
        }

        #endregion

        /// <summary>
        /// Runs the health check, returning the corresponding status of the ping command.
        /// </summary>
        /// <param name="context">A context object associated with the current execution.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the health check.</param>
        /// <returns></returns>
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            if (_pingInterval != 0 && _lastPingTime.AddSeconds(_pingInterval) > DateTime.Now)
            {
                return _lastPingResult;
            }

            try
            {
                using var ping = new Ping();
                _lastPingTime = DateTime.Now;

                var reply = await ping.SendPingAsync(_host, _timeout);

                if (reply.Status != IPStatus.Success)
                {
                    _lastPingResult = HealthCheckResult.Unhealthy();
                }
                else if (reply.RoundtripTime >= _timeout)
                {
                    _lastPingResult = HealthCheckResult.Degraded();
                }
                else
                {
                    _lastPingResult = HealthCheckResult.Healthy();
                }
            }
            catch
            {
                _lastPingResult = HealthCheckResult.Unhealthy();
            }

            return _lastPingResult;
        }
    }
}
