using CryptoMonitor.Data.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CryptoMonitor.Services.Sources
{
    /// <summary>
    /// Клиент для https://api.huobi.pro
    /// </summary>
    public class HuobiStockPriceSource : IStockPriceSource
    {
        private readonly HttpClient _httpClient;

        public HuobiStockPriceSource(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public SymbolSource Source => SymbolSource.Huobi;

        public async Task<IReadOnlyCollection<(string Symbol, decimal Price, decimal? OpenPrice)>> GetAsync(string buySymbol)
        {
            var response = await _httpClient.GetAsync("market/tickers");

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var jobject = JObject.Parse(json);

            var dataArray = (JArray)jobject["data"];

            if (dataArray == null)
            {
                return Array.Empty<(string symbol, decimal price, decimal? openPrice)>();
            }

            var result = new List<(string, decimal, decimal?)>();

            foreach (var token in dataArray)
            {
                var symbol = token.Value<string>("symbol");
                var price = token.Value<decimal>("bid");
                var open = token.Value<decimal>("open");

                if (!string.IsNullOrEmpty(symbol) &&
                    symbol.EndsWith(buySymbol, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add((symbol.Replace(buySymbol, string.Empty, StringComparison.OrdinalIgnoreCase), price, open));
                }
            }

            return result;
        }
    }
}