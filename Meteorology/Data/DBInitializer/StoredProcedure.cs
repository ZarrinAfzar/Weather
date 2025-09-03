using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Weather.Data.DBInitializer
{
    public partial class StoredProcedure : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var SensorData_V = @"CREATE VIEW[dbo].[SensorData] AS
                       SELECT          dbo.SensorDateTime.Data, dbo.SensorDateTime.DateTime, dbo.SensorType.FaName, dbo.SensorSetting.StationId
                       FROM            dbo.SensorDateTime INNER JOIN
                                       dbo.SensorSetting ON dbo.SensorDateTime.SensorSettingId = dbo.SensorSetting.Id INNER JOIN
                                       dbo.SensorType ON dbo.SensorSetting.SensorTypeId = dbo.SensorType.Id";
            var SensorData_SP = @"SET ANSI_NULLS ON
                                  GO
                                  
                                  SET QUOTED_IDENTIFIER ON
                                  GO
                                  
                                  CREATE PROCEDURE [dbo].[Sp_SensorData]
                                  @FromDate varchar(MAX),
                                  @ToDate varchar(MAX),
                                  @StationId varchar(MAX)
                                  AS
                                  DECLARE @cols AS NVARCHAR(MAX),@query  AS NVARCHAR(MAX);
                                  
                                  SET @cols = STUFF((SELECT distinct ',' + QUOTENAME(c.FaName) 
                                              FROM SensorData c
                                              FOR XML PATH(''), TYPE
                                              ).value('.', 'NVARCHAR(MAX)') 
                                          ,1,1,'')
                                  
                                  set @query = 'Select DateTime, ' + @cols + ' from (SELECT * from  SensorData   where StationId = '''+@StationId+''' And [DateTime] BETWEEN  '''+@FromDate+''' And '''+@ToDate+''' )  a
                                              pivot 
                                              (
                                                   max(Data)
                                                  for FaName in (' + @cols + ')
                                              ) p '
                                  execute(@query)  
                                  GO";
            migrationBuilder.Sql(SensorData_V);
            migrationBuilder.Sql(SensorData_SP);
            
        }
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
