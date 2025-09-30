#![allow(unused_macros)]

use std::sync::Mutex;

static LOGGER: Mutex<()> = Mutex::new(());
pub(crate) static mut DEBUG_ENABLED: bool = false;

pub(crate) fn write_line(msg: &str) {
    let _lock = LOGGER.lock().unwrap();
    println!("{msg}");
}

pub(crate) fn enable_debug() {
    unsafe {
        DEBUG_ENABLED = true;
    }
}

macro_rules! log_debug {
    ($($arg:tt)*) => ({
        if unsafe { $crate::logger::DEBUG_ENABLED } {
            $crate::logger::write_line(&format!("\x1b[35m    DEBUG\x1b[0m   {}", format!($($arg)*)));
        }
    })
}

macro_rules! log_info {
    ($($arg:tt)*) => ({
        $crate::logger::write_line(&format!("\x1b[34m     INFO\x1b[0m   {}", format!($($arg)*)));
    })
}

macro_rules! log_warn {
    ($($arg:tt)*) => ({
        $crate::logger::write_line(&format!("\x1b[33m     WARN\x1b[0m   {}", format!($($arg)*)));
    })
}

macro_rules! log_error {
    ($($arg:tt)*) => ({
        $crate::logger::write_line(&format!("\x1b[31m    ERROR\x1b[0m   {}", format!($($arg)*)));
    })
}

macro_rules! log_critical {
    ($($arg:tt)*) => ({
        $crate::logger::write_line(&format!("\x1b[91;1m CRITICAL\x1b[0m   {}", format!($($arg)*)));
        std::process::exit(1);
    })
}

#[allow(unused_imports)]
pub(crate) use {
    log_critical as critical, log_debug as debug, log_error as error, log_info as info,
    log_warn as warn,
};
