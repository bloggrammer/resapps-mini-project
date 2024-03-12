using FluentNHibernate.Mapping;
using WellTestAnalysis.Models;

namespace WellTestAnalysis.App.Data.Mapping
{
    public class PressureTimeTrendMap : SubclassMap<PressureTimeTrend>
    {
        public PressureTimeTrendMap()
        {
            Table("PressureTimeTrend");
            KeyColumn("PressureTimeData_id"); 
            Map(x => x.DetrendedPressure);
        }
    }
}
