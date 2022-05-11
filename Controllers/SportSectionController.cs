using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApi.ApiModels;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/sportSection")]
    public class SportSectionController : Controller
    {
        private readonly SportSectionService _sportSectionService;

        public SportSectionController(SportSectionService sportSectionService)
        {
            _sportSectionService = sportSectionService;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _sportSectionService.GetAllAsync());
        }

        [HttpGet("getById")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var sportSection = await _sportSectionService.GetByIdAsync(id);

            if (sportSection is null)
            {
                return NotFound();
            }

            return Ok(sportSection);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromBody] SportSectionDto sportSection)
        {
            var createSportSection = await _sportSectionService.CreateAsync(sportSection);

            if (createSportSection is null)
            {
                return BadRequest();
            }

            return Ok(createSportSection);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync([FromBody] SportSectionDto sportSection)
        {
            var updateSportSection = await _sportSectionService.UpdateAsync(sportSection);

            if (updateSportSection is null)
            {
                return BadRequest();
            }

            return Ok(updateSportSection);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            bool result = await _sportSectionService.DeleteAsync(id);

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
