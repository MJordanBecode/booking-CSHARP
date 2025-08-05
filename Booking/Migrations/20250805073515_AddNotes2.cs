using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Booking.Migrations
{
    /// <inheritdoc />
    public partial class AddNotes2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Offers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Offers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user1-guid-12345");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "user2-guid-67890");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "DateCreation", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "user1-guid-12345", 0, "static-concurrency-stamp-12345", new DateTime(2024, 1, 15, 10, 30, 0, 0, DateTimeKind.Utc), "marie.dupont@email.com", true, "Marie", "Dupont", false, null, "MARIE.DUPONT@EMAIL.COM", "MARIE.DUPONT@EMAIL.COM", "AQAAAAIAAYagAAAAEJzWlLc+Q3UjKD5z9Z7Z9Z7Z9Z7Z9Z7Z9Z7Z9Z7Z9Z7Z9Z7Z9Z7Z9Z7Z9Z7Z9Z7Z9Z7Z=", "+33123456789", false, "static-security-stamp-12345", false, "marie.dupont@email.com" },
                    { "user2-guid-67890", 0, "static-concurrency-stamp-12345", new DateTime(2024, 1, 20, 10, 30, 0, 0, DateTimeKind.Utc), "jean.martin@email.com", true, "Jean", "Martin", false, null, "JEAN.MARTIN@EMAIL.COM", "JEAN.MARTIN@EMAIL.COM", "AQAAAAIAAYagAAAAEJzWlLc+Q3UjKD5z9Z7Z9Z7Z9Z7Z9Z7Z9Z7Z9Z7Z9Z7Z9Z7Z9Z7Z9Z7Z9Z7Z9Z7Z9Z7Z=", "+33987654321", false, "static-security-stamp-12345", false, "jean.martin@email.com" }
                });

            migrationBuilder.InsertData(
                table: "Offers",
                columns: new[] { "Id", "BathNumber", "BedNumber", "Description", "IdUser", "Image", "Location", "Note", "NumberOfRooms", "OfferUserId", "Price", "Title", "Type" },
                values: new object[,]
                {
                    { 1, 1, 2, "Magnifique appartement de 3 pièces situé en plein centre-ville. Entièrement rénové avec des finitions de qualité. À proximité de tous les commerces et transports en commun. Idéal pour un séjour d'affaires ou touristique.", "user1-guid-12345", "https://media.istockphoto.com/id/1293762741/fr/photo/int%C3%A9rieur-moderne-de-salle-de-vie-rendu-3d.jpg?s=612x612&w=0&k=20&c=BKixm6wq1Y6NFFF-8XllknUQvSboRmCmjn_Lm_erHmQ=", "Paris, France", 0.0, 3, null, 120, "Appartement moderne centre-ville", "Appartement" },
                    { 2, 3, 4, "Superbe villa avec piscine privée située à seulement 5 minutes à pied de la plage. 4 chambres spacieuses, grand salon avec vue sur mer, cuisine équipée, jardin tropical. Parfait pour des vacances en famille ou entre amis.", "user2-guid-67890", "https://www.vacationkey.com/photos/1/1/119108-1.jpg", "Nice, France", 0.0, 6, null, 285, "Villa avec piscine près de la plage", "Villa" }
                });
        }
    }
}
