using Discord.Interactions;

namespace Sushi.Commands
{
    public class InteractionModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("ping", "Pong!")]
        public async Task HandlePingAsync()
        {
            await RespondAsync("Pong!");
        }
    }
}
