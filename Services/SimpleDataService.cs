using ModernWPFApp.Models;
using System.Collections.ObjectModel;

namespace ModernWPFApp.Services
{
    public class SimpleDataService
    {
        private static SimpleDataService? _instance;
        private readonly ObservableCollection<Kunde> _kunden = new();
        private readonly ObservableCollection<Baustelle> _baustellen = new();
        private readonly ObservableCollection<User> _users = new();
        private readonly ObservableCollection<Angebot> _angebote = new();
        private readonly ObservableCollection<Aufmass> _aufmasse = new();
        private readonly ObservableCollection<Rechnung> _rechnungen = new();
        private readonly ObservableCollection<Leistungsverzeichnis> _leistungsverzeichnisse = new();

        public static SimpleDataService Instance => _instance ??= new SimpleDataService();

        private SimpleDataService()
        {
            InitializeSampleData();
        }

        private void InitializeSampleData()
        {
            // No sample data - database will be empty on first run
        }

        // Customer operations
        public ObservableCollection<Kunde> GetKunden() => _kunden;

        public Kunde? GetKundeById(int id) => _kunden.FirstOrDefault(k => k.Id == id);

        public void AddKunde(Kunde kunde)
        {
            kunde.Id = _kunden.Any() ? _kunden.Max(k => k.Id) + 1 : 1;
            kunde.ErstelltAm = DateTime.Now;
            _kunden.Add(kunde);
        }

        public void UpdateKunde(Kunde kunde)
        {
            var existing = _kunden.FirstOrDefault(k => k.Id == kunde.Id);
            if (existing != null)
            {
                var index = _kunden.IndexOf(existing);
                _kunden[index] = kunde;
            }
        }

        public void DeleteKunde(int id)
        {
            var kunde = _kunden.FirstOrDefault(k => k.Id == id);
            if (kunde != null)
            {
                _kunden.Remove(kunde);
            }
        }

        // Construction site operations
        public ObservableCollection<Baustelle> GetBaustellen() => _baustellen;

        public Baustelle? GetBaustelleById(int id) => _baustellen.FirstOrDefault(b => b.Id == id);

        public void AddBaustelle(Baustelle baustelle)
        {
            baustelle.Id = _baustellen.Any() ? _baustellen.Max(b => b.Id) + 1 : 1;
            _baustellen.Add(baustelle);
        }

        public void UpdateBaustelle(Baustelle baustelle)
        {
            var existing = _baustellen.FirstOrDefault(b => b.Id == baustelle.Id);
            if (existing != null)
            {
                var index = _baustellen.IndexOf(existing);
                _baustellen[index] = baustelle;
            }
        }

        public void DeleteBaustelle(int id)
        {
            var baustelle = _baustellen.FirstOrDefault(b => b.Id == id);
            if (baustelle != null)
            {
                _baustellen.Remove(baustelle);
            }
        }

        // Offer operations
        public ObservableCollection<Angebot> GetAngebote() => _angebote;

        public Angebot? GetAngebotById(int id) => _angebote.FirstOrDefault(a => a.Id == id);

        public void AddAngebot(Angebot angebot)
        {
            angebot.Id = _angebote.Any() ? _angebote.Max(a => a.Id) + 1 : 1;
            angebot.ErstelltAm = DateTime.Now;
            _angebote.Add(angebot);
        }

        public void UpdateAngebot(Angebot angebot)
        {
            var existing = _angebote.FirstOrDefault(a => a.Id == angebot.Id);
            if (existing != null)
            {
                var index = _angebote.IndexOf(existing);
                _angebote[index] = angebot;
            }
        }

        public void DeleteAngebot(int id)
        {
            var angebot = _angebote.FirstOrDefault(a => a.Id == id);
            if (angebot != null)
            {
                _angebote.Remove(angebot);
            }
        }

        // Measurement operations
        public ObservableCollection<Aufmass> GetAufmasse() => _aufmasse;

        public Aufmass? GetAufmassById(int id) => _aufmasse.FirstOrDefault(a => a.Id == id);

        public void AddAufmass(Aufmass aufmass)
        {
            aufmass.Id = _aufmasse.Any() ? _aufmasse.Max(a => a.Id) + 1 : 1;
            aufmass.AufmassAm = DateTime.Now;
            _aufmasse.Add(aufmass);
        }

        public void UpdateAufmass(Aufmass aufmass)
        {
            var existing = _aufmasse.FirstOrDefault(a => a.Id == aufmass.Id);
            if (existing != null)
            {
                var index = _aufmasse.IndexOf(existing);
                _aufmasse[index] = aufmass;
            }
        }

        public void DeleteAufmass(int id)
        {
            var aufmass = _aufmasse.FirstOrDefault(a => a.Id == id);
            if (aufmass != null)
            {
                _aufmasse.Remove(aufmass);
            }
        }

        // Invoice operations
        public ObservableCollection<Rechnung> GetRechnungen() => _rechnungen;

        public Rechnung? GetRechnungById(int id) => _rechnungen.FirstOrDefault(r => r.Id == id);

        public void AddRechnung(Rechnung rechnung)
        {
            rechnung.Id = _rechnungen.Any() ? _rechnungen.Max(r => r.Id) + 1 : 1;
            rechnung.Rechnungsdatum = DateTime.Now;
            _rechnungen.Add(rechnung);
        }

        public void UpdateRechnung(Rechnung rechnung)
        {
            var existing = _rechnungen.FirstOrDefault(r => r.Id == rechnung.Id);
            if (existing != null)
            {
                var index = _rechnungen.IndexOf(existing);
                _rechnungen[index] = rechnung;
            }
        }

        public void DeleteRechnung(int id)
        {
            var rechnung = _rechnungen.FirstOrDefault(r => r.Id == id);
            if (rechnung != null)
            {
                _rechnungen.Remove(rechnung);
            }
        }

        // Bill of Quantities operations
        public ObservableCollection<Leistungsverzeichnis> GetLeistungsverzeichnisse() => _leistungsverzeichnisse;

        public Leistungsverzeichnis? GetLeistungsverzeichnisById(int id) => _leistungsverzeichnisse.FirstOrDefault(lv => lv.Id == id);

        public void AddLeistungsverzeichnis(Leistungsverzeichnis lv)
        {
            lv.Id = _leistungsverzeichnisse.Any() ? _leistungsverzeichnisse.Max(l => l.Id) + 1 : 1;
            lv.ErstelltAm = DateTime.Now;
            _leistungsverzeichnisse.Add(lv);
        }

        public void UpdateLeistungsverzeichnis(Leistungsverzeichnis lv)
        {
            var existing = _leistungsverzeichnisse.FirstOrDefault(l => l.Id == lv.Id);
            if (existing != null)
            {
                var index = _leistungsverzeichnisse.IndexOf(existing);
                _leistungsverzeichnisse[index] = lv;
            }
        }

        public void DeleteLeistungsverzeichnis(int id)
        {
            var lv = _leistungsverzeichnisse.FirstOrDefault(l => l.Id == id);
            if (lv != null)
            {
                _leistungsverzeichnisse.Remove(lv);
            }
        }

        // User operations
        public ObservableCollection<User> GetAllUsers() => _users;

        public User? GetUserById(int id) => _users.FirstOrDefault(u => u.Id == id);

        public void AddUser(User user)
        {
            user.Id = _users.Any() ? _users.Max(u => u.Id) + 1 : 1;
            user.CreatedDate = DateTime.Now;
            user.IsActive = true;
            _users.Add(user);
        }

        public void UpdateUser(User user)
        {
            var existing = _users.FirstOrDefault(u => u.Id == user.Id);
            if (existing != null)
            {
                var index = _users.IndexOf(existing);
                _users[index] = user;
            }
        }

        public void DeleteUser(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                _users.Remove(user);
            }
        }

        // Statistics
        public Dictionary<string, object> GetStatistics()
        {
            var stats = new Dictionary<string, object>();

            stats["TotalKunden"] = _kunden.Count;
            stats["TotalBaustellen"] = _baustellen.Count;
            stats["AktiveBaustellen"] = _baustellen.Count(b => b.Status == BaustellenStatus.InBearbeitung);
            stats["TotalAngebote"] = _angebote.Count;
            stats["TotalRechnungen"] = _rechnungen.Count;
            stats["TotalUsers"] = _users.Count;

            // Budget and cost statistics removed as these fields are no longer available
            stats["TotalBudget"] = 0;
            stats["TotalCosts"] = 0;
            stats["BudgetUtilization"] = 0;

            return stats;
        }
    }
}
