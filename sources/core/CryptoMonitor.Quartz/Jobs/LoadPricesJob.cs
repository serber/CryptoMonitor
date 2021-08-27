using System.Threading.Tasks;
using CryptoMonitor.Data.Enums;
using CryptoMonitor.Services.Commands;
using CryptoMonitor.Services.Notifications;
using MediatR;
using Microsoft.Extensions.Logging;
using Quartz;

namespace CryptoMonitor.Quartz.Jobs
{
    public class LoadPricesJob : IJob
    {
        private readonly IMediator _mediator;

        private readonly ILogger<LoadPricesJob> _logger;

        public LoadPricesJob(IMediator mediator, ILogger<LoadPricesJob> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await ExecuteInternalAsync(SymbolSource.Binance, "USDT");

            await ExecuteInternalAsync(SymbolSource.Binance, "BTC");

            await ExecuteInternalAsync(SymbolSource.Huobi, "USDT");

            await ExecuteInternalAsync(SymbolSource.Huobi, "BTC");
        }

        private async Task ExecuteInternalAsync(SymbolSource symbolSource, string buySymbol)
        {
            using (_logger.BeginScope($"SymbolSource = {symbolSource}, BuySymbol = {buySymbol}"))
            {
                await _mediator.Send(new LoadPricesCommand
                {
                    SymbolSource = symbolSource,
                    BuySymbol = buySymbol
                });

                await _mediator.Publish(new LoadPricesNotification
                {
                    SymbolSource = symbolSource,
                    BuySymbol = buySymbol
                });
            }
        }
    }
}