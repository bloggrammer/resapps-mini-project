using WellTestAnalysis.Dtos;
using WellTestAnalysis.Models;

namespace WellTestAnalysis.Services
{
    public interface IWellTestAnalysisService
    {
        IEnumerable<PressureTimeTrend> RunAnalysis(IEnumerable<PressureTimeData> observedData, float startTime, float endTime, float slope);
        IEnumerable<PressureTimeData> SmoothObservedPressure(IEnumerable<PressureTimeData> observeData, float smootheningFactor = 0.5f);
        List<PressureTimeData> GetObservationDataStartingFromTimeBeforeTest(IEnumerable<PressureTimeData> observationDataList, double startTime, float timeBefore = 24.0f);
        double GetSlope(IEnumerable<PressureTimeData> observedData, float startTime, float endTime);
    }
}
