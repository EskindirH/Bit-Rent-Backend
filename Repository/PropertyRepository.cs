using BitRent.Data;
using BitRent.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

namespace BitRent.Repository
{
    public class PropertyRepository(AppDbContext dbContext, IWebHostEnvironment webHostEnvironment) : IProperty
    {
        public async Task<Property> CreateAsync(Property property)
        {
            var result = await dbContext.Properties.AddAsync(property);
            await dbContext.SaveChangesAsync();
            return result.Entity;
        }

        public async Task DeleteAsync(string id)
        {
            var result = await dbContext.Properties.FindAsync(id);
            if (result != null)
            {
                dbContext.Remove(result);
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<byte[]> FetchPhotoData(string filePath)
        {
            string UploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "images");
            var UniqueFileName = Path.Combine(UploadsFolder, filePath);
            
            return await File.ReadAllBytesAsync(UniqueFileName);
        }

        public async Task<IEnumerable<Property>> GetAllAsync()
        {
            return await dbContext.Properties.AsNoTracking().Include(i=>i.Owner).Include(i=>i.Customer).ToListAsync();
        }

        public async Task<Property> GetAsync(string id)
        {
            return (await dbContext.Properties.AsNoTracking().Include(i => i.Owner).Include(i => i.Customer)
                                   .FirstOrDefaultAsync(i => i.Id == id))!;
        }

        public async Task<Property> UpdateAsync(Property property)
        {
            var result = await dbContext.Properties.FindAsync(property.Id);
            if (result != null)
            {
                result.Price = property.Price;
                result.FilePath = property.FilePath;
                result.Description = property.Description;
                result.IsAvailable = property.IsAvailable;
                result.CustomerId = property.CustomerId;
                await dbContext.SaveChangesAsync();
                return result;
            }
            return null!;
        }
    }
}
