use crate::logger;

use std::time::{Duration, Instant};
use tokio::sync::Mutex;

pub struct GeminiState {
    limit: u16,
    used_today: Mutex<u16>,
    last_global: Mutex<Option<Instant>>,
    global_cooldown: Duration,
}

impl GeminiState {
    pub fn new() -> Self {
        Self {
            limit: 1000,
            used_today: Mutex::new(0),
            last_global: Mutex::new(None),
            global_cooldown: Duration::from_secs(900),
        }
    }

    pub async fn reset_daily_usage(&self) {
        let mut used_today = self.used_today.lock().await;
        *used_today = 0;
    }

    pub async fn record_usage(&self, enforce_global: bool) {
        let mut used_today = self.used_today.lock().await;
        *used_today += 1;

        if enforce_global {
            let mut last_global = self.last_global.lock().await;
            *last_global = Some(Instant::now());
        }

        logger::debug!("Recorded usage. Total used today: {}", *used_today);
    }

    pub async fn can_proceed(&self, enforce_global: bool) -> bool {
        let now = Instant::now();
        let last_global = self.last_global.lock().await;
        let used_today = self.used_today.lock().await;

        if *used_today >= self.limit {
            return false;
        }

        if enforce_global {
            if let Some(last) = *last_global {
                if now.duration_since(last) < self.global_cooldown {
                    return false;
                }
            }
        }

        true
    }
}
