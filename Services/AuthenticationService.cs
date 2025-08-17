using ModernWPFApp.Models;
using System.Security.Cryptography;
using System.Text;

namespace ModernWPFApp.Services
{
    public class AuthenticationService
    {
        private static AuthenticationService? _instance;
        private User? _currentUser;
        private readonly List<User> _users;

        public static AuthenticationService Instance => _instance ??= new AuthenticationService();

        public User? CurrentUser => _currentUser;
        public bool IsAuthenticated => _currentUser != null;

        public event EventHandler<User?>? UserLoggedIn;
        public event EventHandler? UserLoggedOut;

        private AuthenticationService()
        {
            _users = new List<User>();
            InitializeDefaultUsers();
        }

        private void InitializeDefaultUsers()
        {
            // Create default admin user
            _users.Add(new User
            {
                Id = 1,
                Username = "admin",
                Email = "admin@company.com",
                FirstName = "System",
                LastName = "Administrator",
                Role = UserRole.Administrator,
                CreatedDate = DateTime.Now,
                IsActive = true
            });

            // Create default manager user
            _users.Add(new User
            {
                Id = 2,
                Username = "manager",
                Email = "manager@company.com",
                FirstName = "Project",
                LastName = "Manager",
                Role = UserRole.Manager,
                CreatedDate = DateTime.Now,
                IsActive = true
            });

            // Create default employee user
            _users.Add(new User
            {
                Id = 3,
                Username = "employee",
                Email = "employee@company.com",
                FirstName = "Field",
                LastName = "Employee",
                Role = UserRole.Employee,
                CreatedDate = DateTime.Now,
                IsActive = true
            });
        }

        public async Task<LoginResult> LoginAsync(string username, string password)
        {
            try
            {
                // Simulate async operation
                await Task.Delay(500);

                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    return new LoginResult { Success = false, ErrorMessage = "Benutzername und Passwort sind erforderlich." };
                }

                var user = _users.FirstOrDefault(u => 
                    u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && 
                    u.IsActive);

                if (user == null)
                {
                    return new LoginResult { Success = false, ErrorMessage = "Benutzername oder Passwort ist falsch." };
                }

                // For demo purposes, we accept "password" as the password for all users
                // In a real application, you would hash and compare passwords properly
                if (password != "password")
                {
                    return new LoginResult { Success = false, ErrorMessage = "Benutzername oder Passwort ist falsch." };
                }

                _currentUser = user;
                UserLoggedIn?.Invoke(this, user);

                return new LoginResult { Success = true, User = user };
            }
            catch (Exception ex)
            {
                return new LoginResult { Success = false, ErrorMessage = $"Anmeldefehler: {ex.Message}" };
            }
        }

        public void Logout()
        {
            _currentUser = null;
            UserLoggedOut?.Invoke(this, EventArgs.Empty);
        }

        public bool HasPermission(string permission)
        {
            if (!IsAuthenticated)
                return false;

            return _currentUser!.Role switch
            {
                UserRole.Administrator => true,
                UserRole.Manager => permission != "SystemSettings",
                UserRole.Employee => permission == "ViewData" || permission == "EditBasicData",
                UserRole.Guest => permission == "ViewData",
                _ => false
            };
        }

        public List<User> GetAllUsers()
        {
            return _users.ToList();
        }

        public void AddUser(User user)
        {
            user.Id = _users.Any() ? _users.Max(u => u.Id) + 1 : 1;
            user.CreatedDate = DateTime.Now;
            _users.Add(user);
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }
    }

    public class LoginResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public User? User { get; set; }
    }
}
