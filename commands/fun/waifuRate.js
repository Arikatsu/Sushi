const discord = require("discord.js");

module.exports = {
    name: "wr",
    aliases: "waifurate",
    category: "fun",
    description: "Rates you as a waifu",
    run: async (client, message, args) => {
       if(message.author.bot) return;
       else if(message.author.id == '775042627837100033') message.channel.send("you are a 100/100 waifu");
       else
       {
          const rnd = Math.floor(Math.random() * 100)
          message.channel.send("you are a "+ rnd +"/100 waifu");
       }
    }
}
