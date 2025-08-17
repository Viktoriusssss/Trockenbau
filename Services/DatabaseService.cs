using Microsoft.EntityFrameworkCore;
using ModernWPFApp.Data;
using ModernWPFApp.Models;
using System.Collections.ObjectModel;

namespace ModernWPFApp.Services
{
    public class DatabaseService
    {
        private static DatabaseService? _instance;
        private readonly ApplicationDbContext _context;

        public static DatabaseService Instance => _instance ??= new DatabaseService();

        private DatabaseService()
        {
            _context = new ApplicationDbContext();
            _context.Database.EnsureCreated();
        }

        #region Kunde Operations

        public async Task<ObservableCollection<Kunde>> GetKundenAsync()
        {
            var kunden = await _context.Kunden.ToListAsync();
            return new ObservableCollection<Kunde>(kunden);
        }

        public async Task<Kunde?> GetKundeByIdAsync(int id)
        {
            return await _context.Kunden.FindAsync(id);
        }

        public async Task<bool> AddKundeAsync(Kunde kunde)
        {
            try
            {
                kunde.ErstelltAm = DateTime.Now;
                _context.Kunden.Add(kunde);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateKundeAsync(Kunde kunde)
        {
            try
            {
                var existingKunde = await _context.Kunden.FindAsync(kunde.Id);
                if (existingKunde != null)
                {
                    _context.Entry(existingKunde).CurrentValues.SetValues(kunde);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteKundeAsync(int id)
        {
            try
            {
                var kunde = await _context.Kunden.FindAsync(id);
                if (kunde != null)
                {
                    _context.Kunden.Remove(kunde);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region User Operations

        public async Task<ObservableCollection<User>> GetUsersAsync()
        {
            var users = await _context.Benutzer.ToListAsync();
            return new ObservableCollection<User>(users);
        }

        public async Task<User?> GetUserByIdAsync(int id)
        {
            return await _context.Benutzer.FindAsync(id);
        }

        public async Task<bool> AddUserAsync(User user)
        {
            try
            {
                user.CreatedDate = DateTime.Now;
                user.IsActive = true;
                _context.Benutzer.Add(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            try
            {
                var existingUser = await _context.Benutzer.FindAsync(user.Id);
                if (existingUser != null)
                {
                    _context.Entry(existingUser).CurrentValues.SetValues(user);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                var user = await _context.Benutzer.FindAsync(id);
                if (user != null)
                {
                    _context.Benutzer.Remove(user);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Baustelle Operations

        public async Task<ObservableCollection<Baustelle>> GetBaustellenAsync()
        {
            var baustellen = await _context.Baustellen.ToListAsync();
            return new ObservableCollection<Baustelle>(baustellen);
        }

        public async Task<Baustelle?> GetBaustelleByIdAsync(int id)
        {
            return await _context.Baustellen.FindAsync(id);
        }

        public async Task<bool> AddBaustelleAsync(Baustelle baustelle)
        {
            try
            {
                _context.Baustellen.Add(baustelle);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateBaustelleAsync(Baustelle baustelle)
        {
            try
            {
                var existingBaustelle = await _context.Baustellen.FindAsync(baustelle.Id);
                if (existingBaustelle != null)
                {
                    _context.Entry(existingBaustelle).CurrentValues.SetValues(baustelle);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteBaustelleAsync(int id)
        {
            try
            {
                var baustelle = await _context.Baustellen.FindAsync(id);
                if (baustelle != null)
                {
                    _context.Baustellen.Remove(baustelle);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Aufmass Operations

        public async Task<ObservableCollection<Aufmass>> GetAufmasseAsync()
        {
            var aufmasse = await _context.Aufmasse.Include(a => a.Positionen).ToListAsync();
            return new ObservableCollection<Aufmass>(aufmasse);
        }

        public async Task<Aufmass?> GetAufmassByIdAsync(int id)
        {
            return await _context.Aufmasse.Include(a => a.Positionen).FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<bool> AddAufmassAsync(Aufmass aufmass)
        {
            try
            {
                _context.Aufmasse.Add(aufmass);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAufmassAsync(Aufmass aufmass)
        {
            try
            {
                var existingAufmass = await _context.Aufmasse.Include(a => a.Positionen).FirstOrDefaultAsync(a => a.Id == aufmass.Id);
                if (existingAufmass != null)
                {
                    _context.Entry(existingAufmass).CurrentValues.SetValues(aufmass);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAufmassAsync(int id)
        {
            try
            {
                var aufmass = await _context.Aufmasse.FindAsync(id);
                if (aufmass != null)
                {
                    _context.Aufmasse.Remove(aufmass);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region AufmassPosition Operations
        public async Task<bool> AddAufmassPositionAsync(AufmassPosition position)
        {
            try
            {
                _context.AufmassPositionen.Add(position);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error adding AufmassPosition: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> UpdateAufmassPositionAsync(AufmassPosition position)
        {
            try
            {
                var existingPosition = await _context.AufmassPositionen.FindAsync(position.Id);
                if (existingPosition != null)
                {
                    _context.Entry(existingPosition).CurrentValues.SetValues(position);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating AufmassPosition: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> DeleteAufmassPositionAsync(int positionId)
        {
            try
            {
                var position = await _context.AufmassPositionen.FindAsync(positionId);
                if (position != null)
                {
                    _context.AufmassPositionen.Remove(position);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error deleting AufmassPosition: {ex.Message}");
                return false;
            }
        }
        #endregion

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
