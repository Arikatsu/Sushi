const ButtonPages = require('discord-button-pages');
const { MessageEmbed } = require('discord.js')
const disbutpages = require("discord-embeds-pages-buttons")
const Discord = require("discord.js");
const disbut = require("discord-buttons");
const MessageButton = require("discord-buttons");

module.exports = {
  name: "help",
  description:
    "Get list of all command and even get to know every command detials",
  usage: "help <cmd>",
  category: "info",
  run: async (client, message, args) => {
    
   let helpEmbed = new MessageEmbed()
      .setTitle("** __Sushi's__ commands**")
      .addField(
        "• **MODERATION**",
        "emojiadd, add-these, announce, bug, addrole, ban, purge, hackban, kick, lock, mute, removerole, resetwarns, setnick, setmodlog, slowmode, unlock, unmute, voicekick, warn, warnings, nuke, lockchannel"
      )
      .addField(
        "• **SUGGESTION**",
        "sreply, setsuggest, suggest"
      )
      .addField(
        "• **UTILITY**",
        "discriminator, findemoji, sudo, haste, say"
      )
      .addField(
        "• **SEARCH**",
        "anime, discord, github, ig, npm, twitter, imdb, pokemon"
      )
      .addField(
        "• **INFO**",
        "servericon, membercount, help, serverinfo, badges, botinfo, embedgen, ping, serverinfo, servericon, snipe, uptime, userinfo, avatar")
      .addField(
        "• **MUSIC**",
        "loop, clear, disable-loop, join, leave, nowplaying, play, playlist, pause, queue, resume, shuffle, skip, stop, volume")
      .addField(
        "• **RANDOM**",
        "advice, slaps, waifu, fact, hug, joke, kiss, math, meme, pat, punch, neko, triggered, achievement, avatarfusion, panda, pp, wr")
     .addField(
        "• **HENTAI** (more commands to be added in the future)",
        "hentai")
      .setThumbnail(client.user.displayAvatarURL())
      .setFooter(
      `Sushi`,
      client.user.displayAvatarURL(),
    );

      message.channel.send(helpEmbed)

  },
};