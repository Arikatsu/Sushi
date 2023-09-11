using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;

namespace Sushi.Commands.Fun
{
    public class PatModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("pat", "Pat someone in the server.")]
        public async Task HandlePatAsync([Summary("user", "The user to pat.")] SocketUser user)
        {
            await DeferAsync();

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://api.nekos.fun:8080/api/pat");
            var content = await response.Content.ReadAsStringAsync();

            JObject patJson = JObject.Parse(content);

            Embed embed = new EmbedBuilder()
                .WithColor(Color.DarkGrey)
                .WithDescription($"{Context.User.Mention} pats {user.Mention}.")
                .WithImageUrl(patJson?["image"]?.ToString())
                .Build();

            await FollowupAsync(embeds: new[] { embed });
        }
    }
}

