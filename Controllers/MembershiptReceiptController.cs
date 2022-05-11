using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApi.ApiModels.Membership;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/membershipReceipt")]
    public class MembershiptReceiptController : Controller
    {
        private readonly MembershipReceiptService _membershipReceiptService;

        public MembershiptReceiptController(MembershipReceiptService membershipReceiptService)
        {
            _membershipReceiptService = membershipReceiptService;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _membershipReceiptService.GetAllAsync());
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromBody] MembershipReceiptDto membershipReceipt)
        {
            var createdReceipt = await _membershipReceiptService.CreateAsync(membershipReceipt);

            if (createdReceipt is null)
            {
                return BadRequest();
            }

            return Ok(createdReceipt);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync([FromBody] MembershipReceiptDto membershipReceipt)
        {
            var updatedReceipt = await _membershipReceiptService.UpdateAsync(membershipReceipt);

            if (updatedReceipt is null)
            {
                return BadRequest();
            }

            return Ok(updatedReceipt);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            bool result = await _membershipReceiptService.DeleteAsync(id);

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
