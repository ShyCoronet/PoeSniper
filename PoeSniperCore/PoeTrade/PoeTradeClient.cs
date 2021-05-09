using Newtonsoft.Json;
using PoeSniperCore.PoeTrade.Models;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace PoeSniperCore.PoeTrade {
    /// <summary>
    /// Provides methods for sending requests to www.poe.trade
    /// </summary>
    public class PoeTradeClient {
        private const string ROUTE = "/live";

        /// <summary>
        /// Send a POST request to get a response containing trade offers.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="url"></param>
        /// <exception cref="HttpRequestException"></exception>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<PoeTradeResponse> PostRequestAsync(string id, string url) {
            string address = url + ROUTE;
            var setup = new List<KeyValuePair<string, string>> {
                new KeyValuePair<string, string>("id", id)
            };
            var content = new FormUrlEncodedContent(setup);

            using (var client = new HttpClient()) {
                var response = await client.PostAsync(address, content);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    throw new HttpRequestException($"Request failed. Status code: {response.StatusCode}");

                return await Parse(response);
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
