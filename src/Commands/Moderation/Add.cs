using Discord;
using Discord.Interactions;
using Sushi.Utils;
using System.Text.RegularExpressions;

namespace Sushi.Commands.Moderation
{
    [Group("add", "Various commands for adding something by moderators")]
    [DefaultMemberPermissions(GuildPermission.ManageEmojisAndStickers)]
    public class Add : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("emotes", "Add emojis to the server from other servers")]
        public async Task HandleEmojisAsync([Summary("emotes", "Add all your emojis in one sentence")] string emoteString)
        {
            await DeferAsync();
            
            var guildUser = Context.Guild.GetUser(Context.User.Id);

            if (!guildUser.GuildPermissions.ManageEmojisAndStickers ||
                !guildUser.GuildPermissions.Administrator)
            {
                await FollowupAsync("You don't have permission to manage emojis in this server.");
                return;
            }

            if (!Context.Guild.CurrentUser.GuildPermissions.ManageEmojisAndStickers ||
                !Context.Guild.CurrentUser.GuildPermissions.Administrator)
            {
                await FollowupAsync("I don't have permission to manage emojis in this server.");
                return;
            }

            List<string> emotes = new();

            foreach (Match match in Regex.Matches(emoteString, @"<a?:\w+:\d+>").Cast<Match>())
            {
                emotes.Add(match.Value);
            }

            await FollowupAsync($"Adding {emotes.Count} emojis...");

            foreach (string emote in emotes)
            {
                try
                {
                    Emote emoteParsed = Emote.Parse(emote);
                    
                    var stream = await new HttpClient().GetStreamAsync(emoteParsed.Url + "?size=64&quality=lossless");

                    await Context.Guild.CreateEmoteAsync(emoteParsed.Name, new Discord.Image(stream));
                    await Context.Channel.SendMessageAsync($"Added {emoteParsed}");
                }
                catch (Exception e)
                {
                    Logger.Error(e.Message);
                    await Context.Channel.SendMessageAsync($"Failed to add emoji {emote}");
                }
            }
        }
    }
}
