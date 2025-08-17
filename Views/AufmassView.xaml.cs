using System.Windows.Controls;
using System.Windows.Input;
using ModernWPFApp.ViewModels;
using ModernWPFApp.Models;
using System.Windows;

namespace ModernWPFApp.Views
{
    public partial class AufmassView : UserControl
    {
        private AufmassViewModel _viewModel;

        public AufmassView()
        {
            InitializeComponent();
            _viewModel = new AufmassViewModel();
            DataContext = _viewModel;
            
            // Subscribe to property changes for debugging
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(AufmassViewModel.SelectedAufmass))
                {
                    // Debug output
                    System.Diagnostics.Debug.WriteLine($"SelectedAufmass changed: {_viewModel.SelectedAufmass?.Titel ?? "null"}");
                }
                else if (e.PropertyName == nameof(AufmassViewModel.Aufmasse))
                {
                    // Debug output for data loading
                    System.Diagnostics.Debug.WriteLine($"Aufmasse loaded: {_viewModel.Aufmasse.Count} aufmasse");
                }
            };
            
            // Load data after initialization
            Loaded += (s, e) =>
            {
                System.Diagnostics.Debug.WriteLine($"AufmassView loaded with {_viewModel.Aufmasse.Count} aufmasse");
            };
        }

        private void DataGridRow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is DataGridRow row && row.DataContext is Aufmass aufmass)
            {
                // Set the selected item
                _viewModel.SelectedAufmass = aufmass;
                
                // Debug output
                System.Diagnostics.Debug.WriteLine($"Row clicked: {aufmass.Titel}");
                
                // Mark the event as handled to prevent cell selection
                e.Handled = true;
            }
        }

        private void AufmassDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AufmassDataGrid.SelectedItem is Aufmass selectedAufmass)
            {
                _viewModel.SelectedAufmass = selectedAufmass;
                System.Diagnostics.Debug.WriteLine($"Selection changed: {selectedAufmass.Titel}");
            }
        }
    }
}
