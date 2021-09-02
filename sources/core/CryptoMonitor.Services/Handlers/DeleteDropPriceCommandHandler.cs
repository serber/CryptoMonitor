using System.Threading;
using System.Threading.Tasks;
using CryptoMonitor.DataAccess.Common.Repositories;
using CryptoMonitor.Services.Commands;
using MediatR;

namespace CryptoMonitor.Services.Handlers
{
    public class DeleteDropPriceCommandHandler : IRequestHandler<DeleteDropPriceCommand>
    {
        private readonly IDropPriceRepository _dropPriceRepository;

        public DeleteDropPriceCommandHandler(IDropPriceRepository dropPriceRepository)
        {
            _dropPriceRepository = dropPriceRepository;
        }

        public async Task<Unit> Handle(DeleteDropPriceCommand request, CancellationToken cancellationToken)
        {
            await _dropPriceRepository.DeleteAsync(request.UserId, request.SellSymbol, request.BuySymbol,
                request.SymbolSource);

            return Unit.Value;
        }
    }
}