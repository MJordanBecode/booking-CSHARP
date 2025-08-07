using Bogus;
using Booking.Models;

// Remplace par le namespace exact de ton ApplicationDbContext

namespace Booking.Data;

public static class DbSeeder
{
    public static void SeedOffers(ApplicationDbContext dbContext)
    {
        // Ne pas exécuter si des données existent déjà
        if (dbContext.Offers.Any())
            return;

        // Ton ID utilisateur fixe
        var myUserId = "3a89363d-d299-4a44-ab14-ace757303d92";

        // Faker pour générer des fausses offres
        var offerFaker = new Faker<Offer>()
            .RuleFor(o => o.IdUser, _ => myUserId) // 👈 ton ID utilisateur
            .RuleFor(o => o.Title, f => f.Commerce.ProductName())
            .RuleFor(o => o.Description, f => f.Lorem.Paragraph())
            .RuleFor(o => o.Location, f => f.Address.City())
            .RuleFor(o => o.Type, f => f.PickRandom("Appartement", "Maison", "Studio", "Villa"))
            .RuleFor(o => o.BedNumber, f => f.Random.Int(1, 5))
            .RuleFor(o => o.BathNumber, f => f.Random.Int(1, 2))
            .RuleFor(o => o.NumberOfRooms, f => f.Random.Int(1, 7))
            .RuleFor(o => o.Note, f => Math.Round(f.Random.Double(2.0, 5.0), 1))
            .RuleFor(o => o.Price, f => f.Random.Int(50, 500))
            .RuleFor(o => o.Image, f => $"images/sample{f.Random.Int(1, 5)}.jpg")
            .RuleFor(o => o.OfferUser, _ => null!); // La navigation est ignorée pour le seed

        var offers = offerFaker.Generate(50); // 20 fausses offres

        dbContext.Offers.AddRange(offers);
        dbContext.SaveChanges();
    }
}