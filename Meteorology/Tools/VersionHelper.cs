using System.Reflection;

namespace Weather.Tools
{
    public static class VersionHelper
    {
        public static string GetVersion()
        {
            return Assembly.GetExecutingAssembly()
                           .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
                           ?.InformationalVersion ?? "Unknown";
        }
    }
}
