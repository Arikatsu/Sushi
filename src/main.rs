#![feature(random)]

mod config;
mod commands;

use std::sync::Arc;

use poise::{serenity_prelude as serenity};
use serenity::GatewayIntents;
use reqwest;

struct Data {
    http_client: Arc<reqwest::Client>,
    app_config: &'static config::Config,
}

type Error = Box<dyn std::error::Error + Send + Sync>;
type Context<'a> = poise::Context<'a, Data, Error>;

#[tokio::main]
async fn main() {
    let config = config::Config::load();

    let intents = GatewayIntents::GUILD_MESSAGES | GatewayIntents::MESSAGE_CONTENT;

    let framework = poise::Framework::<Data, Error>::builder()
        .options(poise::FrameworkOptions {
            commands: commands::all_commands(),
            ..Default::default()
        })
        .setup(move |ctx, _ready, framework| {
            Box::pin(async move {
                poise::builtins::register_in_guild(
                    ctx,
                    &framework.options().commands,
                    config.test_guild_id.into(),
                )
                .await
                .unwrap();

                Ok(Data {
                    http_client: Arc::new(reqwest::Client::new()),
                    app_config: config,
                })
            })
        })
        .build();

    let client = serenity::ClientBuilder::new(&config.discord_token, intents)
        .framework(framework)
        .await;

    client
        .unwrap()
        .start()
        .await
        .unwrap();
}
