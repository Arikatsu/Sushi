using Discord;
using Discord.Interactions;
using Newtonsoft.Json.Linq;

namespace Sushi.Commands.Image
{
    internal class WaifuModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("waifu", "Fetches a random waifu image")]
        public async Task HandleWaifuAsync()
        {
            await DeferAsync();

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("http://api.waifu.pics/sfw/waifu");
            var content = await response.Content.ReadAsStringAsync();

            JObject waifuJson = JObject.Parse(content);

            Embed embed = new EmbedBuilder()
                .WithColor(Color.DarkGrey)
                .WithImageUrl(waifuJson?["url"]?.ToString())
                .Build();

            await FollowupAsync(embeds: new[] { embed });
        }
    }
}
