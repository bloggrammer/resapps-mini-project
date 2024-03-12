namespace WellTestAnalysis.Models
{
    public class AnalysisResult
    {
        public virtual IList<PressureTimeData> BeforeTestData { get; set; }
        public virtual IList<PressureTimeTrend> LinearTrendData { get; set; }
        public virtual TestInfo TestInfo { get; set; }
        public virtual string Name { get; set; }
        public virtual int Id { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
