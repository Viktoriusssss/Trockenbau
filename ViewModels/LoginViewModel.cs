using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ModernWPFApp.Services;

namespace ModernWPFApp.ViewModels
{
    public partial class LoginViewModel : ObservableObject
    {
        private readonly AuthenticationService _authService;

        [ObservableProperty]
        private string _username = string.Empty;

        [ObservableProperty]
        private string _password = string.Empty;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        [ObservableProperty]
        private bool _isLoggingIn = false;

        [ObservableProperty]
        private bool _rememberMe = false;

        public event EventHandler<bool>? LoginCompleted;

        public LoginViewModel()
        {
            _authService = AuthenticationService.Instance;
        }

        [RelayCommand]
        private async Task LoginAsync(object? parameter = null)
        {
            // Prevent multiple login attempts
            if (IsLoggingIn)
                return;

            try
            {
                IsLoggingIn = true;
                ErrorMessage = string.Empty;

                // Ensure we have valid input
                if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
                {
                    ErrorMessage = "Benutzername und Passwort sind erforderlich.";
                    return;
                }

                var result = await _authService.LoginAsync(Username, Password);

                if (result.Success)
                {
                    LoginCompleted?.Invoke(this, true);
                }
                else
                {
                    ErrorMessage = result.ErrorMessage;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Unerwarteter Fehler: {ex.Message}";
            }
            finally
            {
                IsLoggingIn = false;
                Password = string.Empty; // Clear password for security
            }
        }

        [RelayCommand]
        private void Cancel()
        {
            LoginCompleted?.Invoke(this, false);
        }

        partial void OnUsernameChanged(string value)
        {
            ErrorMessage = string.Empty;
        }

        partial void OnPasswordChanged(string value)
        {
            ErrorMessage = string.Empty;
        }
    }
}
