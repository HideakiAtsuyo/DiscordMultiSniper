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
    /// <summary>
    /// This class contains helper methods related to discord
    /// </summary>
    public static class DiscordUtils
    {
        /// <summary>
        /// Gets authentication token from the local file if exists or pends for the user to enter it if doesn't
        /// </summary>
        public static string GetAuthToken()
        {
            Config config = JsonConvert.DeserializeObject<Config>(File.ReadAllText("Config.json"));
            while (string.IsNullOrEmpty(config.Token))
            {
                Console.Write("Discord Token: ");
                config.Token = Console.ReadLine();
                File.WriteAllText("Config.json", JsonConvert.SerializeObject(config));
            }
            return config.Token;
        }

        /// <summary>
        /// Checks if a message contains a valid nitro code
        /// </summary>
        /// <param name="message"></param>
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

        /// <summary>
        /// Checks if a messages comes from two of the most known giveaway bots
        /// </summary>
        /// <param name="args"></param>
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
            string serverName = client.GetGuild(args.Message.GuildId.Value).Name;
            try
            {
                client.AddMessageReaction(client.GetChannel(args.Message.ChannelId).Id, args.Message.Id, "\ud83c\udf89");
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
    }
}
