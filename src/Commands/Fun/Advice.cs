using System.Text;
using Discord;
using Discord.Interactions;
using Newtonsoft.Json.Linq;
using Sushi.Utils;

namespace Sushi.Commands.Fun
{
    public class AdviceModule : InteractionModuleBase<SocketInteractionContext>
    {
        private readonly string _geminiUrl = $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.0-flash-lite:generateContent?key={GlobalVars.Config.GeminiApiKey}";
        string[] promptTopics = {
            "Tell me a piece of advice no one expects but everyone should hear.",
            "What's a controversial but useful life tip?",
            "Give me advice that sounds wrong at first, but is actually brilliant.",
            "Give me a random philosophy someone could live by.",
            "Invent a fictional guru and share their advice.",
            "What's a weird life rule someone might follow?",
            "Say something that could be advice or nonsense—let the reader decide.",
            "Write life advice as if you were an ancient tree talking to a squirrel."
        };

        
        [SlashCommand("advice", "Gives random advice for no reason.")]
        public async Task HandleAdviceAsync([Summary("topic", "Get advice on a specific topic.")] string? topic = null)
        {
            await DeferAsync();
            
            var json = @"
            {
                ""contents"": [
                    {
                        ""role"": ""user"",
                        ""parts"": [
                            { ""text"": ""You are forbidden from starting or ending advice with words like 'embrace', 'live', 'love', 'magic', 'journey', 'moment', or any other vague cliché. Be original, weird, smart, or even darkly humorous. Make sure the advice is a simple one-liner and thats all and nothing extra. Users may ask customized advice as well so respond accordingly."" }
                        ]
                    },
                    {
                        ""role"": ""user"",
                        ""parts"": [
                            { 
                                ""text"": ""Custom advice on: " + (topic == null ? promptTopics[new Random().Next(promptTopics.Length)] : topic) + @"""
                            }
                        ]
                    }
                ],
                ""generationConfig"": {
                    ""temperature"": 0.5,
                    ""topP"": 0.8,
                    ""maxOutputTokens"": 100,
                    ""stopSequences"": [""\n""],
                    ""topK"": 40
                },
            }";
            
            var httpClient = new HttpClient();
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            HttpResponseMessage response = await httpClient.PostAsync(_geminiUrl, content);
            if (!response.IsSuccessStatusCode)
            {
                Logger.Error(await response.Content.ReadAsStringAsync());
                await FollowupAsync("Failed to get advice. Please try again later.");
                return;
            }
            string responseBody = await response.Content.ReadAsStringAsync();
            
            JObject adviceJson = JObject.Parse(responseBody);
            string advice = adviceJson["candidates"]?[0]?["content"]?["parts"]?[0]?["text"]?.ToString() ?? "No advice found.";

            Embed embed = new EmbedBuilder()
                .WithColor(Color.DarkGrey)
                .WithDescription(advice)
                .Build();

            await FollowupAsync(embeds: new[] { embed });
        }
    }
}
