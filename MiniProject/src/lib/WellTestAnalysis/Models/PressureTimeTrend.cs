namespace WellTestAnalysis.Models
{
    public class PressureTimeTrend : PressureTimeData
    {
        protected PressureTimeTrend() { }
        public PressureTimeTrend(double? time, double? pressure, double? detrended, double? trendedPressure) : base(time, pressure)
        {
            DetrendedPressure = detrended;
            TrendedPressure = trendedPressure;
        }
        public virtual double? DetrendedPressure { get; set; }
        public virtual double? TrendedPressure { get; set; }
    }
}
