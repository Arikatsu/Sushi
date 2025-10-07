use std::collections::HashSet;

pub struct PromptDetector {
    distinctive_chunks: HashSet<String>,
}

impl PromptDetector {
    pub fn new(system_prompt: &str) -> Self {
        let prompt_lower = system_prompt.to_lowercase();
        let mut chunks = HashSet::new();

        for i in (0..=prompt_lower.len().saturating_sub(40)).step_by(40 / 3) {
            if let Some(end) = prompt_lower.get(i..i + 40) {
                chunks.insert(end.to_string());
            }
        }

        Self {
            distinctive_chunks: chunks,
        }
    }

    pub fn contains_prompt(&self, message: &str) -> bool {
        let message_lower = message.to_lowercase();
        self.distinctive_chunks
            .iter()
            .any(|chunk| message_lower.contains(chunk))
    }
}