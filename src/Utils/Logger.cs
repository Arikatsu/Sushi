using Discord;

namespace Sushi.Utils
{
    internal class Logger
    {
        private struct LogLevel
        {
            public const string Reset = "\x1b[0m";
            public const string Bold = "\x1b[1m";
            public const string Dim = "\x1b[2m";

            public struct Foreground
            {
                public const string Error = "\x1b[31m";
                public const string Debug = "\x1b[32m";
                public const string Warning = "\x1b[33m";
                public const string Info = "\x1b[34m";
                public const string Raw = "\x1b[37m";
            }

        }

        public static async Task ClientLog(LogMessage message)
        {
            var severity = message.Severity switch
            {
                LogSeverity.Critical => LogLevel.Foreground.Error + LogLevel.Bold + "CTL" + LogLevel.Reset,
                LogSeverity.Error => LogLevel.Foreground.Error + "ERR" + LogLevel.Reset,
                LogSeverity.Warning => LogLevel.Foreground.Warning + "WRN" + LogLevel.Reset,
                LogSeverity.Info => LogLevel.Foreground.Info + "INF" + LogLevel.Reset,
                LogSeverity.Verbose => LogLevel.Foreground.Raw + "RAW" + LogLevel.Reset,
                LogSeverity.Debug => LogLevel.Foreground.Debug + "DBG" + LogLevel.Reset,
                _ => throw new ArgumentOutOfRangeException()
            };

            string time = DateTime.Now.ToString("HH:mm:ss");

            Console.WriteLine($"{LogLevel.Dim}{time}{LogLevel.Reset}    [{severity}]   {message.Message + message.Exception}");

            await Task.CompletedTask;
        }

        private static void Log(string message, string severity = "INFO")
        {
            string time = DateTime.Now.ToString("HH:mm:ss");

            switch (severity)
            {
                case "ERR":
                    severity = LogLevel.Foreground.Error + severity + LogLevel.Reset;
                    break;
                case "WRN":
                    severity = LogLevel.Foreground.Warning + severity + LogLevel.Reset;
                    break;
                case "INF":
                    severity = LogLevel.Foreground.Info + severity + LogLevel.Reset;
                    break;
                case "DBG":
                    severity = LogLevel.Foreground.Debug + severity + LogLevel.Reset;
                    break;
                case "RAW":
                    severity = LogLevel.Foreground.Raw + severity + LogLevel.Reset;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(severity), severity, null);
            }

            Console.WriteLine($"{LogLevel.Dim}{time}{LogLevel.Reset}    [{severity}]   {message}");
        }

        public static void Error(string message) => Log(message, "ERR");
        public static void Warn(string message) => Log(message, "WRN");
        public static void Info(string message) => Log(message, "INF");
        public static void Debug(string message) => Log(message, "DBG");
        public static void Raw(string message) => Log(message, "RAW");
    }
}
