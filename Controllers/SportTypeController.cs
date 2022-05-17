using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApi.Models;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/sportType")]
    public class SportTypeController : Controller
    {
        private readonly SportTypeService _sportTypeService;

        public SportTypeController(SportTypeService sportTypeService)
        {
            _sportTypeService = sportTypeService;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _sportTypeService.GetAllAsync());
        }

        [HttpGet("getById")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var sportType = await _sportTypeService.GetByIdAsync(id);

            if (sportType is null)
            {
                return NotFound();
            }

            return Ok(sportType);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromBody] SportType sportType)
        {
            var createSportType = await _sportTypeService.CreateAsync(sportType.Name);

            if (createSportType is null)
            {
                return BadRequest();
            }

            return Ok(createSportType);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync([FromBody] SportType sportType)
        {
            var updatedSportType = await _sportTypeService.UpdateAsync(sportType);

            if (updatedSportType is null)
            {
                return BadRequest();
            }

            return Ok(updatedSportType);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            bool result = await _sportTypeService.DeleteAsync(id);

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
