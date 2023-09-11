using Discord;
using Discord.Interactions;
using Sushi.Utils;

namespace Sushi.Commands.Info
{
    public class BotInfoModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("bot-info", "Get info about the bot")]
        public async Task HandleBotInfo()
        {
            await DeferAsync();

            Embed embed = new EmbedBuilder()
                .WithColor(Color.Blue)
                .WithTitle("Sushi")
                .WithDescription("Sushi is a Discord bot written in C# which serves absolutely no purpose but here I am rewriting it for the 3rd time because 2 people asked.")
                .WithThumbnailUrl(GlobalVars.DiscordClient.CurrentUser.GetAvatarUrl())
                .AddField("**Version**", GlobalVars.Version, true)
                .AddField("**Source Code**", "[GitHub Link](https://github.com/Arikatsu/Sushi-v3)", true)
                .AddField("**Invite**", $"[Invite Link](https://discord.com/api/oauth2/authorize?client_id={GlobalVars.Config.ClientId}&permissions=8&scope=bot%20applications.commands)", true)
                .AddField("**Server Count**", $"{GlobalVars.DiscordClient.Guilds.Count} servers", true)
                .AddField("**Discord.Net Version**", $"v{DiscordConfig.Version}", true)
                .AddField("**Ping**", $"{GlobalVars.DiscordClient.Latency}ms", true)
                .AddField("**Uptime**", $"{Helpers.GetUptime()}", true)
                .AddField("**Developer**", $"<@{GlobalVars.Config.OwnerId}>", true)
                .Build();

            await FollowupAsync(embeds: new[] { embed });
        }
    }
}
