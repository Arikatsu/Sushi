using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System.Text.RegularExpressions;
using Sushi.Utils;
using Discord.Rest;

namespace Sushi.Commands.Info
{
    [Group("user", "Commands for user info")]
    public class UserModule : InteractionModuleBase<SocketInteractionContext>
    {
        private static readonly Dictionary<string, string> UserState = new()
        {
            { "Online", "https://emoji.gg/assets/emoji/9166_online.png" },
            { "Idle", "https://emoji.gg/assets/emoji/3929_idle.png" },
            { "DoNotDisturb", "https://emoji.gg/assets/emoji/2531_dnd.png" },
            { "Offline", "https://emoji.gg/assets/emoji/7445_status_offline.png" },
        };

        private static readonly Dictionary<string, Color> EmbedColor = new()
        {
            { "Online", Color.Green },
            { "Idle", Color.Gold },
            { "DoNotDisturb", Color.Red },
            { "Offline", Color.LightGrey },
        };

        [SlashCommand("info", "Get info about a user")]
        public async Task HandleInfoAsync([Summary("user", "The user to get info about")] RestUser? user = null)
        {
            await DeferAsync();

            user ??= await GlobalVars.DiscordClient.Rest.GetUserAsync(Context.User.Id);

            SocketGuildUser guildUser = Context.Guild.GetUser(user.Id);

            var badges = guildUser.PublicFlags?
                .ToString()
                .Split(", ")
                .ToList();

            List<string> badgeNameList = new();

            badges ??= new List<string> { "None" };

            foreach (var badge in badges)
            {
                badgeNameList.Add(Helpers.ConvertUpperCamelCaseToWords(badge));
            }   

            string badgesString = string.Join(", ", badgeNameList);

            List<string> activities = new();
            
            if (guildUser.Activities.Any())
            {
                foreach (var activity in guildUser.Activities)
                {
                    activities.Add($"**{activity.Type}** : {activity.Name}");
                }
            }
            else
            {
                activities.Add("None");
            }

            Embed embed = new EmbedBuilder()
                .WithColor(EmbedColor[guildUser.Status.ToString()])
                .WithDescription(string.Join("\n", activities))
                .WithImageUrl(user.GetBannerUrl(size: 2048))
                .AddField("**Account Created**", Helpers.GetDiscordTimestamp(user.CreatedAt))
                .AddField("**Information**", $"**ID**: {user.Id}\n**Username**: {user.Username}\n**Bot**: {user.IsBot}")
                .AddField("**Badges**", badgesString)
                .WithThumbnailUrl(user.GetAvatarUrl())
                .WithFooter(user.Status.ToString(), UserState[user.Status.ToString()])
                .Build();

            await FollowupAsync(embeds: new[] { embed });
        }

        [SlashCommand("avatar", "Fetches a user's avatar")]
        public async Task HandleUserAsync([Summary("user", "The user to fetch the avatar of")] SocketUser? user = null)
        {
            await DeferAsync();

            user ??= Context.User;

            var avatarPngUrl = user.GetAvatarUrl(ImageFormat.Png, size: 2048);
            var avatarJpgUrl = user.GetAvatarUrl(ImageFormat.Jpeg, size: 2048);
            var avatarWebpUrl = user.GetAvatarUrl(ImageFormat.WebP, size: 2048);

            var embed = new EmbedBuilder()
                .WithTitle($"{user.Username}'s avatar")
                .WithDescription($"[png]({avatarPngUrl}) | [jpg]({avatarJpgUrl}) | [webp]({avatarWebpUrl})")
                .WithImageUrl(avatarPngUrl)
                .WithColor(Color.Blue)
                .WithFooter($"Requested by {Context.User.Username}", Context.User.GetAvatarUrl(ImageFormat.Png))
                .Build();

            await FollowupAsync(embeds: new[] { embed });
        }
    }
}
