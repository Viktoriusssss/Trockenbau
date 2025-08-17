using Microsoft.EntityFrameworkCore;
using ModernWPFApp.Models;

namespace ModernWPFApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Kunde> Kunden { get; set; } = null!;
        public DbSet<User> Benutzer { get; set; } = null!;
        public DbSet<Baustelle> Baustellen { get; set; } = null!;
        public DbSet<Aufmass> Aufmasse { get; set; } = null!;
        public DbSet<AufmassPosition> AufmassPositionen { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=EasyProjectManagement.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Kunde configuration
            modelBuilder.Entity<Kunde>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirmenName).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Ansprechpartner).HasMaxLength(200);
                entity.Property(e => e.Strasse).HasMaxLength(200);
                entity.Property(e => e.PLZ).HasMaxLength(10);
                entity.Property(e => e.Ort).HasMaxLength(100);
                entity.Property(e => e.Telefon).HasMaxLength(20);
                entity.Property(e => e.Email).HasMaxLength(200);
            });

            // User configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).IsRequired().HasMaxLength(50);
                entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
                entity.Property(e => e.PasswordHash).HasMaxLength(500);
            });

            // Baustelle configuration
            modelBuilder.Entity<Baustelle>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Beschreibung).HasMaxLength(1000);
                entity.Property(e => e.Strasse).IsRequired().HasMaxLength(200);
                entity.Property(e => e.PLZ).IsRequired().HasMaxLength(10);
                entity.Property(e => e.Ort).IsRequired().HasMaxLength(100);
                
                // Foreign key relationship
                entity.HasOne<Kunde>()
                    .WithMany()
                    .HasForeignKey(e => e.KundeId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // Aufmass configuration
            modelBuilder.Entity<Aufmass>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Nummer).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Titel).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Beschreibung).HasMaxLength(1000);
                entity.Property(e => e.Notizen).HasMaxLength(2000);
                
                // Foreign key relationships
                entity.HasOne<Baustelle>()
                    .WithMany()
                    .HasForeignKey(e => e.BaustelleId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // AufmassPosition configuration
            modelBuilder.Entity<AufmassPosition>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Position).IsRequired().HasMaxLength(50);
                entity.Property(e => e.Beschreibung).IsRequired().HasMaxLength(500);
                entity.Property(e => e.Länge).HasPrecision(10, 2);
                entity.Property(e => e.Breite).HasPrecision(10, 2);
                entity.Property(e => e.Höhe).HasPrecision(10, 2);
                entity.Property(e => e.Std).HasPrecision(10, 2);
                entity.Property(e => e.Stück).HasPrecision(10, 2);
                entity.Property(e => e.Lfm).HasPrecision(10, 2);
                entity.Property(e => e.Einzelpreis).HasPrecision(10, 2);
                entity.Property(e => e.Gesamt).HasPrecision(10, 2);
                
                // Foreign key relationship
                entity.HasOne<Aufmass>()
                    .WithMany(a => a.Positionen)
                    .HasForeignKey(e => e.AufmassId)
                    .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
