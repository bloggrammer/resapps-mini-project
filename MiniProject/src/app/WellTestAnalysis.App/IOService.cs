using System.IO;
using static System.Environment;
using static System.Environment.SpecialFolder;
using static System.IO.Path;
using static System.IO.Directory;
using Microsoft.Win32;

namespace WellTestAnalysis.App
{
    public sealed class IOService
    {
        static IOService()
        {
            CreateDirectory(RootFolder);
            CreateDirectory(LogFolder);
            CreateDirectory(DatabaseFolder);
        }
        public static string RootFolder = Combine(GetFolderPath(CommonApplicationData), "Well Test Analysis");
        public static string LogFolder { get; } = Combine(RootFolder, "Log");
        public static string LogDBFolder { get; } = Combine(RootFolder, "DBLog");
        public static string DatabaseFolder { get; } = Combine(RootFolder, "Data");
        public static string PathToDB { get; } = $"{LogDBFolder}\\{DateTime.Now:dd-MM-yyyy}-DB.log";
        public static string PathToAppLog { get; } = $"{LogFolder}\\{DateTime.Now:dd-MM-yyyy}-App.log";
        public static string ConnectionString { get; } = $"Data Source={DatabaseFolder}\\WellTest.db;";

        public static void LogError(string text)
        {
            File.AppendAllText(PathToAppLog, text);
        }
        public static void LogDB(string message)
        {
            File.AppendAllText(PathToDB, message);
        }
        public static string GetFilePath(string dataName)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = $"{dataName} | *.*"
            };
            if ((bool)openFileDialog.ShowDialog() && !string.IsNullOrWhiteSpace(openFileDialog.FileName))
            {
                return openFileDialog.FileName;
            }
            return null;
        }

        public static string GetWTAPath()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Well Test Analysis (*.wta)|*.wta"
            };
            if ((bool)saveFileDialog.ShowDialog() && !string.IsNullOrWhiteSpace(saveFileDialog.FileName))
            {
                return saveFileDialog.FileName;
            }
            return null;
        }
        public static string GetOpenPath()
        {
            OpenFileDialog saveFileDialog = new OpenFileDialog
            {
                Filter = "Well Test Analysis (*.wta)|*.wta"
            };
            if ((bool)saveFileDialog.ShowDialog() && !string.IsNullOrWhiteSpace(saveFileDialog.FileName))
            {
                return saveFileDialog.FileName;
            }
            return null;
        }
    }
}
