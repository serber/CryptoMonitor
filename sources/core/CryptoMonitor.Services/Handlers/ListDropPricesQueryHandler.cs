using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CryptoMonitor.Data;
using CryptoMonitor.DataAccess.Common.Repositories;
using CryptoMonitor.Services.Queries;
using MediatR;

namespace CryptoMonitor.Services.Handlers
{
    public class ListDropPricesQueryHandler : IRequestHandler<ListDropPricesQuery, (IReadOnlyCollection<DropPrice> Items, long TotalCount)>
    {
        private readonly IDropPriceRepository _dropPriceRepository;

        public ListDropPricesQueryHandler(IDropPriceRepository dropPriceRepository)
        {
            _dropPriceRepository = dropPriceRepository;
        }

        public async Task<(IReadOnlyCollection<DropPrice> Items, long TotalCount)> Handle(ListDropPricesQuery request, CancellationToken cancellationToken)
        {
            return await _dropPriceRepository.ListAsync(request.UserId, request.BuySymbol, request.SymbolSource, request.OrderBy, request.Asc);
        }
    }
}