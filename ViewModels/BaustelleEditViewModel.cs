using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ModernWPFApp.Models;
using ModernWPFApp.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ModernWPFApp.ViewModels
{
    public partial class BaustelleEditViewModel : ObservableObject
    {
        [ObservableProperty] private Baustelle _baustelle;
        [ObservableProperty] private string _errorMessage = string.Empty;
        [ObservableProperty] private bool _isValid = false;
        [ObservableProperty] private ObservableCollection<Kunde> _kunden = new();

        public string WindowTitle => Baustelle.Id == 0 ? "Neue Baustelle hinzufügen" : "Baustelle bearbeiten";
        public event EventHandler? SaveRequested;
        public event EventHandler? CancelRequested;

        public BaustelleEditViewModel()
        {
            Baustelle = new Baustelle
            {
                Name = "",
                Beschreibung = "",
                Strasse = "",
                PLZ = "",
                Ort = "",
                KundeId = 0,
                StartDatum = DateTime.Now,
                EndDatum = null,
                Status = BaustellenStatus.Geplant
            };

            _ = LoadKunden();
            Baustelle.PropertyChanged += OnBaustellePropertyChanged;
            ValidateBaustelle();
        }

        public BaustelleEditViewModel(Baustelle baustelle)
        {
            // Create a copy to avoid modifying the original
            Baustelle = new Baustelle
            {
                Id = baustelle.Id,
                Name = baustelle.Name,
                Beschreibung = baustelle.Beschreibung,
                Strasse = baustelle.Strasse,
                PLZ = baustelle.PLZ,
                Ort = baustelle.Ort,
                KundeId = baustelle.KundeId,
                StartDatum = baustelle.StartDatum,
                EndDatum = baustelle.EndDatum,
                Status = baustelle.Status
            };

            _ = LoadKunden();
            Baustelle.PropertyChanged += OnBaustellePropertyChanged;
            ValidateBaustelle();
        }

        private async Task LoadKunden()
        {
            try
            {
                var dataService = DatabaseService.Instance;
                var kunden = await dataService.GetKundenAsync();

                Kunden.Clear();
                foreach (var kunde in kunden)
                {
                    Kunden.Add(kunde);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Fehler beim Laden der Kunden: {ex.Message}";
            }
        }

        public async Task RefreshKunden()
        {
            await LoadKunden();
        }

        private void OnBaustellePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            ValidateBaustelle();
        }

        private void ValidateBaustelle()
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(Baustelle.Name))
                errors.Add("Name ist erforderlich");

            if (string.IsNullOrWhiteSpace(Baustelle.Strasse))
                errors.Add("Straße ist erforderlich");

            if (string.IsNullOrWhiteSpace(Baustelle.PLZ))
                errors.Add("PLZ ist erforderlich");

            if (string.IsNullOrWhiteSpace(Baustelle.Ort))
                errors.Add("Ort ist erforderlich");

            if (Baustelle.KundeId == 0)
                errors.Add("Kunde ist erforderlich");

            if (Baustelle.StartDatum >= Baustelle.EndDatum && Baustelle.EndDatum.HasValue)
                errors.Add("Enddatum muss nach dem Startdatum liegen");

            ErrorMessage = string.Join("\n", errors);
            IsValid = errors.Count == 0;
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
