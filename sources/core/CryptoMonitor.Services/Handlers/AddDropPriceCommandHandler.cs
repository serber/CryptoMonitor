using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoMonitor.Data;
using CryptoMonitor.DataAccess.Common.Repositories;
using CryptoMonitor.Services.Commands;
using MediatR;

namespace CryptoMonitor.Services.Handlers
{
    public class AddDropPriceCommandHandler : IRequestHandler<AddDropPriceCommand>
    {
        private readonly IDropPriceRepository _dropPriceRepository;

        private readonly ISymbolPriceRepository _symbolPriceRepository;

        public AddDropPriceCommandHandler(IDropPriceRepository dropPriceRepository, ISymbolPriceRepository symbolPriceRepository)
        {
            _dropPriceRepository = dropPriceRepository;
            _symbolPriceRepository = symbolPriceRepository;
        }

        public async Task<Unit> Handle(AddDropPriceCommand request, CancellationToken cancellationToken)
        {
            var symbolPrice = await _symbolPriceRepository.GetAsync(request.SellSymbol, request.BuySymbol, request.SymbolSource);

            var dropPrice = new DropPrice
            {
                BuySymbol = request.BuySymbol,
                SellSymbol = request.SellSymbol,
                Price = request.Price,
                Source = request.SymbolSource,
                UserId = request.UserId
            };

            if (symbolPrice != null)
            {
                dropPrice.Multiplier = Math.Round(request.Price / symbolPrice.Price, 4);
            }

            await _dropPriceRepository.AddAsync(dropPrice);

            return Unit.Value;
        }
    }
}