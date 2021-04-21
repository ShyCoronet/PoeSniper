using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace PoeTrade {
    public class PoeTradeClient {
        private const string route = "/live";
        public async Task<PoeTradeResponse> PostIdToTradeAsync(string id, string url) {
            string address = url + route;
            var setup = new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>("id", id)
            };
            var content = new FormUrlEncodedContent(setup);

            using (var client = new HttpClient()) {
                var httpResponse = await client.PostAsync(address, content);

                return await Parse(httpResponse);
            }
        }

        private async Task<PoeTradeResponse> Parse(HttpResponseMessage httpResponse) {
            var stream = await httpResponse.Content.ReadAsStreamAsync();
            using (var reader = new StreamReader(stream)) {
                using (var jsonReader = new JsonTextReader(reader)) {
                    return JsonSerializer.CreateDefault().Deserialize<PoeTradeResponse>(jsonReader);
                }
            }
        }
    }
}
