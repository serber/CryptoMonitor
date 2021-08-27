using System.Collections.Generic;
using CryptoMonitor.Data;
using CryptoMonitor.Data.Enums;
using MediatR;

namespace CryptoMonitor.Services.Queries
{
    public class ListSymbolPricesQuery : IRequest<(IReadOnlyCollection<SymbolPrice> Items, long TotalCount)>
    {
        public string BuySymbol { get; set; }
        
        public string SellSymbol { get; set; }

        public SymbolSource? SymbolSource { get; set; }

        public string OrderBy { get; set; }

        public bool Asc { get; set; }

        public int Take { get; set; }

        public int Skip { get; set; }
    }
}