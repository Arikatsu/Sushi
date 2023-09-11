using Discord;
using Discord.Interactions;
using Newtonsoft.Json.Linq;

namespace Sushi.Commands.Image
{
    internal class NekoModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("neko", "Fetches a random cat girl image")]
        public async Task HandleNekoAsync()
        {
            await DeferAsync();

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://api.waifu.pics/sfw/neko");
            var content = await response.Content.ReadAsStringAsync();

            JObject nekoJson = JObject.Parse(content);

            Embed embed = new EmbedBuilder()
                .WithColor(Color.DarkGrey)
                .WithImageUrl(nekoJson?["url"]?.ToString())
                .Build();

            await FollowupAsync(embeds: new[] { embed });
        }
    }
}
