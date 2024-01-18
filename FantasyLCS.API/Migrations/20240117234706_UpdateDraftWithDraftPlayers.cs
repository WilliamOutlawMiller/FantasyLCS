using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FantasyLCS.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDraftWithDraftPlayers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailablePlayerIds",
                table: "Drafts");

            migrationBuilder.DropColumn(
                name: "ChosenPlayerIds",
                table: "Drafts");

            migrationBuilder.CreateTable(
                name: "DraftPlayers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Drafted = table.Column<bool>(type: "INTEGER", nullable: false),
                    Position = table.Column<int>(type: "INTEGER", nullable: false),
                    DraftID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DraftPlayers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_DraftPlayers_Drafts_DraftID",
                        column: x => x.DraftID,
                        principalTable: "Drafts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DraftPlayers_DraftID",
                table: "DraftPlayers",
                column: "DraftID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DraftPlayers");

            migrationBuilder.AddColumn<string>(
                name: "AvailablePlayerIds",
                table: "Drafts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ChosenPlayerIds",
                table: "Drafts",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
