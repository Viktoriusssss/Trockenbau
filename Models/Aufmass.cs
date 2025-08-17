using System.ComponentModel;
using System.Collections.ObjectModel;

namespace ModernWPFApp.Models
{
    public class Aufmass : INotifyPropertyChanged
    {
        private string _nummer = string.Empty;
        private string _titel = string.Empty;
        private string _beschreibung = string.Empty;
        private int _baustelleId;
        private DateTime _aufmassAm;
        private AufmassStatus _status;
        private string _notizen = string.Empty;
        private ObservableCollection<AufmassPosition> _positionen = new();

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

        public int BaustelleId
        {
            get => _baustelleId;
            set
            {
                _baustelleId = value;
                OnPropertyChanged(nameof(BaustelleId));
            }
        }



        public DateTime AufmassAm
        {
            get => _aufmassAm;
            set
            {
                _aufmassAm = value;
                OnPropertyChanged(nameof(AufmassAm));
            }
        }

        public AufmassStatus Status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged(nameof(Status));
            }
        }

        public string Notizen
        {
            get => _notizen;
            set
            {
                _notizen = value;
                OnPropertyChanged(nameof(Notizen));
            }
        }

        public ObservableCollection<AufmassPosition> Positionen
        {
            get => _positionen;
            set
            {
                _positionen = value;
                OnPropertyChanged(nameof(Positionen));
            }
        }

        public decimal GesamtMenge => Positionen.Sum(p => p.Gesamt);

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class AufmassPosition : INotifyPropertyChanged
    {
        private string _position = string.Empty;
        private string _beschreibung = string.Empty;
        private decimal _länge;
        private decimal _breite;
        private decimal _höhe;
        private decimal _std;
        private decimal _stück;
        private decimal _lfm;
        private decimal _einzelpreis;
        private decimal _gesamt;

        public int Id { get; set; }
        public int AufmassId { get; set; }

        public string Position
        {
            get => _position;
            set
            {
                _position = value;
                OnPropertyChanged(nameof(Position));
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

        public decimal Länge
        {
            get => _länge;
            set
            {
                _länge = value;
                OnPropertyChanged(nameof(Länge));
                BerechneGesamt();
            }
        }

        public decimal Breite
        {
            get => _breite;
            set
            {
                _breite = value;
                OnPropertyChanged(nameof(Breite));
                BerechneGesamt();
            }
        }

        public decimal Höhe
        {
            get => _höhe;
            set
            {
                _höhe = value;
                OnPropertyChanged(nameof(Höhe));
                BerechneGesamt();
            }
        }

        public decimal Std
        {
            get => _std;
            set
            {
                _std = value;
                OnPropertyChanged(nameof(Std));
                BerechneGesamt();
            }
        }

        public decimal Stück
        {
            get => _stück;
            set
            {
                _stück = value;
                OnPropertyChanged(nameof(Stück));
                BerechneGesamt();
            }
        }

        public decimal Lfm
        {
            get => _lfm;
            set
            {
                _lfm = value;
                OnPropertyChanged(nameof(Lfm));
                BerechneGesamt();
            }
        }

        public decimal Einzelpreis
        {
            get => _einzelpreis;
            set
            {
                _einzelpreis = value;
                OnPropertyChanged(nameof(Einzelpreis));
                BerechneGesamt();
            }
        }

        public decimal Gesamt
        {
            get => _gesamt;
            set
            {
                _gesamt = value;
                OnPropertyChanged(nameof(Gesamt));
            }
        }

        public string GesamtMengeDisplay
        {
            get
            {
                var menge = BerechneMenge();
                return $"{menge:F2}";
            }
        }

        private decimal BerechneMenge()
        {
            // Berechne Menge basierend auf den verfügbaren Werten
            if (Lfm > 0)
                return Lfm;
            else if (Stück > 0)
                return Stück;
            else if (Std > 0)
                return Std;
            else if (Höhe > 0)
                return Länge * Breite * Höhe; // Volumen
            else if (Breite > 0)
                return Länge * Breite; // Fläche
            else
                return Länge; // Länge
        }

        private void BerechneGesamt()
        {
            var menge = BerechneMenge();
            Gesamt = menge * Einzelpreis;
            OnPropertyChanged(nameof(GesamtMengeDisplay));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum AufmassStatus
    {
        Entwurf,
        InBearbeitung,
        Geprueft,
        Freigegeben,
        Abgerechnet
    }
}
