using Microsoft.EntityFrameworkCore;
using MyWebApi.Interfaces.Repositories;
using MyWebApi.Models;

namespace MyWebApi.Repositories
{
    public class EmployeeRepository: IEmployeeRepository
    {
        private readonly TestContext _testDbcontext;

        public EmployeeRepository(TestContext testDbcontext) 
        {
            _testDbcontext = testDbcontext;
        }

        public async Task<IEnumerable<object>> GetAllCustomersAsync(string country)
        {
            var result = await _testDbcontext.Employees
                .Where(e=>e.Country==country)
                .Select(e => new
                {
                    FirstName = e.FirstName,
                    LastName = e.LastName,
                    TitleOfCourtesy = e.TitleOfCourtesy,
                    City = e.City

                })
                .ToListAsync();
            
            return result;
        }


    }
}
