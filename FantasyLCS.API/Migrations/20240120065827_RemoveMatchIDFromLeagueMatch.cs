using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FantasyLCS.API.Migrations
{
    /// <inheritdoc />
    public partial class RemoveMatchIDFromLeagueMatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeagueMatches_Matches_MatchID",
                table: "LeagueMatches");

            migrationBuilder.DropIndex(
                name: "IX_LeagueMatches_MatchID",
                table: "LeagueMatches");

            migrationBuilder.DropColumn(
                name: "MatchID",
                table: "LeagueMatches");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MatchID",
                table: "LeagueMatches",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_LeagueMatches_MatchID",
                table: "LeagueMatches",
                column: "MatchID");

            migrationBuilder.AddForeignKey(
                name: "FK_LeagueMatches_Matches_MatchID",
                table: "LeagueMatches",
                column: "MatchID",
                principalTable: "Matches",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
