using MVVM.LTE.Commands;
using MVVM.LTE.ViewModel;
using OxyPlot.Series;
using OxyPlot;
using System.Collections.ObjectModel;
using System.Windows;
using System.Xml.Serialization;
using WellTestAnalysis.Models;
using WellTestAnalysis.Utils;
using OxyPlot.Axes;
using OxyPlot.Legends;
using WellTestAnalysis.Dtos;

namespace WellTestAnalysis.App
{
    public partial class MainViewModel : ViewModelBase
    {
		private void InitImportVM()
		{
            ObservedRawData = [];
            ClearTestInfoCommand = new SimpleCommand(ClearTestInfoAction);
            ImportTestInfoCommand = new SimpleCommand(ImportTestInfoAction);
            ClearObservedDataCommand = new SimpleCommand(ClearObservedDataAction);
            ImportObservedDataCommand = new SimpleCommand(ImportObservedDataAction);
            ObservedDataPlot = ViewModelHelper.PlotModel(ObservedRawData, "Observed Data");
        }

        private void ImportObservedDataAction()
        {
            ObservedDataloading = true;
            try
            {
                var path = IOService.GetFilePath("Pressure (*.csv)");

                var result = DataIO.LoadObservationData(path, WellName);
                if (result.Succeeded)
                {
                    ObservedRawData = new ObservableCollection<PressureTimeData>(result.Data);
                    ObservedDataPlot = ViewModelHelper.PlotModel(result.Data, "Observed Data");

                }
                else
                {
                    MessageBox.Show(result.Message, "Import Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
            catch
            {

                MessageBox.Show("Something went wrong", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            ObservedDataloading = false;
        }

        private void ClearObservedDataAction()
        {
            ObservedRawData.Clear();
        }

        private void ClearTestInfoAction()
        {
            WellName = string.Empty;
            StartTime = string.Empty;
            EndTime = string.Empty;
            Slope = string.Empty;
        }
        private bool IsValidTestInfo()
        {
          return !string.IsNullOrWhiteSpace(WellName) &&
                !string.IsNullOrWhiteSpace(StartTime) &&
                !string.IsNullOrWhiteSpace(EndTime) &&
                !string.IsNullOrWhiteSpace(Slope);
        }

        private void ImportTestInfoAction()
        {
            try
            {
               var path = IOService.GetFilePath("Well Test Info (*.txt)");
                
                var result = DataIO.LoadTestData(path);
                if (result.Succeeded)
                {
                    WellName = result.Data.WellName;
                    StartTime = result.Data.StartTime.ToString();
                    EndTime = result.Data.EndTime.ToString();
                    Slope = result.Data.Slope.ToString();
                }
                else
                {
                    MessageBox.Show(result.Message, "Import Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
              
            }
            catch
            {

                MessageBox.Show("Something went wrong", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private ObservableCollection<PressureTimeData> observedRawData;

        public ObservableCollection<PressureTimeData> ObservedRawData
        {
            get { return observedRawData; }
            set { observedRawData = value; RaisePropertyChanged(); }
        }
        private string wellName;
		public string WellName
		{
			get { return wellName; }
			set { wellName = value; RaisePropertyChanged(); }
		}
		private string startTime;

		public string StartTime
		{
			get { return startTime; }
			set { startTime = value; RaisePropertyChanged(); }
		}
		private string endTime;

		public string EndTime
		{
			get { return endTime; }
			set { endTime = value; RaisePropertyChanged(); }
		}
		private string slope;

		public string Slope
		{
			get { return slope; }
			set { slope = value; RaisePropertyChanged(); }
		}
        private float timeBeforeTest = 24.0f;

        public float TimeBeforeTest
        {
            get { return timeBeforeTest; }
            set { timeBeforeTest = value; RaisePropertyChanged(); }
        }
        private bool observedDataloading;

        public bool ObservedDataloading
        {
            get { return observedDataloading; }
            set { observedDataloading = value; RaisePropertyChanged(); }
        }


        private PlotModel observedDataPlot;
        [XmlIgnore]
        public PlotModel ObservedDataPlot
        {
            get { return observedDataPlot; }
            set { observedDataPlot = value; RaisePropertyChanged(); }
        }

        [XmlIgnore]
        public SimpleCommand ClearTestInfoCommand { get; private set; }
        [XmlIgnore]
        public SimpleCommand ImportTestInfoCommand { get; private set; }

        [XmlIgnore]
        public SimpleCommand ClearObservedDataCommand { get; private set; }
        [XmlIgnore]
        public SimpleCommand ImportObservedDataCommand { get; private set; }
    }
}
