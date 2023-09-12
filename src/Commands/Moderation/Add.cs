using Discord;
using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Sushi.Commands.Moderation
{
    [Group("add", "Various commands for adding something by moderators")]
    public class Add : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("emojis", "Add emojis to the server from other servers")]
        [DefaultMemberPermissions(GuildPermission.ManageEmojisAndStickers)]
        public async Task HandleEmojisAsync([Summary("emojis", "Add all your emojis in one sentence")] string emojiString)
        {
            await DeferAsync();

            if (!Context.Guild.CurrentUser.GuildPermissions.ManageEmojisAndStickers)
            {
                await FollowupAsync("I don't have permission to manage emojis in this server.");
                return;
            }

            Regex regex = new(@"/<?(a)?:?(\w{2,32}):(\d{17,19})>?/gi");

            MatchCollection matches = regex.Matches(emojiString);

            List<string> emojiNames = new();
            foreach (Match match in matches.Cast<Match>())
            {
                emojiNames.Add(match.Groups[2].Value);
            }

            await FollowupAsync($"Adding {emojiNames.Count} emojis...");

            foreach (string emojiName in emojiNames)
            {
                try
                {
                    var emoji = Emoji.Parse(
                    await Context.Guild.CreateEmoteAsync(emojiName, new(new Uri($"https://cdn.discordapp.com/emojis/{emojiName}.png")));
                }
                catch (Exception)
                {
                    await Context.Channel.SendMessageAsync($"Failed to add emoji {emojiName}");
                }
            }
        }
    }
}
