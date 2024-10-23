using BitRent.Models;

namespace BitRent.Repository
{
    public interface IAppUser
    {
        Task<AppUser> CreateAsync(AppUser user);
        Task<AppUser> UpdateAsync(AppUser user);
        Task DeleteAsync(string id);
        Task<AppUser> GetAsync(string id);
        Task<IEnumerable<AppUser>> GetAllAsync();
        Task<bool> DoesExist(AppUser user);
    }
}
