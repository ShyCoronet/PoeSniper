using Newtonsoft.Json;
using PoeSniperCore.OfficialTrade.Models;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace PoeSniperCore.OfficialTrade {
    /// <summary>
    /// Provides methods for sending requests to www.pathofexile.com/trade.
    /// </summary>
    public class OfficialTradeClient {
        private const string REQUESTURL = "https://www.pathofexile.com/api/trade/fetch/";
        private RequestOptions options;

        public OfficialTradeClient(RequestOptions options) {
            this.options = options;
        }

        /// <summary>
        /// Send a GET request to get a response containing trade offers.
        /// </summary>
        /// <param name="searchId"></param>
        /// <param name="offersId"></param>
        /// <exception cref="HttpRequestException"></exception>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public async Task<OfficialTradeResponse> GetResponseAsync(string searchId, IEnumerable<string> offersId) {
            string address = REQUESTURL + CreateRoute(searchId, offersId);

            using (var client = CreateClientWithHeaders()) {
                var response = await client.GetAsync(address);

                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    throw new HttpRequestException($"Request failed. Status code: {response.StatusCode}");

                return await Parse(response);
            }
        }

        private string CreateRoute(string searchId, IEnumerable<string> offersId) {
            string sequenceId = string.Join(',', offersId);
            string route = $"{sequenceId}?queru={searchId}";

            return route;
        }

        private HttpClient CreateClientWithHeaders() {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Add("Cookie", options.Cookie);
            client.DefaultRequestHeaders.Add("Origin", options.Origin);
            client.DefaultRequestHeaders.Add("User-Agent", options.UserAgent);

            return client;
        }

        private async Task<OfficialTradeResponse> Parse(HttpResponseMessage httpResponse) {
            var stream = await httpResponse.Content.ReadAsStreamAsync();

            using (var reader = new StreamReader(stream)) {
                using (var textReader = new JsonTextReader(reader)) {
                    return JsonSerializer.CreateDefault().Deserialize<OfficialTradeResponse>(textReader);
                }
            }
        }
    }
}
