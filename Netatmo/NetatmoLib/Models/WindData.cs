namespace NetatmoLib.Models
{
    #region Using Directives

    using System;

    #endregion

    public class WindData
    {
        public string ModuleName { get; set; } = string.Empty;
        public bool Reachable { get; set; }
        public double Battery { get; set; }
        public DateTime TimeUtc { get; set; } = new DateTime();
        public double WindStrength { get; set; }
        public double WindAngle { get; set; }
        public double GustStrength { get; set; }
        public double GustAngle { get; set; }
        public double MaxWindStrength { get; set; }
        public double MaxWindAngle { get; set; }
        public DateTime DateMaxWindStrength { get; set; } = new DateTime();

        public void Update(ModuleRawData data)
        {
            ModuleName = data.ModuleName;
            Battery = data.BatteryPercent;
            Reachable = data.Reachable;

            // Update dashboard data.
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeUtc = epoch.AddSeconds(data.DashboardData.TimeUtc);
            WindStrength = data.DashboardData.WindStrength;
            WindAngle = data.DashboardData.WindAngle;
            GustStrength = data.DashboardData.GustStrength;
            GustAngle = data.DashboardData.GustAngle;
            MaxWindStrength = data.DashboardData.MaxWindStr;
            MaxWindAngle = data.DashboardData.MaxWindAngle;
            DateMaxWindStrength = epoch.AddSeconds(data.DashboardData.DateMaxWindStr);
        }
    }
}
