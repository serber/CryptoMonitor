using CryptoMonitor.Data.Enums;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CryptoMonitor.Services.Sources
{
    public class BinanceStockPriceSource : IStockPriceSource
    {
        private readonly HttpClient _httpClient;

        public BinanceStockPriceSource(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public SymbolSource Source => SymbolSource.Binance;

        public async Task<IReadOnlyCollection<(string Symbol, decimal Price, decimal? OpenPrice)>> GetAsync(string buySymbol)
        {
            var response = await _httpClient.GetAsync("api/v3/ticker/price");

            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var dataArray = JArray.Parse(json);

            var result = new List<(string, decimal, decimal?)>();

            foreach (var token in dataArray)
            {
                var symbol = token.Value<string>("symbol");
                var price = token.Value<decimal>("price");

                if (!string.IsNullOrEmpty(symbol) &&
                    symbol.EndsWith(buySymbol, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add((symbol.Replace(buySymbol, string.Empty, StringComparison.OrdinalIgnoreCase), price, null));
                }
            }

            return result;
        }
    }
}