using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Weather.Migrations
{
    public partial class ChangeSensorSettingIdToInt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1. ایجاد جدول موقت با Id از نوع int و ویژگی identity
            migrationBuilder.CreateTable(
                name: "SensorSetting_Temp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StationId = table.Column<long>(type: "bigint", nullable: false),
                    SensorEnable = table.Column<bool>(type: "bit", nullable: false),
                    SensorType = table.Column<int>(type: "int", nullable: false),
                    SensorRow = table.Column<int>(type: "int", nullable: false),
                    SensorTypeId = table.Column<long>(type: "bigint", nullable: true),
                    SensorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitId = table.Column<long>(type: "bigint", nullable: true),
                    SensorDigit = table.Column<int>(type: "int", nullable: true),
                    SensorMin = table.Column<double>(type: "float", nullable: true),
                    SensorMax = table.Column<double>(type: "float", nullable: true),
                    SensorSerial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SensorCompany = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SensorTecnicalType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SensorDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorSetting_Temp", x => x.Id);
                });

            // 2. انتقال داده‌ها از جدول اصلی به جدول موقت
            migrationBuilder.Sql(@"
                SET IDENTITY_INSERT SensorSetting_Temp ON;
                INSERT INTO SensorSetting_Temp (Id, StationId, SensorEnable, SensorType, SensorRow, SensorTypeId, SensorName, UnitId, SensorDigit, SensorMin, SensorMax, SensorSerial, SensorCompany, SensorTecnicalType, SensorDateTime)
                SELECT CAST(Id AS INT), StationId, SensorEnable, SensorType, SensorRow, SensorTypeId, SensorName, UnitId, SensorDigit, SensorMin, SensorMax, SensorSerial, SensorCompany, SensorTecnicalType, SensorDateTime
                FROM SensorSetting;
                SET IDENTITY_INSERT SensorSetting_Temp OFF;
            ");

            // 3. حذف جدول اصلی
            migrationBuilder.DropTable(name: "SensorSetting");

            // 4. تغییر نام جدول موقت به نام اصلی
            migrationBuilder.RenameTable(
                name: "SensorSetting_Temp",
                newName: "SensorSetting");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // 1. ایجاد جدول بازیابی با Id از نوع long
            migrationBuilder.CreateTable(
                name: "SensorSetting_Restore",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StationId = table.Column<long>(type: "bigint", nullable: false),
                    SensorEnable = table.Column<bool>(type: "bit", nullable: false),
                    SensorType = table.Column<int>(type: "int", nullable: false),
                    SensorRow = table.Column<int>(type: "int", nullable: false),
                    SensorTypeId = table.Column<long>(type: "bigint", nullable: true),
                    SensorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnitId = table.Column<long>(type: "bigint", nullable: true),
                    SensorDigit = table.Column<int>(type: "int", nullable: true),
                    SensorMin = table.Column<double>(type: "float", nullable: true),
                    SensorMax = table.Column<double>(type: "float", nullable: true),
                    SensorSerial = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SensorCompany = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SensorTecnicalType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SensorDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SensorSetting_Restore", x => x.Id);
                });

            // 2. انتقال داده‌ها از جدول فعلی به جدول بازیابی
            migrationBuilder.Sql(@"
                SET IDENTITY_INSERT SensorSetting_Restore ON;
                INSERT INTO SensorSetting_Restore (Id, StationId, SensorEnable, SensorType, SensorRow, SensorTypeId, SensorName, UnitId, SensorDigit, SensorMin, SensorMax, SensorSerial, SensorCompany, SensorTecnicalType, SensorDateTime)
                SELECT CAST(Id AS BIGINT), StationId, SensorEnable, SensorType, SensorRow, SensorTypeId, SensorName, UnitId, SensorDigit, SensorMin, SensorMax, SensorSerial, SensorCompany, SensorTecnicalType, SensorDateTime
                FROM SensorSetting;
                SET IDENTITY_INSERT SensorSetting_Restore OFF;
            ");

            // 3. حذف جدول فعلی
            migrationBuilder.DropTable(name: "SensorSetting");

            // 4. تغییر نام جدول بازیابی به نام اصلی
            migrationBuilder.RenameTable(
                name: "SensorSetting_Restore",
                newName: "SensorSetting");
        }
    }
}
