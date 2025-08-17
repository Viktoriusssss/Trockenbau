using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ModernWPFApp.Models;
using ModernWPFApp.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ModernWPFApp.ViewModels
{
    public partial class AufmassEditViewModel : ObservableObject
    {
        [ObservableProperty] private Aufmass _aufmass;
        [ObservableProperty] private string _errorMessage = string.Empty;
        [ObservableProperty] private bool _isValid = false;
        [ObservableProperty] private ObservableCollection<Baustelle> _baustellen = new();

        public string WindowTitle => Aufmass.Id == 0 ? "Neues Aufmaß hinzufügen" : "Aufmaß bearbeiten";
        public event EventHandler? SaveRequested;
        public event EventHandler? CancelRequested;

        public AufmassEditViewModel()
        {
            Aufmass = new Aufmass
                            {
                    Nummer = "",
                    Titel = "",
                    Beschreibung = "",
                    BaustelleId = 0,
                    AufmassAm = DateTime.Now,
                    Status = AufmassStatus.Entwurf,
                    Notizen = "",
                    Positionen = new ObservableCollection<AufmassPosition>()
                };

            _ = LoadData();
            Aufmass.PropertyChanged += OnAufmassPropertyChanged;
            ValidateAufmass();
        }

        public AufmassEditViewModel(Aufmass aufmass)
        {
            // Create a copy to avoid modifying the original
            Aufmass = new Aufmass
            {
                Id = aufmass.Id,
                Nummer = aufmass.Nummer,
                Titel = aufmass.Titel,
                Beschreibung = aufmass.Beschreibung,
                BaustelleId = aufmass.BaustelleId,
                AufmassAm = aufmass.AufmassAm,
                Status = aufmass.Status,
                Notizen = aufmass.Notizen,
                Positionen = new ObservableCollection<AufmassPosition>(aufmass.Positionen)
            };

            _ = LoadData();
            Aufmass.PropertyChanged += OnAufmassPropertyChanged;
            ValidateAufmass();
        }

        private async Task LoadData()
        {
            try
            {
                var dataService = DatabaseService.Instance;
                var baustellen = await dataService.GetBaustellenAsync();

                Baustellen.Clear();
                foreach (var baustelle in baustellen)
                {
                    Baustellen.Add(baustelle);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Fehler beim Laden der Daten: {ex.Message}";
            }
        }

        private void OnAufmassPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            ValidateAufmass();
        }

        private void ValidateAufmass()
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(Aufmass.Nummer))
                errors.Add("Nummer ist erforderlich");

            if (string.IsNullOrWhiteSpace(Aufmass.Titel))
                errors.Add("Titel ist erforderlich");

            if (Aufmass.BaustelleId == 0)
                errors.Add("Baustelle ist erforderlich");



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
