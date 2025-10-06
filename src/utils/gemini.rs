use poise::serenity_prelude::Message;

pub fn make_request_body(
    messages: &[Message],
    client_id: u64,
    client_name: &str,
    system_instruction: &str,
) -> serde_json::Value {
    let mut contents = Vec::new();

    let mention1 = format!("<@{}>", client_id);
    let mention2 = format!("<@!{}>", client_id);

    for message in messages.into_iter().rev() {
        let text = message.content.trim();
        if text.trim().is_empty() {
            continue;
        }

        let text = if text.contains(&mention1) || text.contains(&mention2) {
            text.replace(&mention1, client_name)
                .replace(&mention2, client_name)
        } else {
            text.to_string()
        };

        let role = if message.author.id == client_id {
            "model"
        } else {
            "user"
        };

        let text = if role == "user" {
            format!("{}: {}", message.author.name, text)
        } else {
            text
        };

        contents.push(serde_json::json!({
            "role": role,
            "parts": [ { "text": text } ]
        }));
    }

    serde_json::json!({
        "contents": contents,
        "system_instruction": {
            "parts": [
                { "text": system_instruction }
            ]
        },
        "safetySettings": [
            {
                "category": "HARM_CATEGORY_SEXUALLY_EXPLICIT",
                "threshold": "BLOCK_NONE"
            },
            {
                "category": "HARM_CATEGORY_DANGEROUS_CONTENT",
                "threshold": "BLOCK_NONE"
            },
            {
                "category": "HARM_CATEGORY_HATE_SPEECH",
                "threshold": "BLOCK_NONE"
            },
            {
                "category": "HARM_CATEGORY_HARASSMENT",
                "threshold": "BLOCK_NONE"
            },
        ],
    })
}
