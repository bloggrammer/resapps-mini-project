using MVVM.LTE.Commands;
using OxyPlot;
using System.Collections.ObjectModel;
using System.Windows;
using System.Xml.Serialization;
using WellTestAnalysis.Dtos;
using WellTestAnalysis.Models;
using WellTestAnalysis.Utils;

namespace WellTestAnalysis.App
{
    public partial class MainViewModel
    {
        private void InitAnalysisVM()
        {
            Before24HrPressureData = [];
            TrendPressureData = [];
            TrendBeforeTestVisible = true;
            TrendPressureDataPlot = new PlotModel();
            Before24HrCommand = new SimpleCommand(Before24HrAction);
            TrendDataCommand = new SimpleCommand(TrendDataAction);
            CalculateSlopeCommand = new SimpleCommand(CalculateSlopeAction);
            TrendPressureDataPlot = ViewModelHelper.PlotModel(TrendPressureData, "Trend and Detrended Data");
            Before24HrPlot = ViewModelHelper.PlotModel(Before24HrPressureData, "24hr Before Test");
        }

        private void TrendDataAction()
        {
            if (!IsValidTestInfo())
            {
                MessageBox.Show("Please import test info data", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (SmoothedPressureData.Count == 0)
            {
                MessageBox.Show("Smooth the pressure data first", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(StartTime) || string.IsNullOrWhiteSpace(EndTime) || string.IsNullOrWhiteSpace(Slope))
            {
                MessageBox.Show("Enter values for slope, test start and time", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                float start = float.Parse(StartTime);
                float end = float.Parse(EndTime);
                float slope = float.Parse(Slope);
                var data = _service.RunAnalysis(SmoothedPressureData, start, end, slope);
                TrendPressureData = new ObservableCollection<PressureTimeTrend>(data);
                TrendPressureDataPlot =  ViewModelHelper.PlotModel(TrendPressureData, "Trend and Detrended Data");
                TrendBeforeTestVisible = true;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Import Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Before24HrAction()
        {
            if (SmoothedPressureData.Count == 0)
            {
                MessageBox.Show("Smooth the pressure data first", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(StartTime))
            {
                MessageBox.Show("Enter values for test start time", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                float start = float.Parse(StartTime);
                var data = _service.GetObservationDataStartingFromTimeBeforeTest(SmoothedPressureData, start, TimeBeforeTest);
                Before24HrPressureData = new ObservableCollection<PressureTimeData>(data);
                Before24HrPlot = ViewModelHelper.PlotModel(Before24HrPressureData, "24hr Before Test");
                TrendBeforeTestVisible = false;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Import Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
          
        }

        private void CalculateSlopeAction()
        {
            if(SmoothedPressureData.Count == 0)
            {
                MessageBox.Show("Smooth the pressure data first", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(StartTime) || string.IsNullOrWhiteSpace(EndTime))
            {
                MessageBox.Show("Enter values for test start and end time", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                float start = float.Parse(StartTime);
                float end = float.Parse(EndTime);
                Slope = _service.GetSlope(SmoothedPressureData, start, end).ToString();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Import Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private ObservableCollection<PressureTimeData> before24HrPressureData;

        public ObservableCollection<PressureTimeData> Before24HrPressureData
        {
            get { return before24HrPressureData; }
            set { before24HrPressureData = value; RaisePropertyChanged(); }
        }
        private ObservableCollection<PressureTimeTrend> trendPressureData;

        public ObservableCollection<PressureTimeTrend> TrendPressureData
        {
            get { return trendPressureData; }
            set { trendPressureData = value; RaisePropertyChanged(); }
        }
        private PlotModel trendPressureDataPlot;
        [XmlIgnore]
        public PlotModel TrendPressureDataPlot
        {
            get { return trendPressureDataPlot; }
            set { trendPressureDataPlot = value; RaisePropertyChanged(); }
        }
        private bool trendBeforeTestVisible;

        public bool TrendBeforeTestVisible
        {
            get { return trendBeforeTestVisible; }
            set { trendBeforeTestVisible = value; RaisePropertyChanged(); }
        }

        private PlotModel before24HrPlot;
        [XmlIgnore]
        public PlotModel Before24HrPlot
        {
            get { return before24HrPlot; }
            set { before24HrPlot = value; RaisePropertyChanged(); }
        }
        [XmlIgnore]
        public SimpleCommand Before24HrCommand { get; private set; }
        [XmlIgnore]
        public SimpleCommand TrendDataCommand { get; private set; }
        [XmlIgnore]
        public SimpleCommand CalculateSlopeCommand { get; private set; }
    }
}
