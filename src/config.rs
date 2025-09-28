use std::sync::OnceLock;
use std::fs;

use serde::Deserialize;

#[derive(Debug, Deserialize)]
pub struct Config {
    pub bot: BotConfig,
    pub ai: AIConfig,
}

#[derive(Debug, Deserialize)]
pub struct BotConfig {
    pub discord_token: String,
    pub client_id: u64,
    pub owner_id: u64,
    pub test_guild_id: u64,
}

#[derive(Debug, Deserialize)]
pub struct AIConfig {
    pub gemini_api_key: String,
    pub advice_instruction: String,
    pub message_event_instruction: String,
    // "use a HashSet bro" this list contains like TWO GUILDS brochacho
    pub allowed_guilds: Vec<u64>,
    pub global_cooldown_seconds: u64,
}

static CONFIG: OnceLock<Config> = OnceLock::new();

impl Config {
    pub fn load() -> &'static Config {
        CONFIG.get_or_init(|| {
            let config_str = fs::read_to_string("config.toml").expect("Failed to read config.toml");
            toml::from_str(&config_str).expect("Failed to parse config.toml")
        })
    }
}