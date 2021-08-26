using System.Threading.Tasks;
using CryptoMonitor.Data;

namespace CryptoMonitor.DataAccess.Common.Repositories
{
    public interface IUserRepository
    {
        public Task<bool> Exist(string login, string passwordHash);

        public Task<User> GetAsync(string login, string passwordHash);
    }
}