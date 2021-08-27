using System.Threading;
using System.Threading.Tasks;
using CryptoMonitor.DataAccess.Common.Repositories;
using CryptoMonitor.Services.Notifications;
using MediatR;
using Microsoft.Extensions.Logging;

namespace CryptoMonitor.Services.NotificationHandlers
{
    public class LoadPricesNotificationHandler : INotificationHandler<LoadPricesNotification>
    {
        private readonly ISymbolPriceRepository _symbolPriceRepository;

        private readonly IDropPriceRepository _dropPriceRepository;

        private readonly ILogger<LoadPricesNotificationHandler> _logger;

        public LoadPricesNotificationHandler(ISymbolPriceRepository symbolPriceRepository, IDropPriceRepository dropPriceRepository, ILogger<LoadPricesNotificationHandler> logger)
        {
            _symbolPriceRepository = symbolPriceRepository;
            _dropPriceRepository = dropPriceRepository;
            _logger = logger;
        }

        public async Task Handle(LoadPricesNotification notification, CancellationToken cancellationToken)
        {
            var symbolPrices =
                await _symbolPriceRepository.ListAsync(notification.BuySymbol, notification.SymbolSource);

            foreach (var symbolPrice in symbolPrices)
            {
                await _dropPriceRepository.UpdateSymbolPriceAsync(symbolPrice.SellSymbol, symbolPrice.BuySymbol, symbolPrice.Source, symbolPrice.Price);
            }

            _logger.LogInformation("Drop prices updated");
        }
    }
}