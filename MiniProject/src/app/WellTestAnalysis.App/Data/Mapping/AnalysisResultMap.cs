using FluentNHibernate.Mapping;
using WellTestAnalysis.Models;

namespace WellTestAnalysis.App.Data.Mapping
{
    public class AnalysisResultMap : ClassMap<AnalysisResult>
    {
        public AnalysisResultMap()
        {
            Table("AnalysisResult");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Name).Unique().Not.Nullable();
            HasMany(x => x.BeforeTestData).Cascade.All();
            HasMany(x => x.LinearTrendData).Cascade.All();
            Component(x => x.TestInfo, c =>
            {
                c.Map(x => x.WellName);
                c.Map(x => x.StartTime);
                c.Map(x => x.EndTime);
                c.Map(x => x.Slope);
            });
        }
    }
}
