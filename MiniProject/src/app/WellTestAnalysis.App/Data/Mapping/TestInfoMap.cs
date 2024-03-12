using FluentNHibernate.Mapping;
using WellTestAnalysis.Models;

namespace WellTestAnalysis.App.Data.Mapping
{
    public class TestInfoMap : ClassMap<TestInfo>
    {
        public TestInfoMap()
        {
            Table("TestInfo"); 
            Id(x => x.Id).GeneratedBy.Identity();
            Id(x => x.WellName);
            Map(x => x.StartTime);
            Map(x => x.EndTime);
            Map(x => x.Slope);
            Map(x => x.SmoothingFactor);
        }
    }
}
