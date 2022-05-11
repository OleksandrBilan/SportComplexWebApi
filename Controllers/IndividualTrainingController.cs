using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApi.ApiModels;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/individualTraining")]
    public class IndividualTrainingController : Controller
    {
        private readonly IndividualTrainingService _individualTrainingService;

        public IndividualTrainingController(IndividualTrainingService individualTrainingService)
        {
            _individualTrainingService = individualTrainingService;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _individualTrainingService.GetAllAsync());
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromBody] IndividualTrainingDto individualTraining)
        {
            var createdTraining = await _individualTrainingService.CreateAsync(individualTraining);

            if (createdTraining is null)
            {
                return BadRequest();
            }

            return Ok(createdTraining);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync([FromBody] IndividualTrainingDto individualTraining)
        {
            var updatedTraining = await _individualTrainingService.UpdateAsync(individualTraining);

            if (updatedTraining is null)
            {
                return BadRequest();
            }

            return Ok(updatedTraining);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            bool result = await _individualTrainingService.DeleteAsync(id);

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
