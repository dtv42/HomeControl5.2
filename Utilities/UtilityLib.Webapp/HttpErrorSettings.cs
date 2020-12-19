namespace UtilityLib.Webapp
{
    #region Using Directives

    using System.ComponentModel.DataAnnotations;

    #endregion Using Directives

    /// <summary>
    /// HttpError policy settings.
    /// </summary>
    public class HttpErrorSettings : IHttpErrorSettings
    {
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
