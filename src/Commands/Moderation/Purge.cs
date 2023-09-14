using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sushi.Commands.Moderation
{
    public class PurgeModue : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("purge", "Deletes a number of messages.")]
        [DefaultMemberPermissions(GuildPermission.ManageMessages)]
        public async Task HandlePurgeAsync(
            [Summary("amount", "The number of messages to delete.")]
            int amount,
            [Summary("user", "To delete messages from a specific user")]
            SocketGuildUser? user = null)
        {
            await DeferAsync();

            if (!Context.Guild.CurrentUser.GuildPermissions.ManageMessages ||
                !Context.Guild.CurrentUser.GuildPermissions.Administrator)
            {
                await FollowupAsync("I don't have permission to manage messages.");
                return;
            }

            var commandMessage = await Context.Interaction.GetOriginalResponseAsync();

            if (user is null)
            {
                try
                {
                    var messages = await Context.Channel.GetMessagesAsync(amount + 1).FlattenAsync();
                    messages = messages.Where(x => x.Id != commandMessage.Id);

                    await ((ITextChannel)Context.Channel).DeleteMessagesAsync(messages);
                    await FollowupAsync($"Deleted {amount} messages.");
                }
                catch (ArgumentOutOfRangeException)
                {
                    await FollowupAsync("You can only delete messages sent within the last 14 days.");
                }
            }
            else
            {
                try
                {
                    var messages = await Context.Channel.GetMessagesAsync().FlattenAsync();
                    if (user.Id == Context.Client.CurrentUser.Id)
                    {
                        messages = messages.Where(x => x.Id != commandMessage.Id);
                    }
                    var messagesToDelete = messages.Where(x => x.Author.Id == user.Id).Take(amount);

                    await ((ITextChannel)Context.Channel).DeleteMessagesAsync(messagesToDelete);
                    await FollowupAsync($"Deleted {amount} messages from {user.Mention}.");
                }
                catch (ArgumentOutOfRangeException)
                {
                    await FollowupAsync("You can only delete messages sent within the last 14 days.");
                }
            }
        }
    }
}
