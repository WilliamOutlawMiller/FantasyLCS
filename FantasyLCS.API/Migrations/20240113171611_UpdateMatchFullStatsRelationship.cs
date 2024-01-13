using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FantasyLCS.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMatchFullStatsRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FullStats_Matches_MatchID",
                table: "FullStats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FullStats",
                table: "FullStats");

            migrationBuilder.DropIndex(
                name: "IX_FullStats_MatchID",
                table: "FullStats");

            migrationBuilder.DropColumn(
                name: "ID",
                table: "FullStats");

            migrationBuilder.AlterColumn<int>(
                name: "MatchID",
                table: "FullStats",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FullStats",
                table: "FullStats",
                columns: new[] { "MatchID", "PlayerID" });

            migrationBuilder.AddForeignKey(
                name: "FK_FullStats_Matches_MatchID",
                table: "FullStats",
                column: "MatchID",
                principalTable: "Matches",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FullStats_Matches_MatchID",
                table: "FullStats");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FullStats",
                table: "FullStats");

            migrationBuilder.AlterColumn<int>(
                name: "MatchID",
                table: "FullStats",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<int>(
                name: "ID",
                table: "FullStats",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0)
                .Annotation("Sqlite:Autoincrement", true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FullStats",
                table: "FullStats",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_FullStats_MatchID",
                table: "FullStats",
                column: "MatchID");

            migrationBuilder.AddForeignKey(
                name: "FK_FullStats_Matches_MatchID",
                table: "FullStats",
                column: "MatchID",
                principalTable: "Matches",
                principalColumn: "ID");
        }
    }
}
