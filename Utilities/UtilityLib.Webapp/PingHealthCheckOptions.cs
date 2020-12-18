namespace UtilityLib.Webapp
{
    #region Using Directives

    using System.ComponentModel.DataAnnotations;

    #endregion Using Directives

    public class PingHealthCheckOptions : IPingHealthCheckOptions
    {
        /// <summary>
        /// The Ping host name or address.
        /// </summary>
        public string Address { get; set; } = "localhost";

        /// <summary>
        /// The Ping timeout (msec).
        /// </summary>
        [Range(0, int.MaxValue)]
        public int Timeout { get; set; } = 100;

        /// <summary>
        /// The Ping health check interval (sec).
        /// </summary>
        [Range(0, int.MaxValue)]
        public int Interval { get; set; } = 10;
    }
}
