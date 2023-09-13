using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace Sushi.Commands.Moderation
{
    [Group("role", "Various commands for managing roles")]
    [DefaultMemberPermissions(GuildPermission.ManageRoles)]
    public class RoleModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("add", "Adds a role to a user")]
        public async Task HandleAddAsync(
            [Summary("user", "The user to add the role to")]
            SocketGuildUser guildUser,
            [Summary("role", "The role to add")]
            SocketRole role)
        {
            await DeferAsync();

            if (!Context.Guild.CurrentUser.GuildPermissions.ManageRoles ||
                !Context.Guild.CurrentUser.GuildPermissions.Administrator)
            {
                await FollowupAsync("I don't have permission to manage roles in this server.");
                return;
            }

            if (guildUser.Roles.Contains(role))
            {
                await FollowupAsync("That user already has that role.");
                return;
            }

            if (guildUser.Roles.Max(x => x.Position) >= Context.Guild.CurrentUser.Roles.Max(x => x.Position))
            {
                await FollowupAsync("I can't add a role to a user that is higher than my highest role.");
                return;
            }

            if (role.Position >= Context.Guild.CurrentUser.Roles.Max(x => x.Position))
            {
                await FollowupAsync("I can't add a role that is higher than my highest role.");
                return;
            }

            if (role.IsManaged)
            {
                await FollowupAsync("I can't add a managed role.");
                return;
            }

            await guildUser.AddRoleAsync(role);
            await FollowupAsync($"Added role `{role.Name}` to `{guildUser.Username}`");
        }

        [SlashCommand("remove", "Removes a role from a user")]
        public async Task HandleRemoveAsync(
            [Summary("user", "The user to remove the role from")]
            SocketGuildUser guildUser,
            [Summary("role", "The role to remove")]
            SocketRole role)
        {
            await DeferAsync();

            if (!Context.Guild.CurrentUser.GuildPermissions.ManageRoles ||
                !Context.Guild.CurrentUser.GuildPermissions.Administrator)
            {
                await FollowupAsync("I don't have permission to manage roles in this server.");
                return;
            }

            if (!guildUser.Roles.Contains(role))
            {
                await FollowupAsync("That user doesn't have that role.");
                return;
            }

            if (guildUser.Roles.Max(x => x.Position) >= Context.Guild.CurrentUser.Roles.Max(x => x.Position))
            {
                await FollowupAsync("I can't remove a role from a user that is higher than my highest role.");
                return;
            }

            if (role.Position >= Context.Guild.CurrentUser.Roles.Max(x => x.Position))
            {
                await FollowupAsync("I can't remove a role that is higher than my highest role.");
                return;
            }

            if (role.IsManaged)
            {
                await FollowupAsync("I can't remove a managed role.");
                return;
            }

            await guildUser.RemoveRoleAsync(role);
            await FollowupAsync($"Removed role `{role.Name}` from `{guildUser.Username}`");
        }
    }
}
