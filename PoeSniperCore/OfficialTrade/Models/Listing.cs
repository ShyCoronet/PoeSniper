using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoeSniperCore.OfficialTrade.Models {
    public class Listing {
        [JsonProperty("account")]
        public Account Account { get; set; }

        [JsonProperty("price")]
        public Price Price { get; set; }

        [JsonProperty("stash")]
        public Stash Stash { get; set; }
    }
}
