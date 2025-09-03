using System;
using System.Linq;
using System.Reflection;
using Weather.Data;
using Weather.Models;

namespace Weather.Tools
{
    public static class RecordCurrentVersion
    {
        public static void Execute(DataBaseContext context)
        {
            // نسخه پروژه از Assembly
            var projectVersion = Assembly.GetExecutingAssembly()
                                         .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                                         ?.InformationalVersion ?? "1.0.0";

            // آخرین نسخه ثبت شده در دیتابیس
            var lastVersion = context.VersionHistories
                                     .OrderByDescending(v => v.Id)
                                     .FirstOrDefault()?.Version ?? "0.0.0";

            if (projectVersion != lastVersion)
            {
                // نسخه جدید را ثبت می‌کنیم
                context.VersionHistories.Add(new VersionHistory
                {
                    Version = projectVersion,
                    ReleaseDate = DateTime.UtcNow
                });
                context.SaveChanges();
            }
        }
    }
}
