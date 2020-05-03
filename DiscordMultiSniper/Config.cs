using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace DiscordMultiSniper
{
    public class Config
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("nitrosniper")]
        public string NitroSniper { get; set; }

        [JsonProperty("giveawaysniper")]
        public string GiveawaySniper { get; set; }

        [JsonProperty("privnotesniper")]
        public string PrivnoteSniper { get; set; }

        [JsonProperty("slotbotsniper")]
        public string SlotbotSniper { get; set; }

    }
}
