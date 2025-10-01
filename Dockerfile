# ---- Build ----
FROM rust:1.90 as builder

WORKDIR /usr/src/app
COPY . .
RUN cargo build --release

# ---- Runtime ----
FROM debian:bookworm-slim

RUN apt-get update && apt-get install -y ca-certificates \
 && rm -rf /var/lib/apt/lists/*

WORKDIR /app
COPY --from=builder /usr/src/app/target/release/sushi-rs /app/
CMD ["./sushi-rs"]