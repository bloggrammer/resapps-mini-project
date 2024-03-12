namespace WellTestAnalysis.Utils
{
    public static class StringExt
    {
        public static double? ToDouble(this string value)
        {
            try
            {
                return double.Parse(value);
            }
            catch
            {

                return null;
            }
        }
    }
}
