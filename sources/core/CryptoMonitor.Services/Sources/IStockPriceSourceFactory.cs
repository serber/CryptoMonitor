using CryptoMonitor.Data.Enums;

namespace CryptoMonitor.Services.Sources
{
    public interface IStockPriceSourceFactory
    {
        IStockPriceSource Create(SymbolSource symbolSource);
    }
}