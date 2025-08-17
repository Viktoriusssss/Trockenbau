using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ModernWPFApp.Models;
using ModernWPFApp.Services;
using System.Collections.ObjectModel;
using System.Windows;

namespace ModernWPFApp.ViewModels
{
    public partial class BenutzerViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<User> _benutzer = new();

        [ObservableProperty]
        private ObservableCollection<User> _filteredBenutzer = new();

        private User? _selectedBenutzer;
        public User? SelectedBenutzer
        {
            get => _selectedBenutzer;
            set
            {
                _selectedBenutzer = value;
                OnPropertyChanged(nameof(SelectedBenutzer));
                
                // Update can edit property based on whether selected user can be edited/deleted
                CanEditSelectedUser = value != null && CanDeleteUser(value);
            }
        }

        [ObservableProperty]
        private string _searchText = string.Empty;

        [ObservableProperty]
        private bool _isLoading = false;

        [ObservableProperty]
        private string _statusMessage = "Bereit";

        [ObservableProperty]
        private bool _isBenutzerSelected = false;

        [ObservableProperty]
        private bool _isAdmin = false;

        [ObservableProperty]
        private bool _canEditSelectedUser = false;

        private bool CanDeleteUser(User user)
        {
            var currentUser = AuthenticationService.Instance.CurrentUser;
            if (currentUser == null || user == null) return false;
            
            // System administrators can delete anyone except themselves
            if (currentUser.Role == UserRole.SystemAdministrator)
            {
                return user.Id != currentUser.Id;
            }
            
            // Regular administrators can delete anyone except SystemAdministrators and themselves
            if (currentUser.Role == UserRole.Administrator)
            {
                return user.Role != UserRole.SystemAdministrator && user.Id != currentUser.Id;
            }
            
            return false;
        }

        public BenutzerViewModel()
        {
            // Check if current user is admin or system admin
            var authService = AuthenticationService.Instance;
            IsAdmin = authService.CurrentUser?.Role == UserRole.Administrator || authService.CurrentUser?.Role == UserRole.SystemAdministrator;
            
            _ = LoadSampleData();
            
            // Subscribe to SelectedBenutzer changes to update IsBenutzerSelected
            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(SelectedBenutzer))
                {
                    IsBenutzerSelected = SelectedBenutzer != null;
                }
            };
        }

        private async Task LoadSampleData()
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Lade Benutzer...";

                // Load data from DatabaseService
                var dataService = DatabaseService.Instance;
                var users = await dataService.GetUsersAsync();
                
                Benutzer = new ObservableCollection<User>(users);
                FilteredBenutzer = new ObservableCollection<User>(Benutzer);

                StatusMessage = $"{Benutzer.Count} Benutzer geladen";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Fehler beim Laden der Benutzer: {ex.Message}";
                MessageBox.Show($"Fehler beim Laden der Benutzer:\n{ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task AddBenutzer()
        {
            if (!IsAdmin)
            {
                MessageBox.Show("Nur Administratoren können neue Benutzer hinzufügen.", "Zugriff verweigert", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var editViewModel = new BenutzerEditViewModel();
                var editWindow = new Views.BenutzerEditWindow(editViewModel);
                
                // Set the owner window to center the dialog
                editWindow.Owner = System.Windows.Application.Current.MainWindow;
                
                // Show dialog and get result
                var result = editWindow.ShowDialog();
                
                if (result == true)
                {
                    // Add new user
                    var newBenutzer = editViewModel.Benutzer;
                    
                    // Add to DatabaseService
                    var dataService = DatabaseService.Instance;
                    var success = await dataService.AddUserAsync(newBenutzer);
                    
                    if (success)
                    {
                        // Refresh data from database
                        await LoadSampleData();
                        
                        // Find and select the newly added user
                        var addedBenutzer = Benutzer.FirstOrDefault(b => b.Username == newBenutzer.Username);
                        if (addedBenutzer != null)
                        {
                            SelectedBenutzer = addedBenutzer;
                        }

                        StatusMessage = $"Benutzer '{newBenutzer.FullName}' hinzugefügt";
                        MessageBox.Show($"Neuer Benutzer '{newBenutzer.FullName}' wurde erfolgreich hinzugefügt.", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Fehler beim Speichern des Benutzers in der Datenbank.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Fehler beim Hinzufügen des Benutzers: {ex.Message}";
                MessageBox.Show($"Fehler beim Hinzufügen des Benutzers:\n{ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private async Task EditBenutzer(User? benutzer = null)
        {
            if (!IsAdmin)
            {
                MessageBox.Show("Nur Administratoren können Benutzer bearbeiten.", "Zugriff verweigert", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var benutzerToEdit = benutzer ?? SelectedBenutzer;
            if (benutzerToEdit == null)
            {
                MessageBox.Show("Bitte wählen Sie einen Benutzer aus.", "Hinweis", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Check if user can be edited
            var currentUser = AuthenticationService.Instance.CurrentUser;
            if (currentUser != null && benutzerToEdit.Id == currentUser.Id)
            {
                MessageBox.Show("Sie können Ihr eigenes Konto nicht bearbeiten.\n\nBitte lassen Sie sich von einem anderen Administrator bearbeiten.", 
                               "Bearbeitung nicht erlaubt", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            // Regular administrators cannot edit SystemAdministrators
            if (currentUser?.Role == UserRole.Administrator && benutzerToEdit.Role == UserRole.SystemAdministrator)
            {
                MessageBox.Show("Sie können Systemadministratoren nicht bearbeiten.\n\nNur Systemadministratoren können andere Systemadministratoren verwalten.", 
                               "Bearbeitung nicht erlaubt", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var editViewModel = new BenutzerEditViewModel(benutzerToEdit);
                var editWindow = new Views.BenutzerEditWindow(editViewModel);
                
                // Set the owner window to center the dialog
                editWindow.Owner = System.Windows.Application.Current.MainWindow;
                
                // Show dialog and get result
                var result = editWindow.ShowDialog();
                
                if (result == true)
                {
                    // Update the existing user
                    var updatedBenutzer = editViewModel.Benutzer;
                    
                    // Update in DatabaseService
                    var dataService = DatabaseService.Instance;
                    var success = await dataService.UpdateUserAsync(updatedBenutzer);
                    
                    if (success)
                    {
                        // Refresh data from database
                        await LoadSampleData();
                        
                        // Find and select the updated user
                        var updatedBenutzerFromDb = Benutzer.FirstOrDefault(b => b.Id == updatedBenutzer.Id);
                        if (updatedBenutzerFromDb != null)
                        {
                            SelectedBenutzer = updatedBenutzerFromDb;
                        }

                        StatusMessage = $"Benutzer '{updatedBenutzer.FullName}' aktualisiert";
                        MessageBox.Show($"Benutzer '{updatedBenutzer.FullName}' wurde erfolgreich aktualisiert.", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Fehler beim Aktualisieren des Benutzers in der Datenbank.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Fehler beim Bearbeiten des Benutzers: {ex.Message}";
                MessageBox.Show($"Fehler beim Bearbeiten des Benutzers:\n{ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private async Task DeleteBenutzer(User? benutzer = null)
        {
            if (!IsAdmin)
            {
                MessageBox.Show("Nur Administratoren können Benutzer löschen.", "Zugriff verweigert", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var benutzerToDelete = benutzer ?? SelectedBenutzer;
            if (benutzerToDelete == null)
            {
                MessageBox.Show("Bitte wählen Sie einen Benutzer aus.", "Hinweis", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Check if user can be deleted
            if (!CanDeleteUser(benutzerToDelete))
            {
                var currentUser = AuthenticationService.Instance.CurrentUser;
                if (currentUser != null && benutzerToDelete.Id == currentUser.Id)
                {
                    MessageBox.Show("Sie können Ihr eigenes Konto nicht löschen.\n\nBitte lassen Sie sich von einem anderen Administrator löschen.", 
                                   "Löschen nicht erlaubt", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                else if (benutzerToDelete.Role == UserRole.SystemAdministrator)
                {
                    MessageBox.Show("Systemadministratoren können nicht gelöscht werden.\n\nNur Systemadministratoren können andere Systemadministratoren verwalten.", 
                                   "Löschen nicht erlaubt", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                return;
            }

            var result = MessageBox.Show($"Möchten Sie den Benutzer '{benutzerToDelete.FullName}' wirklich löschen?\n\n" +
                                        "Diese Aktion kann nicht rückgängig gemacht werden.",
                                        "Benutzer löschen", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    // Delete from DatabaseService
                    var dataService = DatabaseService.Instance;
                    var success = await dataService.DeleteUserAsync(benutzerToDelete.Id);
                    
                                         if (success)
                     {
                         // Refresh data from database
                         await LoadSampleData();
                         SelectedBenutzer = null;

                         StatusMessage = $"Benutzer '{benutzerToDelete.FullName}' gelöscht";
                         MessageBox.Show($"Benutzer '{benutzerToDelete.FullName}' wurde erfolgreich gelöscht.", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
                     }
                    else
                    {
                        MessageBox.Show("Fehler beim Löschen des Benutzers aus der Datenbank.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    StatusMessage = $"Fehler beim Löschen des Benutzers: {ex.Message}";
                    MessageBox.Show($"Fehler beim Löschen des Benutzers:\n{ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        [RelayCommand]
        private void Search()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SearchText))
                {
                    // If search text is empty, show all users
                    // Clear and repopulate the existing collection to preserve DataGrid state
                    FilteredBenutzer.Clear();
                    foreach (var user in Benutzer)
                    {
                        FilteredBenutzer.Add(user);
                    }
                    StatusMessage = $"Alle {Benutzer.Count} Benutzer angezeigt";
                }
                else
                {
                    // Filter users based on search text
                    var searchTerm = SearchText.ToLower();
                    var filtered = Benutzer.Where(u => 
                        u.Username.ToLower().Contains(searchTerm) ||
                        u.FirstName.ToLower().Contains(searchTerm) ||
                        u.LastName.ToLower().Contains(searchTerm) ||
                        u.FullName.ToLower().Contains(searchTerm) ||
                        u.Email.ToLower().Contains(searchTerm) ||
                        u.Role.ToString().ToLower().Contains(searchTerm)
                    ).ToList();

                    // Clear and repopulate the existing collection to preserve DataGrid state
                    FilteredBenutzer.Clear();
                    foreach (var user in filtered)
                    {
                        FilteredBenutzer.Add(user);
                    }
                    StatusMessage = $"{filtered.Count} von {Benutzer.Count} Benutzern gefunden";
                }

                // Clear selection when searching
                SelectedBenutzer = null;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Fehler bei der Suche: {ex.Message}";
                MessageBox.Show($"Fehler bei der Suche:\n{ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        partial void OnSearchTextChanged(string value)
        {
            // Perform search automatically when search text changes
            Search();
        }

        public async Task RefreshData()
        {
            await LoadSampleData();
        }

    }
}
