using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Sushi.Commands.Fun
{
    public class KissModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("kiss", "Kiss someone in the server.")]
        public async Task HandleKissAsync([Summary("user", "The user to kiss.")] SocketUser user)
        {
            await DeferAsync();

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://api.nekos.fun:8080/api/kiss");
            var content = await response.Content.ReadAsStringAsync();

            JObject kissJson = JObject.Parse(content);

            Embed embed = new EmbedBuilder()
                .WithColor(Color.DarkGrey)
                .WithDescription($"{Context.User.Mention} kisses {user.Mention}.")
                .WithImageUrl(kissJson?["image"]?.ToString())
                .Build();

            await FollowupAsync(embeds: new[] { embed });
        }
    }
}
