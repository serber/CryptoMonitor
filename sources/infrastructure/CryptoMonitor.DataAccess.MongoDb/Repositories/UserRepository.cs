using System;
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

        public async Task<bool> ExistAsync(string login)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Login, login);

            var cursor = await _mongoCollection.FindAsync(filter);

            return await cursor.AnyAsync();
        }

        public async Task<User> GetAsync(string login, string passwordHash)
        {
            var filter = Builders<User>.Filter.And(Builders<User>.Filter.Eq(x => x.Login, login),
                Builders<User>.Filter.Eq(x => x.PasswordHash, passwordHash));

            var cursor = await _mongoCollection.FindAsync(filter);

            return await cursor.SingleOrDefaultAsync();
        }

        public async Task AddAsync(string login, string passwordHash)
        {
            await _mongoCollection.InsertOneAsync(new User
            {
                Login = login,
                PasswordHash = passwordHash,
                UserId = Guid.NewGuid().ToString("D")
            });
        }
    }
}