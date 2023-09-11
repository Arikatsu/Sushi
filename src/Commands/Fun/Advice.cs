using Discord;
using Discord.Interactions;
using Newtonsoft.Json.Linq;

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
            
            JObject adviceJson = JObject.Parse(content);

            Embed embed = new EmbedBuilder()
                .WithColor(Color.DarkGrey)
                .WithDescription(adviceJson?["slip"]?["advice"]?.ToString())
                .Build();

            await FollowupAsync(embeds: new[] { embed });
        }
    }
}
