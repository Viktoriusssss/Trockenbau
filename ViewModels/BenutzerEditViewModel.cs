using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ModernWPFApp.Models;
using ModernWPFApp.Services;
using System.ComponentModel;

namespace ModernWPFApp.ViewModels
{
    public partial class BenutzerEditViewModel : ObservableObject
    {
        [ObservableProperty]
        private User _benutzer;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        [ObservableProperty]
        private bool _isValid = false;

        private string _password = string.Empty;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
                ValidateBenutzer();
            }
        }

        private string _confirmPassword = string.Empty;
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
                ValidateBenutzer();
            }
        }

        public string WindowTitle => Benutzer.Id == 0 ? "Neuen Benutzer hinzufügen" : "Benutzer bearbeiten";

        public bool CanAssignSystemAdministratorRole
        {
            get
            {
                var currentUser = AuthenticationService.Instance.CurrentUser;
                return currentUser?.Role == UserRole.SystemAdministrator;
            }
        }

        public List<UserRole> AvailableRoles
        {
            get
            {
                var roles = new List<UserRole>
                {
                    UserRole.Administrator,
                    UserRole.Manager,
                    UserRole.Employee,
                    UserRole.Guest
                };

                // Only SystemAdministrators can assign SystemAdministrator role
                if (CanAssignSystemAdministratorRole)
                {
                    roles.Insert(0, UserRole.SystemAdministrator);
                }

                return roles;
            }
        }

        public BenutzerEditViewModel()
        {
            Benutzer = new User
            {
                CreatedDate = DateTime.Now,
                IsActive = true
            };
            
            // Subscribe to property changes for validation
            Benutzer.PropertyChanged += OnBenutzerPropertyChanged;
            ValidateBenutzer();
        }

        public BenutzerEditViewModel(User benutzer)
        {
            // Create a copy of the user to avoid modifying the original
            Benutzer = new User
            {
                Id = benutzer.Id,
                Username = benutzer.Username,
                FirstName = benutzer.FirstName,
                LastName = benutzer.LastName,
                Email = benutzer.Email,
                Role = benutzer.Role,
                CreatedDate = benutzer.CreatedDate,
                LastLoginDate = benutzer.LastLoginDate,
                TotalLoginTime = benutzer.TotalLoginTime,
                IsActive = benutzer.IsActive
            };
            
            // Subscribe to property changes for validation
            Benutzer.PropertyChanged += OnBenutzerPropertyChanged;
            ValidateBenutzer();
        }

        private void OnBenutzerPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            ValidateBenutzer();
        }

        private void ValidateBenutzer()
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(Benutzer.Username))
                errors.Add("Benutzername ist erforderlich");

            if (string.IsNullOrWhiteSpace(Benutzer.FirstName))
                errors.Add("Vorname ist erforderlich");

            if (string.IsNullOrWhiteSpace(Benutzer.LastName))
                errors.Add("Nachname ist erforderlich");

            if (string.IsNullOrWhiteSpace(Benutzer.Email))
                errors.Add("Email ist erforderlich");
            else if (!IsValidEmail(Benutzer.Email))
                errors.Add("Email hat ein ungültiges Format");

            // Password validation for new users
            if (Benutzer.Id == 0) // New user
            {
                if (string.IsNullOrWhiteSpace(Password))
                    errors.Add("Passwort ist erforderlich");
                else if (Password.Length < 6)
                    errors.Add("Passwort muss mindestens 6 Zeichen lang sein");
                else if (Password != ConfirmPassword)
                    errors.Add("Passwörter stimmen nicht überein");
            }

            ErrorMessage = string.Join("\n", errors);
            IsValid = errors.Count == 0;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        [RelayCommand]
        private void Save()
        {
            if (IsValid)
            {
                // Hash password for new users
                if (Benutzer.Id == 0 && !string.IsNullOrWhiteSpace(Password))
                {
                    Benutzer.PasswordHash = HashPassword(Password);
                }

                // Set DialogResult to true to close the window with success
                var window = System.Windows.Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.DataContext == this);
                if (window != null)
                {
                    window.DialogResult = true;
                    window.Close();
                }
            }
        }

        private string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var hashedBytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        [RelayCommand]
        private void Cancel()
        {
            // Set DialogResult to false to close the window with cancel
            var window = System.Windows.Application.Current.Windows.OfType<System.Windows.Window>().FirstOrDefault(w => w.DataContext == this);
            if (window != null)
            {
                window.DialogResult = false;
                window.Close();
            }
        }
    }
}

