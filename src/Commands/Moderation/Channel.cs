using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace Sushi.Commands.Moderation
{
    [Group("channel", "Manage channels")]
    public class ChannelModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("lock", "Prevents people from sending new messages.")]
        [DefaultMemberPermissions(GuildPermission.ManageChannels)]
        public async Task HandleLockAsync([Summary("channel", "The channel to lock.")] SocketGuildChannel channel)
        {
            await DeferAsync();

            if (!Context.Guild.CurrentUser.GuildPermissions.ManageChannels ||
                !Context.Guild.CurrentUser.GuildPermissions.Administrator)
            {
                await FollowupAsync("I don't have permission to manage channels.");
                return;
            }

            if (channel is SocketTextChannel textChannel)
            {
                await textChannel.AddPermissionOverwriteAsync(Context.Guild.EveryoneRole, new OverwritePermissions(sendMessages: PermValue.Deny));
                await FollowupAsync($"Locked {channel.Name}.");
            }
            else
            {
                await FollowupAsync("You can only lock text channels.");
            }
        }

        [SlashCommand("unlock", "Allows people to send new messages.")]
        [DefaultMemberPermissions(GuildPermission.ManageChannels)]
        public async Task HandleUnlockAsync([Summary("channel", "The channel to unlock.")] SocketGuildChannel channel)
        {
            await DeferAsync();

            if (!Context.Guild.CurrentUser.GuildPermissions.ManageChannels ||
                !Context.Guild.CurrentUser.GuildPermissions.Administrator)
            {
                await FollowupAsync("I don't have permission to manage channels.");
                return;
            }

            if (channel is SocketTextChannel textChannel)
            {
                await textChannel.AddPermissionOverwriteAsync(Context.Guild.EveryoneRole, new OverwritePermissions(sendMessages: PermValue.Inherit));
                await FollowupAsync($"Unlocked {channel.Name}.");
            }
            else
            {
                await FollowupAsync("You can only unlock text channels.");
            }
        }

        [SlashCommand("slowmode", "Sets the slowmode of a channel.")]
        [DefaultMemberPermissions(GuildPermission.ManageChannels)]
        public async Task HandleSlowmodeAsync(
            [Summary("channel", "The channel to set the slowmode of.")] 
            SocketGuildChannel channel, 
            [Summary("seconds", "The number of seconds to set the slowmode to. 0 removes slowmode")] 
            int seconds)
        {
            await DeferAsync();

            if (!Context.Guild.CurrentUser.GuildPermissions.ManageChannels ||
                !Context.Guild.CurrentUser.GuildPermissions.Administrator)
            {
                await FollowupAsync("I don't have permission to manage channels.");
                return;
            }

            if (channel is SocketTextChannel textChannel)
            {
                await textChannel.ModifyAsync(x => x.SlowModeInterval = seconds);
                _ = seconds == 0
                    ? await FollowupAsync($"Removed the slowmode of {channel.Name}.")
                    : await FollowupAsync($"Set the slowmode of {channel.Name} to {seconds} seconds.");
            }
            else
            {
                await FollowupAsync("You can only set the slowmode of text channels.");
            }
        }
    }
}
