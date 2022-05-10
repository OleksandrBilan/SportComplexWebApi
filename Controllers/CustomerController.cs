using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/customer")]
    public class CustomerController : Controller
    {
        private readonly CustomerService _customerService;

        public CustomerController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("getById")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var customer = await _customerService.GetByIdAsync(id);

            if (customer is null)
            {
                return NotFound();
            }

            return Ok(customer);
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _customerService.GetAllAsync());
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromBody] Customer customer)
        {
            var createdCustomer = await _customerService.CreateAsync(customer);

            if (createdCustomer is null)
            {
                return BadRequest();
            }

            return Ok(createdCustomer);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync([FromBody] Customer customer)
        {
            var updatedCustomer = await _customerService.UpdateAsync(customer);

            if (updatedCustomer is null)
            {
                return BadRequest();
            }

            return Ok(updatedCustomer);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            bool result = await _customerService.DeleteAsync(id);

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
