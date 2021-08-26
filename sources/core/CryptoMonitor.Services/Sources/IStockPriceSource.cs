using CryptoMonitor.Data.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoMonitor.Services.Sources
{
    public interface IStockPriceSource
    {
        SymbolSource Source { get; }

        Task<IReadOnlyCollection<(string Symbol, decimal Price, decimal? OpenPrice)>> GetAsync(string buySymbol);
    }
}