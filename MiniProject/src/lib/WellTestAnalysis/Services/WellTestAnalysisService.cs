using WellTestAnalysis.Models;
using WellTestAnalysis.Utils;
using static System.Formats.Asn1.AsnWriter;

namespace WellTestAnalysis.Services
{
    public class WellTestAnalysisService : IWellTestAnalysisService
    {
        public IEnumerable<PressureTimeTrend> RunAnalysis(IList<PressureTimeData> observedData, float startTime, float slope, bool usePi)
        {        

            double intercept = GetIntercept(observedData, startTime, slope, usePi);

            foreach (var data in observedData)
            {
                if (data.Time < startTime)
                {
                    double? trendPressure = MathUtil.CalculateLinearTrend(data.Time, slope, intercept);
                    double? detrendedPressure = MathUtil.CalculateDetrendedData(data.Pressure, trendPressure);

                    yield return new PressureTimeTrend(MathUtil.Round(data.Time), MathUtil.Round(data.Pressure), MathUtil.Round(detrendedPressure), MathUtil.Round(trendPressure));
                }
            }
        }

        private double GetIntercept(IList<PressureTimeData> observedData, float startTime, float slope, bool usePi)
        {
           if(usePi)
                return Convert.ToDouble(observedData[0].Pressure);

            var (time, pressure) = ExtractPressureTimeSeries(observedData, startTime);
           return MathUtil.CalculateIntercept(time, pressure, slope);
        }

        public (double slope, double intercept) GetSlopeAndIntercept(IEnumerable<PressureTimeData> observedData, float startTime)
        {
            var (time, pressure) = ExtractPressureTimeSeries(observedData, startTime);

            return MathUtil.CalculateSlope(time, pressure);
        }
        public List<PressureTimeData> GetObservationDataStartingFromTimeBeforeTest(IEnumerable<PressureTimeData> observationDataList, double startTime, float timeBefore = 24.0f)
        {
            double startTime24HoursBefore = startTime - timeBefore;

            List<PressureTimeData> filteredData = observationDataList.Where(data => data.Time >= startTime24HoursBefore).ToList();

            return filteredData;
        }

        public IEnumerable<PressureTimeData> SmoothObservedPressure(IEnumerable<PressureTimeData> observeData, float smootheningFactor = 0.5f)
        {
            if (observeData == null || !observeData.Any()) yield return (PressureTimeData)Enumerable.Empty<PressureTimeData>();

            double previousPressure = double.MinValue;

            foreach (var observation in observeData)
            {
                if (IsPressureSmoothable(observation.Pressure, previousPressure, smootheningFactor))
                {
                    previousPressure = Convert.ToDouble(observation.Pressure);
                    yield return observation;
                }
            }
        }
        private static bool IsValidObservation(float startTime, PressureTimeData x)
        {
            return x.Time < startTime && x.Pressure != null && x.Time != null;
        }

        private bool IsPressureSmoothable(double? currentPressure, double? previousPressure, double smootheningFactor)
        {
            if (currentPressure == null) return false;

            return Math.Abs(Convert.ToDouble(currentPressure) - Convert.ToDouble(previousPressure)) >= smootheningFactor;
        }
        private (double[] time, double[] pressure) ExtractPressureTimeSeries(IEnumerable<PressureTimeData> observedData, float startTime)
        {
            var time = observedData.Where(x => IsValidObservation(startTime, x)).Select(x => Convert.ToDouble(x.Time)).ToArray();
            var pressure = observedData.Where(x => IsValidObservation(startTime, x)).Select(x => Convert.ToDouble(x.Pressure)).ToArray();

            return (time, pressure);
        }
    }
}
