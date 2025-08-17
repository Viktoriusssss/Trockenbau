using System.ComponentModel;

namespace ModernWPFApp.Models
{
    public class Baustelle : INotifyPropertyChanged
    {
        private string _name = string.Empty;
        private string _beschreibung = string.Empty;
        private string _strasse = string.Empty;
        private string _plz = string.Empty;
        private string _ort = string.Empty;
        private int _kundeId;
        private DateTime _startDatum;
        private DateTime? _endDatum;
        private BaustellenStatus _status;


        public int Id { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string Beschreibung
        {
            get => _beschreibung;
            set
            {
                _beschreibung = value;
                OnPropertyChanged(nameof(Beschreibung));
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

        public int KundeId
        {
            get => _kundeId;
            set
            {
                _kundeId = value;
                OnPropertyChanged(nameof(KundeId));
            }
        }

        public DateTime StartDatum
        {
            get => _startDatum;
            set
            {
                _startDatum = value;
                OnPropertyChanged(nameof(StartDatum));
            }
        }

        public DateTime? EndDatum
        {
            get => _endDatum;
            set
            {
                _endDatum = value;
                OnPropertyChanged(nameof(EndDatum));
            }
        }

        public BaustellenStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }



        public string VollAdresse => $"{Strasse}, {PLZ} {Ort}";
        
        public string KundeName
        {
            get
            {
                var dataService = ModernWPFApp.Services.DatabaseService.Instance;
                var kunde = dataService.GetKundeByIdAsync(KundeId).Result;
                return kunde?.FirmenName ?? $"Kunde {KundeId}";
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum BaustellenStatus
    {
        Geplant,
        InBearbeitung,
        Pausiert,
        Abgeschlossen,
        Storniert
    }
}
