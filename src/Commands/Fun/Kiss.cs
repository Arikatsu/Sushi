using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Newtonsoft.Json;

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
            var gif = JsonConvert.DeserializeObject<KissJson>(content);

            Embed embed = new EmbedBuilder()
                .WithColor(Color.DarkGrey)
                .WithDescription($"{Context.User.Mention} kisses {user.Mention}.")
                .WithImageUrl(gif?.Image)
                .Build();

            await FollowupAsync(embeds: new[] { embed });
        }
    }

    internal class KissJson
    {
        [JsonProperty("image")]
        public string? Image { get; set; }
    }
}
