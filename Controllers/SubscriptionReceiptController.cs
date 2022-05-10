using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebApi.ApiModels.GroupTrainingSubscription;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/subscriptionReceipt")]
    public class SubscriptionReceiptController : Controller
    {
        private readonly SubscriptionReceiptService _subscriptionReceiptService;

        public SubscriptionReceiptController(SubscriptionReceiptService subscriptionReceiptService)
        {
            _subscriptionReceiptService = subscriptionReceiptService;
        }

        [HttpGet("getAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            return Ok(await _subscriptionReceiptService.GetAllAsync());
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromBody] SubscriptionReceiptApiModel subscriptionReceipt)
        {
            var createdReceipt = await _subscriptionReceiptService.CreateAsync(subscriptionReceipt);

            if (createdReceipt is null)
            {
                return BadRequest();
            }

            return Ok(createdReceipt);
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync([FromBody] SubscriptionReceiptApiModel subscriptionReceipt)
        {
            var updatedReceipt = await _subscriptionReceiptService.UpdateAsync(subscriptionReceipt);

            if (updatedReceipt is null)
            {
                return BadRequest();
            }

            return Ok(updatedReceipt);
        }

        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            bool result = await _subscriptionReceiptService.DeleteAsync(id);

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
