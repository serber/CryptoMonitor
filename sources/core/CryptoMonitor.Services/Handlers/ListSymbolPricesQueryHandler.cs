using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CryptoMonitor.Data;
using CryptoMonitor.DataAccess.Common.Repositories;
using CryptoMonitor.Services.Queries;
using MediatR;

namespace CryptoMonitor.Services.Handlers
{
    public class ListSymbolPricesQueryHandler : IRequestHandler<ListSymbolPricesQuery, (IReadOnlyCollection<SymbolPrice> Items, long TotalCount)>
    {
        private readonly ISymbolPriceRepository _symbolPriceRepository;

        public ListSymbolPricesQueryHandler(ISymbolPriceRepository symbolPriceRepository)
        {
            _symbolPriceRepository = symbolPriceRepository;
        }

        public async Task<(IReadOnlyCollection<SymbolPrice> Items, long TotalCount)> Handle(ListSymbolPricesQuery request, CancellationToken cancellationToken)
        {
            return await _symbolPriceRepository.ListAsync(request.Skip, request.Take, default, request.BuySymbol, request.SymbolSource, request.Query, request.OrderBy, request.Asc);
        }
    }
}