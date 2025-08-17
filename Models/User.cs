using System.ComponentModel;

namespace ModernWPFApp.Models
{
    public class User : INotifyPropertyChanged
    {
        private string _username = string.Empty;
        private string _email = string.Empty;
        private string _firstName = string.Empty;
        private string _lastName = string.Empty;
        private string _passwordHash = string.Empty;
        private UserRole _role;
        private DateTime _createdDate;
        private DateTime? _lastLoginDate;
        private TimeSpan _totalLoginTime;
        private bool _isActive;

        public int Id { get; set; }
        
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string FirstName
        {
            get => _firstName;
            set
            {
                _firstName = value;
                OnPropertyChanged(nameof(FirstName));
            }
        }

        public string LastName
        {
            get => _lastName;
            set
            {
                _lastName = value;
                OnPropertyChanged(nameof(LastName));
            }
        }

        public string PasswordHash
        {
            get => _passwordHash;
            set
            {
                _passwordHash = value;
                OnPropertyChanged(nameof(PasswordHash));
            }
        }

        public UserRole Role
        {
            get => _role;
            set
            {
                _role = value;
                OnPropertyChanged(nameof(Role));
            }
        }

        public DateTime CreatedDate
        {
            get => _createdDate;
            set
            {
                _createdDate = value;
                OnPropertyChanged(nameof(CreatedDate));
            }
        }

        public bool IsActive
        {
            get => _isActive;
            set
            {
                _isActive = value;
                OnPropertyChanged(nameof(IsActive));
            }
        }

        public DateTime? LastLoginDate
        {
            get => _lastLoginDate;
            set
            {
                _lastLoginDate = value;
                OnPropertyChanged(nameof(LastLoginDate));
            }
        }

        public TimeSpan TotalLoginTime
        {
            get => _totalLoginTime;
            set
            {
                _totalLoginTime = value;
                OnPropertyChanged(nameof(TotalLoginTime));
            }
        }

        public string FullName => $"{FirstName} {LastName}";

        public string LastLoginDisplay => LastLoginDate?.ToString("dd.MM.yyyy HH:mm") ?? "Nie angemeldet";

        public string TotalLoginTimeDisplay => TotalLoginTime.TotalHours > 0 
            ? $"{TotalLoginTime.TotalHours:F1} Stunden" 
            : $"{TotalLoginTime.TotalMinutes:F0} Minuten";

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum UserRole
    {
        SystemAdministrator,
        Administrator,
        Manager,
        Employee,
        Guest
    }
}
