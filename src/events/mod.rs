mod message;

use crate::{Data, Error};

use poise::serenity_prelude as serenity;

pub async fn handler(
    ctx: &serenity::Context,
    event: &serenity::FullEvent,
    _framework: poise::FrameworkContext<'_, Data, Error>,
    data: &Data,
) -> Result<(), Error> {
    match event {
        serenity::FullEvent::Message { new_message } => {
            message::handle(ctx, new_message, data).await;
        }
        _ => {}
    }
    Ok(())
}
