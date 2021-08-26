using CryptoMonitor.Data;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace CryptoMonitor.DataAccess.MongoDb
{
    public static class DocumentMapping
    {
        public static void Map()
        {
            BsonClassMap.RegisterClassMap<SymbolPrice>(map =>
            {
                map.MapProperty(x => x.SellSymbol).SetElementName("sell_symbol");
                map.MapProperty(x => x.BuySymbol).SetElementName("buy_symbol");
                map.MapProperty(x => x.Source).SetElementName("source");
                map.MapProperty(x => x.Price).SetElementName("price")
                    .SetSerializer(new DecimalSerializer(BsonType.Decimal128));
                map.MapProperty(x => x.LoadedAt).SetElementName("loaded_at");
                map.MapProperty(x => x.Change).SetElementName("change");
                map.MapProperty(x => x.OpenPrice).SetElementName("open_price");
            });

            BsonClassMap.RegisterClassMap<User>(map =>
            {
                map.MapIdProperty(x => x.UserId);
                map.MapProperty(x => x.Login).SetElementName("login");
                map.MapProperty(x => x.PasswordHash).SetElementName("password_hash");
            });

            BsonClassMap.RegisterClassMap<DropPrice>(map =>
            {
                map.MapProperty(x => x.UserId).SetElementName("user_id");
                map.MapProperty(x => x.SellSymbol).SetElementName("sell_symbol");
                map.MapProperty(x => x.BuySymbol).SetElementName("buy_symbol");
                map.MapProperty(x => x.Source).SetElementName("source");
                map.MapProperty(x => x.Price).SetElementName("price")
                    .SetSerializer(new DecimalSerializer(BsonType.Decimal128));
                map.MapProperty(x => x.Multiplier).SetElementName("multiplier")
                    .SetSerializer(new DecimalSerializer(BsonType.Decimal128));
            });
        }
    }
}