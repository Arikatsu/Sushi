using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;

namespace Sushi.Commands.Fun
{
    public class SlapModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("slap", "slap someone in the server.")]
        public async Task HandleSlapAsync([Summary("user", "The user to slap.")] SocketUser user)
        {
            await DeferAsync();

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://api.nekos.fun:8080/api/slap");
            var content = await response.Content.ReadAsStringAsync();

            JObject slapJson = JObject.Parse(content);

            Embed embed = new EmbedBuilder()
                .WithColor(Color.DarkGrey)
                .WithDescription($"{Context.User.Mention} slaps {user.Mention}.")
                .WithImageUrl(slapJson?["image"]?.ToString())
                .Build();

            await FollowupAsync(embeds: new[] { embed });
        }
    }
}

