namespace UtilityLib.Webapp
{
    #region Using Directives

    using System.ComponentModel.DataAnnotations;

    #endregion Using Directives

    /// <summary>
    /// Gateway (Http client) settings.
    /// </summary>
    public interface IGatewaySettings : IHttpClientSettings, IHttpErrorSettings
    {}
}
