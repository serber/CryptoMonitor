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
                var dropPrice = await _dropPriceRepository.GetAsync(symbolPrice.SellSymbol, symbolPrice.BuySymbol, symbolPrice.Source);
                if (dropPrice != null)
                {
                    await _dropPriceRepository.UpdateSymbolPriceAsync(dropPrice.SellSymbol, dropPrice.BuySymbol, dropPrice.Source, symbolPrice.Price, dropPrice.Price / symbolPrice.Price);
                }
            }

            _logger.LogInformation("Drop prices updated");
        }
    }
}