using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Weather.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Weather.Data
{
    public class DataBaseContext : IdentityDbContext<User, Role, long>
    {
        public DataBaseContext(DbContextOptions<DataBaseContext> options)
            : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<SensorDateTime>().HasIndex(b => b.DateTime);

            base.OnModelCreating(modelBuilder); 
        }

        public void OpenConnection()
        {
            Database.OpenConnection();
        }

        public DbCommand Command()
        {
            DbCommand cmd = Database.GetDbConnection().CreateCommand();
            return cmd;
        }
        public DbSet<City> City { get; set; }
        public DbSet<Correspondence> Correspondence { get; set; }
        public DbSet<DataLogger> DataLogger { get; set; }
        public DbSet<Files> Files { get; set; }
        public DbSet<ModemType> ModemType { get; set; }
        public DbSet<Notification> Notification { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<SmsCharge> SmsCharge { get; set; }
        public DbSet<State> State { get; set; } 
        public DbSet<Station> Station { get; set; }
        public DbSet<StationFile> StationFile { get; set; }
        public DbSet<StationTel> StationTel { get; set; }
        public DbSet<ManagerTel> ManagerTel { get; set; }
        public DbSet<StationType> StationType { get; set; }
        public DbSet<UserStation> UserStation { get; set; }
        public DbSet<UserAction> UserAction { get; set; }
        //-----------------------------------------------------
        public DbSet<Alarm> Alarm { get; set; } 
        public DbSet<AlarmLog> AlarmLog { get; set; } 
        public DbSet<AlarmTell> AlarmTell { get; set; }
        public DbSet<ForecastsAlarmDetail> ForecastsAlarmDetail { get; set; }
        public DbSet<ForecastsAlarmParameter> ForecastsAlarmParameter { get; set; }
        public DbSet<ForecastsLog> ForecastsLog { get; set; } 
        public DbSet<PestAlarmDetail> PestAlarmDetails { get; set; }
        public DbSet<SensorAlarmDetail> SensorAlarmDetail { get; set; }
        //public DbSet<SensorData> SensorData { get; set; }
        public DbSet<SensorDateTime> SensorDateTime { get; set; }
        public DbSet<SendSMS> SendSMS { get; set; }
        public DbSet<SensorSetting> SensorSetting { get; set; }
        public DbSet<SensorType> SensorType { get; set; }
        public DbSet<SensorAlarmDetail> SensorAlarmDetails { get; set; } 
        public DbSet<UserLoginHistory> UserLoginHistory { get; set; }
        public DbSet<Unit> Unit { get; set; }
        public DbSet<ReceivedSMS> ReceivedSMS { get; set; }
        public DbSet<VirtualSensorBase> VirtualSensorBase { get; set; }
        public DbSet<VirtualSensorDetail> VirtualSensorDetail { get; set; }
       
    }
    #region IDesignTimeDbContextFactory
    //public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<DataBaseContext>
    //{
    //    public DataBaseContext CreateDbContext(string[] args)
    //    {
    //        IConfigurationRoot configuration = new ConfigurationBuilder()
    //            .SetBasePath(Directory.GetCurrentDirectory())
    //            .AddJsonFile("appsettings.json")
    //            .Build();
    //        var builder = new DbContextOptionsBuilder<DataBaseContext>();
    //        var connectionString = configuration.GetConnectionString("DefaultConnection");
    //        builder.UseSqlServer(connectionString);
    //        return new DataBaseContext(builder.Options);
    //    }
    //}
    #endregion

}
