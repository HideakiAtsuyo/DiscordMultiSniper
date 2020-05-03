using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordMultiSniper.Models
{
    public class Nitro
    {
        public string Type { get; set; }
        public string Server { get; set; }
        public DateTime DateOfClaim { get; set; }
        public double Cost { get; set; }
        public string Code { get; set; }
    }
}
