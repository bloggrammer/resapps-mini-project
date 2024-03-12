using MVVM.LTE.Commands;
using System.Collections.ObjectModel;
using System.Xml.Serialization;
using WellTestAnalysis.App.Data;
using WellTestAnalysis.Models;
using WellTestAnalysis.Services;

namespace WellTestAnalysis.App
{
    public partial class MainViewModel
    {
        public MainViewModel()
        {
            InitImportVM();
            InitSmoothPressureVM();
            InitAnalysisVM();
            SaveCommand = new SimpleCommand(SaveAction);
           
        }

        private void SaveAction()
        {
            var model = new AnalysisResult
            {
                Name = $"{wellName}_{DateTime.Now}",
                BeforeTestData = Before24HrPressureData,
                LinearTrendData = TrendPressureData,
            };
            var testInfo = new TestInfo()
            {
                WellName = WellName,
                StartTime = float.Parse(StartTime),
                EndTime = float.Parse(EndTime),
                Slope = float.Parse(Slope),
                SmoothingFactor = SmoothingFactor,

            };
            model.TestInfo = testInfo;
            _repository.Add(model);
            _repository.CommitToDatabase();
        }

        public void Initialize(IWellTestAnalysisService service , IRepository repository)
        {
            _service = service;
            _repository = repository;
            var data = _repository.GetAll();
            AnalysisResults = new ObservableCollection<AnalysisResult>(data);
        }
        public void Replot()
        {

            SmoothedDataPlot = ViewModelHelper.PlotModel(SmoothedPressureData, "Smoothed Pressure Data");
            ObservedDataPlot = ViewModelHelper.PlotModel(ObservedRawData, "Observed Data");
            TrendPressureDataPlot = ViewModelHelper.PlotModel(TrendPressureData, "Trend and Detrended Data");
            Before24HrPlot = ViewModelHelper.PlotModel(Before24HrPressureData, "24hr Before Test");
           
        }

        private void UpdateData()
        {
            if (SelectedAnalysisResult == null) return;
            WellName = SelectedAnalysisResult.TestInfo?.WellName;
            StartTime = SelectedAnalysisResult.TestInfo?.StartTime.ToString();
            EndTime = SelectedAnalysisResult.TestInfo?.EndTime.ToString();
            Slope = SelectedAnalysisResult.TestInfo?.Slope.ToString();
            SmoothingFactor = SelectedAnalysisResult.TestInfo.SmoothingFactor;

            Before24HrPressureData = new ObservableCollection<PressureTimeData>(SelectedAnalysisResult.BeforeTestData);
            TrendPressureData = new ObservableCollection<PressureTimeTrend>(SelectedAnalysisResult.LinearTrendData);
            Replot();

        }

        private AnalysisResult selectedAnalysisResult;
        [XmlIgnore]
        public AnalysisResult SelectedAnalysisResult
        {
            get { return selectedAnalysisResult; }
            set { selectedAnalysisResult = value;
                UpdateData();
                RaisePropertyChanged(); 
            }
        }
        private ObservableCollection<AnalysisResult> analysisResults;
        [XmlIgnore]
        public ObservableCollection<AnalysisResult> AnalysisResults
        {
            get { return analysisResults; }
            set { analysisResults = value; RaisePropertyChanged(); }
        }
        [XmlIgnore]
        public SimpleCommand SaveCommand { get; private set; }
        private IWellTestAnalysisService _service;
        private IRepository _repository;

    }
}
