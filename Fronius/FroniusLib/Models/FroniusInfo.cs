namespace FroniusLib.Models
{
    #region Using Directives

    using UtilityLib;

    #endregion

    public class FroniusInfo
    {
        public FroniusSettings Settings    { get; set; } = new FroniusSettings();
        public bool            IsStartupOk { get; set; }
        public bool            IsLocked    { get; set; }
        public DataStatus      Status      { get; set; } = new DataStatus();
        }
}
