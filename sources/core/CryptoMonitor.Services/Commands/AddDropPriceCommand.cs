using CryptoMonitor.Data.Enums;
using MediatR;

namespace CryptoMonitor.Services.Commands
{
    public class AddDropPriceCommand : IRequest
    {
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

        /// <summary>
        /// Цена выпадения
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public string UserId { get; set; }
    }
}