using Discord;
using Discord.Interactions;
using Sushi.Utils;
using System.Text.RegularExpressions;

namespace Sushi.Commands.Moderation
{
    [Group("emotes", "Various commands for adding emotes")]
    [DefaultMemberPermissions(GuildPermission.ManageEmojisAndStickers)]
    public class EmotesModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("add", "Add emojis to the server from other servers")]
        public async Task HandleAddAsync([Summary("emotes", "Add all your emojis in one sentence")] string emoteString)
        {
            await DeferAsync();

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

        [SlashCommand("remove", "Remove emojis from the server")]
        public async Task HandleRemoveAsync([Summary("emotes", "Remove all your emojis in one sentence")] string emoteString)
        {
            await DeferAsync();

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

            await FollowupAsync($"Removing {emotes.Count} emojis...");

            foreach (string emote in emotes)
            {
                try
                {
                    Emote emoteParsed = Emote.Parse(emote);
                    GuildEmote? guildEmote = await Context.Guild.GetEmoteAsync(emoteParsed.Id);

                    if (guildEmote == null)
                    {
                        await Context.Channel.SendMessageAsync($"Emoji {emoteParsed} not found in this server");
                        continue;
                    }

                    await Context.Guild.DeleteEmoteAsync(guildEmote);
                    await Context.Channel.SendMessageAsync($"Removed {emoteParsed}");
                }
                catch (Exception e)
                {
                    Logger.Error(e.Message);
                    await Context.Channel.SendMessageAsync($"Failed to remove emoji {emote}");
                }
            }
        }
    }
}
