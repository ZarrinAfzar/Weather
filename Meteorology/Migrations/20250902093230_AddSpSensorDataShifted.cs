using Microsoft.EntityFrameworkCore.Migrations;

namespace Weather.Migrations
{
    public partial class AddSpSensorDataShifted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sp = @"
CREATE PROCEDURE [dbo].[Sp_SensorData_Shifted]
    @FromDate   DATETIME,
    @ToDate     DATETIME,
    @StationId  INT,
    @DateType   NVARCHAR(20),   -- انتظار: 'DAY'
    @Fields     NVARCHAR(MAX),  -- دقیقا همان رشته‌ای که getField می‌سازد
    @Take       INT = 500,
    @Skip       INT = 0
AS
BEGIN
    SET NOCOUNT ON;

    IF UPPER(@DateType) <> 'DAY'
    BEGIN
        RAISERROR(N'This procedure only supports @DateType = DAY.', 16, 1);
        RETURN;
    END;

    DECLARE @sql NVARCHAR(MAX) = N'
    SELECT 
        CAST(
            DATEADD(
                DAY, 1,
                DATEADD(DAY, DATEDIFF(DAY, 0, DATEADD(MINUTE, -30, DATEADD(HOUR, -18, sdt.[DateTime]))), 0)
            )
        AS DATETIME) AS [DateTime]' + ISNULL(@Fields, N'') + N'
    FROM [WeatherDB].[dbo].[SensorDateTime] sdt
    INNER JOIN [WeatherDB].[dbo].[SensorSetting] SensorSetting
        ON sdt.SensorSettingId = SensorSetting.Id
    WHERE 
        SensorSetting.StationId = @StationId
        AND sdt.[DateTime] BETWEEN @FromDate AND @ToDate
    GROUP BY 
        CAST(
            DATEADD(
                DAY, 1,
                DATEADD(DAY, DATEDIFF(DAY, 0, DATEADD(MINUTE, -30, DATEADD(HOUR, -18, sdt.[DateTime]))), 0)
            )
        AS DATETIME)
    ORDER BY [DateTime]
    OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY;
    ';

    EXEC sp_executesql
        @sql,
        N'@FromDate DATETIME, @ToDate DATETIME, @StationId INT, @Skip INT, @Take INT',
        @FromDate = @FromDate,
        @ToDate   = @ToDate,
        @StationId= @StationId,
        @Skip     = @Skip,
        @Take     = @Take;
END
";
            migrationBuilder.Sql(sp);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS [dbo].[Sp_SensorData_Shifted]");
        }
    }
}
