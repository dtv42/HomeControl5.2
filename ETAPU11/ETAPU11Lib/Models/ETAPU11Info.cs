namespace ETAPU11Lib.Models
{
    #region Using Directives

    using UtilityLib;

    #endregion

    public class ETAPU11Info
    {
        public ETAPU11Settings Settings { get; set; } = new ETAPU11Settings();
        public bool IsStartupOk { get; set; }
        public bool IsLocked { get; set; }
        public DataStatus Status { get; set; } = new DataStatus();
    }
}
