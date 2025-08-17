using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ModernWPFApp.Models;
using ModernWPFApp.Services;
using System.Collections.ObjectModel;
using System.Windows;
using System.Linq;

namespace ModernWPFApp.ViewModels
{
    public partial class AufmassDetailViewModel : ObservableObject
    {
        [ObservableProperty] private Aufmass _aufmass;
        [ObservableProperty] private ObservableCollection<AufmassPosition> _positionen = new();
        [ObservableProperty] private AufmassPosition? _selectedPosition;
        [ObservableProperty] private string _newBeschreibung = string.Empty;
        [ObservableProperty] private string _newPosition = string.Empty;
        [ObservableProperty] private decimal _newLänge;
        [ObservableProperty] private decimal _newBreite;
        [ObservableProperty] private decimal _newHöhe;
        [ObservableProperty] private decimal _newStd;
        [ObservableProperty] private decimal _newStück;
        [ObservableProperty] private decimal _newLfm;
        [ObservableProperty] private decimal _newEinzelpreis;
        [ObservableProperty] private decimal _newGesamt;
        [ObservableProperty] private bool _isEditMode = false;
        [ObservableProperty] private int? _editingPositionId = null;
        [ObservableProperty] private string _errorMessage = string.Empty;
        [ObservableProperty] private bool _isValid = false;

        public string GesamtMengeDisplay
        {
            get
            {
                var menge = BerechneMenge();
                return $"{menge:F2}";
            }
        }

        public string ButtonText => IsEditMode ? "Speichern" : "Hinzufügen";

        public decimal GesamtSumme => Positionen.Sum(p => p.Gesamt);

        public AufmassDetailViewModel(Aufmass aufmass)
        {
            Aufmass = aufmass;
            LoadPositionen();
            PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(NewLänge) || e.PropertyName == nameof(NewBreite) || 
                e.PropertyName == nameof(NewHöhe) || e.PropertyName == nameof(NewStd) || 
                e.PropertyName == nameof(NewStück) || e.PropertyName == nameof(NewLfm) || 
                e.PropertyName == nameof(NewEinzelpreis))
            {
                CalculateNewPositionValues();
            }
        }

        private decimal BerechneMenge()
        {
            // Berechne Menge basierend auf den verfügbaren Werten
            if (NewLfm > 0)
                return NewLfm;
            else if (NewStück > 0)
                return NewStück;
            else if (NewStd > 0)
                return NewStd;
            else if (NewHöhe > 0)
                return NewLänge * NewBreite * NewHöhe; // Volumen
            else if (NewBreite > 0)
                return NewLänge * NewBreite; // Fläche
            else
                return NewLänge; // Länge
        }

        private void CalculateNewPositionValues()
        {
            // Calculate Gesamt Preis
            var menge = BerechneMenge();
            decimal gesamtPreis = 0;
            if (menge > 0 && NewEinzelpreis > 0)
            {
                gesamtPreis = menge * NewEinzelpreis;
            }
            NewGesamt = gesamtPreis;

            // Notify UI that GesamtMengeDisplay has changed
            OnPropertyChanged(nameof(GesamtMengeDisplay));
        }

        private async void LoadPositionen()
        {
            try
            {
                var dataService = DatabaseService.Instance;
                var aufmassWithPositions = await dataService.GetAufmassByIdAsync(Aufmass.Id);
                
                if (aufmassWithPositions != null)
                {
                    Positionen.Clear();
                    foreach (var position in aufmassWithPositions.Positionen)
                    {
                        Positionen.Add(position);
                    }
                }
                
                OnPropertyChanged(nameof(GesamtSumme));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Positionen: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private async Task AddOrUpdatePosition()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NewBeschreibung))
                {
                    MessageBox.Show("Bitte geben Sie eine Beschreibung ein.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Calculate values when button is clicked
                CalculateNewPositionValues();

                var dataService = DatabaseService.Instance;

                if (IsEditMode && EditingPositionId.HasValue)
                {
                    // Update existing position
                    var existingPosition = Positionen.FirstOrDefault(p => p.Id == EditingPositionId.Value);
                    if (existingPosition != null)
                    {
                        existingPosition.Position = NewPosition;
                        existingPosition.Beschreibung = NewBeschreibung;
                        existingPosition.Länge = NewLänge;
                        existingPosition.Breite = NewBreite;
                        existingPosition.Höhe = NewHöhe;
                        existingPosition.Std = NewStd;
                        existingPosition.Stück = NewStück;
                        existingPosition.Lfm = NewLfm;
                        existingPosition.Einzelpreis = NewEinzelpreis;
                        existingPosition.Gesamt = NewGesamt;

                        var success = await dataService.UpdateAufmassPositionAsync(existingPosition);
                        if (success)
                        {
                            ClearForm();
                            ExitEditMode();
                            LoadPositionen();
                        }
                        else
                        {
                            MessageBox.Show("Fehler beim Aktualisieren der Position.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else
                {
                    // Add new position
                    var positionText = string.IsNullOrWhiteSpace(NewPosition) ? (Positionen.Count + 1).ToString() : NewPosition;

                    var newPosition = new AufmassPosition
                    {
                        AufmassId = Aufmass.Id,
                        Position = positionText,
                        Beschreibung = NewBeschreibung,
                        Länge = NewLänge,
                        Breite = NewBreite,
                        Höhe = NewHöhe,
                        Std = NewStd,
                        Stück = NewStück,
                        Lfm = NewLfm,
                        Einzelpreis = NewEinzelpreis,
                        Gesamt = NewGesamt
                    };

                    var success = await dataService.AddAufmassPositionAsync(newPosition);
                    if (success)
                    {
                        ClearForm();
                        LoadPositionen();
                    }
                    else
                    {
                        MessageBox.Show("Fehler beim Hinzufügen der Position.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        [RelayCommand]
        private void EditPosition()
        {
            if (SelectedPosition == null) return;

            // Load current values into form for editing
            NewPosition = SelectedPosition.Position;
            NewBeschreibung = SelectedPosition.Beschreibung;
            NewLänge = SelectedPosition.Länge;
            NewBreite = SelectedPosition.Breite;
            NewHöhe = SelectedPosition.Höhe;
            NewStd = SelectedPosition.Std;
            NewStück = SelectedPosition.Stück;
            NewLfm = SelectedPosition.Lfm;
            NewEinzelpreis = SelectedPosition.Einzelpreis;
            NewGesamt = SelectedPosition.Gesamt;

            // Enter edit mode
            IsEditMode = true;
            EditingPositionId = SelectedPosition.Id;
            OnPropertyChanged(nameof(ButtonText));
        }

        private void ExitEditMode()
        {
            IsEditMode = false;
            EditingPositionId = null;
            OnPropertyChanged(nameof(ButtonText));
        }

        [RelayCommand]
        private async Task DeletePosition()
        {
            if (SelectedPosition == null) return;

            var result = MessageBox.Show($"Möchten Sie die Position '{SelectedPosition.Beschreibung}' wirklich löschen?",
                "Bestätigung", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var dataService = DatabaseService.Instance;
                    var success = await dataService.DeleteAufmassPositionAsync(SelectedPosition.Id);
                    
                    if (success)
                    {
                        LoadPositionen();
                    }
                    else
                    {
                        MessageBox.Show("Fehler beim Löschen der Position.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Fehler beim Löschen: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ClearForm()
        {
            NewPosition = string.Empty;
            NewBeschreibung = string.Empty;
            NewLänge = 0;
            NewBreite = 0;
            NewHöhe = 0;
            NewStd = 0;
            NewStück = 0;
            NewLfm = 0;
            NewEinzelpreis = 0;
            NewGesamt = 0;
        }
    }
}
