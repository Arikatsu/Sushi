use poise::serenity_prelude as serenity;
use std::random;

use crate::{Context, Error};

const PROMPT_TOPICS: [&str; 8] = [
    "Tell me a piece of advice no one expects but everyone should hear.",
    "What's a controversial but useful life tip?",
    "Give me advice that sounds wrong at first, but is actually brilliant.",
    "Give me a random philosophy someone could live by.",
    "Invent a fictional guru and share their advice.",
    "What's a weird life rule someone might follow?",
    "Say something that could be advice or nonsense—let the reader decide.",
    "Write life advice as if you were an ancient tree talking to a squirrel.",
];

/// Gives random advice for no reason.
#[poise::command(slash_command)]
pub async fn advice(
    ctx: Context<'_>,
    #[description = "Get advice on a specific topic."] topic: Option<String>,
) -> Result<(), Error> {
    ctx.defer().await?;

    let data = ctx.data();
    let topic = topic.unwrap_or_else(|| {
        let idx = random::random::<usize>() % PROMPT_TOPICS.len();
        PROMPT_TOPICS[idx].to_string()
    });

    let request_body = serde_json::json!({
        "contents": [
            {
                "role": "user",
                "parts": [
                    { "text": format!("Custom advice on: {}", topic) }
                ]
            }
        ],
        "system_instruction": {
            "parts": [
                { "text": data.app_config.advice_instruction }
            ]
        },
        "generationConfig": {
            "temperature": 0.5,
            "topP": 0.8,
            "maxOutputTokens": 100,
            "stopSequences": ["\n"],
            "topK": 40
        }
    });

    let response = data
        .http_client
        .post("https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash-lite:generateContent")
        .header("x-goog-api-key", &data.app_config.gemini_api_key)
        .json(&request_body)
        .send()
        .await?;

    if !response.status().is_success() {
        let err_text = response.text().await.unwrap_or_default();
        println!("Gemini API error: {}", err_text);
        ctx.say("Failed to get advice. Please try again later.")
            .await?;
        return Ok(());
    }

    let resp_json: serde_json::Value = response.json().await?;
    let advice = resp_json["candidates"]
        .get(0)
        .and_then(|c| c["content"]["parts"].get(0))
        .and_then(|p| p["text"].as_str())
        .unwrap_or("No advice found.");

    let embed = serenity::CreateEmbed::new()
        .description(advice)
        .color(serenity::Colour::DARK_GREY);

    let message = poise::CreateReply::default().embed(embed);
    ctx.send(message).await?;

    Ok(())
}
