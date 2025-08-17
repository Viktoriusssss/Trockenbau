using System.Windows;
using ModernWPFApp.ViewModels;

namespace ModernWPFApp.Views
{
    public partial class BenutzerEditWindow : Window
    {
        public BenutzerEditWindow(BenutzerEditViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
        }
    }
}
