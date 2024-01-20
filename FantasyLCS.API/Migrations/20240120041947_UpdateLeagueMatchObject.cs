using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FantasyLCS.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateLeagueMatchObject : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MatchID",
                table: "LeagueMatches",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_LeagueMatches_LeagueID",
                table: "LeagueMatches",
                column: "LeagueID");

            migrationBuilder.CreateIndex(
                name: "IX_LeagueMatches_MatchID",
                table: "LeagueMatches",
                column: "MatchID");

            migrationBuilder.AddForeignKey(
                name: "FK_LeagueMatches_Leagues_LeagueID",
                table: "LeagueMatches",
                column: "LeagueID",
                principalTable: "Leagues",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LeagueMatches_Matches_MatchID",
                table: "LeagueMatches",
                column: "MatchID",
                principalTable: "Matches",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LeagueMatches_Leagues_LeagueID",
                table: "LeagueMatches");

            migrationBuilder.DropForeignKey(
                name: "FK_LeagueMatches_Matches_MatchID",
                table: "LeagueMatches");

            migrationBuilder.DropIndex(
                name: "IX_LeagueMatches_LeagueID",
                table: "LeagueMatches");

            migrationBuilder.DropIndex(
                name: "IX_LeagueMatches_MatchID",
                table: "LeagueMatches");

            migrationBuilder.DropColumn(
                name: "MatchID",
                table: "LeagueMatches");
        }
    }
}
