using WellTestAnalysis.Dtos;
using WellTestAnalysis.Models;

namespace WellTestAnalysis.Services
{
    public interface IWellTestAnalysisService
    {
        IEnumerable<PressureTimeTrend> RunAnalysis(IList<PressureTimeData> observedData, float startTime, float slope, bool usePi);
        IEnumerable<PressureTimeData> SmoothObservedPressure(IEnumerable<PressureTimeData> observeData, float smootheningFactor = 0.5f);
        List<PressureTimeData> GetObservationDataStartingFromTimeBeforeTest(IEnumerable<PressureTimeData> observationDataList, double startTime, float timeBefore = 24.0f);
        (double slope, double intercept) GetSlopeAndIntercept(IEnumerable<PressureTimeData> observedData, float startTime);
    }
}
