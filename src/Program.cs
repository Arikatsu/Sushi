using System.Reflection;
using Discord;
using Discord.WebSocket;
using MongoDB.Driver;
using Sushi.Utils;

namespace Sushi
{
    internal class Sushi
    {
        private DiscordSocketClient? _client;
        private MongoClient? _mongoClient;

        public static Task Main(string[] args) => new Sushi().MainAsync();

        public async Task MainAsync()
        {
            await Config.LoadConfig();

            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                MessageCacheSize = 100,
                GatewayIntents = GatewayIntents.All,
                LogGatewayIntentWarnings = false,
            });
            
            _client.Log += Logger.ClientLog;

            _mongoClient = new MongoClient(GlobalVars.Config.MongoSRV);

            GlobalVars.Database = _mongoClient.GetDatabase("Sushi");
            Logger.Info("Connected to MongoDB.", Assembly.GetExecutingAssembly().Location);

            await _client.LoginAsync(TokenType.Bot, GlobalVars.Config.Token);
            await _client.StartAsync();

            await Task.Delay(Timeout.Infinite);
        }
    }
}