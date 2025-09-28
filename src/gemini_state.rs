// TODO: persist state to local sqlite db so it survives restarts
// TODO: track cooldowns per guild instead of globally

use crate::logger;

use chrono::Utc;
use chrono_tz::US::Pacific;
use std::time::{Duration, Instant, SystemTime};
use tokio::sync::Mutex;

const LIMIT: u16 = 1000;

pub struct GeminiState {
    used_today: Mutex<u16>,
    reset_timestamp: Mutex<u64>,
    last_global: Mutex<Option<Instant>>,
    global_cooldown: Duration,
}

impl GeminiState {
    pub fn new(global_cooldown: u64) -> Self {
        Self {
            used_today: Mutex::new(0),
            reset_timestamp: Mutex::new(Self::calculate_next_reset_timestamp()),
            last_global: Mutex::new(None),
            global_cooldown: Duration::from_secs(global_cooldown),
        }
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
        self.check_and_reset_quota().await;

        let used_today = self.used_today.lock().await;

        if *used_today >= LIMIT {
            return false;
        }

        if enforce_global {
            let last_global = self.last_global.lock().await;
            if let Some(last) = *last_global {
                if Instant::now().duration_since(last) < self.global_cooldown {
                    return false;
                }
            }
        }

        true
    }
    fn calculate_next_reset_timestamp() -> u64 {
        let now_pacific = Utc::now().with_timezone(&Pacific);
        let next_reset_pacific = now_pacific
            .date_naive()
            .succ_opt()
            .unwrap()
            .and_hms_opt(0, 0, 0)
            .unwrap();
        next_reset_pacific.and_utc().timestamp() as u64
    }

    async fn check_and_reset_quota(&self) {
        let now_timestamp = SystemTime::now()
            .duration_since(SystemTime::UNIX_EPOCH)
            .unwrap_or_else(|_| Duration::from_secs(0))
            .as_secs();

        let mut reset_ts = self.reset_timestamp.lock().await;

        if now_timestamp >= *reset_ts {
            let mut used_today = self.used_today.lock().await;
            *used_today = 0;

            *reset_ts = Self::calculate_next_reset_timestamp();
            logger::debug!("Daily quota has been reset.");
        }
    }
}
