using Discord;
using Discord.Interactions;
using MongoDB.Driver;
using Sushi.Models;

namespace Sushi.Commands.Moderation
{
    [Group("warn", "Commands to manage warns of a user for a server.")]
    public class WarnModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("limit", "Sets the minimum number of warns before a user is automatically banned.")]
        [DefaultMemberPermissions(GuildPermission.BanMembers)]
        public async Task HandleLimitAsync([Summary("amount", "The minimum amount to be set. Default is already set to 3.")] int limit)
        {
            await DeferAsync();

            var collection = GlobalVars.Database.GetCollection<WarnCollection.MaxWarnsObject>("max_warns");
            var isDocExists = await collection.Find(x => x.GuildId == Context.Guild.Id.ToString()).AnyAsync();

            if (isDocExists == true)
            {
                var filter = Builders<WarnCollection.MaxWarnsObject>.Filter.Eq(obj => obj.GuildId, Context.Guild.Id.ToString());
                var update = Builders<WarnCollection.MaxWarnsObject>.Update.Set(obj => obj.MaxWarns, limit);

                await collection.FindOneAndUpdateAsync(filter, update);
                await FollowupAsync($"Updated max warns for server: `{Context.Guild.Name}` to `{limit}`");
                return;
            }

            await collection.InsertOneAsync(new WarnCollection.MaxWarnsObject
            {
                GuildId = Context.Guild.Id.ToString(),
                MaxWarns = limit
            });
            await FollowupAsync($"Updated max warns for server: `{Context.Guild.Name}` to `{limit}`");
        }
    }
}
