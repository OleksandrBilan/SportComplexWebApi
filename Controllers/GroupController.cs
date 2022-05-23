using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApi.ApiModels;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/group")]
    public class GroupController : Controller
    {

        private readonly GroupService _groupService;

        public GroupController(GroupService GroupService)
        {
            _groupService = GroupService;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _groupService.GetAllAsync());
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromBody] GroupDto group)
        {
            var createdGroup = await _groupService.CreateAsync(group);

            if (createdGroup is null)
            {
                return BadRequest();
            }

            return Ok(createdGroup);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync([FromBody] GroupDto group)
        {
            var updatedGroup = await _groupService.UpdateAsync(group);

            if (updatedGroup is null)
            {
                return BadRequest();
            }

            return Ok(updatedGroup);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            bool result = await _groupService.DeleteAsync(id);

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpGet("getDays")]
        public async Task<IActionResult> GetDaysAsync()
        {
            return Ok(await _groupService.GetDaysAsync());
        }
    }
}
