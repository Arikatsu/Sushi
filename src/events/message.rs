use crate::utils::gemini;
use crate::utils::logger;

use poise::serenity_prelude as serenity;
use serenity::GetMessages;
use serenity::model::channel::Message;

static API_URL: &str =
    "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash-lite:generateContent";

pub async fn handle(_ctx: &serenity::Context, _new_message: &Message, _data: &crate::Data) {
    let guild_id = match _new_message.guild_id {
        Some(id) => u64::from(id),
        None => return,
    };

    if _new_message.author.bot || !_data.app_config.ai.allowed_guilds.contains(&guild_id) {
        return;
    }

    if !_new_message.mentions_me(&_ctx.http).await.unwrap_or(false) {
        return;
    }

    let typing = _new_message.channel_id.start_typing(&_ctx.http);

    let past_messages = _new_message
        .channel_id
        .messages(&_ctx.http, GetMessages::new().limit(5))
        .await
        .unwrap_or_else(|e| {
            logger::error!("Failed to fetch messages: {}", e);
            vec![]
        });

    let request_body = gemini::make_request_body(
        &past_messages,
        _data.app_config.bot.client_id,
        &_ctx.cache.current_user().name,
        &_data.app_config.ai.message_event_instruction,
    );

    let response = _data
        .http_client
        .post(API_URL)
        .header("x-goog-api-key", &_data.app_config.ai.gemini_api_key)
        .json(&request_body)
        .send()
        .await;

    if response.is_err() {
        logger::error!("HTTP request error: {}", response.err().unwrap());
        return;
    }

    let response = match response {
        Ok(resp) => resp,
        Err(err) => {
            logger::error!("Failed to get response: {}", err);
            return;
        }
    };

    if !response.status().is_success() {
        let err_text = response.text().await.unwrap_or_default();
        logger::error!("Gemini API error: {}", err_text);
        return;
    }

    let resp_json: serde_json::Value = response.json().await.unwrap_or_default();
    let reply = resp_json["candidates"]
        .get(0)
        .and_then(|c| c["content"]["parts"].get(0))
        .and_then(|p| p["text"].as_str())
        .unwrap_or("...");

    let reply = if _data.prompt_detector.contains_prompt(reply)
    { "go away" } else { reply };

    typing.stop();
    if let Err(err) = _new_message.reply(&_ctx.http, reply).await {
        logger::error!("Failed to send message: {}", err);
    }
}
