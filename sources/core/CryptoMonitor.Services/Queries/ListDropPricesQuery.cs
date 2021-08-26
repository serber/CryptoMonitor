using System.Collections.Generic;
using CryptoMonitor.Data;
using CryptoMonitor.Data.Enums;
using MediatR;

namespace CryptoMonitor.Services.Queries
{
    public class ListDropPricesQuery : IRequest<(IReadOnlyCollection<DropPrice> Items, long TotalCount)>
    {
        public string UserId { get; set; }

        /// <summary>
        /// Валюта покупки. BTC/USDT
        /// </summary>
        public string BuySymbol { get; set; }

        /// <summary>
        /// Источник
        /// </summary>
        public SymbolSource SymbolSource { get; set; }

        public string OrderBy { get; set; }

        public bool Asc { get; set; }
    }
}