using CryptoMonitor.Data.Enums;
using MediatR;

namespace CryptoMonitor.Services.Commands
{
    public class LoadPricesCommand : IRequest
    {
        public SymbolSource SymbolSource { get; set; }
        
        public string BuySymbol { get; set; }
    }
}