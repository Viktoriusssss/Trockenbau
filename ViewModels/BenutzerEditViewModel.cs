using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ModernWPFApp.Models;
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

        public string WindowTitle => Benutzer.Id == 0 ? "Neuen Benutzer hinzufügen" : "Benutzer bearbeiten";

        public event EventHandler? SaveRequested;
        public event EventHandler? CancelRequested;

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
                SaveRequested?.Invoke(this, EventArgs.Empty);
            }
        }

        [RelayCommand]
        private void Cancel()
        {
            CancelRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}

