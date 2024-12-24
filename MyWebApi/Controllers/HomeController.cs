using Microsoft.AspNetCore.Mvc;
using MyWebApi.Interfaces.Services;
using MyWebApi.Services;
using Microsoft.AspNetCore.Authorization;


namespace MyWebApi.Controllers
{
    public class HomeController : ControllerBase
    {
        private readonly IEmployeeService _employeeService;

        public HomeController(IEmployeeService employeeService) 
        {
            _employeeService = employeeService;
        }

        [HttpGet("employee")]
        [Authorize]
        public async Task<IActionResult> GetEmployees(string country)
        {
            try
            {
                var employees = await _employeeService.GetAllCustomersAsync(country);

                if (!employees.Any()) // 如果沒有數據
                {
                    return NoContent(); // 返回 204 No Content
                }

                return Ok(employees); // 返回 JSON 格式的數據
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred.", details = ex.Message });
            }
        }



    }
}
