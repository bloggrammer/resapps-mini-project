namespace WellTestAnalysis.Models
{
    public class TestInfo
    {
        public virtual int Id { get; set; }
        public virtual string WellName { get; set; }
        public virtual float StartTime { get; set; }
        public virtual float EndTime { get; set; }
        public virtual float Slope { get; set; }
        public virtual float SmoothingFactor { get; set; }

        public override string ToString()
        {
            return WellName;
        }
    }
}
