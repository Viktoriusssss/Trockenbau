using ModernWPFApp.Models;
using System.Security.Cryptography;
using System.Text;

namespace ModernWPFApp.Services
{
    public class AuthenticationService
    {
        private static AuthenticationService? _instance;
        private User? _currentUser;
        private readonly DatabaseService _databaseService;

        public static AuthenticationService Instance => _instance ??= new AuthenticationService();

        public User? CurrentUser => _currentUser;
        public bool IsAuthenticated => _currentUser != null;

        public event EventHandler<User?>? UserLoggedIn;
        public event EventHandler? UserLoggedOut;

        private AuthenticationService()
        {
            _databaseService = DatabaseService.Instance;
            InitializeDefaultUsers();
        }

        private async void InitializeDefaultUsers()
        {
            try
            {
                // Check if admin user already exists
                var existingUsers = await _databaseService.GetUsersAsync();
                
                if (!existingUsers.Any(u => u.Username == "admin"))
                {
                    // Create system administrator with simple credentials
                    var adminUser = new User
                    {
                        Username = "admin",
                        Email = "admin@company.com",
                        FirstName = "System",
                        LastName = "Administrator",
                        Role = UserRole.SystemAdministrator,
                        CreatedDate = DateTime.Now,
                        IsActive = true,
                        PasswordHash = HashPassword("admin123")
                    };
                    await _databaseService.AddUserAsync(adminUser);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error initializing default users: {ex.Message}");
            }
        }

        public async Task<LoginResult> LoginAsync(string username, string password)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
                {
                    return new LoginResult { Success = false, ErrorMessage = "Benutzername und Passwort sind erforderlich." };
                }

                // Get all users from database
                var users = await _databaseService.GetUsersAsync();
                var user = users.FirstOrDefault(u => 
                    u.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && 
                    u.IsActive);

                if (user == null)
                {
                    return new LoginResult { Success = false, ErrorMessage = "Benutzername oder Passwort ist falsch." };
                }

                // Verify password hash
                var hashedPassword = HashPassword(password);
                if (user.PasswordHash != hashedPassword)
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
                UserRole.SystemAdministrator => true,
                UserRole.Administrator => true,
                UserRole.Manager => permission != "SystemSettings",
                UserRole.Employee => permission == "ViewData" || permission == "EditBasicData",
                UserRole.Guest => permission == "ViewData",
                _ => false
            };
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            var users = await _databaseService.GetUsersAsync();
            return users.ToList();
        }

        public async Task<bool> AddUserAsync(User user)
        {
            try
            {
                // Ensure password is hashed if not already
                if (string.IsNullOrEmpty(user.PasswordHash))
                {
                    // Don't set a default password - this should be handled by the UI
                    return false;
                }
                
                return await _databaseService.AddUserAsync(user);
            }
            catch
            {
                return false;
            }
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
