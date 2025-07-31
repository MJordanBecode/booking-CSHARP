using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Booking.Migrations
{
    /// <inheritdoc />
    public partial class AddNote : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Price",
                table: "Offers",
                type: "int",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AddColumn<double>(
                name: "Note",
                table: "Offers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.UpdateData(
                table: "Offers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Note", "Price" },
                values: new object[] { 0.0, 120 });

            migrationBuilder.UpdateData(
                table: "Offers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Note", "Price" },
                values: new object[] { 0.0, 285 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Note",
                table: "Offers");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Offers",
                type: "decimal(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.UpdateData(
                table: "Offers",
                keyColumn: "Id",
                keyValue: 1,
                column: "Price",
                value: 120.50m);

            migrationBuilder.UpdateData(
                table: "Offers",
                keyColumn: "Id",
                keyValue: 2,
                column: "Price",
                value: 285.00m);
        }
    }
}
