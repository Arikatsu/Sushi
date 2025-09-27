mod config;

use poise::{async_trait, serenity_prelude as serenity};
use serenity::GatewayIntents;

type Error = Box<dyn std::error::Error + Send + Sync>;
type Context<'a> = poise::Context<'a, (), Error>;

#[poise::command(slash_command, prefix_command)]
async fn age(
    ctx: Context<'_>,
    #[description = "Selected user"] user: Option<serenity::User>,
) -> Result<(), Error> {
    let u = user.as_ref().unwrap_or_else(|| ctx.author());
    let response = format!("{}'s account was created at {}", u.name, u.created_at());
    ctx.say(response).await?;
    Ok(())
}

struct MessageHandler;

#[async_trait]
impl serenity::EventHandler for MessageHandler {
    async fn message(&self, ctx: serenity::Context, msg: serenity::Message) {
        if msg.content == "!ping" {
            if let Err(why) = msg.channel_id.say(&ctx.http, "Pong!").await {
                println!("Error sending message: {:?}", why);
            }
        }
    }
}

#[tokio::main]
async fn main() {
    config::Config::load();

    let intents = GatewayIntents::GUILD_MESSAGES | GatewayIntents::MESSAGE_CONTENT;

    let framework = poise::Framework::builder()
        .options(poise::FrameworkOptions {
            commands: vec![age()],
            ..Default::default()
        })
        .setup(|ctx, _ready, framework| {
            Box::pin(async move {
                poise::builtins::register_in_guild(
                    ctx,
                    &framework.options().commands,
                    config::Config::get().test_guild_id.into(),
                )
                .await
                .unwrap();
                Ok(())
            })
        })
        .build();

    let client = serenity::ClientBuilder::new(&config::Config::get().discord_token, intents)
        .event_handler(MessageHandler)
        .framework(framework)
        .await;

    client
        .unwrap()
        .start()
        .await
        .unwrap();
}
