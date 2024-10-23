using BitRent.Data;
using BitRent.Models;
using Microsoft.EntityFrameworkCore;

namespace BitRent.Repository
{
    public class CustomerRepository : ICustomer
    {
        private readonly AppDbContext dbContext;

        public CustomerRepository(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Customer> CreateAsync(Customer customer)
        {
            var result = await dbContext.Customers.AddAsync(customer);
            await dbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task DeleteAsync(string id)
        {
            var result = await dbContext.Customers.FindAsync(id);
            if (result != null)
            {
                dbContext.Customers.Remove(result);
                await dbContext.SaveChangesAsync();
                return;
            }
        }

        public async Task<bool> DoesExist(Customer user)
        {
            return await dbContext.Customers.AnyAsync(i => i.Id != user.Id && (i.PhoneNumber == user.PhoneNumber || i.Email == user.Email));
        }

        public async Task<IEnumerable<Customer>> GetAll()
        {
            return await dbContext.Customers.AsNoTracking().Include(i => i.Properties).Include(i => i.BitTransaction).ToListAsync();
        }

        public async Task<Customer> GetCustomer(string id)
        {
            return (await dbContext.Customers.AsNoTracking().Include(i => i.Properties)
                        .Include(i => i.BitTransaction).FirstOrDefaultAsync(i => i.Id == id))!;
        }

        public async Task<Customer> UpdateAsync(Customer customer)
        {
            var result = await dbContext.Customers.FindAsync(customer.Id);
            if (result != null)
            {
                result.FirstName = customer.FirstName;
                result.LastName = customer.LastName;
                result.Email = customer.Email;
                result.PhoneNumber = customer.PhoneNumber;
                result.AccountNumber = customer.AccountNumber;
                await dbContext.SaveChangesAsync();
                return result;
            }
            return null!;
        }
    }
}
