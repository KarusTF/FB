using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FizzBuzz.Migrations
{
    /// <inheritdoc />
    public partial class AddFizzBuzzRuleWithPairs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Divisor",
                table: "FizzBuzzRules");

            migrationBuilder.DropColumn(
                name: "Word",
                table: "FizzBuzzRules");

            migrationBuilder.CreateTable(
                name: "DivisorWordPairs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FizzBuzzRuleId = table.Column<int>(type: "INTEGER", nullable: false),
                    Divisor = table.Column<int>(type: "INTEGER", nullable: false),
                    Word = table.Column<string>(type: "TEXT", nullable: false)
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

            migrationBuilder.AddColumn<int>(
                name: "Divisor",
                table: "FizzBuzzRules",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Word",
                table: "FizzBuzzRules",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
