using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using Sushi.Utils;

namespace Sushi.Commands.Info
{
    [Group("server", "Commands for server info")]
    public class ServerModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("info", "Get info about the server")]
        public async Task HandleInfoAsync()
        {
            await DeferAsync();

            Embed embed = new EmbedBuilder()
                .WithColor(Color.Orange)
                .WithTitle(Context.Guild.Name)
                .WithDescription(Context.Guild.Description)
                .WithThumbnailUrl(Context.Guild.IconUrl)
                .WithImageUrl(Context.Guild.BannerUrl)
                .AddField("**Owner**", $"<@{Context.Guild.Owner.Id}>", true)
                .AddField("**Members**", $"{Context.Guild.MemberCount} members", true)
                .AddField("**Roles**", $"{Context.Guild.Roles.Count} roles", true)
                .AddField("**Text Channels**", $"{Context.Guild.Channels.Count(x => x is SocketTextChannel)} text channels", true)
                .AddField("**Voice Channels**", $"{Context.Guild.Channels.Count(x => x is SocketVoiceChannel)} voice channels", true)
                .AddField("**Emotes**", $"{Context.Guild.Emotes.Count} emotes", true)
                .AddField("**Features**", $"{string.Join(", ", Context.Guild.Features)}", true)
                .AddField("**Boosts**", $"{Context.Guild.PremiumSubscriptionCount} boosts", true)
                .AddField("**Boost Tier**", $"{Context.Guild.PremiumTier}", true)
                .AddField("**Verification Level**", $"{Context.Guild.VerificationLevel}", true)
                .AddField("**Region**", $"{Context.Guild.VoiceRegionId}", true)
                .AddField("**AFK Channel**", $"{Context.Guild.AFKChannel?.Name ?? "None"}", true)
                .AddField("**AFK Timeout**", $"{Context.Guild.AFKTimeout} seconds", true)
                .AddField("**Created At**", Helpers.GetDiscordTimestamp(Context.Guild.CreatedAt), true)
                .AddField("**Icon**", $"{(Context.Guild.IconUrl != null ? $"[Icon Link]({Context.Guild.IconUrl})" : "None")}", true)
                .AddField("**Banner**", $"{(Context.Guild.BannerUrl != null ? $"[Banner Link]({Context.Guild.BannerUrl})" : "None")}", true)
                .Build();

            await FollowupAsync(embeds: new[] { embed });
        }

        [SlashCommand("icon", "Get the server's icon")]
        public async Task HandleIconAsync()
        {
            await DeferAsync();
            await FollowupAsync(Context.Guild.IconUrl + "?size=2048");
        }
    }
}
