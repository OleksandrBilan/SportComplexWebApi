using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApi.Models.EmployeeInfo;
using WebApi.Services;

namespace WebApi.Controllers
{
    public class CityController : Controller
    {
        private readonly CityService _cityService;

        public CityController(CityService cityService)
        {
            _cityService = cityService;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _cityService.GetAllAsync());
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromBody] City city)
        {
            await _cityService.CreateAsync(city);
            return Ok();
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync([FromBody] City city)
        {
            await _cityService.UpdateAsync(city);
            return Ok();
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _cityService.DeleteAsync(id);
            return Ok();
        }
    }
}
