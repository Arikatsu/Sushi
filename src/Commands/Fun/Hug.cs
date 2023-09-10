using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Newtonsoft.Json;

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
            var gif = JsonConvert.DeserializeObject<HugJson>(content);

            Embed embed = new EmbedBuilder()
                .WithColor(Color.DarkGrey)
                .WithDescription($"{Context.User.Mention} hugs {user.Mention}.")
                .WithImageUrl(gif?.Image)
                .Build();

            await FollowupAsync(embeds: new[] { embed });
        }
    }

    internal class HugJson
    {
        [JsonProperty("image")]
        public string? Image { get; set; }
    }
}
