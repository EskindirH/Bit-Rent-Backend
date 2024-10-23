using BitRent.Models;

namespace BitRent.Repository
{
    public interface ICustomer
    {
        Task<Customer> CreateAsync(Customer customer);
        Task<Customer> UpdateAsync(Customer customer);
        Task DeleteAsync(string id);
        Task<Customer> GetCustomer(string id);
        Task<IEnumerable<Customer>> GetAll();
        Task<bool> DoesExist(Customer user);
    }
}
