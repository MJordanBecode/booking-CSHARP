using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Solution.Models;
using Microsoft.AspNetCore.Identity;

namespace Booking.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ApplicationUser> Users { get; set; }
    public virtual DbSet<Offer> Offers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Offer>(entity =>
        {
            entity.HasKey(o => o.Id);

            entity.Property(o => o.Title)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(o => o.Description)
                .IsRequired()
                .HasMaxLength(2000);

            entity.Property(o => o.Location)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(o => o.Type)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(o => o.BedNumber)
                .IsRequired();

            entity.Property(o => o.BathNumber)
                .IsRequired();

            entity.Property(o => o.NumberOfRooms)
                .IsRequired();

            entity.Property(o => o.Price)
                .IsRequired()
                .HasPrecision(10, 2);

            entity.Property(o => o.Image)
                .HasMaxLength(500);

            // Relation avec User
            entity.HasOne<ApplicationUser>()
                .WithMany(u => u.Offers) // Un utilisateur peut avoir plusieurs offres
                .HasForeignKey(o => o.IdUser)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configuration des propriétés supplémentaires pour ApplicationUser
        modelBuilder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(u => u.FirstName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(u => u.LastName)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(u => u.PhoneNumber)
                .HasMaxLength(20);

            entity.Property(u => u.DateCreation)
                .IsRequired()
                .HasDefaultValueSql("GETUTCDATE()"); // Valeur par défaut en base
        });

        // Données de test (Seed Data)
        SeedData(modelBuilder);

        base.OnModelCreating(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Création du hasher pour les mots de passe
        var hasher = new PasswordHasher<ApplicationUser>();
        // Exemple de hachage test
        var passwordHash = "AQAAAAIAAYagAAAAEJzWlLc+Q3UjKD5z9Z7Z9Z7Z9Z7Z9Z7Z9Z7Z9Z7Z9Z7Z9Z7Z9Z7Z9Z7Z9Z7Z9Z7Z9Z7Z=";

        // Création de 2 utilisateurs
        var dateCreation = new DateTime(2024, 1, 15, 10, 30, 0, DateTimeKind.Utc);
        
        var user1 = new ApplicationUser
        {
            Id = "user1-guid-12345",
            UserName = "marie.dupont@email.com",
            NormalizedUserName = "MARIE.DUPONT@EMAIL.COM",
            Email = "marie.dupont@email.com",
            NormalizedEmail = "MARIE.DUPONT@EMAIL.COM",
            EmailConfirmed = true,
            FirstName = "Marie",
            LastName = "Dupont",
            PhoneNumber = "+33123456789",
            DateCreation = dateCreation,
            /*SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString()
            */
            SecurityStamp = "static-security-stamp-12345",
            ConcurrencyStamp = "static-concurrency-stamp-12345",
            PasswordHash = passwordHash
        };
      //  user1.PasswordHash = hasher.HashPassword(user1, "Password123!");

        var user2 = new ApplicationUser
        {
            Id = "user2-guid-67890",
            UserName = "jean.martin@email.com",
            NormalizedUserName = "JEAN.MARTIN@EMAIL.COM",
            Email = "jean.martin@email.com",
            NormalizedEmail = "JEAN.MARTIN@EMAIL.COM",
            EmailConfirmed = true,
            FirstName = "Jean",
            LastName = "Martin",
            PhoneNumber = "+33987654321",
            DateCreation = dateCreation.AddDays(5),
          /*  SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString()
            */
          SecurityStamp = "static-security-stamp-12345",
          ConcurrencyStamp = "static-concurrency-stamp-12345",
          PasswordHash = passwordHash
        };
       // user2.PasswordHash = hasher.HashPassword(user2, "Password456!");

        modelBuilder.Entity<ApplicationUser>().HasData(user1, user2);

        // Création de 2 offres
        var offer1 = new Offer
        {
            Id = 1,
            Title = "Appartement moderne centre-ville",
            Description = "Magnifique appartement de 3 pièces situé en plein centre-ville. Entièrement rénové avec des finitions de qualité. À proximité de tous les commerces et transports en commun. Idéal pour un séjour d'affaires ou touristique.",
            Location = "Paris, France",
            Type = "Appartement",
            BedNumber = 2,
            BathNumber = 1,
            NumberOfRooms = 3,
            Price = 120.50m,
            Image = "https://media.istockphoto.com/id/1293762741/fr/photo/int%C3%A9rieur-moderne-de-salle-de-vie-rendu-3d.jpg?s=612x612&w=0&k=20&c=BKixm6wq1Y6NFFF-8XllknUQvSboRmCmjn_Lm_erHmQ=",
            IdUser = "user1-guid-12345"
        };

        var offer2 = new Offer
        {
            Id = 2,
            Title = "Villa avec piscine près de la plage",
            Description = "Superbe villa avec piscine privée située à seulement 5 minutes à pied de la plage. 4 chambres spacieuses, grand salon avec vue sur mer, cuisine équipée, jardin tropical. Parfait pour des vacances en famille ou entre amis.",
            Location = "Nice, France",
            Type = "Villa",
            BedNumber = 4,
            BathNumber = 3,
            NumberOfRooms = 6,
            Price = 285.00m,
            Image = "https://www.vacationkey.com/photos/1/1/119108-1.jpg",
            IdUser = "user2-guid-67890"
        };

        modelBuilder.Entity<Offer>().HasData(offer1, offer2);
    }
}