using System.Windows;
using System.Windows.Controls;
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

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is BenutzerEditViewModel viewModel)
            {
                viewModel.Password = PasswordBox.Password;
            }
        }

        private void ConfirmPasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (DataContext is BenutzerEditViewModel viewModel)
            {
                viewModel.ConfirmPassword = ConfirmPasswordBox.Password;
            }
        }
    }
}
