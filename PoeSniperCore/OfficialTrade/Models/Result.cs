using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoeSniperCore.OfficialTrade.Models {
    public class Result {
        [JsonProperty("item")]
        public Item Item { get; set; }

        [JsonProperty("listing")]
        public Listing Listing { get; set; }
    }
}
