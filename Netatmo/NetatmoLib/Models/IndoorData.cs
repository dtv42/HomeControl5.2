namespace NetatmoLib.Models
{
    #region Using Directives

    using System;

    #endregion

    public class IndoorData
    {
        public string ModuleName { get; set; } = string.Empty;
        public bool Reachable { get; set; }
        public double Battery { get; set; }
        public DateTime TimeUtc { get; set; } = new DateTime();
        public double Temperature { get; set; }
        public double CO2 { get; set; }
        public double Humidity { get; set; }
        public double MinTemperature { get; set; }
        public double MaxTemperature { get; set; }
        public DateTime DateMaxTemperature { get; set; } = new DateTime();
        public DateTime DateMinTemperature { get; set; } = new DateTime();

        public void Update(ModuleRawData data)
        {
            ModuleName = data.ModuleName;
            Battery = data.BatteryPercent;
            Reachable = data.Reachable;

            // Update dashboard data.
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeUtc = epoch.AddSeconds(data.DashboardData.TimeUtc);
            Temperature = data.DashboardData.Temperature;
            CO2 = data.DashboardData.CO2;
            Humidity = data.DashboardData.Humidity;
            MinTemperature = data.DashboardData.MinTemp;
            MaxTemperature = data.DashboardData.MaxTemp;
            DateMaxTemperature = epoch.AddSeconds(data.DashboardData.DateMaxTemp);
            DateMinTemperature = epoch.AddSeconds(data.DashboardData.DateMinTemp);
        }
    }
}
