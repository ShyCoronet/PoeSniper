using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoeSniperCore.OfficialTrade.Models {
    public class Item {
        [JsonProperty("baseType")]
        public string Name { get; set; }

        [JsonProperty("league")]
        public string League { get; set; }
    }
}
