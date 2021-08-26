using CryptoMonitor.Data.Enums;
using System;

namespace CryptoMonitor.Data
{
    public class SymbolPrice
    {
        public string SellSymbol { get; set; }

        /// <summary>
        /// BTC/USDT
        /// </summary>
        public string BuySymbol { get; set; }

        public SymbolSource Source { get; set; }

        public decimal Price { get; set; }

        public DateTime LoadedAt { get; set; }

        public decimal? Change { get; set; }
        
        public decimal? OpenPrice { get; set; }
    }
}