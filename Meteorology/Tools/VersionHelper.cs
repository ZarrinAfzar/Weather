using System;
using System.IO;
using Newtonsoft.Json;
using Weather.Models;
using System.Globalization;
using Weather.Data;

public static class VersionHelper
{
    private static string versionFile = Path.Combine(AppContext.BaseDirectory, "version.json");

    public static string GetVersion()
    {
        if (File.Exists(versionFile))
        {
            var json = File.ReadAllText(versionFile);
            dynamic obj = JsonConvert.DeserializeObject(json);
            return (string)obj.version;
        }

        return "Unknown";
    }

    public static (string Version, DateTime ReleaseDate) GetVersionInfo()
    {
        if (File.Exists(versionFile))
        {
            var json = File.ReadAllText(versionFile);
            dynamic obj = JsonConvert.DeserializeObject(json);
            string version = obj.version;
            DateTime releaseDate = DateTime.ParseExact((string)obj.ReleaseDate, "yyyy/MM/dd", CultureInfo.InvariantCulture);
            return (version, releaseDate);
        }

        return ("Unknown", DateTime.MinValue);
    }

    public static void PublishVersion(DataBaseContext db, string newVersion, string notes = "")
    {
        var versionEntry = new VersionHistory
        {
            Version = newVersion,
            ReleaseDate = DateTime.UtcNow,
            Notes = notes
        };

        db.VersionHistories.Add(versionEntry);
        db.SaveChanges();

        var jsonObj = new
        {
            version = newVersion,
            ReleaseDate = versionEntry.ReleaseDate.ToString("yyyy/MM/dd")
        };

        var json = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
        File.WriteAllText(versionFile, json);
    }
}
