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
namespace ModbusRTU.Services
{
    #region Using Directives

    using System;
    using System.Net.NetworkInformation;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Diagnostics.HealthChecks;

    using ModbusLib;

    #endregion Using Directives

    /// <summary>
    /// Helper class implementing a ModbusRTU gateway health check.
    /// </summary>
    public class RtuHealthCheck : IHealthCheck
    {
        #region Private Data Members

        private readonly IRtuModbusClient _client;

        #endregion Private Data Members

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="RtuHealthCheck"/> class.
        /// </summary>
        /// <param name="client">The RtuModbus client.</param>
        public RtuHealthCheck(IRtuModbusClient client)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        #endregion

        /// <summary>
        /// Runs the health check returning the ping health status.
        /// </summary>
        /// <param name="context">A context object associated with the current execution.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> that can be used to cancel the health check.</param>
        /// <returns>
        /// A Task that completes when the health check has finished, yielding the status of the ping being executed.
        /// </returns>
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    return Task.FromResult(new HealthCheckResult(context.Registration.FailureStatus, description: $"{nameof(RtuHealthCheck)} execution is cancelled."));
                }

                if (_client.Connected)
                {
                    return Task.FromResult(HealthCheckResult.Healthy(description: $"RtuModbusClient at port {_client.RtuMaster.SerialPort} connected."));
                }
                else
                {
                    if (_client.Connect())
                    {
                        _client.Disconnect();
                        return Task.FromResult(HealthCheckResult.Healthy(description: $"RtuModbusClient at port {_client.RtuMaster.SerialPort} sucessfully connected."));
                    }
                    else {
                        return Task.FromResult(HealthCheckResult.Unhealthy(description: $"RtuModbusClient at port {_client.RtuMaster.SerialPort} not connected."));
                    }
                }
            }
            catch(Exception ex)
            {
                return Task.FromResult(new HealthCheckResult(context.Registration.FailureStatus, exception: ex));
            }
        }
    }
}
