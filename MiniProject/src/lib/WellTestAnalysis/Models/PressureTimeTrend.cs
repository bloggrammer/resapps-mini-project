namespace WellTestAnalysis.Models
{
    public class PressureTimeTrend : PressureTimeData
    {
        protected PressureTimeTrend() { }
        public PressureTimeTrend(double? time, double? pressure, double? detrended) : base(time, pressure)
        {
            DetrendedPressure = detrended;
        }
        public virtual double? DetrendedPressure { get; set; }
    }
}
