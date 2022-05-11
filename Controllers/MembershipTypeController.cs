using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApi.ApiModels.Membership;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/membershipType")]
    public class MembershipTypeController : Controller
    {
        private readonly MembershipTypeService _membershipTypeService;

        public MembershipTypeController(MembershipTypeService membershipTypeService)
        {
            _membershipTypeService = membershipTypeService;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _membershipTypeService.GetAllAsync());
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromBody] MembershipTypeDto membershipType)
        {
            var createdType = await _membershipTypeService.CreateAsync(membershipType);

            if (createdType is null)
            {
                return BadRequest();
            }

            return Ok(createdType);
        }

        [HttpPut("udpate")]
        public async Task<IActionResult> UpdateAsync([FromBody] MembershipTypeDto membershipType)
        {
            var updatedType = await _membershipTypeService.UpdateAsync(membershipType);

            if (updatedType is null)
            {
                return BadRequest();
            }

            return Ok(updatedType);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            bool result = await _membershipTypeService.DeleteAsync(id);

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
