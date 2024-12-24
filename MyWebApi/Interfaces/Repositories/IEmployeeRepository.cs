using MyWebApi.Models;

namespace MyWebApi.Interfaces.Repositories
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<object>> GetAllCustomersAsync(string country);
    }
}
