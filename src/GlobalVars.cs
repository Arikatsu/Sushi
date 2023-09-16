using Sushi.Utils;
using MongoDB.Driver;
using Discord.WebSocket;
using Discord.Interactions;
using Sushi.Services;

namespace Sushi
{
    internal class GlobalVars
    {
#pragma warning disable CS8618
        public static string Version { get; set; } = "3.0.0";
        public static ConfigStruct Config { get; set; }
        public static DiscordSocketClient DiscordClient { get; set; }
        public static MongoClient DatabaseClient { get; set; }
        public static IMongoDatabase Database { get; set; }
        public static IServiceProvider Services { get; set; }
        public static InteractionService InteractionCommands { get; set; }
        public static InteractionHandler InteractionHandler { get; set; }
#pragma warning restore CS8618
    }
}
