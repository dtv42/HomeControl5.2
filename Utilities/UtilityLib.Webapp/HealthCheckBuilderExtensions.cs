using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System;
using System.Collections.Generic;

namespace UtilityLib.Webapp
{
    public static class HealthCheckBuilderExtensions
    {
        const string NAME = "ping";

        /// <summary>
        /// Add a health check using ping.
        /// </summary>
        /// <param name="builder">The <see cref="IHealthChecksBuilder"/>.</param>
        /// <param name="name">The health check name. Optional. If <c>null</c> the type name 'uri-group' will be used for the name.</param>
        /// <param name="address>"The ping host or address."</param>
        /// <param name="failureStatus"></param>
        /// The <see cref="HealthStatus"/> that should be reported when the health check fails. Optional. If <c>null</c> then
        /// the default status of <see cref="HealthStatus.Unhealthy"/> will be reported.
        /// <param name="tags">A list of tags that can be used to filter sets of health checks. Optional.</param>
        /// <param name="timeout">An optional System.TimeSpan representing the timeout of the check.</param>
        /// <returns>The <see cref="IHealthChecksBuilder"/>.</returns>
        public static IHealthChecksBuilder AddPing(
            this IHealthChecksBuilder builder,
            PingHealthCheckOptions options,
            string name)
        {
            var registrationName = name ?? NAME;

            return builder.Add(
                new HealthCheckRegistration(
                    registrationName,
                    sp =>
                    {
                        return new PingHealthCheck(options);
                    },
                    null,
                    null,
                    null));
        }
    }
}
