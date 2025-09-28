mod commands;
mod events;
mod config;
mod logger;

use poise::serenity_prelude as serenity;
use serenity::GatewayIntents;

struct Data {
    http_client: reqwest::Client,
    app_config: &'static config::Config,
    start_time: std::time::Instant,
}

type Error = Box<dyn std::error::Error + Send + Sync>;
type Context<'a> = poise::Context<'a, Data, Error>;

#[tokio::main]
async fn main() {
    let start_time = std::time::Instant::now();

    #[cfg(debug_assertions)]
    logger::enable_debug();

    let config = config::Config::load();
    logger::info!("Config loaded");

    let intents = GatewayIntents::GUILD_MESSAGES | GatewayIntents::MESSAGE_CONTENT;

    logger::info!("Setting up framework...");
    let framework = poise::Framework::<Data, Error>::builder()
        .options(poise::FrameworkOptions {
            commands: commands::all_commands(),
            event_handler: |ctx, event, framework, data| {
                Box::pin(events::handler(ctx, event, framework, data))
            },
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
                    http_client: reqwest::Client::new(),
                    app_config: config,
                    start_time,
                })
            })
        })
        .build();

    let client = serenity::ClientBuilder::new(&config.discord_token, intents)
        .framework(framework)
        .await;

    logger::info!("Running client...");

    if let Err(err) = client.unwrap().start().await {
        logger::error!("Error running client: {:?}", err);
    }
}
