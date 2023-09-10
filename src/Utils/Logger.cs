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
                LogSeverity.Critical => LogLevel.Foreground.Error + LogLevel.Bold + "CRITICAL" + LogLevel.Reset,
                LogSeverity.Error => LogLevel.Foreground.Error + "ERROR" + LogLevel.Reset,
                LogSeverity.Warning => LogLevel.Foreground.Warning + "WARNING" + LogLevel.Reset,
                LogSeverity.Info => LogLevel.Foreground.Info + "INFO" + LogLevel.Reset,
                LogSeverity.Verbose => LogLevel.Foreground.Raw + "RAW" + LogLevel.Reset,
                LogSeverity.Debug => LogLevel.Foreground.Debug + "DEBUG" + LogLevel.Reset,
                _ => throw new ArgumentOutOfRangeException()
            };

            string time = DateTime.Now.ToString("HH:mm:ss");

            Console.WriteLine($"{LogLevel.Dim}{time}{LogLevel.Reset}    [{severity}:{message.Source}]   {message.Message}");

            await Task.CompletedTask;
        }

        private static void Log(string message, string source, string severity = "INFO")
        {
            string time = DateTime.Now.ToString("HH:mm:ss");
            string location = Path.GetFileName(source);

            switch (severity)
            {
                case "ERROR":
                    severity = LogLevel.Foreground.Error + LogLevel.Bold + severity + LogLevel.Reset;
                    break;
                case "WARNING":
                    severity = LogLevel.Foreground.Warning + severity + LogLevel.Reset;
                    break;
                case "INFO":
                    severity = LogLevel.Foreground.Info + severity + LogLevel.Reset;
                    break;
                case "DEBUG":
                    severity = LogLevel.Foreground.Debug + severity + LogLevel.Reset;
                    break;
                case "RAW":
                    severity = LogLevel.Foreground.Raw + severity + LogLevel.Reset;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(severity), severity, null);
            }

            Console.WriteLine($"{LogLevel.Dim}{time}{LogLevel.Reset}    [{severity}:{location}]   {message}");
        }

        public static void Error(string message, string source) => Log(message, source, "ERROR");
        public static void Warn(string message, string source) => Log(message, source, "WARN");
        public static void Info(string message, string source) => Log(message, source, "INFO");
        public static void Debug(string message, string source) => Log(message, source, "DEBUG");
        public static void Raw(string message, string source) => Log(message, source, "RAW");
    }
}
