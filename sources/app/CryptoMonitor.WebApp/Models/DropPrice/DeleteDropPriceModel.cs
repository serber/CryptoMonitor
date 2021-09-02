using CryptoMonitor.Data.Enums;

namespace CryptoMonitor.WebApp.Models.DropPrice
{
    public class DeleteDropPriceModel
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
    }
}