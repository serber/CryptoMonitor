using System;
using System.Threading.Tasks;
using CryptoMonitor.Data;
using CryptoMonitor.Data.Enums;
using CryptoMonitor.DataAccess.Common.Repositories;
using CryptoMonitor.DataAccess.MongoDb;
using CryptoMonitor.DataAccess.MongoDb.Repositories;
using CryptoMonitor.Services.Commands;
using CryptoMonitor.Services.Sources;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using NLog.Extensions.Logging;

namespace CryptoMonitor.App
{
    internal static class Program
    {
        internal static async Task Main()
        {
            var serviceProvider = BuildServiceProvider();

            var mediator = serviceProvider.GetService<IMediator>();

            await mediator.Send(new LoadPricesCommand
            {
                SymbolSource = SymbolSource.Binance,
                BuySymbol = "USDT"
            });

            await mediator.Send(new LoadPricesCommand
            {
                SymbolSource = SymbolSource.Binance,
                BuySymbol = "BTC"
            });

            await mediator.Send(new LoadPricesCommand
            {
                SymbolSource = SymbolSource.Huobi,
                BuySymbol = "USDT"
            });

            await mediator.Send(new LoadPricesCommand
            {
                SymbolSource = SymbolSource.Huobi,
                BuySymbol = "BTC"
            });
        }

        private static IServiceProvider BuildServiceProvider()
        {
            DocumentMapping.Map();

            IConfiguration configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables()
                .Build();

            var serviceCollection =
                    new ServiceCollection()
                        .AddLogging(loggingBuilder =>
                        {
                            loggingBuilder.SetMinimumLevel(LogLevel.Trace);
                            loggingBuilder.AddNLog(configuration);
                        })
                        .AddMediatR(typeof(LoadPricesCommand))

                        .AddTransient<ISymbolPriceRepository, SymbolPriceRepository>()
                        .AddTransient<IUserRepository, UserRepository>()

                        .AddTransient<IStockPriceSourceFactory, StockPriceSourceFactory>()
                        .AddTransient<IMongoClient>(provider =>
                        {
                            var conectionString = configuration.GetConnectionString("MongoDb");
                            return new MongoClient(conectionString);
                        })
                        .AddTransient(provider =>
                        {
                            var mongoClient = provider.GetService<IMongoClient>();
                            return mongoClient.GetDatabase("cryptomonitor");
                        })
                        .AddMongoCollection<SymbolPrice>("symbol_price")
                        .AddMongoCollection<User>("user");

            serviceCollection
                .AddHttpClient<IStockPriceSource, HuobiStockPriceSource>(client => client.BaseAddress = new Uri(configuration.GetConnectionString("HuobiBaseUri")));

            serviceCollection
                .AddHttpClient<IStockPriceSource, BinanceStockPriceSource>(nameof(BinanceStockPriceSource), client => client.BaseAddress = new Uri(configuration.GetConnectionString("BinanceBaseUri")));

            return serviceCollection.BuildServiceProvider();
        }

        private static IServiceCollection AddMongoCollection<T>(this IServiceCollection serviceCollection, string name)
        {
            return serviceCollection.AddTransient(provider =>
            {
                var mongoDatabase = provider.GetService<IMongoDatabase>();
                return mongoDatabase.GetCollection<T>(name);
            });
        }
    }
}