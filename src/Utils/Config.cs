using System.Text.Json;

namespace Sushi.Utils
{
    internal class Config
    {
        public static Task LoadConfig()
        {
            string configPath = Path.Combine(Directory.GetCurrentDirectory(), "config.json");
            if (!File.Exists(configPath))
            {
                throw new Exception("Config file not found. Please create a config.json file in the current directory.");
            }

            string configJson = File.ReadAllText(configPath);
            ConfigStruct config = JsonSerializer.Deserialize<ConfigStruct>(configJson) ??
                throw new Exception("Config file is empty.");

            GlobalVars.Config = config;

            return Task.CompletedTask;
        }
    }

    internal class ConfigStruct
    {
        public string Token { get; set; } = "";
        public string OwnerId { get; set; } = "";
        public string Prefix { get; set; } = "";
        public string ClientId { get; set; } = "";
        public string TestGuildId { get; set; } = "";
        public string BugReportChannelId { get; set; } = "";
        public string MongoSRV { get; set; } = "";
    }
}
