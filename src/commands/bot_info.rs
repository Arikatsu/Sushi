use poise::serenity_prelude as serenity;

use crate::{Context, Error};

macro_rules! format_time {
    ($secs:expr) => {{
        let days = $secs / 86400;
        let hours = ($secs % 86400) / 3600;
        let minutes = ($secs % 3600) / 60;
        let seconds = $secs % 60;
        format!("{}d {}h {}m {}s", days, hours, minutes, seconds)
    }};
}

/// Displays information about the bot.
#[poise::command(slash_command, rename = "bot-info")]
pub async fn bot_info(ctx: Context<'_>) -> Result<(), Error> {
    ctx.defer().await?;

    let app_config = ctx.data().app_config;

    let avatar_url = ctx.cache().current_user().avatar_url().unwrap_or_default();
    let guild_count = ctx.cache().guilds().len();

    let embed = serenity::CreateEmbed::new()
        .title("Sushi")
        // this description makes sense if you look at command implementation in the C# branch
        .description("Sushi is a Discord bot written in Rust which serves absolutely no purpose but here I am rewriting it for the 4th time yet NO one asked.")
        .thumbnail(avatar_url)
        .field("**Version**", env!("CARGO_PKG_VERSION"), true)
        .field("**Source Code**", "[GitHub](https://github.com/Arikatsu/Sushi)", true)
        .field("**Invite**", format!("[Invite Link](https://discord.com/api/oauth2/authorize?client_id={}&permissions=8&scope=bot%20applications.commands)", app_config.bot.client_id), true)
        .field("**Server Count**", format!("{}", guild_count), true)
        .field("**Latency**", format!("{}ms", ctx.ping().await.as_millis()), true)
        .field("**Uptime**", format_time!(ctx.data().start_time.elapsed().as_secs()), true)
        .field("**Developer**", format!("<@{}>", &app_config.bot.owner_id), true)
        .field("**Library**", "[Poise](https://github.com/serenity-rs/poise)", true)
        .color(serenity::Colour::BLUE);

    let message = poise::CreateReply::default().embed(embed);
    ctx.send(message).await?;

    Ok(())
}
