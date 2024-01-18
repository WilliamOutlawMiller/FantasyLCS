using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FantasyLCS.API.Migrations
{
    /// <inheritdoc />
    public partial class AddDraft : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Drafts",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LeagueID = table.Column<int>(type: "INTEGER", nullable: false),
                    AvailablePlayerIds = table.Column<string>(type: "TEXT", nullable: false),
                    ChosenPlayerIds = table.Column<string>(type: "TEXT", nullable: false),
                    DraftOrder = table.Column<string>(type: "TEXT", nullable: false),
                    CurrentRound = table.Column<int>(type: "INTEGER", nullable: false),
                    CurrentPickIndex = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drafts", x => x.ID);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Drafts");
        }
    }
}
