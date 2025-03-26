using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FizzBuzz.Migrations
{
    /// <inheritdoc />
    public partial class SeedGamesWithDivisorWordPairs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "FizzBuzzRules",
                columns: new[] { "Id", "Author", "GameName" },
                values: new object[,]
                {
                    { 1, "Admin", "Game1" },
                    { 2, "Admin1", "Game2" },
                    { 3, "Admin2", "Game3" }
                });

            migrationBuilder.InsertData(
                table: "DivisorWordPairs",
                columns: new[] { "Id", "Divisor", "FizzBuzzRuleId", "Word" },
                values: new object[,]
                {
                    { 1, 3, 1, "Fizz" },
                    { 2, 5, 1, "Buzz" },
                    { 3, 3, 2, "Foo" },
                    { 4, 7, 2, "Bar" },
                    { 5, 4, 3, "Qux" },
                    { 6, 6, 3, "Quux" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DivisorWordPairs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "DivisorWordPairs",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "DivisorWordPairs",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "DivisorWordPairs",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "DivisorWordPairs",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "DivisorWordPairs",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "FizzBuzzRules",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "FizzBuzzRules",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "FizzBuzzRules",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
