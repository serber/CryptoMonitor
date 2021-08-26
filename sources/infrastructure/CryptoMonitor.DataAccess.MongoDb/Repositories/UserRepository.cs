using System.Threading.Tasks;
using CryptoMonitor.Data;
using CryptoMonitor.DataAccess.Common.Repositories;
using MongoDB.Driver;

namespace CryptoMonitor.DataAccess.MongoDb.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoCollection<User> _mongoCollection;

        public UserRepository(IMongoCollection<User> mongoCollection)
        {
            _mongoCollection = mongoCollection;
        }

        public async Task<bool> Exist(string login, string passwordHash)
        {
            var filter = Builders<User>.Filter.Where(x => x.Login == login && x.PasswordHash == passwordHash);

            var cursor = _mongoCollection.Find(filter);

            return await cursor.AnyAsync();
        }

        public async Task<User> GetAsync(string login, string passwordHash)
        {
            var filter = Builders<User>.Filter.Where(x => x.Login == login && x.PasswordHash == passwordHash);

            var cursor = _mongoCollection.Find(filter);

            return await cursor.SingleOrDefaultAsync();
        }
    }
}