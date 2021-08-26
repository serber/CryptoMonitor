using CryptoMonitor.Data.Enums;
using System.Collections.Generic;
using System.Linq;

namespace CryptoMonitor.Services.Sources
{
    public class StockPriceSourceFactory : IStockPriceSourceFactory
    {
        private readonly IEnumerable<IStockPriceSource> _stockPriceSources;

        public StockPriceSourceFactory(IEnumerable<IStockPriceSource> stockPriceSources)
        {
            _stockPriceSources = stockPriceSources;
        }

        public IStockPriceSource Create(SymbolSource symbolSource)
        {
            return _stockPriceSources.Single(x => x.Source == symbolSource);
        }
    }
}