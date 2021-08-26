using System;
using CryptoMonitor.Data;
using CryptoMonitor.DataAccess.Common.Repositories;
using CryptoMonitor.DataAccess.MongoDb;
using CryptoMonitor.DataAccess.MongoDb.Repositories;
using CryptoMonitor.Quartz;
using CryptoMonitor.Quartz.Jobs;
using CryptoMonitor.Services.Commands;
using CryptoMonitor.Services.Sources;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Quartz;

namespace CryptoMonitor.WebApp.Registries
{
    internal static class ServicesRegistry
    {
        internal static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
        {
            DocumentMapping.Map();

            services
                .Configure<SchedulerOptions>(configuration.GetSection(nameof(SchedulerOptions)))
                .AddHostedService<SchedulerHost>()
                .AddQuartz(configurator =>
                {
                    configurator.UseInMemoryStore();
                    configurator.UseMicrosoftDependencyInjectionJobFactory();
                })
                .AddScoped<LoadPricesJob>()

                .AddMediatR(typeof(LoadPricesCommand))

                .AddTransient<ISymbolPriceRepository, SymbolPriceRepository>()
                .AddTransient<IUserRepository, UserRepository>()
                .AddTransient<IDropPriceRepository, DropPriceRepository>()

                .AddTransient<IStockPriceSourceFactory, StockPriceSourceFactory>()
                .AddTransient<IMongoClient>(_ => new MongoClient(configuration.GetConnectionString("MongoDb")))
                .AddTransient(provider =>
                {
                    var mongoClient = provider.GetRequiredService<IMongoClient>();
                    return mongoClient.GetDatabase("cryptomonitor");
                })
                .AddMongoCollection<SymbolPrice>("symbol_price")
                .AddMongoCollection<User>("user")
                .AddMongoCollection<DropPrice>("drop_price");

            services
                .AddHttpClient<IStockPriceSource, HuobiStockPriceSource>(client => client.BaseAddress = new Uri(configuration.GetConnectionString("HuobiBaseUri")));

            services
                .AddHttpClient<IStockPriceSource, BinanceStockPriceSource>(nameof(BinanceStockPriceSource), client => client.BaseAddress = new Uri(configuration.GetConnectionString("BinanceBaseUri")));

            return services;
        }

        private static IServiceCollection AddMongoCollection<T>(this IServiceCollection serviceCollection, string name)
        {
            return serviceCollection.AddTransient(provider =>
            {
                var mongoDatabase = provider.GetRequiredService<IMongoDatabase>();
                return mongoDatabase.GetCollection<T>(name);
            });
        }
    }
}