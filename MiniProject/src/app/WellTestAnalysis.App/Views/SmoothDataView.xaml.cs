using System.Windows.Controls;

namespace WellTestAnalysis.App.Views
{
    /// <summary>
    /// Interaction logic for SmoothDataView.xaml
    /// </summary>
    public partial class SmoothDataView : UserControl
    {
        public SmoothDataView()
        {
            InitializeComponent();
        }

        private void SmoothInfo_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = $"  {(e.Row.GetIndex() + 1)}";
        }
    }
}
