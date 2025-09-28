use poise::serenity_prelude as serenity;
use serenity::model::channel::Message;

pub async fn handle(
    _ctx: &serenity::Context,
    _new_message: &Message,
    _data: &crate::Data,
) {
    if _new_message.content == "!ping" {
        if let Err(e) = _new_message.channel_id.say(&_ctx.http, "Pong!").await {
            crate::logger::info!("{}", _data.app_config.client_id);
            crate::logger::error!("Failed to send message: {}", e);
        }
    }
}
