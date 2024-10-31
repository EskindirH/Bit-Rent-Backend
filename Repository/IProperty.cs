using BitRent.Models;

namespace BitRent.Repository
{
    public interface IProperty
    {
        Task<Property> CreateAsync(Property property);
        Task<Property> UpdateAsync(Property property);
        Task DeleteAsync(string id);
        Task<Property> GetAsync(string id);
        Task<IEnumerable<Property>> GetAllAsync();
        Task<byte[]> FetchPhotoData(string filePath);
    }
}
