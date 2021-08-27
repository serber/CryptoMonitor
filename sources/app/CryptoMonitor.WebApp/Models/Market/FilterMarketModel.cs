using CryptoMonitor.Data.Enums;
using CryptoMonitor.Services.Queries;

namespace CryptoMonitor.WebApp.Models.Market
{
    public class FilterMarketModel
    {
        public string BuySymbol { get; set; }

        public SymbolSource? SymbolSource { get; set; }

        public string Query { get; set; }

        public int Take { get; set; }

        public int Skip { get; set; }

        public string OrderBy { get; set; }

        internal ListSymbolPricesQuery ToQuery()
        {
            return new ListSymbolPricesQuery
            {
                BuySymbol = BuySymbol,
                SellSymbol = Query,
                SymbolSource = SymbolSource,
                Skip = Skip,
                Take = Take,
                OrderBy = !string.IsNullOrEmpty(OrderBy) ? OrderBy.TrimStart('^') : default,
                Asc = string.IsNullOrEmpty(OrderBy) || !OrderBy.StartsWith("^")
            };
        }
    }
}