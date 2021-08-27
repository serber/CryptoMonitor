using System.Threading.Tasks;
using CryptoMonitor.Data;

namespace CryptoMonitor.DataAccess.Common.Repositories
{
    public interface IUserRepository
    {
        public Task<bool> ExistAsync(string login);

        public Task<User> GetAsync(string login, string passwordHash);
        Task AddAsync(string login, string passwordHash);
    }
}