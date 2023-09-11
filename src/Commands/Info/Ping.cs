using Discord.Interactions;

namespace Sushi.Commands.Info
{
    public class PingModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("ping", "Get the bot's ping")]
        public async Task HandlePingAsync()
        {
            await DeferAsync();
            await FollowupAsync($"Pong! {GlobalVars.DiscordClient.Latency}ms");
        }
    }
}
