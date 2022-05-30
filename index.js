const discord = require("discord.js");
const client = new discord.Client({disableEveryone: false});
const express = require('express')
const app = express();
const port = 3000;

app.get('/', (req, res) => res.send('bot online yay boy!!'));

app.listen(port, () => console.log(`Your app is listening a http://localhost:${port}`));

require("dotenv").config();

const { default_prefix } = require("./config.json");

client.queue = new Map();
client.vote = new Map();

require("./database.js");
client.commands = new discord.Collection();
client.aliases = new discord.Collection();

["command"].forEach(handler => {
  require(`./handlers/${handler}`)(client);
});
client.queue = new Map()
process.on('unhandledRejection', console.error);
  
client.on("message", async message => {
  const prefixMention = new RegExp(`^<@!?${client.user.id}>( |)$`);
  if (message.content.match(prefixMention)) {
    return message.reply(`My prefix is \`${default_prefix}\``);
  }
  if (message.author.bot) return;
  if (!message.guild) return;
  if (!message.content.startsWith(default_prefix)) return;
  if (!message.member)
    message.member = message.guild.fetchMember(message);

  const args = message.content
    .slice(default_prefix.length)
    .trim()
    .split(/ +/g);
  const cmd = args.shift().toLowerCase();
  if (cmd.length === 0) return;
  let command = client.commands.get(cmd);
  if (!command) command = client.commands.get(client.aliases.get(cmd));
  if (command) command.run(client, message, args);
});

client.snipes = new Map()
client.on('messageDelete', function(message, channel){  
  client.snipes.set(message.channel.id, {
    content:message.content,
    author:message.author.tag,
    image:message.attachments.first() ? message.attachments.first().proxyURL : null
  })
});

client.on("ready", () => {
    client.user.setStatus("online");
    console.log("Bot is working!!")
});

client.on
client.on("ready", () => {
    client.user.setActivity("your mom", { type: "STREAMING", url: "https://www.twitch.tv/scaldwashere_"})
});

const { Player } = require("discord-music-player");
const player = new Player(client, {
    leaveOnEnd: false,
    leaveOnStop: false,
    leaveOnEmpty: false,
    volume: 150,
    quality: 'high',
});

client.player = player;

const fs = require('fs')

client.on('guildCreate', guild =>{
    const channelId = '970054253525733386';
    const channel = client.channels.cache.get(channelId);     
    if(!channel) return; 
    const embed = new discord.MessageEmbed()
        .setTitle('I Joined A Guild!')
        .setDescription(`**Guild Name:** ${guild.name} (${guild.id})\n**Members:** ${guild.memberCount}`)
        .setTimestamp()
        .setColor('RANDOM')
        .setFooter(`I'm in ${client.guilds.cache.size} Guilds Now!`);
    channel.send(embed);
});


client.on('guildDelete', guild =>{
    const channelId = '970054253525733386';
    const channel = client.channels.cache.get(channelId);     
    if(!channel) return; 
    const embed = new discord.MessageEmbed()
        .setTitle('I Left A Guild!')
        .setDescription(`**Guild Name:** ${guild.name} (${guild.id})\n**Members:** ${guild.memberCount}`)
        .setTimestamp()
        .setColor('RED')
        .setFooter(`I'm in ${client.guilds.cache.size} Guilds Now!`);
    channel.send(embed);
});

let random = 10;
client.on('message', message => {
  const rndIlu = Math.floor(Math.random() * 50) + 1
  const rndInt = Math.floor(Math.random() * 30) + 1
  const rndMEI = Math.floor(Math.random() * 12) + 1
  {
    if(message.author.bot) return;
    else if(message.author.id === '558566227946242048' && random == rndInt)
      message.channel.send("kizu leave now!");
    else if(random == rndInt && message.author.id != '558566227946242048' && message.author.id != '775042627837100033') 
      message.channel.send(message.content);
    else if(message.author.id === '775042627837100033' && random == rndMEI)
      message.channel.send(message.content);
    else if(message.author.id === '775042627837100033' && rndIlu == 35)
      message.channel.send("I love you Fuyu");
  }
})

client.login(process.env.TOKEN);

client
    .on("debug", console.log)
    .on("warn", console.log)