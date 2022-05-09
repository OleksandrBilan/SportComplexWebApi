using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApi.ApiModels.Employee;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/employee")]
    public class EmployeeController : Controller
    {
        private readonly EmployeeService _employeeService;

        public EmployeeController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginApiModel loginApiModel)
        {
            var employee = await _employeeService.LoginAsync(loginApiModel.Login, loginApiModel.Password);

            if (employee is null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [HttpGet("getById")]
        public async Task<IActionResult> LoginAsync(int id)
        {
            var employee = await _employeeService.GetByIdAsync(id);

            if (employee is null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _employeeService.GetAllAsync());
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromBody] EmployeeApiModel employee)
        {
            var createdEmployee = await _employeeService.CreateAsync(employee);

            if (createdEmployee is null)
            {
                return BadRequest();
            }

            return Ok(createdEmployee);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync([FromBody] EmployeeApiModel employee)
        {
            var updatedEmployee = await _employeeService.UpdateAsync(employee);

            if (updatedEmployee is null)
            {
                return BadRequest();
            }

            return Ok(updatedEmployee);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            bool result = await _employeeService.DeleteAsync(id);

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
