using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApi.ApiModels.Employee;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/gym")]
    public class GymController : Controller
    {

        private readonly GymService _gymService;

        public GymController(GymService gymService)
        {
            _gymService = gymService;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _gymService.GetAllAsync());
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromBody] GymDto gym)
        {
            var createdGym = await _gymService.CreateAsync(gym);

            if (createdGym is null)
            {
                return BadRequest();
            }

            return Ok(createdGym);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync([FromBody] GymDto gym)
        {
            var updatedGym = await _gymService.UpdateAsync(gym);

            if (updatedGym is null)
            {
                return BadRequest();
            }

            return Ok(updatedGym);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            bool result = await _gymService.DeleteAsync(id);

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
