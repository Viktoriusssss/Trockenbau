using ModernWPFApp.ViewModels;
using ModernWPFApp.Services;
using System.Windows;

namespace ModernWPFApp
{
    public partial class MainWindow : Window
    {
        public MainWindowViewModel ViewModel { get; }

        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainWindowViewModel();
            DataContext = ViewModel;

            // Subscribe to logout event
            AuthenticationService.Instance.UserLoggedOut += OnUserLoggedOut;
        }

        private void OnUserLoggedOut(object? sender, EventArgs e)
        {
            // Close this window and let App.xaml.cs handle showing login
            Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            AuthenticationService.Instance.UserLoggedOut -= OnUserLoggedOut;
            base.OnClosed(e);
        }
    }
}
