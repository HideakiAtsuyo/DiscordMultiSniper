using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace DiscordMultiSniper
{
    public class Config
    {
        [JsonProperty("auth_token")]
        public string Token { get; set; }
    }
}
