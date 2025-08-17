using ModernWPFApp.ViewModels;
using System.Windows;

namespace ModernWPFApp.Views
{
    public partial class BaustelleEditWindow : Window
    {
        public BaustelleEditWindow(BaustelleEditViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;

            viewModel.SaveRequested += (s, e) =>
            {
                DialogResult = true;
                Close();
            };

            viewModel.CancelRequested += (s, e) =>
            {
                DialogResult = false;
                Close();
            };
        }
    }
}


