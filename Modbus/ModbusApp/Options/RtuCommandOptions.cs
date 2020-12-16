namespace ModbusApp.Options
{
    #region Using Directives

    using ModbusLib.Models;

    #endregion

    public class RtuCommandOptions : IRtuClientSettings
    {
        public RtuMasterData RtuMaster { get; set; } = new RtuMasterData();
        public RtuSlaveData RtuSlave { get; set; } = new RtuSlaveData();
    }
}
