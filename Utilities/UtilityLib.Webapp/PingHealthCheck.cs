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
    using Microsoft.Extensions.Logging;

    #endregion Using Directives

    /// <summary>
    /// Helper class implementing a ping health check.
    /// </summary>
    public class PingHealthCheck : IHealthCheck
    {
        #region Private Data Members

        private readonly IPingHealthCheckOptions _options;
        private DateTime _lastPingTime = DateTime.MinValue;
        private HealthCheckResult _lastPingResult = HealthCheckResult.Healthy();

        #endregion Private Data Members

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="PingHealthCheck"/> class.
        /// </summary>
        /// <param name="options">The ping options.</param>
        public PingHealthCheck(IPingHealthCheckOptions options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        #endregion

        /// <summary>
        /// Runs the health check, returning the corresponding status of the ping command.
        /// </summary>
        /// <param name="context">A context object associated with the current execution.</param>
        /// <param name="cancellationToken">A cancellation token that can be used to cancel the health check.</param>
        /// <returns>A HealthCheckResult object.</returns>
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return new HealthCheckResult(context.Registration.FailureStatus, description: $"{nameof(PingHealthCheck)} execution is cancelled.");
                }

                if (_options.Interval != 0 && _lastPingTime.AddSeconds(_options.Interval) > DateTime.Now)
                {
                    return _lastPingResult;
                }

                using var ping = new Ping();
                _lastPingTime = DateTime.Now;

                var reply = await ping.SendPingAsync(_options.Address, _options.Timeout);

                if (reply.Status != IPStatus.Success)
                {
                    _lastPingResult = HealthCheckResult.Unhealthy(description: $"Ping to address #{_options.Address} is not successful.");

                }
                else if (reply.RoundtripTime >= _options.Timeout)
                {
                    _lastPingResult = HealthCheckResult.Degraded(description: $"Ping to address #{_options.Address} exceeded timeout.");
                }
                else
                {
                    _lastPingResult = HealthCheckResult.Healthy();
                }
            }
            catch(Exception ex)
            {
                _lastPingResult = new HealthCheckResult(context.Registration.FailureStatus, exception: ex);
            }

            return _lastPingResult;
        }
    }
}
