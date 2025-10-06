mod advice;
mod bot_info;

pub use advice::advice;
pub use bot_info::bot_info;

pub fn get_commands() -> Vec<poise::Command<crate::Data, crate::Error>> {
    vec![advice(), bot_info()]
}