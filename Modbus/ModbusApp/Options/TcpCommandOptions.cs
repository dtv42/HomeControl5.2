namespace ModbusApp.Options
{
    #region Using Directives

    using ModbusLib.Models;

    #endregion

    public class TcpCommandOptions : ITcpClientSettings
    {
        public TcpMasterData TcpMaster { get; set; } = new TcpMasterData();
        public TcpSlaveData TcpSlave { get; set; } = new TcpSlaveData();
    }
}
