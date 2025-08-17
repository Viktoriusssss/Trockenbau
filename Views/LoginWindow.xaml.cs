using ModernWPFApp.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System;

namespace ModernWPFApp.Views
{
    public partial class LoginWindow : Window
    {
        public LoginViewModel ViewModel { get; }
        
        public event EventHandler<bool>? LoginCompleted;

        public LoginWindow()
        {
            InitializeComponent();
            ViewModel = new LoginViewModel();
            DataContext = ViewModel;
            
            ViewModel.LoginCompleted += OnLoginCompleted;
            
            // Focus username textbox when window loads
            Loaded += (s, e) => UsernameTextBox.Focus();
        }

        private void OnLoginCompleted(object? sender, bool success)
        {
            LoginCompleted?.Invoke(this, success);
            Close();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                ViewModel.Password = passwordBox.Password;
            }
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            UsernameTextBox.Focus();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            ViewModel.LoginCompleted -= OnLoginCompleted;
            base.OnClosed(e);
        }
    }
}
