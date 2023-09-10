using System.Reflection;
using Discord;
using Discord.WebSocket;
using MongoDB.Driver;
using Sushi.Utils;

namespace Sushi
{
    internal class Sushi
    {
        public static Task Main(string[] args) => new Sushi().MainAsync();
        
        public async Task MainAsync()
        {
            await Config.LoadConfig();

            GlobalVars.DiscordClient = new DiscordSocketClient(new DiscordSocketConfig
            {
                MessageCacheSize = 100,
                GatewayIntents = GatewayIntents.All,
                LogGatewayIntentWarnings = false,
            });

            GlobalVars.DiscordClient.Log += Logger.ClientLog;

            GlobalVars.DiscordClient.Ready += ClientReady;

            GlobalVars.DatabaseClient = new MongoClient(GlobalVars.Config.MongoSRV);
            Logger.Info("Connected to Database.");

            await GlobalVars.DiscordClient.LoginAsync(TokenType.Bot, GlobalVars.Config.Token);
            await GlobalVars.DiscordClient.StartAsync();

            await Task.Delay(Timeout.Infinite);
        }

        public async Task ClientReady()
        {
            await GlobalVars.DiscordClient.SetGameAsync(
                name: "deez nuts",
                type: ActivityType.Watching
            );
            Logger.Info("Client is ready.");
        }
    }
}