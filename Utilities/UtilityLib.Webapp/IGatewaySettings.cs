namespace UtilityLib.Webapp
{
    #region Using Directives

    using System.ComponentModel.DataAnnotations;

    #endregion Using Directives

    /// <summary>
    /// Gateway (Http client) settings.
    /// </summary>
    public interface IGatewaySettings
    {
        /// <summary>
        /// The Http client base address.
        /// </summary>
        [Uri]
        string Address { get; set; }

        /// <summary>
        /// The Http client timeout (msec).
        /// </summary>
        [Range(0, int.MaxValue)]
        int Timeout { get; set; }

        /// <summary>
        /// The number of retries (Polly retry policy).
        /// </summary>
        [Range(1, int.MaxValue)]
        int Retries { get; set; }

        /// <summary>
        /// The Polly retry policy wait interval (msec) .
        /// </summary>
        [Range(0, int.MaxValue)]
        int Wait { get; set; }
    }
}
