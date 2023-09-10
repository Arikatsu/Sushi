using Discord;
using Discord.WebSocket;
using Discord.Interactions;
using MongoDB.Driver;
using Sushi.Utils;
using Sushi.Services;

namespace Sushi
{
    internal class Sushi
    {        
        public static Task Main() => new Sushi().MainAsync();
        
        public async Task MainAsync()
        {
            await Config.LoadConfig();

            GlobalVars.DiscordClient = new DiscordSocketClient(new DiscordSocketConfig
            {
                MessageCacheSize = 100,
                GatewayIntents = GatewayIntents.All,
                LogGatewayIntentWarnings = false,
            });

            GlobalVars.DatabaseClient = new MongoClient(GlobalVars.Config.MongoSRV);
            Logger.Info("Connected to Database.");

            GlobalVars.InteractionCommands = new InteractionService(GlobalVars.DiscordClient, new InteractionServiceConfig
            {
                ThrowOnError = false,
            });

            GlobalVars.InteractionHandler = new InteractionHandler(GlobalVars.DiscordClient, GlobalVars.InteractionCommands, GlobalVars.Services);
            await GlobalVars.InteractionHandler.InitializeAsync();

            GlobalVars.DiscordClient.Log += Logger.ClientLog;
            GlobalVars.InteractionCommands.Log += Logger.ClientLog;
            
            GlobalVars.DiscordClient.Ready += ClientReady;

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
            
#if DEBUG
            await GlobalVars.InteractionCommands.RegisterCommandsToGuildAsync(ulong.Parse(GlobalVars.Config.TestGuildId));
            Logger.Info($"Registered {GlobalVars.InteractionCommands.Modules.Count} commands to test guild.");
#else
            await GlobalVars.InteractionCommands.RegisterCommandsGloballyAsync();
            Logger.Info($"Registered {GlobalVars.InteractionCommands.Modules.Count} commands globally.");
#endif

            Logger.Info("Client is ready.");
        }
    }
}