// --------------------------------------------------------------------------------------------------------------------
// <copyright file="StatusCheck.cs" company="DTV-Online">
//   Copyright(c) 2020 Dr. Peter Trimmel. All rights reserved.
// </copyright>
// <license>
//   Licensed under the MIT license. See the LICENSE file in the project root for more information.
// </license>
// <created>13-5-2020 13:53</created>
// <author>Peter Trimmel</author>
// --------------------------------------------------------------------------------------------------------------------
namespace UtilityLib.Webapp
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Diagnostics.HealthChecks;
    using Microsoft.Extensions.Logging;

    #endregion

    /// <summary>
    /// Helper class implementing a status health check for a gateway.
    /// </summary>
    public class GatewayHealthCheck : BaseClass, IHealthCheck
    {
        #region Private Fields

        private readonly IGateway _gateway;

        #endregion Private Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="StatusCheck"/> class.
        /// </summary>
        /// <param name="gateway">The gateway instance.</param>
        /// <param name="logger">The logger instance.</param>
        public GatewayHealthCheck(IGateway gateway, ILogger<GatewayHealthCheck> logger)
            : base(logger)
        {
            _gateway = gateway;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Runs the health check returning the gateway health status.
        /// </summary>
        /// <param name="context">A context object associated with the current execution.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> that can be used to cancel the health check.</param>
        /// <returns>
        /// A Task that completes when the health check has finished, yielding the status of the gateway being checked.
        /// </returns>
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                if (await _gateway.CheckAccessAsync())
                {
                    if (_gateway.Status.IsGood)
                    {
                        return HealthCheckResult.Healthy($"Gateway status is Good: {_gateway.Status.Explanation}",
                            new Dictionary<string, object>() { { "Status", _gateway.Status } });
                    }
                    else if (_gateway.Status.IsUncertain)
                    {
                        return HealthCheckResult.Degraded($"Gateway status is Uncertain: {_gateway.Status.Explanation}", null,
                            new Dictionary<string, object>() { { "Status", _gateway.Status } });
                    }
                    else if (_gateway.Status.IsBad)
                    {
                        return HealthCheckResult.Unhealthy($"Gateway status is Bad: {_gateway.Status.Explanation}", null,
                            new Dictionary<string, object>() { { "Status", _gateway.Status } });
                    }
                    else
                    {
                        return HealthCheckResult.Degraded($"Gateway status Unknown: {_gateway.Status.Explanation}", null,
                                new Dictionary<string, object>() { { "Status", _gateway.Status } });
                    }
                }
                else
                {
                    return HealthCheckResult.Degraded($"Gateway status check failed - status: {_gateway.Status.Explanation}", null,
                            new Dictionary<string, object>() { { "Status", _gateway.Status } });
                }
            }
            catch
            {
                return HealthCheckResult.Unhealthy($"Error in gateway status check - status:  {_gateway.Status.Explanation}", null,
                        new Dictionary<string, object>() { { "Status", _gateway.Status } });
            }
        }

        #endregion
    }
}
