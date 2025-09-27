mod advice;

pub use advice::advice;

pub fn all_commands() -> Vec<poise::Command<crate::Data, crate::Error>> {
    vec![advice()]
}