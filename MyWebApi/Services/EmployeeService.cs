using MyWebApi.Interfaces.Repositories;
using MyWebApi.Interfaces.Services;
using MyWebApi.Models;
using NuGet.Protocol.Core.Types;
using System.Diagnostics.Metrics;

namespace MyWebApi.Services
{
    public class EmployeeService: IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        public async Task<IEnumerable<object>> GetAllCustomersAsync(string country) 
        {
            return await _employeeRepository.GetAllCustomersAsync( country);
        }

    }
}
