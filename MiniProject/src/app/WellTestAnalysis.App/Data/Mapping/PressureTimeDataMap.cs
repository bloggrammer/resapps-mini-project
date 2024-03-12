using FluentNHibernate.Mapping;
using WellTestAnalysis.Models;

namespace WellTestAnalysis.App.Data.Mapping
{
    public class PressureTimeDataMap : ClassMap<PressureTimeData>
    {
        public PressureTimeDataMap()
        {
            Table("PressureTimeData");
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Time); 
            Map(x => x.Pressure);
        }
    }
}
