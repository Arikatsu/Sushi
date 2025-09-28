use crate::logger;

use poise::serenity_prelude as serenity;
use serenity::model::channel::Message;

const API_URL: &str =
    "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash-lite:generateContent";

pub async fn handle(_ctx: &serenity::Context, _new_message: &Message, _data: &crate::Data) {
    let guild_id = if let Some(gid) = _new_message.guild_id {
        u64::from(gid)
    } else {
        return;
    };

    if _new_message.author.bot || !_data.app_config.ai.allowed_guilds.contains(&guild_id) {
        return;
    }

    if !_data.gemini_state.can_proceed(true).await {
        return;
    }

    let request_body = serde_json::json!({
        "contents": [
            {
                "role": "user",
                "parts": [
                    { "text": _new_message.content }
                ]
            }
        ],
        "system_instruction": {
            "parts": [
                { "text": _data.app_config.ai.message_event_instruction }
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

    _data.gemini_state.record_usage(true).await;

    let resp_json: serde_json::Value = response.json().await.unwrap_or_default();
    let reply = resp_json["candidates"]
        .get(0)
        .and_then(|c| c["content"]["parts"].get(0))
        .and_then(|p| p["text"].as_str())
        .unwrap_or("...");

    if let Err(err) = _new_message.reply(&_ctx.http, reply).await {
        logger::error!("Failed to send message: {}", err);
    }
}
