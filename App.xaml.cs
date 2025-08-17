using ModernWPFApp.Views;
using ModernWPFApp.Services;
using System.Windows;

namespace ModernWPFApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Set up authentication flow
            ShowLoginWindow();
        }

        private void ShowLoginWindow()
        {
            var loginWindow = new LoginWindow();
            
            // Set the login window as the main window temporarily
            MainWindow = loginWindow;
            
            // Show the login window
            loginWindow.Show();
            
            // Listen for login completion
            loginWindow.LoginCompleted += OnLoginCompleted;
        }

        private void OnLoginCompleted(object? sender, bool success)
        {
            if (success)
            {
                // User logged in successfully, show main window
                ShowMainWindow();
            }
            else
            {
                // User cancelled login or failed, exit application
                Shutdown();
            }
        }

        private void ShowMainWindow()
        {
            try
            {
                var mainWindow = new MainWindow();
                MainWindow = mainWindow;
                mainWindow.Show();

                // Listen for logout events to show login again
                AuthenticationService.Instance.UserLoggedOut += OnUserLoggedOut;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error creating main window: {ex.Message}", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
                Shutdown();
            }
        }

        private void OnUserLoggedOut(object? sender, EventArgs e)
        {
            // Close main window
            MainWindow?.Close();
            
            // Unsubscribe from event to prevent memory leaks
            AuthenticationService.Instance.UserLoggedOut -= OnUserLoggedOut;
            
            // Show login window again
            ShowLoginWindow();
        }
    }
}
