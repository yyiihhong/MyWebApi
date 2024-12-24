using MyWebApi.Models;

namespace MyWebApi.Interfaces.Services
{
    public interface IEmployeeService
    {
        Task<IEnumerable<object>> GetAllCustomersAsync(string country);
    }
}
