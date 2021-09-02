using System.Threading;
using System.Threading.Tasks;
using CryptoMonitor.Data;
using CryptoMonitor.DataAccess.Common.Repositories;
using CryptoMonitor.Services.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CryptoMonitor.Services.Handlers
{
    public class AddDropPriceCommandHandler : IRequestHandler<AddDropPriceCommand>
    {
        private readonly IDropPriceRepository _dropPriceRepository;

        private readonly ISymbolPriceRepository _symbolPriceRepository;

        private readonly ILogger<AddDropPriceCommandHandler> _logger;

        public AddDropPriceCommandHandler(IDropPriceRepository dropPriceRepository, ISymbolPriceRepository symbolPriceRepository, ILogger<AddDropPriceCommandHandler> logger)
        {
            _dropPriceRepository = dropPriceRepository;
            _symbolPriceRepository = symbolPriceRepository;
            _logger = logger;
        }

        public async Task<Unit> Handle(AddDropPriceCommand request, CancellationToken cancellationToken)
        {
            var symbolPrice =
                await _symbolPriceRepository.GetAsync(request.SellSymbol, request.BuySymbol, request.SymbolSource);

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
                dropPrice.SymbolPrice = symbolPrice.Price;
                dropPrice.Multiplier = request.Price / symbolPrice.Price;
            }
            
            await _dropPriceRepository.AddAsync(dropPrice);

            _logger.LogInformation("Created drop price");

            return Unit.Value;
        }
    }
}