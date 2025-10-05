using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using Weather.Data;
using Weather.Data.Base;
using Weather.Data.Handlers;
using Weather.Data.Interface;
using Weather.Data.Repository;
using Weather.Data.UnitOfWork;
using Weather.Models;
using Weather.Services;
using Weather.Tools;
using wrmsms;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;
var env = builder.Environment;

// ------------------------
// Database Configuration
// ------------------------
builder.Services.AddDbContext<DataBaseContext>(options =>
{
    options.UseSqlServer(config.GetConnectionString("DefaultConnection"));

#if DEBUG
    options.EnableSensitiveDataLogging(); // نمایش جزئیات خطا در محیط توسعه
    options.LogTo(Console.WriteLine, LogLevel.Information);
#endif
});

// ------------------------
// Identity Configuration
// ------------------------
builder.Services.AddIdentity<User, Role>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<DataBaseContext>()
.AddDefaultTokenProviders()
.AddErrorDescriber<CustomIdentityErrorDescriber>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.LoginPath = "/Account/Login";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.SlidingExpiration = true;
});

// ------------------------
// Dependency Injection Setup
// ------------------------

// EF و Repository
builder.Services.AddScoped<DataBaseContext>();
builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IGenericUoW, GenericUoW>();
builder.Services.AddScoped<GenericUoW>();

// Handlers & Services
builder.Services.AddScoped<ManageSMS>();
builder.Services.AddScoped<RainfallEventHandler>();
builder.Services.AddScoped<SensorProcessor>();

// Background Services
builder.Services.AddHostedService<CheckSensorDataBackgroundService>();

// External Web Service (SMS)
builder.Services.AddSingleton<SMSwsdlPortType, SMSwsdlPortTypeClient>();

// MVC / Razor
builder.Services.AddControllersWithViews();

// ------------------------
// Build the App
// ------------------------
var app = builder.Build();

// ------------------------
// Middleware Pipeline
// ------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// ------------------------
// Routing
// ------------------------
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// ------------------------
// Automatic Migration
// ------------------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DataBaseContext>();
    db.Database.Migrate();
}

// ------------------------
// Run Application
// ------------------------
app.Run();
