using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApi.ApiModels;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/coach")]
    public class CoachController : Controller
    {
        private readonly CoachService _coachService;

        public CoachController(CoachService coachService)
        {
            _coachService = coachService;
        }

        [HttpGet("getById")]
        public async Task<IActionResult> GetByIdAsync(int id)
        {
            var coach = await _coachService.GetByIdAsync(id);

            if (coach is null)
            {
                return NotFound();
            }

            return Ok(coach);
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _coachService.GetAllAsync());
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromBody] CoachDto coach)
        {
            var createdCoach = await _coachService.CreateAsync(coach);

            if (createdCoach is null)
            {
                return BadRequest();
            }

            return Ok(createdCoach);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync([FromBody] CoachDto coach)
        {
            var updatedCoach = await _coachService.UpdateAsync(coach);

            if (updatedCoach is null)
            {
                return BadRequest();
            }

            return Ok(updatedCoach);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            bool result = await _coachService.DeleteAsync(id);

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpGet("individualCoaches")]
        public async Task<IActionResult> GetIndividualCoachesAsync()
        {
            return Ok(await _coachService.GetIndividualCoachesAsync()); 
        }
    }
}
