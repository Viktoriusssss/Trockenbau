using System.ComponentModel;
using System.Collections.ObjectModel;

namespace ModernWPFApp.Models
{
    public class Leistungsverzeichnis : INotifyPropertyChanged
    {
        private string _titel = string.Empty;
        private string _beschreibung = string.Empty;
        private string _projektNummer = string.Empty;
        private int? _baustelleId;
        private DateTime _erstelltAm;
        private LVStatus _status;
        private ObservableCollection<LVPosition> _positionen = new();

        public int Id { get; set; }

        public string Titel
        {
            get => _titel;
            set
            {
                _titel = value;
                OnPropertyChanged(nameof(Titel));
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

        public string ProjektNummer
        {
            get => _projektNummer;
            set
            {
                _projektNummer = value;
                OnPropertyChanged(nameof(ProjektNummer));
            }
        }

        public int? BaustelleId
        {
            get => _baustelleId;
            set
            {
                _baustelleId = value;
                OnPropertyChanged(nameof(BaustelleId));
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

        public LVStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        public ObservableCollection<LVPosition> Positionen
        {
            get => _positionen;
            set
            {
                _positionen = value;
                OnPropertyChanged(nameof(Positionen));
            }
        }

        public decimal GesamtMenge => Positionen.Sum(p => p.Menge);
        public decimal GesamtPreis => Positionen.Sum(p => p.Gesamtpreis);

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class LVPosition : INotifyPropertyChanged
    {
        private string _positionsNummer = string.Empty;
        private string _kurzText = string.Empty;
        private string _langText = string.Empty;
        private decimal _menge;
        private string _einheit = string.Empty;
        private decimal _einzelpreis;
        private LVPositionsTyp _typ;
        private string _oberkategorie = string.Empty;
        private string _unterkategorie = string.Empty;

        public int Id { get; set; }
        public int LeistungsverzeichnisId { get; set; }

        public string PositionsNummer
        {
            get => _positionsNummer;
            set
            {
                _positionsNummer = value;
                OnPropertyChanged(nameof(PositionsNummer));
            }
        }

        public string KurzText
        {
            get => _kurzText;
            set
            {
                _kurzText = value;
                OnPropertyChanged(nameof(KurzText));
            }
        }

        public string LangText
        {
            get => _langText;
            set
            {
                _langText = value;
                OnPropertyChanged(nameof(LangText));
            }
        }

        public decimal Menge
        {
            get => _menge;
            set
            {
                _menge = value;
                OnPropertyChanged(nameof(Menge));
                OnPropertyChanged(nameof(Gesamtpreis));
            }
        }

        public string Einheit
        {
            get => _einheit;
            set
            {
                _einheit = value;
                OnPropertyChanged(nameof(Einheit));
            }
        }

        public decimal Einzelpreis
        {
            get => _einzelpreis;
            set
            {
                _einzelpreis = value;
                OnPropertyChanged(nameof(Einzelpreis));
                OnPropertyChanged(nameof(Gesamtpreis));
            }
        }

        public LVPositionsTyp Typ
        {
            get => _typ;
            set
            {
                _typ = value;
                OnPropertyChanged(nameof(Typ));
            }
        }

        public string Oberkategorie
        {
            get => _oberkategorie;
            set
            {
                _oberkategorie = value;
                OnPropertyChanged(nameof(Oberkategorie));
            }
        }

        public string Unterkategorie
        {
            get => _unterkategorie;
            set
            {
                _unterkategorie = value;
                OnPropertyChanged(nameof(Unterkategorie));
            }
        }

        public decimal Gesamtpreis => Menge * Einzelpreis;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum LVStatus
    {
        Entwurf,
        InBearbeitung,
        Freigegeben,
        Abgeschlossen
    }

    public enum LVPositionsTyp
    {
        Normalposition,
        Alternativposition,
        Eventualposition,
        Grundposition,
        Zuschlagsposition
    }
}
