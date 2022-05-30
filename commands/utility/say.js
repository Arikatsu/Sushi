const discord = require("discord.js");
var code = '';

module.exports = {
  name: "say",
  category: "utility",
  description: "repeats a user's message",
  run: async (client, message, args) => {
   if (message.author.bot) return;
   if (message.author.id == '549084051261227041' && message.content.includes('kimi') || message.content.includes('<@325290687698567168>')) {
     message.delete()
     return message.channel.send("Jun u are banned from mentioning Kimi"); // lmao kimi asked for this
   }
   else {
     code = message.content.substr(6);
     message.delete()
     message.channel.send(code);
   }
 }
}