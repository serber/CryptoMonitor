using CryptoMonitor.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoMonitor.Data.Enums;

namespace CryptoMonitor.DataAccess.Common.Repositories
{
    public interface ISymbolPriceRepository
    {
        Task AddAsync(IEnumerable<SymbolPrice> symbolPrices);

        Task<SymbolPrice> GetAsync(string sellSymbol, string buySymbol, SymbolSource symbolSource);

        Task<(IReadOnlyCollection<SymbolPrice> Items, long TotalCount)> ListAsync(int skip = 0, int take = 50, string sellSymbol = null, string buySymbol = null, SymbolSource? symbolSource = null, string orderBy = null, bool asc = true);
        
        Task<IReadOnlyCollection<SymbolPrice>> ListAsync(string buySymbol, SymbolSource symbolSource);
    }
}