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
        private readonly string _host;
        private readonly int _timeout;
        private readonly int _pingInterval;
        private DateTime _lastPingTime = DateTime.MinValue;
        private HealthCheckResult _lastPingResult = HealthCheckResult.Healthy();

        public PingHealthCheck(string host, int timeout, int pingInterval = 0)
        {
            _host = host;
            _timeout = timeout;
            _pingInterval = pingInterval;
        }

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
