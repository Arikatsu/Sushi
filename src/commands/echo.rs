use crate::{Context, Error};

/// A simple command that echoes back the user's input.
#[poise::command(slash_command)]
pub async fn echo(
    ctx: Context<'_>,
    #[description = "The text to echo back"] text: String,
) -> Result<(), Error> {
    let reply = poise::CreateReply::default()
        .content("Sending message.")
        .ephemeral(true);

    ctx.send(reply).await?;
    ctx.channel_id().say(&ctx.http(), &text).await?;
    Ok(())
}