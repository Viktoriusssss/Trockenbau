using System.Windows.Controls;
using System.Windows.Input;
using ModernWPFApp.ViewModels;
using ModernWPFApp.Models;
using System.Windows;

namespace ModernWPFApp.Views
{
    public partial class KundenView : UserControl
    {
        private KundenViewModel _viewModel;

        public KundenView()
        {
            InitializeComponent();
            _viewModel = new KundenViewModel();
            DataContext = _viewModel;
            
            // Subscribe to property changes for debugging
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(KundenViewModel.SelectedKunde))
                {
                    // Debug output
                    System.Diagnostics.Debug.WriteLine($"SelectedKunde changed: {_viewModel.SelectedKunde?.FirmenName ?? "null"}");
                }
                else if (e.PropertyName == nameof(KundenViewModel.Kunden))
                {
                    // Debug output for data loading
                    System.Diagnostics.Debug.WriteLine($"Kunden loaded: {_viewModel.Kunden.Count} customers");
                }
            };
            
            // Load data after initialization
            Loaded += (s, e) =>
            {
                System.Diagnostics.Debug.WriteLine($"KundenView loaded with {_viewModel.Kunden.Count} customers");
            };
        }

        private void DataGridRow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGridRow row && row.DataContext is Kunde kunde)
            {
                // Set the selected item
                _viewModel.SelectedKunde = kunde;
                
                // Debug output
                System.Diagnostics.Debug.WriteLine($"Row clicked: {kunde.FirmenName}");
                
                // Mark the event as handled to prevent cell selection
                e.Handled = true;
            }
        }

        private void KundenDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (KundenDataGrid.SelectedItem is Kunde selectedKunde)
            {
                _viewModel.SelectedKunde = selectedKunde;
                System.Diagnostics.Debug.WriteLine($"Selection changed: {selectedKunde.FirmenName}");
            }
        }
    }
}
