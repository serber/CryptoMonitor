using CryptoMonitor.Data.Enums;

namespace CryptoMonitor.WebApp.Models.DropPrice
{
    public class AddDropPriceModel
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
    }
}