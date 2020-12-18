namespace UtilityLib.Webapp
{
    #region Using Directives

    using System.ComponentModel.DataAnnotations;

    #endregion Using Directives

    /// <summary>
    /// Gateway (Http client) settings.
    /// </summary>
    public class GatewaySettings : IGatewaySettings
    {
        /// <summary>
        /// The Http client base address.
        /// </summary>
        [Uri]
        public string Address { get; set; } = "http://localhost";

        /// <summary>
        /// The Http client timeout (msec).
        /// </summary>
        [Range(0, int.MaxValue)]
        public int Timeout { get; set; } = 1000;

        /// <summary>
        /// The number of retries (Polly retry policy).
        /// </summary>
        [Range(1, int.MaxValue)]
        public int Retries { get; set; } = 3;

        /// <summary>
        /// The Polly retry policy wait interval (msec) .
        /// </summary>
        [Range(0, int.MaxValue)]
        public int Wait { get; set; } = 600;
    }
}
