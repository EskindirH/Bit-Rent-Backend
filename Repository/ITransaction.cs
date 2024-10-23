using BitRent.Models;

namespace BitRent.Repository
{
    public interface ITransaction
    {
        Task<BitTransaction> CreateAsync(BitTransaction transaction);
        Task<BitTransaction> UpdateAsync(BitTransaction transaction);
        Task DeleteAsync(string id);
        Task<BitTransaction> GetByIdAsync(string id);
        Task<IEnumerable<BitTransaction>> GetAllAsync();
    }
}
