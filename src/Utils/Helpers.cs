using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Sushi.Utils
{
    internal class Helpers
    {
        public static string ConvertUpperCamelCaseToWords(string input)
        {
            return Regex.Replace(input, "([A-Z])", " $1", RegexOptions.Compiled).Trim();
        }

        public static string GetUptime()
        {
            TimeSpan uptime = DateTime.Now - Process.GetCurrentProcess().StartTime;
            return $"{uptime.Days} days, {uptime.Hours} hours, {uptime.Minutes} minutes, {uptime.Seconds} seconds";
        }

        public static string GetDiscordTimestamp(DateTimeOffset dateTimeOffset)
        {
            return $"<t:{dateTimeOffset.ToUnixTimeSeconds()}:F>";
        }
    }
}
