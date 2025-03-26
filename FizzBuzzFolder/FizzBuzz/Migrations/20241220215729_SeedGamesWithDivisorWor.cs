using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FizzBuzz.Migrations
{
    /// <inheritdoc />
    public partial class SeedGamesWithDivisorWor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "DivisorWordPairs",
                keyColumn: "Id",
                keyValue: 3,
                column: "Word",
                value: "Three");

            migrationBuilder.UpdateData(
                table: "DivisorWordPairs",
                keyColumn: "Id",
                keyValue: 4,
                column: "Word",
                value: "Seven");

            migrationBuilder.UpdateData(
                table: "DivisorWordPairs",
                keyColumn: "Id",
                keyValue: 5,
                column: "Word",
                value: "FF");

            migrationBuilder.UpdateData(
                table: "DivisorWordPairs",
                keyColumn: "Id",
                keyValue: 6,
                column: "Word",
                value: "SS");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "DivisorWordPairs",
                keyColumn: "Id",
                keyValue: 3,
                column: "Word",
                value: "Foo");

            migrationBuilder.UpdateData(
                table: "DivisorWordPairs",
                keyColumn: "Id",
                keyValue: 4,
                column: "Word",
                value: "Bar");

            migrationBuilder.UpdateData(
                table: "DivisorWordPairs",
                keyColumn: "Id",
                keyValue: 5,
                column: "Word",
                value: "Qux");

            migrationBuilder.UpdateData(
                table: "DivisorWordPairs",
                keyColumn: "Id",
                keyValue: 6,
                column: "Word",
                value: "Quux");
        }
    }
}
