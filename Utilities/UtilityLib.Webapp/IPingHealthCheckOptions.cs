namespace UtilityLib.Webapp
{
    #region Using Directives

    using System.ComponentModel.DataAnnotations;

    #endregion Using Directives

    public interface IPingHealthCheckOptions
    {
        /// <summary>
        /// The Ping host name or address.
        /// </summary>
        string Address { get; set; }

        /// <summary>
        /// The Ping timeout (msec).
        /// </summary>
        [Range(0, int.MaxValue)]
        int Timeout { get; set; }

        /// <summary>
        /// The Ping health check interval (sec).
        /// </summary>
        [Range(0, int.MaxValue)]
        int Interval { get; set; }
    }
}
