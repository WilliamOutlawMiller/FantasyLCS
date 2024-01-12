using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FantasyLCS.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateChampionStatsKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ChampionStats",
                table: "ChampionStats");

            migrationBuilder.AlterColumn<int>(
                name: "ChampionID",
                table: "ChampionStats",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .OldAnnotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChampionStats",
                table: "ChampionStats",
                columns: new[] { "ChampionID", "PlayerID" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ChampionStats",
                table: "ChampionStats");

            migrationBuilder.AlterColumn<int>(
                name: "ChampionID",
                table: "ChampionStats",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER")
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChampionStats",
                table: "ChampionStats",
                column: "ChampionID");
        }
    }
}
