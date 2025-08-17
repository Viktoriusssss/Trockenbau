using ModernWPFApp.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ModernWPFApp.Views
{
    public partial class BaustellenView : UserControl
    {
        private readonly BaustelleViewModel _viewModel;

        public BaustellenView()
        {
            InitializeComponent();
            _viewModel = new BaustelleViewModel();
            DataContext = _viewModel;

            // Subscribe to property changes for debugging
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(BaustelleViewModel.SelectedBaustelle))
                {
                    System.Diagnostics.Debug.WriteLine($"SelectedBaustelle changed: {_viewModel.SelectedBaustelle?.Name ?? "null"}");
                }
            };
        }

        private void DataGridRow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGridRow row && row.DataContext is Models.Baustelle baustelle)
            {
                _viewModel.SelectedBaustelle = baustelle;
                System.Diagnostics.Debug.WriteLine($"Row clicked: {baustelle.Name}");
            }
        }

        private void BaustelleDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is DataGrid dataGrid && dataGrid.SelectedItem is Models.Baustelle baustelle)
            {
                _viewModel.SelectedBaustelle = baustelle;
                System.Diagnostics.Debug.WriteLine($"Selection changed: {baustelle.Name}");
            }
        }
    }
}
