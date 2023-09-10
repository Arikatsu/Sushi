using Discord;
using Discord.Interactions;
using Newtonsoft.Json;

namespace Sushi.Commands.Fun
{
    public class AdviceModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("advice", "Gives random advice for no reason.")]
        public async Task HandleAdviceAsync()
        {
            await DeferAsync();

            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync("https://api.adviceslip.com/advice");
            var content = await response.Content.ReadAsStringAsync();
            var advice = JsonConvert.DeserializeObject<AdviceJson>(content);

            Embed embed = new EmbedBuilder()
                .WithColor(Color.DarkGrey)
                .WithDescription(advice?.Slip?.Advice ?? "No advice found.")
                .Build();

            await FollowupAsync(embeds: new[] { embed });
        }
    }

    internal class SlipJson
    {
        [JsonProperty("advice")]
        public string? Advice { get; set; }

        [JsonProperty("id")]
        public string? Id { get; set; }
    }

    internal class AdviceJson
    {
        [JsonProperty("slip")]
        public SlipJson? Slip { get; set; }
    }
}
