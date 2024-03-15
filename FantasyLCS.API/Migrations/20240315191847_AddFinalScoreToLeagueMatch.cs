﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FantasyLCS.API.Migrations
{
    /// <inheritdoc />
    public partial class AddFinalScoreToLeagueMatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TeamOneFinalScore",
                table: "LeagueMatches",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TeamTwoFinalScore",
                table: "LeagueMatches",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TeamOneFinalScore",
                table: "LeagueMatches");

            migrationBuilder.DropColumn(
                name: "TeamTwoFinalScore",
                table: "LeagueMatches");
        }
    }
}
