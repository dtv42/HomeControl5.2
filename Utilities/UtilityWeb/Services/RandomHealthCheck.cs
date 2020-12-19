namespace UtilityWeb.Services
{
    #region Using Directives

    using System;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Diagnostics.HealthChecks;

    #endregion Using Directives

    /// <summary>
    /// Example of a simple custom health check.
    /// </summary>
    public class RandomHealthCheck : IHealthCheck
    {
        /// <summary>
        /// Runs the health check returning a (random) health status.
        /// </summary>
        /// <param name="context">A context object associated with the current execution.</param>
        /// <param name="cancellationToken"> A <see cref="CancellationToken"/> that can be used to cancel the health check.</param>
        /// <returns>
        /// A Task that completes when the health check has finished, yielding the status of the component being checked.
        /// </returns>
        public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            if (DateTime.UtcNow.Minute % 2 == 0)
            {
                return Task.FromResult(HealthCheckResult.Healthy());
            }

            return Task.FromResult(HealthCheckResult.Unhealthy(description: $"The healthcheck {context.Registration.Name} failed at minute {DateTime.UtcNow.Minute}"));
        }
    }
}
