using System.ComponentModel;

namespace ModernWPFApp.Models
{
    public class Kunde : INotifyPropertyChanged
    {
        private string _firmenName = string.Empty;
        private string _ansprechpartner = string.Empty;
        private string _strasse = string.Empty;
        private string _plz = string.Empty;
        private string _ort = string.Empty;
        private string _telefon = string.Empty;
        private string _email = string.Empty;

        private DateTime _erstelltAm;

        public int Id { get; set; }

        public string FirmenName
        {
            get => _firmenName;
            set
            {
                _firmenName = value;
                OnPropertyChanged(nameof(FirmenName));
            }
        }

        public string Ansprechpartner
        {
            get => _ansprechpartner;
            set
            {
                _ansprechpartner = value;
                OnPropertyChanged(nameof(Ansprechpartner));
            }
        }

        public string Strasse
        {
            get => _strasse;
            set
            {
                _strasse = value;
                OnPropertyChanged(nameof(Strasse));
            }
        }

        public string PLZ
        {
            get => _plz;
            set
            {
                _plz = value;
                OnPropertyChanged(nameof(PLZ));
            }
        }

        public string Ort
        {
            get => _ort;
            set
            {
                _ort = value;
                OnPropertyChanged(nameof(Ort));
            }
        }

        public string Telefon
        {
            get => _telefon;
            set
            {
                _telefon = value;
                OnPropertyChanged(nameof(Telefon));
            }
        }

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }



        public DateTime ErstelltAm
        {
            get => _erstelltAm;
            set
            {
                _erstelltAm = value;
                OnPropertyChanged(nameof(ErstelltAm));
            }
        }

        public string VollAdresse => $"{Strasse}, {PLZ} {Ort}";

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
