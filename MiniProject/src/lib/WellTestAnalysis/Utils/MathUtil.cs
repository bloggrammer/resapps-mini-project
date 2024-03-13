namespace WellTestAnalysis.Utils
{
    public static class MathUtil
    {
        public static (double slope, double intercept) CalculateSlope(double[] x, double[] y)
        {
            if (x == null || y == null || x.Length != y.Length || x.Length < 2)
            {
                throw new ArgumentException("Invalid input data. Both x and y must be non-null lists of the same length with at least two elements.");
            }

            int n = x.Length;
            double sumX = 0, sumY = 0, sumXY = 0, sumX2 = 0;

            for (int i = 0; i < n; i++)
            {
                double xi = x[i];
                double yi = y[i];

                sumX += xi;
                sumY += yi;
                sumXY += xi * yi;
                sumX2 += xi * xi;
            }

            double meanX = sumX / n;
            double meanY = sumY / n;

            double slope = (n * sumXY - sumX * sumY) / (n * sumX2 - sumX * sumX);
            double intercept = meanY - slope * meanX;

            return (slope, intercept);
        }

        public static double CalculateIntercept(double[] x, double[] y, double slope)
        {
            if (x == null || y == null || x.Length != y.Length || x.Length < 2)
            {
                throw new ArgumentException("Invalid input data. Both x and y must be non-null lists of the same length with at least two elements.");
            }

            int n = x.Length;
            double sumX = 0, sumY = 0;

            for (int i = 0; i < n; i++)
            {
                double xi = x[i];
                double yi = y[i];

                sumX += xi;
                sumY += yi;
            }

            double meanX = sumX / n;
            double meanY = sumY / n;

            double intercept = meanY - slope * meanX;

            return intercept;
        }

        public static double? CalculateLinearTrend(double? x, double slope, double intercept)
        {
            return x * slope + intercept;
        }
        public static double? CalculateDetrendedData(double? smoothedPressure, double? linearTrendPressure)
        {
            return Math.Abs((double)(smoothedPressure - linearTrendPressure));
        }
        public static double? Round(double? value)
        {
            if(value == null) return null;
            return Math.Round(Convert.ToDouble(value), 2);
        }
    }
}
