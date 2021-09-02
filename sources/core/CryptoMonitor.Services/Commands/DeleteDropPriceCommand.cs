using CryptoMonitor.Data.Enums;
using MediatR;

namespace CryptoMonitor.Services.Commands
{
    public class DeleteDropPriceCommand : IRequest
    {
        public string UserId { get; set; }

        /// <summary>
        /// Валюта продажи
        /// </summary>
        public string SellSymbol { get; set; }

        /// <summary>
        /// Валюта покупки. BTC/USDT
        /// </summary>
        public string BuySymbol { get; set; }

        /// <summary>
        /// Источник
        /// </summary>
        public SymbolSource SymbolSource { get; set; }
    }
}