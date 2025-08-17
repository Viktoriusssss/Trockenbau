using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ModernWPFApp.Models;
using ModernWPFApp.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Linq;

namespace ModernWPFApp.ViewModels
{
    public partial class AufmassViewModel : ObservableObject
    {
        [ObservableProperty] private ObservableCollection<Aufmass> _aufmasse = new();
        [ObservableProperty] private ObservableCollection<Aufmass> _filteredAufmasse = new();
        [ObservableProperty] private Aufmass? _selectedAufmass;
        [ObservableProperty] private string _searchText = string.Empty;
        [ObservableProperty] private bool _isLoading = false;
        [ObservableProperty] private string _statusMessage = "Bereit";
        [ObservableProperty] private bool _isAufmassSelected = false;

        public AufmassViewModel()
        {
            _ = LoadSampleData();
            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(SelectedAufmass))
                {
                    IsAufmassSelected = SelectedAufmass != null;
                }
            };
        }

        private async Task LoadSampleData()
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Lade Aufmaße...";

                var dataService = DatabaseService.Instance;
                var aufmasse = await dataService.GetAufmasseAsync();

                Aufmasse.Clear();
                foreach (var aufmass in aufmasse)
                {
                    Aufmasse.Add(aufmass);
                }

                // Initialize filtered collection
                FilteredAufmasse.Clear();
                foreach (var aufmass in Aufmasse)
                {
                    FilteredAufmasse.Add(aufmass);
                }

                StatusMessage = $"{Aufmasse.Count} Aufmaße geladen";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Fehler beim Laden: {ex.Message}";
                MessageBox.Show($"Fehler beim Laden der Aufmaße: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task AddAufmass()
        {
            try
            {
                var viewModel = new AufmassEditViewModel();
                var window = new Views.AufmassEditWindow(viewModel);
                
                if (window.ShowDialog() == true)
                {
                    var populatedAufmass = viewModel.Aufmass;
                    var dataService = DatabaseService.Instance;
                    var success = await dataService.AddAufmassAsync(populatedAufmass);
                    
                    if (success)
                    {
                        // Refresh data from database
                        await LoadSampleData();
                        
                        // Find and select the newly added aufmass
                        var addedAufmass = Aufmasse.FirstOrDefault(a => a.Nummer == populatedAufmass.Nummer);
                        if (addedAufmass != null)
                        {
                            SelectedAufmass = addedAufmass;
                        }
                        
                        StatusMessage = $"Aufmaß '{populatedAufmass.Titel}' hinzugefügt";
                    }
                    else
                    {
                        MessageBox.Show("Fehler beim Speichern des Aufmaßes in der Datenbank.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Fehler beim Hinzufügen: {ex.Message}";
                MessageBox.Show($"Fehler beim Hinzufügen des Aufmaßes: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private async Task EditAufmass(Aufmass? aufmass = null)
        {
            try
            {
                var aufmassToEdit = aufmass ?? SelectedAufmass;
                if (aufmassToEdit == null)
                {
                    MessageBox.Show("Bitte wählen Sie ein Aufmaß aus.", "Hinweis", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var viewModel = new AufmassEditViewModel(aufmassToEdit);
                var window = new Views.AufmassEditWindow(viewModel);
                
                if (window.ShowDialog() == true)
                {
                    var updatedAufmass = viewModel.Aufmass;
                    var dataService = DatabaseService.Instance;
                    var success = await dataService.UpdateAufmassAsync(updatedAufmass);
                    
                    if (success)
                    {
                        // Refresh data from database
                        await LoadSampleData();
                        
                        // Find and select the updated aufmass
                        var updatedAufmassFromDb = Aufmasse.FirstOrDefault(a => a.Id == updatedAufmass.Id);
                        if (updatedAufmassFromDb != null)
                        {
                            SelectedAufmass = updatedAufmassFromDb;
                        }
                        
                        StatusMessage = $"Aufmaß '{updatedAufmass.Titel}' aktualisiert";
                    }
                    else
                    {
                        MessageBox.Show("Fehler beim Aktualisieren des Aufmaßes in der Datenbank.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Fehler beim Bearbeiten: {ex.Message}";
                MessageBox.Show($"Fehler beim Bearbeiten des Aufmaßes: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private async Task DeleteAufmass(Aufmass? aufmass = null)
        {
            try
            {
                var aufmassToDelete = aufmass ?? SelectedAufmass;
                if (aufmassToDelete == null)
                {
                    MessageBox.Show("Bitte wählen Sie ein Aufmaß aus.", "Hinweis", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var result = MessageBox.Show(
                    $"Möchten Sie das Aufmaß '{aufmassToDelete.Titel}' wirklich löschen?",
                    "Löschen bestätigen",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    var dataService = DatabaseService.Instance;
                    var success = await dataService.DeleteAufmassAsync(aufmassToDelete.Id);
                    
                    if (success)
                    {
                        // Refresh data from database
                        await LoadSampleData();
                        SelectedAufmass = null;
                        
                        StatusMessage = $"Aufmaß '{aufmassToDelete.Titel}' gelöscht";
                    }
                    else
                    {
                        MessageBox.Show("Fehler beim Löschen des Aufmaßes aus der Datenbank.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Fehler beim Löschen: {ex.Message}";
                MessageBox.Show($"Fehler beim Löschen des Aufmaßes: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    FilteredAufmasse.Clear();
                    foreach (var aufmass in Aufmasse)
                    {
                        FilteredAufmasse.Add(aufmass);
                    }
                    StatusMessage = $"Alle {Aufmasse.Count} Aufmaße angezeigt";
                }
                else
                {
                    var searchTerm = SearchText.ToLower();
                    var filtered = Aufmasse.Where(a =>
                        a.Nummer.ToLower().Contains(searchTerm) ||
                        a.Titel.ToLower().Contains(searchTerm) ||
                        a.Beschreibung.ToLower().Contains(searchTerm) ||
                        a.Status.ToString().ToLower().Contains(searchTerm)
                    ).ToList();

                    // Clear and repopulate the existing collection to preserve DataGrid state
                    FilteredAufmasse.Clear();
                    foreach (var aufmass in filtered)
                    {
                        FilteredAufmasse.Add(aufmass);
                    }
                    StatusMessage = $"{filtered.Count} von {Aufmasse.Count} Aufmaßen gefunden";
                }
                SelectedAufmass = null;
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

        [RelayCommand]
        private void OpenPositionManagement(Aufmass? aufmass = null)
        {
            try
            {
                var aufmassToManage = aufmass ?? SelectedAufmass;
                if (aufmassToManage == null)
                {
                    MessageBox.Show("Bitte wählen Sie ein Aufmaß aus.", "Hinweis", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                var detailWindow = new Views.AufmassDetailWindow(aufmassToManage);
                detailWindow.Show();
            }
            catch (Exception ex)
            {
                StatusMessage = $"Fehler beim Öffnen der Positionsverwaltung: {ex.Message}";
                MessageBox.Show($"Fehler beim Öffnen der Positionsverwaltung: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
