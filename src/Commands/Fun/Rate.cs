using Discord;
using Discord.Interactions;
using Discord.WebSocket;

namespace Sushi.src.Commands.Fun
{
    [Group("rate", "Rates various attributes")]
    public class RateModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("pp", "Rates your or someone else's pp size")]
        public async Task HandlePPAsync([Summary("user", "The user to rate the pp size of")] SocketUser? user = null)
        { 
            await DeferAsync();

            bool nullUser = user == null;
            user ??= Context.User;

            if (user?.Id == 593787701409611776)
            {

                await FollowupAsync($"{(nullUser ? "Your" : user?.Username + "'s")} dick score is 100/100");
                return;
            }

            var ppRate = new Random().Next(0, 100);

            await FollowupAsync($"{(nullUser ? "Your" : user?.Username + "'s")} dick score is {ppRate}/100");
        }

        [SlashCommand("waifu", "Rates how wife-able you or someone else is are")]
        public async Task HandleWaifuAsync([Summary("user", "The user to rate how wife-able they are")] SocketUser? user = null)
        {
            await DeferAsync();

            var waifuRate = new Random().Next(0, 100);

            await FollowupAsync($"{(user == null ? "You are" : user?.Username + " is")} a {waifuRate}/100 waifu");
        }
    }
}
