using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FantasyLCS.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserNullableTeamAndLeague : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TeamID",
                table: "Users",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserIDs",
                table: "Leagues",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Users_TeamID",
                table: "Users",
                column: "TeamID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Teams_TeamID",
                table: "Users",
                column: "TeamID",
                principalTable: "Teams",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Teams_TeamID",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_TeamID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TeamID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "UserIDs",
                table: "Leagues");
        }
    }
}
