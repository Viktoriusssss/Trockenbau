using ModernWPFApp.ViewModels;
using System.Windows;

namespace ModernWPFApp.Views
{
    public partial class AufmassEditWindow : Window
    {
        public AufmassEditWindow(AufmassEditViewModel viewModel)
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


