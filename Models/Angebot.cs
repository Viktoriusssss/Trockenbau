using System.ComponentModel;
using System.Collections.ObjectModel;

namespace ModernWPFApp.Models
{
    public class Angebot : INotifyPropertyChanged
    {
        private string _nummer = string.Empty;
        private string _titel = string.Empty;
        private string _beschreibung = string.Empty;
        private int _kundeId;
        private int? _baustelleId;
        private DateTime _erstelltAm;
        private DateTime _gueltigBis;
        private AngebotStatus _status;
        private decimal _gesamtPreis;
        private decimal _mwst;
        private string _zahlungsbedingungen = string.Empty;
        private ObservableCollection<AngebotPosition> _positionen = new();

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

        public string Beschreibung
        {
            get => _beschreibung;
            set
            {
                _beschreibung = value;
                OnPropertyChanged(nameof(Beschreibung));
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

        public DateTime GueltigBis
        {
            get => _gueltigBis;
            set
            {
                _gueltigBis = value;
                OnPropertyChanged(nameof(GueltigBis));
            }
        }

        public AngebotStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        public decimal GesamtPreis
        {
            get => _gesamtPreis;
            set
            {
                _gesamtPreis = value;
                OnPropertyChanged(nameof(GesamtPreis));
            }
        }

        public decimal MWST
        {
            get => _mwst;
            set
            {
                _mwst = value;
                OnPropertyChanged(nameof(MWST));
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

        public ObservableCollection<AngebotPosition> Positionen
        {
            get => _positionen;
            set
            {
                _positionen = value;
                OnPropertyChanged(nameof(Positionen));
            }
        }

        public decimal GesamtBrutto => GesamtPreis + (GesamtPreis * MWST / 100);

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class AngebotPosition : INotifyPropertyChanged
    {
        private string _beschreibung = string.Empty;
        private decimal _menge;
        private string _einheit = string.Empty;
        private decimal _einzelpreis;

        public int Id { get; set; }
        public int AngebotId { get; set; }

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

    public enum AngebotStatus
    {
        Entwurf,
        Gesendet,
        Angenommen,
        Abgelehnt,
        Abgelaufen
    }
}
