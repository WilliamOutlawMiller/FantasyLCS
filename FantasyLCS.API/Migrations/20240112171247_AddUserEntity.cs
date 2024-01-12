using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FantasyLCS.API.Migrations
{
    /// <inheritdoc />
    public partial class AddUserEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataUpdateLogs",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    DataType = table.Column<string>(type: "TEXT", nullable: false),
                    LastUpdated = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataUpdateLogs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Matches",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Matches", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    TeamID = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    OwnerName = table.Column<string>(type: "TEXT", nullable: false),
                    LogoUrl = table.Column<string>(type: "TEXT", nullable: false),
                    LogoPath = table.Column<string>(type: "TEXT", nullable: false),
                    Wins = table.Column<int>(type: "INTEGER", nullable: false),
                    Losses = table.Column<int>(type: "INTEGER", nullable: false),
                    PlayerIDs = table.Column<string>(type: "TEXT", nullable: false),
                    SubIDs = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "FullStats",
                columns: table => new
                {
                    ID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayerID = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Champion = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<string>(type: "TEXT", nullable: false),
                    Level = table.Column<string>(type: "TEXT", nullable: false),
                    Kills = table.Column<string>(type: "TEXT", nullable: false),
                    Deaths = table.Column<string>(type: "TEXT", nullable: false),
                    Assists = table.Column<string>(type: "TEXT", nullable: false),
                    KDA = table.Column<string>(type: "TEXT", nullable: false),
                    CS = table.Column<string>(type: "TEXT", nullable: false),
                    CSTeamJG = table.Column<string>(type: "TEXT", nullable: false),
                    CSEnemyJG = table.Column<string>(type: "TEXT", nullable: false),
                    CSM = table.Column<string>(type: "TEXT", nullable: false),
                    Gold = table.Column<string>(type: "TEXT", nullable: false),
                    GPM = table.Column<string>(type: "TEXT", nullable: false),
                    GoldPercent = table.Column<string>(type: "TEXT", nullable: false),
                    VisionScore = table.Column<string>(type: "TEXT", nullable: false),
                    WardsPlaced = table.Column<string>(type: "TEXT", nullable: false),
                    WardsDestroyed = table.Column<string>(type: "TEXT", nullable: false),
                    ControlWardsPurchased = table.Column<string>(type: "TEXT", nullable: false),
                    ControlWardsPlaced = table.Column<string>(type: "TEXT", nullable: false),
                    VisionScorePerMinute = table.Column<string>(type: "TEXT", nullable: false),
                    WardsPerMinute = table.Column<string>(type: "TEXT", nullable: false),
                    ControlWardsPerMinute = table.Column<string>(type: "TEXT", nullable: false),
                    WardsKilledPerMinute = table.Column<string>(type: "TEXT", nullable: false),
                    VisionScorePercent = table.Column<string>(type: "TEXT", nullable: false),
                    TotalDamageToChampions = table.Column<string>(type: "TEXT", nullable: false),
                    PhysicalDamage = table.Column<string>(type: "TEXT", nullable: false),
                    MagicDamage = table.Column<string>(type: "TEXT", nullable: false),
                    TrueDamage = table.Column<string>(type: "TEXT", nullable: false),
                    DPM = table.Column<string>(type: "TEXT", nullable: false),
                    DamagePercent = table.Column<string>(type: "TEXT", nullable: false),
                    KillsPlusAssistsPerMinute = table.Column<string>(type: "TEXT", nullable: false),
                    KillPercent = table.Column<string>(type: "TEXT", nullable: false),
                    SoloKills = table.Column<string>(type: "TEXT", nullable: false),
                    DoubleKills = table.Column<string>(type: "TEXT", nullable: false),
                    TripleKills = table.Column<string>(type: "TEXT", nullable: false),
                    QuadraKills = table.Column<string>(type: "TEXT", nullable: false),
                    PentaKills = table.Column<string>(type: "TEXT", nullable: false),
                    GD15 = table.Column<string>(type: "TEXT", nullable: false),
                    CSD15 = table.Column<string>(type: "TEXT", nullable: false),
                    XPD15 = table.Column<string>(type: "TEXT", nullable: false),
                    LVLD15 = table.Column<string>(type: "TEXT", nullable: false),
                    ObjectivesStolen = table.Column<string>(type: "TEXT", nullable: false),
                    DmgToTurrets = table.Column<string>(type: "TEXT", nullable: false),
                    DmgToBuildings = table.Column<string>(type: "TEXT", nullable: false),
                    TotalHeal = table.Column<string>(type: "TEXT", nullable: false),
                    TotalHealOnTeammates = table.Column<string>(type: "TEXT", nullable: false),
                    DamageSelfMitigated = table.Column<string>(type: "TEXT", nullable: false),
                    TotalDamageShieldedOnTeammates = table.Column<string>(type: "TEXT", nullable: false),
                    TimeCCingOthers = table.Column<string>(type: "TEXT", nullable: false),
                    TotalTimeCCDealt = table.Column<string>(type: "TEXT", nullable: false),
                    TotalDamageTaken = table.Column<string>(type: "TEXT", nullable: false),
                    TotalTimeSpentDead = table.Column<string>(type: "TEXT", nullable: false),
                    ConsumablesPurchased = table.Column<string>(type: "TEXT", nullable: false),
                    ItemsPurchased = table.Column<string>(type: "TEXT", nullable: false),
                    ShutdownBountyCollected = table.Column<string>(type: "TEXT", nullable: false),
                    ShutdownBountyLost = table.Column<string>(type: "TEXT", nullable: false),
                    MatchID = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FullStats", x => x.ID);
                    table.ForeignKey(
                        name: "FK_FullStats_Matches_MatchID",
                        column: x => x.MatchID,
                        principalTable: "Matches",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "AggressionStats",
                columns: table => new
                {
                    PlayerID = table.Column<int>(type: "INTEGER", nullable: false),
                    DPM = table.Column<string>(type: "TEXT", nullable: false),
                    DamagePercent = table.Column<string>(type: "TEXT", nullable: false),
                    KAPerMinute = table.Column<string>(type: "TEXT", nullable: false),
                    SoloKills = table.Column<string>(type: "TEXT", nullable: false),
                    Pentakills = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AggressionStats", x => x.PlayerID);
                    table.ForeignKey(
                        name: "FK_AggressionStats_Players_PlayerID",
                        column: x => x.PlayerID,
                        principalTable: "Players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ChampionStats",
                columns: table => new
                {
                    ChampionID = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayerID = table.Column<int>(type: "INTEGER", nullable: false),
                    Champion = table.Column<string>(type: "TEXT", nullable: false),
                    GamesPlayed = table.Column<string>(type: "TEXT", nullable: false),
                    WinRate = table.Column<string>(type: "TEXT", nullable: false),
                    KDA = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChampionStats", x => x.ChampionID);
                    table.ForeignKey(
                        name: "FK_ChampionStats_Players_PlayerID",
                        column: x => x.PlayerID,
                        principalTable: "Players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EarlyGameStats",
                columns: table => new
                {
                    PlayerID = table.Column<int>(type: "INTEGER", nullable: false),
                    AheadInCSAt15Percent = table.Column<string>(type: "TEXT", nullable: false),
                    CSD15 = table.Column<string>(type: "TEXT", nullable: false),
                    GD15 = table.Column<string>(type: "TEXT", nullable: false),
                    XPD15 = table.Column<string>(type: "TEXT", nullable: false),
                    FirstBloodParticipantPercent = table.Column<string>(type: "TEXT", nullable: false),
                    FirstBloodVictimPercent = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EarlyGameStats", x => x.PlayerID);
                    table.ForeignKey(
                        name: "FK_EarlyGameStats_Players_PlayerID",
                        column: x => x.PlayerID,
                        principalTable: "Players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GeneralStats",
                columns: table => new
                {
                    PlayerID = table.Column<int>(type: "INTEGER", nullable: false),
                    Record = table.Column<string>(type: "TEXT", nullable: false),
                    WinRate = table.Column<string>(type: "TEXT", nullable: false),
                    KDA = table.Column<string>(type: "TEXT", nullable: false),
                    CsPerMin = table.Column<string>(type: "TEXT", nullable: false),
                    GoldPerMin = table.Column<string>(type: "TEXT", nullable: false),
                    GoldPercent = table.Column<string>(type: "TEXT", nullable: false),
                    KillParticipation = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneralStats", x => x.PlayerID);
                    table.ForeignKey(
                        name: "FK_GeneralStats_Players_PlayerID",
                        column: x => x.PlayerID,
                        principalTable: "Players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VisionStats",
                columns: table => new
                {
                    PlayerID = table.Column<int>(type: "INTEGER", nullable: false),
                    VSPM = table.Column<string>(type: "TEXT", nullable: false),
                    WPM = table.Column<string>(type: "TEXT", nullable: false),
                    VWPM = table.Column<string>(type: "TEXT", nullable: false),
                    WCPM = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisionStats", x => x.PlayerID);
                    table.ForeignKey(
                        name: "FK_VisionStats_Players_PlayerID",
                        column: x => x.PlayerID,
                        principalTable: "Players",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChampionStats_PlayerID",
                table: "ChampionStats",
                column: "PlayerID");

            migrationBuilder.CreateIndex(
                name: "IX_FullStats_MatchID",
                table: "FullStats",
                column: "MatchID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AggressionStats");

            migrationBuilder.DropTable(
                name: "ChampionStats");

            migrationBuilder.DropTable(
                name: "DataUpdateLogs");

            migrationBuilder.DropTable(
                name: "EarlyGameStats");

            migrationBuilder.DropTable(
                name: "FullStats");

            migrationBuilder.DropTable(
                name: "GeneralStats");

            migrationBuilder.DropTable(
                name: "Teams");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "VisionStats");

            migrationBuilder.DropTable(
                name: "Matches");

            migrationBuilder.DropTable(
                name: "Players");
        }
    }
}
