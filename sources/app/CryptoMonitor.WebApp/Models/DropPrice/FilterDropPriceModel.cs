using CryptoMonitor.Data.Enums;
using CryptoMonitor.Services.Queries;

namespace CryptoMonitor.WebApp.Models.DropPrice
{
    public class FilterDropPriceModel
    {
        public string BuySymbol { get; set; }

        public SymbolSource SymbolSource { get; set; }

        public string OrderBy { get; set; }

        internal ListDropPricesQuery ToQuery(string userId)
        {
            return new ListDropPricesQuery
            {
                UserId = userId,
                BuySymbol = BuySymbol,
                SymbolSource = SymbolSource,
                OrderBy = !string.IsNullOrEmpty(OrderBy) ? OrderBy.TrimStart('^') : default,
                Asc = string.IsNullOrEmpty(OrderBy) || !OrderBy.StartsWith("^")
            };
        }
    }
}