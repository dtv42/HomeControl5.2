namespace EM300LRLib.Models
{
    #region Using Directives

    using UtilityLib;

    #endregion

    public class EM300LRInfo
    {
        public IEM300LRSettings Settings   { get; set; } = new EM300LRSettings();
        public bool            IsStartupOk { get; set; }
        public bool            IsLocked    { get; set; }
        public DataStatus      Status      { get; set; } = new DataStatus();
        }
}
