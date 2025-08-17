using System.ComponentModel;
using System.Collections.ObjectModel;

namespace ModernWPFApp.Models
{
    public class Rechnung : INotifyPropertyChanged
    {
        private string _nummer = string.Empty;
        private string _titel = string.Empty;
        private int _kundeId;
        private int? _angebotId;
        private int? _baustelleId;
        private DateTime _rechnungsdatum;
        private DateTime _faelligkeitsdatum;
        private RechnungStatus _status;
        private decimal _gesamtNetto;
        private decimal _mwstSatz;
        private decimal _mwstBetrag;
        private decimal _gesamtBrutto;
        private string _zahlungsbedingungen = string.Empty;
        private string _verwendungszweck = string.Empty;
        private ObservableCollection<RechnungPosition> _positionen = new();

        public int Id { get; set; }

        public string Nummer
        {
            get => _nummer;
            set
            {
                _nummer = value;
                OnPropertyChanged(nameof(Nummer));
            }
        }

        public string Titel
        {
            get => _titel;
            set
            {
                _titel = value;
                OnPropertyChanged(nameof(Titel));
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

        public int? AngebotId
        {
            get => _angebotId;
            set
            {
                _angebotId = value;
                OnPropertyChanged(nameof(AngebotId));
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

        public DateTime Rechnungsdatum
        {
            get => _rechnungsdatum;
            set
            {
                _rechnungsdatum = value;
                OnPropertyChanged(nameof(Rechnungsdatum));
            }
        }

        public DateTime Faelligkeitsdatum
        {
            get => _faelligkeitsdatum;
            set
            {
                _faelligkeitsdatum = value;
                OnPropertyChanged(nameof(Faelligkeitsdatum));
            }
        }

        public RechnungStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        public decimal GesamtNetto
        {
            get => _gesamtNetto;
            set
            {
                _gesamtNetto = value;
                OnPropertyChanged(nameof(GesamtNetto));
                BerechneGesamt();
            }
        }

        public decimal MwstSatz
        {
            get => _mwstSatz;
            set
            {
                _mwstSatz = value;
                OnPropertyChanged(nameof(MwstSatz));
                BerechneGesamt();
            }
        }

        public decimal MwstBetrag
        {
            get => _mwstBetrag;
            set
            {
                _mwstBetrag = value;
                OnPropertyChanged(nameof(MwstBetrag));
            }
        }

        public decimal GesamtBrutto
        {
            get => _gesamtBrutto;
            set
            {
                _gesamtBrutto = value;
                OnPropertyChanged(nameof(GesamtBrutto));
            }
        }

        public string Zahlungsbedingungen
        {
            get => _zahlungsbedingungen;
            set
            {
                _zahlungsbedingungen = value;
                OnPropertyChanged(nameof(Zahlungsbedingungen));
            }
        }

        public string Verwendungszweck
        {
            get => _verwendungszweck;
            set
            {
                _verwendungszweck = value;
                OnPropertyChanged(nameof(Verwendungszweck));
            }
        }

        public ObservableCollection<RechnungPosition> Positionen
        {
            get => _positionen;
            set
            {
                _positionen = value;
                OnPropertyChanged(nameof(Positionen));
            }
        }

        private void BerechneGesamt()
        {
            MwstBetrag = GesamtNetto * MwstSatz / 100;
            GesamtBrutto = GesamtNetto + MwstBetrag;
        }

        public int TageUeberfaellig => Status == RechnungStatus.Ueberfaellig ? 
            (DateTime.Now - Faelligkeitsdatum).Days : 0;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RechnungPosition : INotifyPropertyChanged
    {
        private string _bezeichnung = string.Empty;
        private string _beschreibung = string.Empty;
        private decimal _menge;
        private string _einheit = string.Empty;
        private decimal _einzelpreis;

        public int Id { get; set; }
        public int RechnungId { get; set; }

        public string Bezeichnung
        {
            get => _bezeichnung;
            set
            {
                _bezeichnung = value;
                OnPropertyChanged(nameof(Bezeichnung));
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

        public decimal Gesamtpreis => Menge * Einzelpreis;

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum RechnungStatus
    {
        Entwurf,
        Versendet,
        Bezahlt,
        TeilweiseBezahlt,
        Ueberfaellig,
        Storniert
    }
}
