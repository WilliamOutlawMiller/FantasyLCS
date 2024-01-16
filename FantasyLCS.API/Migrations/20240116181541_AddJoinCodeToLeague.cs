﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FantasyLCS.API.Migrations
{
    /// <inheritdoc />
    public partial class AddJoinCodeToLeague : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JoinCode",
                table: "Leagues",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JoinCode",
                table: "Leagues");
        }
    }
}
