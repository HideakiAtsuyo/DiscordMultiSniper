using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Discord;
using Discord.Gateway;
using DiscordMultiSniper.Models;
namespace DiscordMultiSniper.Utilities
{
    public static class DiscordUtils
    {
        public static string GetAuthToken()
        {
            Config config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("config.json"));
            while (string.IsNullOrEmpty(config.Token))
            {
                Console.Write("Discord Token: ");
                config.Token = Console.ReadLine();
                File.WriteAllText("config.json", JsonConvert.SerializeObject(config));
            }
            return config.Token;
        }

        public static bool IsNitro(string message)
        {
            if((message.Contains("discord.gift/") && message.Length == 37) || (message.Contains("discordapp.com/gifts/") && message.Length == 53))
            {
                return true;
            }
            return false;
        }

        public static Models.Nitro GetNitroInfo(string nitroCode, DiscordSocketClient client)
        {
            Models.Nitro nitro = new Models.Nitro();
            string nitroInfo = client.GetNitroGift(nitroCode).SubscriptionPlan.Name;

            if (nitroInfo.Contains("Classic"))
            {
                nitro.Type = "Classic";

                if (nitroInfo.Contains("Yearly"))
                {
                    nitro.Cost = 49.99;
                }
                else
                {
                    nitro.Cost = 4.99;
                }
            }
            else
            {
                nitro.Type = "Booster";

                if (nitroInfo.Contains("Yearly"))
                {
                    nitro.Cost = 99.99;
                }
                else if (nitroInfo.Contains("Quarterly"))
                {
                    nitro.Cost = 29.97;
                }
                else
                {
                    nitro.Cost = 9.99;
                }
            }

            nitro.Code = nitroCode;
            nitro.DateOfClaim = DateTime.Now;

            return nitro;
        }

        public static bool IsGiveaway(MessageEventArgs args)
        {
            if (args.Message.Author.User.Id.ToString() == "582537632991543307" || args.Message.Author.User.Id.ToString() == "294882584201003009")
            {
                return true;
            }
            return false;
        }

        public static bool JoinGiveaway(DiscordSocketClient client, MessageEventArgs args)
        {
            string serverName = client.GetGuild(args.Message.Guild.Id).Name;
            try
            {
                client.AddMessageReaction(client.GetChannel(args.Message.Channel.Id).Id, args.Message.Id, "\ud83c\udf89");
                StringBuilder sbGiveawayNotification = new StringBuilder("[Joined giveaway] Server: ");
                sbGiveawayNotification.Append(serverName);
                Console.WriteLine(sbGiveawayNotification.ToString());
                return true;
            }
            catch
            {
                Console.WriteLine("[ERROR] Could not join to the giveaway. Server:{0} Date:{1}", serverName, DateTime.Now);
                return false;
            }
        }

        public static bool IsSlotbot(MessageEventArgs args)
        {
            if (args.Message.Author.User.Id.ToString() == "346353957029019648")
            {
                return true;
            }
            return false;
        }

        //public static bool GrabSlotbot(DiscordSocketClient client, MessageEventArgs args)
        public static bool GrabSlotbot(DiscordSocketClient client, MessageEventArgs args)
        {


            string serverName = client.GetGuild(args.Message.Guild.Id).Name;
            try
            {
                args.Message.Channel.SendMessage("~grab");
                return true;
            } catch (Exception){
                Console.WriteLine("[ERROR] Could not send the message. Server:{0} Date:{1}", serverName, DateTime.Now);
                //SlotBotData()
                return false;
            }

        }

        public static bool IsPrivnote(MessageEventArgs args)
        {
            if (args.Message.Author.User.Id.ToString() == "346353957029019648")
            {
                return true;
            }
            return false;
        }

        //public static bool GrabSlotbot(DiscordSocketClient client, MessageEventArgs args)
        public static bool GrabPrivnote(DiscordSocketClient client, MessageEventArgs args)
        {


            string serverName = client.GetGuild(args.Message.Guild.Id).Name;
            try
            {
                args.Message.Channel.SendMessage("~grab");
                return true;
            }
            catch (Exception)
            {
                Console.WriteLine("[ERROR] Could not send the message. Server:{0} Date:{1}", serverName, DateTime.Now);
                //SlotBotData()
                return false;
            }

        }
    }
}
