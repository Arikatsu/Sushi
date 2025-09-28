use std::sync::OnceLock;
use std::fs;

use serde::Deserialize;

#[derive(Debug, Deserialize)]
pub struct Config {
    pub discord_token: String,
    pub test_guild_id: u64,
    pub gemini_api_key: String,
    pub advice_instruction: String,
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