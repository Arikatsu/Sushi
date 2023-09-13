using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace Sushi.Commands.Moderation
{
    public class BanModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("ban", "Ban a user from the server")]
        [DefaultMemberPermissions(GuildPermission.BanMembers)]
        public async Task BanAsync(
            [Summary("user", "The user to ban")]
            SocketGuildUser guildUser,
            [Summary("prune-messages", "The number of days to remove messages from this user for - must be between [0,7]")]
            int pruneMessages = 0,
            [Summary("reason", "The reason for the ban")]
            string reason = "No reason provided")
        {
            await DeferAsync();

            if (guildUser.Id == Context.User.Id)
            {
                await FollowupAsync("You can't ban yourself.");
                return;
            }

            if (guildUser.Id == Context.Guild.OwnerId)
            {
                await FollowupAsync("You can't ban the server owner.");
                return;
            }

            if (guildUser.Roles.Max(x => x.Position) >= Context.Guild.CurrentUser.Roles.Max(x => x.Position))
            {
                await FollowupAsync("I can't ban a user that is higher than my highest role.");
                return;
            }

            var contextGuildUser = Context.Guild.GetUser(Context.User.Id);

            if (guildUser.Roles.Max(x => x.Position) >= contextGuildUser.Roles.Max(x => x.Position))
            {
                await FollowupAsync("You can't ban a user that is higher than your highest role.");
                return;
            }

            if (guildUser.Id == Context.Guild.CurrentUser.Id)
            {
                await FollowupAsync("You can't ban me.");
                return;
            }

            await guildUser.BanAsync(pruneMessages, reason);
            await FollowupAsync($"Banned `{guildUser.Username}` for `{reason}` and pruned `{pruneMessages}` days worth of messages.");
        }
    }
}
