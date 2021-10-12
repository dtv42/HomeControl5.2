namespace UtilityWeb.Models
{
    #region Using Directives

    using UtilityLib;

    #endregion

    public class WebGatewayInfo
    {
        public WebGatewaySettings Settings    { get; set; } = new WebGatewaySettings();
        public bool               IsStartupOk { get; set; }
        public bool               IsLocked    { get; set; }
        public DataStatus         Status      { get; set; } = new DataStatus();
    }
}
