using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PoeSniperCore {
    public class SniperOption {

        private Dictionary<string, string> parameters;

        public SniperOption() { }
        public SniperOption(Dictionary<string, string> param) {
            parameters = param;
        }

        public string this[string key] {
            get => parameters[key];

            set => parameters[key] = value;
        }
    }
}
