using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ModernWPFApp.Models;
using ModernWPFApp.Services;
using System.Collections.ObjectModel;
using System.Windows;

namespace ModernWPFApp.ViewModels
{
    public partial class KundenViewModel : ObservableObject
    {
        [ObservableProperty]
        private ObservableCollection<Kunde> _kunden = new();

        [ObservableProperty]
        private ObservableCollection<Kunde> _filteredKunden = new();

        [ObservableProperty]
        private Kunde? _selectedKunde;

        [ObservableProperty]
        private string _searchText = string.Empty;

        [ObservableProperty]
        private bool _isLoading = false;

        [ObservableProperty]
        private string _statusMessage = "Bereit";

        [ObservableProperty]
        private bool _isKundeSelected = false;

        public KundenViewModel()
        {
            _ = LoadSampleData();
            
            // Subscribe to SelectedKunde changes to update IsKundeSelected
            PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(SelectedKunde))
                {
                    IsKundeSelected = SelectedKunde != null;
                }
            };
        }

        private async Task LoadSampleData()
        {
            try
            {
                IsLoading = true;
                StatusMessage = "Lade Kunden...";

                // Load data from DatabaseService
                var dataService = DatabaseService.Instance;
                var kunden = await dataService.GetKundenAsync();
                
                Kunden = new ObservableCollection<Kunde>(kunden);
                FilteredKunden = new ObservableCollection<Kunde>(Kunden);

                StatusMessage = $"{Kunden.Count} Kunden geladen";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Fehler beim Laden der Kunden: {ex.Message}";
                MessageBox.Show($"Fehler beim Laden der Kunden:\n{ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private async Task AddKunde()
        {
            try
            {
                var editViewModel = new KundeEditViewModel();
                var editWindow = new Views.KundeEditWindow(editViewModel);
                
                // Set the owner window to center the dialog
                editWindow.Owner = System.Windows.Application.Current.MainWindow;
                
                var result = editWindow.ShowDialog();
                
                if (result == true)
                {
                    // Add new customer
                    var newKunde = editViewModel.Kunde;
                    
                    // Add to DatabaseService
                    var dataService = DatabaseService.Instance;
                    var success = await dataService.AddKundeAsync(newKunde);
                    
                    if (success)
                    {
                        // Refresh data from database
                        await LoadSampleData();
                        
                        // Find and select the newly added customer
                        var addedKunde = Kunden.FirstOrDefault(k => k.FirmenName == newKunde.FirmenName);
                        if (addedKunde != null)
                        {
                            SelectedKunde = addedKunde;
                        }

                        StatusMessage = $"Kunde '{newKunde.FirmenName}' hinzugefügt";
                        MessageBox.Show($"Neuer Kunde '{newKunde.FirmenName}' wurde erfolgreich hinzugefügt.", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Fehler beim Speichern des Kunden in der Datenbank.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Fehler beim Hinzufügen des Kunden: {ex.Message}";
                MessageBox.Show($"Fehler beim Hinzufügen des Kunden:\n{ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private async Task EditKunde(Kunde? kunde = null)
        {
            var kundeToEdit = kunde ?? SelectedKunde;
            if (kundeToEdit == null)
            {
                MessageBox.Show("Bitte wählen Sie einen Kunden aus.", "Hinweis", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            try
            {
                var editViewModel = new KundeEditViewModel(kundeToEdit);
                var editWindow = new Views.KundeEditWindow(editViewModel);
                
                // Set the owner window to center the dialog
                editWindow.Owner = System.Windows.Application.Current.MainWindow;
                
                var result = editWindow.ShowDialog();
                
                if (result == true)
                {
                    // Update the existing customer
                    var updatedKunde = editViewModel.Kunde;
                    
                    // Update in DatabaseService
                    var dataService = DatabaseService.Instance;
                    var success = await dataService.UpdateKundeAsync(updatedKunde);
                    
                    if (success)
                    {
                        // Refresh data from database
                        await LoadSampleData();
                        
                        // Find and select the updated customer
                        var updatedKundeFromDb = Kunden.FirstOrDefault(k => k.Id == updatedKunde.Id);
                        if (updatedKundeFromDb != null)
                        {
                            SelectedKunde = updatedKundeFromDb;
                        }

                        StatusMessage = $"Kunde '{updatedKunde.FirmenName}' aktualisiert";
                        MessageBox.Show($"Kunde '{updatedKunde.FirmenName}' wurde erfolgreich aktualisiert.", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Fehler beim Aktualisieren des Kunden in der Datenbank.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Fehler beim Bearbeiten des Kunden: {ex.Message}";
                MessageBox.Show($"Fehler beim Bearbeiten des Kunden:\n{ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private async Task DeleteKunde(Kunde? kunde = null)
        {
            var kundeToDelete = kunde ?? SelectedKunde;
            if (kundeToDelete == null)
            {
                MessageBox.Show("Bitte wählen Sie einen Kunden aus.", "Hinweis", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var result = MessageBox.Show($"Möchten Sie den Kunden '{kundeToDelete.FirmenName}' wirklich löschen?\n\n" +
                                        "Diese Aktion kann nicht rückgängig gemacht werden.",
                                        "Kunde löschen", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    // Delete from DatabaseService
                    var dataService = DatabaseService.Instance;
                    var success = await dataService.DeleteKundeAsync(kundeToDelete.Id);
                    
                    if (success)
                    {
                        // Refresh data from database
                        await LoadSampleData();
                        SelectedKunde = null;

                        StatusMessage = $"Kunde '{kundeToDelete.FirmenName}' gelöscht";
                        MessageBox.Show($"Kunde '{kundeToDelete.FirmenName}' wurde erfolgreich gelöscht.", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Fehler beim Löschen des Kunden aus der Datenbank.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    StatusMessage = $"Fehler beim Löschen des Kunden: {ex.Message}";
                    MessageBox.Show($"Fehler beim Löschen des Kunden:\n{ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
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
                    // If search text is empty, show all customers
                    // Clear and repopulate the existing collection to preserve DataGrid state
                    FilteredKunden.Clear();
                    foreach (var kunde in Kunden)
                    {
                        FilteredKunden.Add(kunde);
                    }
                    StatusMessage = $"Alle {Kunden.Count} Kunden angezeigt";
                }
                else
                {
                    // Filter customers based on search text
                    var searchTerm = SearchText.ToLower();
                    var filtered = Kunden.Where(k => 
                        k.FirmenName.ToLower().Contains(searchTerm) ||
                        k.Ansprechpartner.ToLower().Contains(searchTerm) ||
                        k.Email.ToLower().Contains(searchTerm) ||
                        k.Telefon.ToLower().Contains(searchTerm) ||
                        k.Ort.ToLower().Contains(searchTerm) ||
                        k.Strasse.ToLower().Contains(searchTerm) ||
                        k.PLZ.ToLower().Contains(searchTerm)
                    ).ToList();

                    // Clear and repopulate the existing collection to preserve DataGrid state
                    FilteredKunden.Clear();
                    foreach (var kunde in filtered)
                    {
                        FilteredKunden.Add(kunde);
                    }
                    StatusMessage = $"{filtered.Count} von {Kunden.Count} Kunden gefunden";
                }

                // Clear selection when searching
                SelectedKunde = null;
            }
            catch (Exception ex)
            {
                StatusMessage = $"Fehler bei der Suche: {ex.Message}";
                MessageBox.Show($"Fehler bei der Suche:\n{ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        partial void OnSelectedKundeChanged(Kunde? value)
        {
            OnPropertyChanged(nameof(IsKundeSelected));
            
            // Debug output
            if (value != null)
            {
                System.Diagnostics.Debug.WriteLine($"SelectedKunde changed to: {value.FirmenName}");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("SelectedKunde changed to: null");
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
