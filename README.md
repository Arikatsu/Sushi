# Sushi

A Discord bot written in Rust that serves absolutely no purpose but here I am rewriting it for the 4th time yet NO one asked.

> **Note**: This bot is currently under heavy development. Features and commands are not final and may change frequently.

## Setup

### Prerequisites

- Rust (2024 edition)
- Discord Bot Token
- Google Gemini API Key

### Installation

1. Clone the repository:
```bash
git clone https://github.com/Arikatsu/Sushi
cd sushi-rs
```

2. Copy the example configuration:
```bash
cp config.example.toml config.toml
```

3. Edit `config.toml` with your credentials:
```toml
[bot]
discord_token = "YOUR_DISCORD_BOT_TOKEN"
client_id = 123456789012345678
owner_id = 123456789012345678
test_guild_id = 123456789012345678

[ai]
gemini_api_key = "YOUR_GEMINI_API_KEY"
advice_instruction = "Give helpful life advice"
message_event_instruction = "Respond to messages naturally"
allowed_guilds = [123456789012345678]  # Guilds that can use AI features through message events
global_cooldown_seconds = 3600
```

4. Build and run:
```bash
cargo build --release
cargo run --release
```

## Development

### Debug Mode
In debug builds, commands are registered to the test guild only for faster testing. In release builds, commands are registered globally.

## Version History

- **v4**: Current Rust rewrite with Gemini AI integration
- **v3**: Previous rewrite in C#
- **v1 & v2**: Original versions in JS
