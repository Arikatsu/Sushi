const discord = require("discord.js");

module.exports = {
    name: "wr",
    aliases: "waifurate",
    category: "fun",
    description: "Rates you as a waifu",
    run: async (client, message, args) => {
        if(message.author.bot) return;
        else if(message.author.id == 593787701409611776)
          message.channel.send("your dick rating is 100/100"); // I have to be the best smh
        else
        {
          const rnd = Math.floor(Math.random() * 100)
          message.channel.send("your dick rating is "+ rnd +"/100");
        }
    }
}