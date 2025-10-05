using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Weather.Migrations
{
    /// <inheritdoc />
    public partial class RainfallEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProcessedDatas",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SensorSettingId = table.Column<int>(type: "int", nullable: false),
                    LastProcessedId = table.Column<long>(type: "bigint", nullable: true),
                    DataValue = table.Column<double>(type: "float", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessedDatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProcessedDatas_SensorSetting_SensorSettingId",
                        column: x => x.SensorSettingId,
                        principalTable: "SensorSetting",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RainfallEvents",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SensorSettingId = table.Column<int>(type: "int", nullable: false),
                    StationId = table.Column<long>(type: "bigint", nullable: true),
                    StationType = table.Column<int>(type: "int", nullable: true),
                    RainStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RainEnd = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FirstIdWithRain = table.Column<long>(type: "bigint", nullable: true),
                    LastIdWithRain = table.Column<long>(type: "bigint", nullable: true),
                    IsRaining = table.Column<bool>(type: "bit", nullable: false),
                    RainfallVolume = table.Column<double>(type: "float", nullable: false),
                    IsStartSMSSent = table.Column<bool>(type: "bit", nullable: false),
                    IsEndSMSSent = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RainfallEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RainfallEvents_SensorSetting_SensorSettingId",
                        column: x => x.SensorSettingId,
                        principalTable: "SensorSetting",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RainfallEvents_Station_StationId",
                        column: x => x.StationId,
                        principalTable: "Station",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProcessedDatas_SensorSettingId",
                table: "ProcessedDatas",
                column: "SensorSettingId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RainfallEvents_SensorSettingId",
                table: "RainfallEvents",
                column: "SensorSettingId");

            migrationBuilder.CreateIndex(
                name: "IX_RainfallEvents_StationId",
                table: "RainfallEvents",
                column: "StationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProcessedDatas");

            migrationBuilder.DropTable(
                name: "RainfallEvents");
        }
    }
}
