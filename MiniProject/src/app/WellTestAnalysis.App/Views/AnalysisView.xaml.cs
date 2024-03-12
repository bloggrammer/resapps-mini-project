using System.Windows.Controls;

namespace WellTestAnalysis.App.Views
{
    /// <summary>
    /// Interaction logic for AnalysisView.xaml
    /// </summary>
    public partial class AnalysisView : UserControl
    {
        public AnalysisView()
        {
            InitializeComponent();
        }
        private void Analysis_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = $"  {(e.Row.GetIndex() + 1)}";
        }
        private void Trend_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = $"  {(e.Row.GetIndex() + 1)}";
        }
    }
}
