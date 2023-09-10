using Sushi.Utils;
using MongoDB.Driver;
using Discord.WebSocket;

namespace Sushi
{
    internal class GlobalVars
    {
#pragma warning disable CS8618

        public static ConfigStruct Config { get; set; }
        public static DiscordSocketClient DiscordClient { get; set; }
        public static MongoClient DatabaseClient { get; set; }
#pragma warning restore CS8618
    }
}
