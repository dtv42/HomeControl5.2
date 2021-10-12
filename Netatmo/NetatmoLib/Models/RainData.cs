namespace NetatmoLib.Models
{
    #region Using Directives

    using System;

    #endregion

    public class RainData
    {
        public string ModuleName { get; set; } = string.Empty;
        public bool Reachable { get; set; }
        public double Battery { get; set; }
        public DateTime TimeUtc { get; set; } = new DateTime();
        public double Rain { get; set; }
        public double SumRain1 { get; set; }
        public double SumRain24 { get; set; }

        public void Update(ModuleRawData data)
        {
            ModuleName = data.ModuleName;
            Battery = data.BatteryPercent;
            Reachable = data.Reachable;

            // Update dashboard data.
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeUtc = epoch.AddSeconds(data.DashboardData.TimeUtc);
            Rain = data.DashboardData.Rain;
            SumRain1 = data.DashboardData.SumRain1;
            SumRain24 = data.DashboardData.SumRain24;
        }
    }
}
