using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FantasyLCS.API.Migrations
{
    /// <inheritdoc />
    public partial class AddScores : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "FinalScore",
                table: "Scores",
                type: "REAL",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddColumn<double>(
                name: "BountyCollectedScore",
                table: "Scores",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "BountyLostScore",
                table: "Scores",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CCInstancesScore",
                table: "Scores",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CCTimeScore",
                table: "Scores",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "CSD15Score",
                table: "Scores",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DPMScore",
                table: "Scores",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DamageTakenScore",
                table: "Scores",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "DoubleKillScore",
                table: "Scores",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "GD15Score",
                table: "Scores",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "KDAScore",
                table: "Scores",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "KPScore",
                table: "Scores",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "MatchID",
                table: "Scores",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "ObjectiveStealScore",
                table: "Scores",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "PentaKillScore",
                table: "Scores",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "QuadraKillScore",
                table: "Scores",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "SoloKillScore",
                table: "Scores",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TeamHealingScore",
                table: "Scores",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TeamShieldingScore",
                table: "Scores",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TripleKillScore",
                table: "Scores",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TurretDamageScore",
                table: "Scores",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "VSPMScore",
                table: "Scores",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BountyCollectedScore",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "BountyLostScore",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "CCInstancesScore",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "CCTimeScore",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "CSD15Score",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "DPMScore",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "DamageTakenScore",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "DoubleKillScore",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "GD15Score",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "KDAScore",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "KPScore",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "MatchID",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "ObjectiveStealScore",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "PentaKillScore",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "QuadraKillScore",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "SoloKillScore",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "TeamHealingScore",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "TeamShieldingScore",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "TripleKillScore",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "TurretDamageScore",
                table: "Scores");

            migrationBuilder.DropColumn(
                name: "VSPMScore",
                table: "Scores");

            migrationBuilder.AlterColumn<int>(
                name: "FinalScore",
                table: "Scores",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "REAL");
        }
    }
}
