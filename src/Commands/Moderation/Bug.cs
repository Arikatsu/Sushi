using Discord;
using Discord.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sushi.Commands.Moderation
{
    public class BugModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("bug", "Report a bug to the developer.")]
        [DefaultMemberPermissions(GuildPermission.Administrator)]
        public async Task HandleBugAsync([Summary("bug", "Describe the bug in full detail.")] string bug)
        {
            Embed embed = new EmbedBuilder()
                .WithColor(Color.Red)
                .WithAuthor($"Bug Report from {Context.User.Username}#{Context.User.Discriminator}", Context.User.GetAvatarUrl())
                .WithDescription(bug)
                .WithFooter($"User ID: {Context.User.Id}")
                .WithCurrentTimestamp()
                .Build();

            await Context.Client
                .GetGuild(ulong.Parse(GlobalVars.Config.TestGuildId))
                .GetTextChannel(ulong.Parse(GlobalVars.Config.BugReportChannelId))
                .SendMessageAsync(embed: embed);

            await FollowupAsync("Bug report sent.", ephemeral: true);
        }
    }
}
