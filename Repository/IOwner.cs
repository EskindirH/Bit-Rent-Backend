using BitRent.Models;

namespace BitRent.Repository
{
    public interface IOwner
    {
        Task<Owner> CreateAsync(Owner owner);
        Task<Owner> UpdateAsync(Owner owner);
        Task DeleteAsync(Owner owner);
        Task<Owner> GetOwner(string ownerId);
        Task<IEnumerable<Owner>> GetAllAsync();
        Task<bool> DoesExist(Owner user);
    }
}
