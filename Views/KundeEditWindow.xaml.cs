using ModernWPFApp.ViewModels;
using System.Windows;

namespace ModernWPFApp.Views
{
    public partial class KundeEditWindow : Window
    {
        public KundeEditWindow(KundeEditViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            
            // Subscribe to events
            viewModel.SaveRequested += OnSaveRequested;
            viewModel.CancelRequested += OnCancelRequested;
            
            // Focus the first textbox when window loads
            Loaded += (s, e) => this.Focus();
        }

        private void OnSaveRequested(object? sender, EventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void OnCancelRequested(object? sender, EventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}

