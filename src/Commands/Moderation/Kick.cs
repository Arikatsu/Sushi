using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace Sushi.Commands.Moderation
{
    public class KickModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("kick", "Kick a user from the server.")]
        [DefaultMemberPermissions(GuildPermission.KickMembers)]
        public async Task HandleKickAsync(
            [Summary("user", "The user to kick")]
            SocketGuildUser guildUser,
            [Summary("reason", "The reason for kicking the user.")]
            string reason = "No reason provided.")
        {
            if (guildUser.Id == Context.User.Id)
            {
                await FollowupAsync("You can't kick yourself.");
                return;
            }

            if (guildUser.Id == Context.Guild.OwnerId)
            {
                await FollowupAsync("You can't kick the server owner.");
                return;
            }

            if (guildUser.Roles.Max(x => x.Position) >= Context.Guild.CurrentUser.Roles.Max(x => x.Position))
            {
                await FollowupAsync("I can't kick a user that is higher than my highest role.");
                return;
            }

            var contextGuildUser = Context.Guild.GetUser(Context.User.Id);

            if (guildUser.Roles.Max(x => x.Position) >= contextGuildUser.Roles.Max(x => x.Position))
            {
                await FollowupAsync("You can't kick a user that is higher than your highest role.");
                return;
            }

            if (guildUser.Id == Context.Guild.CurrentUser.Id)
            {
                await FollowupAsync("You can't kick me.");
                return;
            }

            await guildUser.KickAsync(reason);
            await FollowupAsync($"Kicked `{guildUser.Username}` for `{reason}`.");
        }
    }
}
