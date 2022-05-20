using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApi.ApiModels;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/groupTraining")]
    public class GroupTrainingController : Controller
    {
        private readonly GroupTrainingService _groupTrainingService;

        public GroupTrainingController(GroupTrainingService groupTrainingService)
        {
            _groupTrainingService = groupTrainingService;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _groupTrainingService.GetAllAsync());
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromBody] GroupTrainingDto groupTraining)
        {
            await _groupTrainingService.CreateAsync(groupTraining);
            return Ok();
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync([FromBody] GroupTrainingDto groupTraining)
        {
            await _groupTrainingService.UpdateAsync(groupTraining);
            return Ok();
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _groupTrainingService.DeleteAsync(id);
            return Ok();
        }
    }
}
