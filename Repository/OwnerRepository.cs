using BitRent.Data;
using BitRent.Models;
using Microsoft.EntityFrameworkCore;

namespace BitRent.Repository
{
    public class OwnerRepository(AppDbContext dbContext) : IOwner
    {

        public async Task<Owner> CreateAsync(Owner owner)
        {
            var result = await dbContext.Owners.AddAsync(owner);
            await dbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task DeleteAsync(Owner owner)
        {
            var result = await dbContext.Owners.FindAsync(owner.Id);
            if (result != null)
            {
                dbContext.Remove(result);
                return;
            }
        }

        public async Task<bool> DoesExist(Owner user)
        {
            return await dbContext.Owners.AnyAsync(i => i.Id != user.Id && (i.PhoneNumber == user.PhoneNumber || i.Email == user.Email));
        }

        public async Task<IEnumerable<Owner>> GetAllAsync()
        {
            return await dbContext.Owners.AsNoTracking().Include(i=>i.Properties).Include(i=>i.Transactions).ToListAsync();
        }

        public async Task<Owner> GetOwner(string ownerId)
        {
            return (await dbContext.Owners.AsNoTracking().Include(i => i.Properties).Include(i => i.Transactions)
                                   .FirstOrDefaultAsync(i => i.Id == ownerId))!;
        }

        public async Task<Owner> UpdateAsync(Owner owner)
        {
            var result = await dbContext.Owners.FindAsync(owner.Id);
            if (result != null)
            {
                result.FirstName = owner.FirstName;
                result.LastName = owner.LastName;
                result.Email = owner.Email;
                result.PhoneNumber = owner.PhoneNumber;
                result.AccountNumber = owner.AccountNumber;
                await dbContext.SaveChangesAsync();
                return result;
            }
            return null!;
        }
    }
}
