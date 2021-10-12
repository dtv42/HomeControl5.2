namespace NetatmoLib.Models
{
    #region Using Directives

    using System;

    #endregion

    public class MainData
    {
        public string ModuleName { get; set; } = string.Empty;
        public bool Reachable { get; set; }
        public double WifiStatus { get; set; }
        public DateTime TimeUtc { get; set; } = new DateTime();
        public double Temperature { get; set; }
        public double CO2 { get; set; }
        public double Humidity { get; set; }
        public double Noise { get; set; }
        public double Pressure { get; set; }
        public double AbsolutePressure { get; set; }
        public double MinTemperature { get; set; }
        public double MaxTemperature { get; set; }
        public DateTime DateMaxTemperature { get; set; } = new DateTime();
        public DateTime DateMinTemperature { get; set; } = new DateTime();
        public string PressureTrend { get; set; } = string.Empty;

        public void Update(DeviceRawData data)
        {
            ModuleName = data.ModuleName;
            WifiStatus = data.WifiStatus;
            Reachable = data.Reachable;

            // Update dashboard data.
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            TimeUtc = epoch.AddSeconds(data.DashboardData.TimeUtc);
            Temperature = data.DashboardData.Temperature;
            CO2 = data.DashboardData.CO2;
            Humidity = data.DashboardData.Humidity;
            Noise = data.DashboardData.Noise;
            Pressure = data.DashboardData.Pressure;
            AbsolutePressure = data.DashboardData.AbsolutePressure;
            MinTemperature = data.DashboardData.MinTemp;
            MaxTemperature = data.DashboardData.MaxTemp;
            DateMaxTemperature = epoch.AddSeconds(data.DashboardData.DateMaxTemp);
            DateMinTemperature = epoch.AddSeconds(data.DashboardData.DateMinTemp);
            PressureTrend = data.DashboardData.PressureTrend;
        }

    }
}
