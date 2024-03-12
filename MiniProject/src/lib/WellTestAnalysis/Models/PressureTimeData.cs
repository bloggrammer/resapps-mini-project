namespace WellTestAnalysis.Models
{
    public class PressureTimeData
    {
        protected PressureTimeData() { }
        public PressureTimeData(double? time, double? pressure)
        {
            Time = time;
            Pressure = pressure;
        }
        public virtual double? Time { get; set; }     

        public virtual double? Pressure { get; set; }
        public virtual int Id { get; set; }
    }
}
