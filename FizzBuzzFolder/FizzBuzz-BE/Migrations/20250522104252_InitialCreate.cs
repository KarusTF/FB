using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FizzBuzz.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FizzBuzzRules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GameName = table.Column<string>(type: "text", nullable: false),
                    Author = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FizzBuzzRules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DivisorWordPairs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FizzBuzzRuleId = table.Column<int>(type: "integer", nullable: false),
                    Divisor = table.Column<int>(type: "integer", nullable: false),
                    Word = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DivisorWordPairs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DivisorWordPairs_FizzBuzzRules_FizzBuzzRuleId",
                        column: x => x.FizzBuzzRuleId,
                        principalTable: "FizzBuzzRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                    { 3, 3, 2, "Three" },
                    { 4, 7, 2, "Seven" },
                    { 5, 4, 3, "FF" },
                    { 6, 6, 3, "SS" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_DivisorWordPairs_FizzBuzzRuleId",
                table: "DivisorWordPairs",
                column: "FizzBuzzRuleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DivisorWordPairs");

            migrationBuilder.DropTable(
                name: "FizzBuzzRules");
        }
    }
}
