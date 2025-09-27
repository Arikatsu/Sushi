use std::sync::OnceLock;
use std::fs;

use toml;
use serde::Deserialize;

#[derive(Debug, Deserialize)]
pub struct Config {
    pub discord_token: String,
    pub test_guild_id: u64,
}

static CONFIG: OnceLock<Config> = OnceLock::new();

impl Config {
    pub fn load() {
        let config_str = fs::read_to_string("config.toml").expect("Failed to read config.toml");
        let config: Config = toml::from_str(&config_str).expect("Failed to parse config.toml");
        CONFIG.set(config).map_err(|_| "Config already set").expect("Failed to set config");
    }

    pub fn get() -> &'static Config {
        CONFIG.get().expect("Config not loaded")
    }
}