using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FantasyLCS.API.Migrations
{
    /// <inheritdoc />
    public partial class AddLeagueMatches : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LeagueMatches",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LeagueID = table.Column<int>(type: "INTEGER", nullable: false),
                    Week = table.Column<string>(type: "TEXT", nullable: false),
                    MatchDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    TeamOneID = table.Column<int>(type: "INTEGER", nullable: false),
                    TeamTwoID = table.Column<int>(type: "INTEGER", nullable: false),
                    Winner = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeagueMatches", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LeagueMatches_Teams_TeamOneID",
                        column: x => x.TeamOneID,
                        principalTable: "Teams",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeagueMatches_Teams_TeamTwoID",
                        column: x => x.TeamTwoID,
                        principalTable: "Teams",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeagueMatches_TeamOneID",
                table: "LeagueMatches",
                column: "TeamOneID");

            migrationBuilder.CreateIndex(
                name: "IX_LeagueMatches_TeamTwoID",
                table: "LeagueMatches",
                column: "TeamTwoID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeagueMatches");
        }
    }
}
