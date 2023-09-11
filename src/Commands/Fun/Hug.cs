using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;

namespace Sushi.Commands.Fun
{
    public class HugModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("hug", "Hug someone in the server.")]
        public async Task HandleHugAsync([Summary("user", "The user to hug.")] SocketUser user)
        {
            await DeferAsync();
            
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://api.nekos.fun:8080/api/hug");
            var content = await response.Content.ReadAsStringAsync();

            JObject hugJson = JObject.Parse(content);

            Embed embed = new EmbedBuilder()
                .WithColor(Color.DarkGrey)
                .WithDescription($"{Context.User.Mention} hugs {user.Mention}.")
                .WithImageUrl(hugJson?["image"]?.ToString())
                .Build();

            await FollowupAsync(embeds: new[] { embed });
        }
    }
}
