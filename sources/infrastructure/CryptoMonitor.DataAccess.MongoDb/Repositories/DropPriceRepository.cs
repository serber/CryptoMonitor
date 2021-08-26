using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoMonitor.Data;
using CryptoMonitor.Data.Enums;
using CryptoMonitor.DataAccess.Common.Repositories;
using MongoDB.Driver;

namespace CryptoMonitor.DataAccess.MongoDb.Repositories
{
    public class DropPriceRepository : IDropPriceRepository
    {
        private readonly IMongoCollection<DropPrice> _mongoCollection;

        public DropPriceRepository(IMongoCollection<DropPrice> mongoCollection)
        {
            _mongoCollection = mongoCollection;
        }

        public async Task AddAsync(DropPrice dropPrice)
        {
            var filter = Builders<DropPrice>.Filter.And(
                    Builders<DropPrice>.Filter.Eq(x => x.SellSymbol, dropPrice.SellSymbol),
                    Builders<DropPrice>.Filter.Eq(x => x.BuySymbol, dropPrice.BuySymbol),
                    Builders<DropPrice>.Filter.Eq(x => x.UserId, dropPrice.UserId),
                    Builders<DropPrice>.Filter.Eq(x => x.Source, dropPrice.Source));

            var update = Builders<DropPrice>.Update
                    .Set(x => x.Price, dropPrice.Price)
                    .Set(x => x.Multiplier, dropPrice.Multiplier);

            await _mongoCollection.UpdateOneAsync(filter, update, new UpdateOptions { IsUpsert = true });
        }

        public async Task DeleteAsync(string userId, string sellSymbol, string buySymbol, SymbolSource symbolSource)
        {
            var filter = Builders<DropPrice>.Filter.And(
                Builders<DropPrice>.Filter.Eq(x => x.SellSymbol, sellSymbol),
                Builders<DropPrice>.Filter.Eq(x => x.BuySymbol, buySymbol),
                Builders<DropPrice>.Filter.Eq(x => x.UserId, userId),
                Builders<DropPrice>.Filter.Eq(x => x.Source, symbolSource));

            await _mongoCollection.DeleteOneAsync(filter);
        }

        public async Task<(IReadOnlyCollection<DropPrice> Items, long Count)> ListAsync(string userId, string buySymbol, SymbolSource symbolSource, string orderBy, bool asc)
        {
            var filter = Builders<DropPrice>.Filter.And(
                Builders<DropPrice>.Filter.Eq(x => x.BuySymbol, buySymbol),
                Builders<DropPrice>.Filter.Eq(x => x.UserId, userId),
                Builders<DropPrice>.Filter.Eq(x => x.Source, symbolSource));

            var count = await _mongoCollection.CountDocumentsAsync(filter);

            var findOptions = new FindOptions<DropPrice, DropPrice>
            {
                Sort = orderBy?.ToLower() switch
                {
                    "price" => asc ? Builders<DropPrice>.Sort.Ascending(x => x.Price) : Builders<DropPrice>.Sort.Descending(x => x.Price),
                    "multiplier" => asc ? Builders<DropPrice>.Sort.Ascending(x => x.Multiplier) : Builders<DropPrice>.Sort.Descending(x => x.Multiplier),
                    _ => asc ? Builders<DropPrice>.Sort.Ascending(x => x.SellSymbol) : Builders<DropPrice>.Sort.Descending(x => x.SellSymbol)
                }
            };

            using var cursor = await _mongoCollection.FindAsync(filter, findOptions);
            var result = new List<DropPrice>();

            while (await cursor.MoveNextAsync())
            {
                result.AddRange(cursor.Current);
            }

            return (result, count);
        }
    }
}