using WellTestAnalysis.Models;
using WellTestAnalysis.Utils;

namespace WellTestAnalysis.Services
{
    public class WellTestAnalysisService : IWellTestAnalysisService
    {
        public IEnumerable<PressureTimeTrend> RunAnalysis(IEnumerable<PressureTimeData> observedData, float startTime, float endTime, float slope)
        {
            var (time, pressure) = ExtractPressureTimeRange(observedData, startTime, endTime);
            double intercept = MathUtil.CalculateIntercept(time, pressure, slope);

            foreach (var data in observedData)
            {
                if (data.Time >= startTime && data.Time <= endTime)
                {
                    double? trendPressure = MathUtil.CalculateLinearTrend(data.Time, slope, intercept);
                    double? detrendedPressure = MathUtil.CalculateDetrendedData(data.Pressure, trendPressure);

                    if (detrendedPressure > 0)
                        yield return new PressureTimeTrend(data.Time, MathUtil.Round(trendPressure), MathUtil.Round(detrendedPressure));
                }
            }
        }
        public double GetSlope(IEnumerable<PressureTimeData> observedData, float startTime, float endTime)
        {
            var (time, pressure) = ExtractPressureTimeRange(observedData, startTime, endTime);
            var tuple = MathUtil.CalculateSlope(time, pressure); ;

            return tuple.slope;
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
            //smoothedSeries.Add(observeData[0]); // Add the first data point as it's always included

            //for (int i = 1; i < observations.Count; i++)
            //{
            //    var previousPressure = smoothedSeries[smoothedSeries.Count - 1].Pressure;
            //    var currentPressure = observations[i].Pressure;

            //    if (IsPressureSmoothable(currentPressure, previousPressure, smootheningFactor))
            //    {
            //        smoothedSeries.Add(observations[i]);
            //    }
            //}
            //  return ResultStatus<List<PressureTimeData>>.Pass(smoothedSeries);
        }
        /// <summary>
        /// Get pressure vs time based on a specific time range
        /// </summary>
        /// <param name="observedData"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns>time, pressue</returns>
        private static (double[] time, double[] pressure) ExtractPressureTimeRange(IEnumerable<PressureTimeData> observedData, float startTime, float endTime)
        {
            var pressure = observedData.Where(x => x.Time >= startTime && x.Time <= endTime).Select(x => Convert.ToDouble(x.Pressure)).ToArray();

            var time = observedData.Where(x => x.Time >= startTime && x.Time <= endTime).Select(x => Convert.ToDouble(x.Time)).ToArray();

            return (time, pressure);

        }
        private bool IsPressureSmoothable(double? currentPressure, double? previousPressure, double smootheningFactor)
        {
            if (currentPressure == null) return false;

            return Math.Abs(Convert.ToDouble(currentPressure) - Convert.ToDouble(previousPressure)) >= smootheningFactor;
        }

    }
}
