using CryptoMonitor.Data.Enums;
using MediatR;

namespace CryptoMonitor.Services.Notifications
{
    public class LoadPricesNotification : INotification
    {
        public SymbolSource SymbolSource { get; set; }

        public string BuySymbol { get; set; }
    }
}