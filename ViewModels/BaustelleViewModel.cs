using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ModernWPFApp.Models;
using ModernWPFApp.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Linq;

namespace ModernWPFApp.ViewModels
{
    public partial class BaustelleViewModel : ObservableObject
    {
        [ObservableProperty] private ObservableCollection<Baustelle> _baustellen = new();
        [ObservableProperty] private ObservableCollection<Baustelle> _filteredBaustellen = new();
        [ObservableProperty] private Baustelle? _selectedBaustelle;
        [ObservableProperty] private string _searchText = string.Empty;
        [ObservableProperty] private bool _isLoading = false;
        [ObservableProperty] private string _statusMessage = "Bereit";
        [ObservableProperty] private bool _isBaustelleSelected = false;

        public BaustelleViewModel()
        {
            _ = LoadSampleData();
            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(SelectedBaustelle))
                {
                    IsBaustelleSelected = SelectedBaustelle != null;
                }
            };
        }

        private async Task LoadSampleData()
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Lade Baustellen...";

                var dataService = DatabaseService.Instance;
                var baustellen = await dataService.GetBaustellenAsync();

                Baustellen.Clear();
                foreach (var baustelle in baustellen)
                {
                    Baustellen.Add(baustelle);
                }

                // Initialize filtered collection
                FilteredBaustellen.Clear();
                foreach (var baustelle in Baustellen)
                {
                    FilteredBaustellen.Add(baustelle);
                }

                StatusMessage = $"{Baustellen.Count} Baustellen geladen";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Fehler beim Laden: {ex.Message}";
                MessageBox.Show($"Fehler beim Laden der Baustellen: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

                [RelayCommand]
        private async Task AddBaustelle()
        {
            try
            {
                var viewModel = new BaustelleEditViewModel();
                
                // Refresh customers list to include any newly added customers
                await viewModel.RefreshKunden();
                
                var window = new Views.BaustelleEditWindow(viewModel);
                
                if (window.ShowDialog() == true)
                {
                    // Use the Baustelle object from the ViewModel (which contains the user input)
                    var newBaustelle = viewModel.Baustelle;
                    
                    var dataService = DatabaseService.Instance;
                    var success = await dataService.AddBaustelleAsync(newBaustelle);
                    
                     if (success)
                     {
                         // Refresh data from database
                         await LoadSampleData();
                         
                         // Find and select the newly added baustelle
                         var addedBaustelle = Baustellen.FirstOrDefault(b => b.Name == newBaustelle.Name);
                         if (addedBaustelle != null)
                         {
                             SelectedBaustelle = addedBaustelle;
                         }
                         
                         StatusMessage = $"Baustelle '{newBaustelle.Name}' hinzugefügt";
                     }
                     else
                     {
                         MessageBox.Show("Fehler beim Speichern der Baustelle in der Datenbank.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                     }
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Fehler beim Hinzufügen: {ex.Message}";
                MessageBox.Show($"Fehler beim Hinzufügen der Baustelle: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

                [RelayCommand]
        private async Task EditBaustelle(Baustelle? baustelle = null)
        {
            try
            {
                var baustelleToEdit = baustelle ?? SelectedBaustelle;
                if (baustelleToEdit == null)
                {
                    MessageBox.Show("Bitte wählen Sie eine Baustelle aus.", "Hinweis", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var viewModel = new BaustelleEditViewModel(baustelleToEdit);
                
                // Refresh customers list to include any newly added customers
                await viewModel.RefreshKunden();
                
                var window = new Views.BaustelleEditWindow(viewModel);
                
                if (window.ShowDialog() == true)
                {
                    // Use the Baustelle object from the ViewModel (which contains the user input)
                    var updatedBaustelle = viewModel.Baustelle;
                    
                    var dataService = DatabaseService.Instance;
                    var success = await dataService.UpdateBaustelleAsync(updatedBaustelle);
                    
                     if (success)
                     {
                         // Refresh data from database
                         await LoadSampleData();
                         
                         // Find and select the updated baustelle
                         var updatedBaustelleFromDb = Baustellen.FirstOrDefault(b => b.Id == updatedBaustelle.Id);
                         if (updatedBaustelleFromDb != null)
                         {
                             SelectedBaustelle = updatedBaustelleFromDb;
                         }
                         
                         StatusMessage = $"Baustelle '{updatedBaustelle.Name}' aktualisiert";
                     }
                     else
                     {
                         MessageBox.Show("Fehler beim Aktualisieren der Baustelle in der Datenbank.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                     }
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Fehler beim Bearbeiten: {ex.Message}";
                MessageBox.Show($"Fehler beim Bearbeiten der Baustelle: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private async Task DeleteBaustelle(Baustelle? baustelle = null)
        {
            try
            {
                var baustelleToDelete = baustelle ?? SelectedBaustelle;
                if (baustelleToDelete == null)
                {
                    MessageBox.Show("Bitte wählen Sie eine Baustelle aus.", "Hinweis", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var result = MessageBox.Show(
                    $"Möchten Sie die Baustelle '{baustelleToDelete.Name}' wirklich löschen?",
                    "Löschen bestätigen",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    var dataService = DatabaseService.Instance;
                    var success = await dataService.DeleteBaustelleAsync(baustelleToDelete.Id);
                    
                                         if (success)
                     {
                         // Refresh data from database
                         await LoadSampleData();
                         SelectedBaustelle = null;
                         
                         StatusMessage = $"Baustelle '{baustelleToDelete.Name}' gelöscht";
                     }
                    else
                    {
                        MessageBox.Show("Fehler beim Löschen der Baustelle aus der Datenbank.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Fehler beim Löschen: {ex.Message}";
                MessageBox.Show($"Fehler beim Löschen der Baustelle: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void Search()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(SearchText))
                {
                    // Clear and repopulate the existing collection to preserve DataGrid state
                    FilteredBaustellen.Clear();
                    foreach (var baustelle in Baustellen)
                    {
                        FilteredBaustellen.Add(baustelle);
                    }
                    StatusMessage = $"Alle {Baustellen.Count} Baustellen angezeigt";
                }
                else
                {
                    var searchTerm = SearchText.ToLower();
                    var filtered = Baustellen.Where(b =>
                        b.Name.ToLower().Contains(searchTerm) ||
                        b.Beschreibung.ToLower().Contains(searchTerm) ||
                        b.Strasse.ToLower().Contains(searchTerm) ||
                        b.PLZ.ToLower().Contains(searchTerm) ||
                        b.Ort.ToLower().Contains(searchTerm) ||
                        b.VollAdresse.ToLower().Contains(searchTerm) ||
                        b.Status.ToString().ToLower().Contains(searchTerm)
                    ).ToList();

                    // Clear and repopulate the existing collection to preserve DataGrid state
                    FilteredBaustellen.Clear();
                    foreach (var baustelle in filtered)
                    {
                        FilteredBaustellen.Add(baustelle);
                    }
                    StatusMessage = $"{filtered.Count} von {Baustellen.Count} Baustellen gefunden";
                }
                SelectedBaustelle = null;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Fehler bei der Suche: {ex.Message}";
                MessageBox.Show($"Fehler bei der Suche: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        partial void OnSearchTextChanged(string value)
        {
            Search();
        }

        public async Task RefreshData()
        {
            await LoadSampleData();
        }

    }
}
