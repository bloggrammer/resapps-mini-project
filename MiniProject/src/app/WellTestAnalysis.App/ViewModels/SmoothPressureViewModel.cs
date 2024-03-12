using MVVM.LTE.Commands;
using OxyPlot;
using System.Collections.ObjectModel;
using System.Windows;
using System.Xml.Serialization;
using WellTestAnalysis.Dtos;
using WellTestAnalysis.Models;

namespace WellTestAnalysis.App
{
    public partial class MainViewModel
    {
        private void InitSmoothPressureVM()
        {
            SmoothedPressureData = [];
            SmoothPressureCommand = new SimpleCommand(SmoothPressureCommandAction);
            SmoothedDataPlot = ViewModelHelper.PlotModel(SmoothedPressureData, "Smoothed Pressure Data");
        }

        private void SmoothPressureCommandAction(object obj)
        {
            if (ObservedRawData.Count == 0)
            {
                MessageBox.Show("Please import observed pressure vs. time data", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var smooths = _service.SmoothObservedPressure(ObservedRawData, SmoothingFactor);
            SmoothedPressureData = new ObservableCollection<PressureTimeData>(smooths);
            SmoothedDataPlot = ViewModelHelper.PlotModel(smooths, "Smoothed Pressure Data");
        }

        private ObservableCollection<PressureTimeData> smoothedpressureData;

        public ObservableCollection<PressureTimeData> SmoothedPressureData
        {
            get { return smoothedpressureData; }
            set { smoothedpressureData = value; RaisePropertyChanged(); }
        }
        private float smoothingFactor = 0.5f;

        public float SmoothingFactor
        {
            get { return smoothingFactor; }
            set { smoothingFactor = value; RaisePropertyChanged(); }
        }
        private PlotModel smoothDataPlot;
        [XmlIgnore]
        public PlotModel SmoothedDataPlot
        {
            get { return smoothDataPlot; }
            set { smoothDataPlot = value; RaisePropertyChanged(); }
        }
        [XmlIgnore]
        public SimpleCommand SmoothPressureCommand { get; private set; }
    }
}
