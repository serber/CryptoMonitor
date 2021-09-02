using CryptoMonitor.Data.Enums;

namespace CryptoMonitor.Data
{
    public class DropPrice
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
        public SymbolSource Source { get; set; }

        /// <summary>
        /// Цена выпадения
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Идентификатор пользователя
        /// </summary>
        public string UserId { get; set; }

        public decimal SymbolPrice { get; set; }
        
        public decimal Multiplier { get; set; }
    }
}