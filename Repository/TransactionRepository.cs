using BitRent.Data;
using BitRent.Models;
using Microsoft.EntityFrameworkCore;

namespace BitRent.Repository
{
    public class TransactionRepository(AppDbContext dbContext) : ITransaction
    {
        public async Task<BitTransaction> CreateAsync(BitTransaction transaction)
        {
            var result = await dbContext.BitTransactions.AddAsync(transaction);
            await dbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task DeleteAsync(string id)
        {
            var result = await dbContext.BitTransactions.FindAsync(id);
            if (result != null)
            {
                dbContext.Remove(result);
                await dbContext.SaveChangesAsync(true);
            }
        }

        public async Task<IEnumerable<BitTransaction>> GetAllAsync()
        {
            return await dbContext.BitTransactions.AsNoTracking().Include(i=>i.Owner).Include(i=>i.Customer).ToListAsync();
        }

        public async Task<BitTransaction> GetByIdAsync(string id)
        {
            return (await dbContext.BitTransactions.AsNoTracking().Include(i => i.Owner).Include(i => i.Customer)
                                   .FirstOrDefaultAsync(i => i.Id == id))!;
        }

        public async Task<BitTransaction> UpdateAsync(BitTransaction transaction)
        {
            var result = await dbContext.BitTransactions.FirstOrDefaultAsync(i=>i.Id == transaction.Id);
            if (result != null)
            {
                return result;
            }
            return null!;
        }
    }
}
