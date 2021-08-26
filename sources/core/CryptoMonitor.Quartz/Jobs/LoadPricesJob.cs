using System.Threading.Tasks;
using CryptoMonitor.Data.Enums;
using CryptoMonitor.Services.Commands;
using MediatR;
using Quartz;

namespace CryptoMonitor.Quartz.Jobs
{
    public class LoadPricesJob : IJob
    {
        private readonly IMediator _mediator;

        public LoadPricesJob(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            await _mediator.Send(new LoadPricesCommand
            {
                SymbolSource = SymbolSource.Binance,
                BuySymbol = "USDT"
            });

            await _mediator.Send(new LoadPricesCommand
            {
                SymbolSource = SymbolSource.Binance,
                BuySymbol = "BTC"
            });

            await _mediator.Send(new LoadPricesCommand
            {
                SymbolSource = SymbolSource.Huobi,
                BuySymbol = "USDT"
            });

            await _mediator.Send(new LoadPricesCommand
            {
                SymbolSource = SymbolSource.Huobi,
                BuySymbol = "BTC"
            });
        }
    }
}