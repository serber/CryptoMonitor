using CryptoMonitor.Data;
using CryptoMonitor.DataAccess.Common.Repositories;
using CryptoMonitor.Services.Commands;
using CryptoMonitor.Services.Sources;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CryptoMonitor.Services.Handlers
{
    public class LoadPricesCommandHandler : IRequestHandler<LoadPricesCommand>
    {
        private readonly IStockPriceSourceFactory _stockPriceSourceFactory;

        private readonly ISymbolPriceRepository _symbolPriceRepository;

        private readonly ILogger<LoadPricesCommandHandler> _logger;

        public LoadPricesCommandHandler(IStockPriceSourceFactory stockPriceSourceFactory,
            ISymbolPriceRepository symbolPriceRepository,
            ILogger<LoadPricesCommandHandler> logger)
        {
            _stockPriceSourceFactory = stockPriceSourceFactory;
            _symbolPriceRepository = symbolPriceRepository;
            _logger = logger;
        }

        public async Task<Unit> Handle(LoadPricesCommand request, CancellationToken cancellationToken)
        {
            var stockPriceSource = _stockPriceSourceFactory.Create(request.SymbolSource);
            var priceDataCollection = await stockPriceSource.GetAsync(request.BuySymbol);

            var now = DateTime.UtcNow;

            var symbolPrices = priceDataCollection.Select(x => new SymbolPrice
            {
                Source = stockPriceSource.Source,
                LoadedAt = now,
                SellSymbol = x.Symbol.ToUpper(),
                BuySymbol = request.BuySymbol.ToUpper(),
                Price = x.Price,
                OpenPrice = x.OpenPrice,
                Change = x.OpenPrice is > 0 ? CalculateChange(x.OpenPrice.Value, x.Price) : null
            });

            await _symbolPriceRepository.AddAsync(symbolPrices);

            _logger.LogInformation($"Saved symbol prices for {request.SymbolSource}");

            return Unit.Value;
        }

        private static decimal CalculateChange(decimal previous, decimal current)
        {
            if (previous == 0)
            {
                throw new InvalidOperationException();
            }

            var change = current - previous;
            return Math.Round(change / previous, 4);
        }
    }
}