using System;
using System.Threading.Tasks;
using Discord;
using Discord.Gateway;
using DiscordMultiSniper.Utilities;
using DiscordMultiSniper.Models;

namespace DiscordMultiSniper
{
    class Program
    {
        public static DiscordSocketClient Client { get; private set; }

        static void Main(string[] args)
        {
            MainAsync(args).GetAwaiter().GetResult();
        }

        static async Task MainAsync(string[] args)
        {
            string token = DiscordUtils.GetAuthToken();

            Client = new DiscordSocketClient();
            Client.OnLoggedIn += Client_OnLoggedIn;
            Client.OnMessageReceived += Client_OnMessageReceived;
            Client.Login(token);

            await Task.Delay(-1);
        }

        private static void Client_OnMessageReceived(DiscordSocketClient client, MessageEventArgs args)
        {
            if (DiscordUtils.IsNitro(args.Message.Content))
            {
                int startIndexNitroCode= args.Message.Content.IndexOf("discord.gift/");
                string code = args.Message.Content.Substring(startIndexNitroCode);
                try
                {
                    Client.RedeemNitroGift(code, args.Message.ChannelId);

                    Models.Nitro nitroInfo = DiscordUtils.GetNitroInfo(code, Client);
                    Console.WriteLine("[SUCCESS] Nitro gift redeemed \nCode:{0} Type:{1} Price:{2} Date:{3}",nitroInfo.Code, nitroInfo.Type, nitroInfo.Cost, nitroInfo.DateOfClaim);
                }
                catch (DiscordHttpException ex)
                {
                    switch (ex.Code)
                    {
                        case DiscordError.NitroGiftRedeemed:
                            Console.WriteLine("[ERROR] Nitro gift already redeemed: " + code);
                            break;
                        case DiscordError.UnknownGiftCode:
                            Console.WriteLine("[ERROR] Invalid nitro gift: " + code);
                            break;
                        default:
                            Console.WriteLine($"[ERROR] Unknown error: {ex.Code} | {ex.ErrorMessage}");
                            break;
                    }
                }
            }
            else if (DiscordUtils.IsGiveaway(args))
            {
                string serverName = Client.GetGuild(args.Message.GuildId.Value).Name;
                if (args.Message.Author.User.Id.ToString() == "582537632991543307") //Santa Giveaway bot's ID
                {
                    if (args.Message.Content.Contains("Generating..."))
                    {
                        DiscordUtils.JoinGiveaway(client, args);
                    }
                    else if (args.Message.Content.Contains(string.Format("<@{0}>", Program.Client.User.Id)))
                    {
                        Console.WriteLine("[WON] Giveaway in Server: {0}", serverName);
                    }
                }
                else if(args.Message.Author.User.Id.ToString() == "294882584201003009") //Giveaway bot's ID
                {
                    if(!args.Message.Content.Contains("A winner could not be determined!") && 
                        !args.Message.Content.Contains("Giveaway time must not be shorter than") && 
                        !args.Message.Content.Contains("\ud83d\udca5") && 
                        !args.Message.Content.Contains("Please type the name of a channel in this server.") && 
                        !args.Message.Content.Contains("how long should the giveaway last?") && 
                        !args.Message.Content.Contains("Now, how many winners should there be?") && 
                        !args.Message.Content.Contains("Finally, what do you want to give away?") && 
                        !args.Message.Content.Contains("The new winner is") && 
                        !args.Message.Content.Contains("Done! The giveaway for the") &&
                        !args.Message.Content.Contains("Congratulations"))
                    {
                        DiscordUtils.JoinGiveaway(client, args);
                    }
                    else if (args.Message.Content.Contains(string.Format("<@{0}>", Program.Client.User.Id)))
                    {
                        Console.WriteLine("[WON] Giveaway in Server: {0}", serverName);
                    }
                }
                
                
            }
        }

        private static void Client_OnLoggedIn(DiscordSocketClient client, LoginEventArgs args)
        {
            Console.WriteLine($"Successfully logged into {args.User}!");
        }
    }
}
