namespace UtilityLib.Webapp
{
    #region Using Directives

    using System.ComponentModel.DataAnnotations;

    #endregion Using Directives

    /// <summary>
    /// HttpError policy settings.
    /// </summary>
    public interface IHttpErrorSettings
    {
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
