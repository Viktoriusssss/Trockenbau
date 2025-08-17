using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ModernWPFApp.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ModernWPFApp.ViewModels
{
    public partial class KundeEditViewModel : ObservableObject
    {
        [ObservableProperty]
        private Kunde _kunde;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        [ObservableProperty]
        private bool _isValid = false;

        public string WindowTitle => Kunde.Id == 0 ? "Neuen Kunden hinzufügen" : "Kunden bearbeiten";

        public event EventHandler? SaveRequested;
        public event EventHandler? CancelRequested;

        public KundeEditViewModel()
        {
            Kunde = new Kunde
            {
                ErstelltAm = DateTime.Now
            };
            
            // Subscribe to property changes for validation
            Kunde.PropertyChanged += OnKundePropertyChanged;
            ValidateKunde();
        }

        public KundeEditViewModel(Kunde kunde)
        {
            // Create a copy of the customer to avoid modifying the original
            Kunde = new Kunde
            {
                Id = kunde.Id,
                FirmenName = kunde.FirmenName,
                Ansprechpartner = kunde.Ansprechpartner,
                Strasse = kunde.Strasse,
                PLZ = kunde.PLZ,
                Ort = kunde.Ort,
                Telefon = kunde.Telefon,
                Email = kunde.Email,
                ErstelltAm = kunde.ErstelltAm
            };
            
            // Subscribe to property changes for validation
            Kunde.PropertyChanged += OnKundePropertyChanged;
            ValidateKunde();
        }

        private void OnKundePropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            ValidateKunde();
        }

        private void ValidateKunde()
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(Kunde.FirmenName))
                errors.Add("Firmenname ist erforderlich");

            if (string.IsNullOrWhiteSpace(Kunde.Ansprechpartner))
                errors.Add("Ansprechpartner ist erforderlich");

            if (string.IsNullOrWhiteSpace(Kunde.Strasse))
                errors.Add("Straße ist erforderlich");

            if (string.IsNullOrWhiteSpace(Kunde.PLZ))
                errors.Add("PLZ ist erforderlich");

            if (string.IsNullOrWhiteSpace(Kunde.Ort))
                errors.Add("Ort ist erforderlich");

            if (string.IsNullOrWhiteSpace(Kunde.Telefon))
                errors.Add("Telefon ist erforderlich");

            if (string.IsNullOrWhiteSpace(Kunde.Email))
                errors.Add("Email ist erforderlich");
            else if (!IsValidEmail(Kunde.Email))
                errors.Add("Email hat ein ungültiges Format");

            ErrorMessage = string.Join("\n", errors);
            IsValid = errors.Count == 0;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
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
