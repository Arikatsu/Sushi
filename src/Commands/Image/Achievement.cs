using Discord.Interactions;

namespace Sushi.Commands.Image
{
    internal class AchievementModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("achievement", "Turns text into a Minecraft achievement")]
        public async Task HandleAchievementAsync([Summary("text", "The text to turn into an achievement")] string text)
        {
            await DeferAsync();

            string replacedText = text.Replace(" ", "+");

            await FollowupAsync($"https://minecraftskinstealer.com/achievement/12/Achievement%20Get!/{replacedText}");
        }
    }
}
