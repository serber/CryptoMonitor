using CryptoMonitor.Data;
using CryptoMonitor.DataAccess.Common.Repositories;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoMonitor.Data.Enums;
using MongoDB.Bson;

namespace CryptoMonitor.DataAccess.MongoDb.Repositories
{
    public class SymbolPriceRepository : ISymbolPriceRepository
    {
        private readonly IMongoCollection<SymbolPrice> _mongoCollection;

        public SymbolPriceRepository(IMongoCollection<SymbolPrice> mongoCollection)
        {
            _mongoCollection = mongoCollection;
        }

        public async Task AddAsync(IEnumerable<SymbolPrice> symbolPrices)
        {
            var bulkOperations = new List<UpdateOneModel<SymbolPrice>>();

            foreach (var symbolPrice in symbolPrices)
            {
                var filter = Builders<SymbolPrice>.Filter.And(
                    Builders<SymbolPrice>.Filter.Eq(s => s.SellSymbol, symbolPrice.SellSymbol),
                    Builders<SymbolPrice>.Filter.Eq(s => s.BuySymbol, symbolPrice.BuySymbol),
                    Builders<SymbolPrice>.Filter.Eq(s => s.Source, symbolPrice.Source));

                var update = Builders<SymbolPrice>.Update
                    .Set(s => s.Price, symbolPrice.Price)
                    .Set(s => s.LoadedAt, symbolPrice.LoadedAt)
                    .Set(s => s.Change, symbolPrice.Change)
                    .Set(s => s.OpenPrice, symbolPrice.OpenPrice)
                    ;

                var upsertOne = new UpdateOneModel<SymbolPrice>(filter, update) { IsUpsert = true };

                bulkOperations.Add(upsertOne);
            }

            await _mongoCollection.BulkWriteAsync(bulkOperations);
        }

        public async Task<SymbolPrice> GetAsync(string sellSymbol, string buySymbol, SymbolSource symbolSource)
        {
            var filter = Builders<SymbolPrice>.Filter.And(Builders<SymbolPrice>.Filter.Eq(x => x.SellSymbol, sellSymbol),
                Builders<SymbolPrice>.Filter.Eq(x => x.BuySymbol, buySymbol),
                Builders<SymbolPrice>.Filter.Eq(x => x.Source, symbolSource));

            using var cursor = await _mongoCollection.FindAsync(filter);
            return await cursor.SingleOrDefaultAsync();
        }

        public async Task<(IReadOnlyCollection<SymbolPrice> Items, long TotalCount)> ListAsync(int skip = 0, int take = 50, string sellSymbol = null, string buySymbol = null, SymbolSource? symbolSource = null, string query = null, string orderBy = null, bool asc = true)
        {
            var filterDefinitions = new List<FilterDefinition<SymbolPrice>>();

            if (!string.IsNullOrEmpty(sellSymbol))
            {
                filterDefinitions.Add(Builders<SymbolPrice>.Filter.Eq(s => s.SellSymbol, sellSymbol));
            }

            if (!string.IsNullOrEmpty(buySymbol))
            {
                filterDefinitions.Add(Builders<SymbolPrice>.Filter.Eq(s => s.BuySymbol, buySymbol));
            }

            if (symbolSource.HasValue)
            {
                filterDefinitions.Add(Builders<SymbolPrice>.Filter.Eq(s => s.Source, symbolSource.Value));
            }

            if (!string.IsNullOrEmpty(query))
            {
                filterDefinitions.Add(Builders<SymbolPrice>.Filter.Regex(x => x.SellSymbol, new BsonRegularExpression(query, "i")));
            }

            var sort = orderBy?.ToLower() switch
            {
                "price" => asc ? Builders<SymbolPrice>.Sort.Ascending(x => x.Price) : Builders<SymbolPrice>.Sort.Descending(x => x.Price),
                "change" => asc ? Builders<SymbolPrice>.Sort.Ascending(x => x.Change) : Builders<SymbolPrice>.Sort.Descending(x => x.Change),
                _ => asc ? Builders<SymbolPrice>.Sort.Ascending(x => x.SellSymbol) : Builders<SymbolPrice>.Sort.Descending(x => x.SellSymbol)
            };

            var filter = filterDefinitions.Count == 0
                ? Builders<SymbolPrice>.Filter.Empty
                : Builders<SymbolPrice>.Filter.And(filterDefinitions);

            var count = await _mongoCollection.CountDocumentsAsync(filter);

            var findOptions = new FindOptions<SymbolPrice, SymbolPrice>
            {
                Sort = sort,
                Skip = skip,
                Limit = take
            };

            using var cursor = await _mongoCollection.FindAsync(filter, findOptions);
            var result = new List<SymbolPrice>();

            while (await cursor.MoveNextAsync())
            {
                result.AddRange(cursor.Current);
            }

            return (result, count);
        }
    }
}