namespace UtilityLib.Webapp
{
    #region Using Directives

    using System.ComponentModel.DataAnnotations;

    #endregion Using Directives

    /// <summary>
    /// HttpClient settings.
    /// </summary>
    public interface IHttpClientSettings
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
    }
}
