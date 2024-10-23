using BitRent.Data;
using BitRent.Models;
using Microsoft.EntityFrameworkCore;

namespace BitRent.Repository
{
    public class AppUserRepository : IAppUser
    {
        private readonly AppDbContext dbContext;

        public AppUserRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<AppUser> CreateAsync(AppUser user)
        {
            var result = await dbContext.AppUsers.AddAsync(user);
            await dbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task DeleteAsync(string id)
        {
            var result = await dbContext.AppUsers.FindAsync(id);
            if (result != null)
            {
                dbContext.Remove(result);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<bool> DoesExist(AppUser user)
        {
            return await dbContext.AppUsers.AnyAsync(i=>i.Id != user.Id && (i.PhoneNumber == user.PhoneNumber || i.Email == user.Email));
        }

        public async Task<IEnumerable<AppUser>> GetAllAsync()
        {
            return await dbContext.AppUsers.AsNoTracking().ToListAsync();
        }

        public async Task<AppUser> GetAsync(string id)
        {
            return (await dbContext.AppUsers.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id))!;
        }

        public async Task<AppUser> UpdateAsync(AppUser user)
        {
            var result = await dbContext.AppUsers.FindAsync(user.Id);
            if (result != null)
            {
                result.FirstName = user.FirstName;
                result.LastName = user.LastName;
                result.PhoneNumber = user.PhoneNumber;
                result.Email = user.Email;

                await dbContext.SaveChangesAsync();
                return result;
            }
            return null!;
        }
    }
}
