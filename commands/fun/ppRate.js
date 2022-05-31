const discord = require("discord.js");

module.exports = {
    name: "pp",
    aliases: "pprate",
    category: "fun",
    description: "Rates you your pp size",
    run: async (client, message, args) => {
       if(message.author.bot) return;
       else if(message.author.id == '593787701409611776') message.channel.send("your dick score is 100/100");
       else
       {
          const rnd = Math.floor(Math.random() * 100)
          message.channel.send("your dick score is "+ rnd +"/100");
       }
    }
}
