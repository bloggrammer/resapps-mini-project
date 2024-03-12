using System.Xml.Serialization;
using WellTestAnalysis.Dtos;
using WellTestAnalysis.Models;

namespace WellTestAnalysis.Utils
{
    public static class DataIO
    {
        public static Result<TestInfo> LoadTestData(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return ResultStatus<TestInfo>.Fail("File path cannot be empty");

            if (!File.Exists(filePath)) return ResultStatus<TestInfo>.Fail("File not found");

            TestInfo testInfo = new();
            try
            {
                string[] lines = File.ReadAllLines(filePath);
                testInfo.WellName = lines[0].Split(':')[1].Trim();
                testInfo.StartTime = int.Parse(lines[1].Split(':')[1].Trim().Split()[0]);
                testInfo.EndTime = int.Parse(lines[2].Split(':')[1].Trim().Split()[0]);
                testInfo.Slope = float.Parse(lines[3].Split(':')[1].Trim().Split()[0]);
            }
            catch (Exception ex)
            {

                return ResultStatus<TestInfo>.Fail(ex.Message);
            }
            return ResultStatus<TestInfo>.Pass(testInfo);

            //string[] lines = File.ReadAllLines(filePath);
            //foreach (string line in lines)
            //{
            //    if (line.StartsWith("Test start time:"))
            //        StartTime = int.Parse(line.Split(':')[1].Trim().Split()[0]);
            //    else if (line.StartsWith("Test end time:"))
            //        EndTime = int.Parse(line.Split(':')[1].Trim().Split()[0]);
            //    else if (line.StartsWith("Pre-test trend slope:"))
            //        PreTestTrendSlope = double.Parse(line.Split(':')[1].Trim().Split()[0]);
            //}
        }

        public static Result<List<PressureTimeData>> LoadObservationData(string filePath, string wellName)
        {
            if (string.IsNullOrEmpty(filePath)) return ResultStatus<List<PressureTimeData>>.Fail("File path cannot be empty");

            if (!File.Exists(filePath)) return ResultStatus<List<PressureTimeData>>.Fail("File not found");

            List<PressureTimeData> observations = [];
            try
            {
                string[] lines = File.ReadAllLines(filePath);
                string[] headers = lines[1].Split(',');
                int columnIndex = Array.IndexOf(headers, wellName);
                if (columnIndex == -1)
                {
                    return ResultStatus<List<PressureTimeData>>.Fail("Provided well name does not exists in the dataset");
                }
                for (int i = 4; i < lines.Length; i++)
                {

                    string[] fields = lines[i].Split(',');
                    double? time = fields[0].ToDouble();
                    double? pressure = fields[columnIndex].ToDouble();
                    observations.Add(new PressureTimeData(time, pressure));

                }
            }
            catch (Exception ex)
            {

                return ResultStatus<List<PressureTimeData>>.Fail(ex.Message); // Log exception

            }
            //try
            //{
            //    using (var reader = new StreamReader(filePath))
            //    {
            //        string line;
            //        string[] headers = null;
            //        int columnIndex = -1;
            //        int lineNumber = 0;

            //        while ((line = reader.ReadLine()) != null)
            //        {
            //            lineNumber++;

            //            if (lineNumber == 1)
            //                continue; // Skip header line

            //            if (lineNumber == 2)
            //            {
            //                headers = line.Split(',');
            //                columnIndex = Array.IndexOf(headers, wellName);
            //                if (columnIndex == -1)
            //                    return ResultStatus<List<PressureTimeData>>.Fail("Provided well name does not exist in the dataset");
            //                continue;
            //            }

            //           if(lineNumber > 4)
            //            {
            //                string[] fields = line.Split(',');

            //                if (fields.Length <= columnIndex)
            //                {
            //                    // Log or handle incomplete data row
            //                    continue;
            //                }

            //                double? time = fields[0].ToDouble();
            //                double? pressure = fields[columnIndex].ToDouble();

            //                observations.Add(new PressureTimeData(time, pressure));
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return ResultStatus<List<PressureTimeData>>.Fail(ex.Message);
            //}
            return ResultStatus<List<PressureTimeData>>.Pass(observations);
        }

        public static T ReadXML<T>(string path) where T : class
        {
            XmlSerializer xs = new(typeof(T));
            using var stream = File.OpenRead(path);
            return xs.Deserialize(stream) as T;
        }

        public static void WriteToXML(string path, object obj)
        {
            XmlSerializer xs = new(obj.GetType());
            using var stream = File.OpenWrite(path);
            xs.Serialize(stream, obj);
        }
    }
}
