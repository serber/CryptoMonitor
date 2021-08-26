using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoMonitor.Data;
using CryptoMonitor.Data.Enums;

namespace CryptoMonitor.DataAccess.Common.Repositories
{
    public interface IDropPriceRepository
    {
        Task AddAsync(DropPrice dropPrice);
        
        Task DeleteAsync(string userId, string sellSymbol, string buySymbol, SymbolSource symbolSource);
        
        Task<(IReadOnlyCollection<DropPrice> Items, long Count)> ListAsync(string userId, string buySymbol, SymbolSource symbolSource, string orderBy, bool asc);
    }
}